using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WowAirwaysPrinter.Models;
using WowAirwaysPrinter.Services;

namespace WowAirwaysPrinter
{
    public partial class Form1 : Form
    {

        private PdfService _pdfService;
        private ExcelService _excelService;
        private BackgroundWorker _backgroundWorker;

        public Form1()
        {
            InitializeComponent();

            _pdfService = new PdfService();
            _excelService = new ExcelService();

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Log($"{e.UserState}");
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Log($"Error: {e.Error.Message}");
            }
            else
            {
                Log($"{e.Result}");
                // opens the folder in explorer
                Process.Start($@"{AppDomain.CurrentDomain.BaseDirectory}\BoardingPassPDFs");
            }

            ToggleControls();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var counter = 1;

            _backgroundWorker.ReportProgress(1, $"Reading contents of {txtFilename.Text}...");

            var attendees = _excelService.Read(txtFilename.Text);

            _backgroundWorker.ReportProgress(2, $"Total of {attendees.Count} attendees.");

            _backgroundWorker.ReportProgress(3, "Creating PDFs...");

            foreach (var attendee in attendees)
            {
                _backgroundWorker.ReportProgress(4, $"Creating PDF of {counter} of {attendees.Count}...");

                _pdfService.CreateBoardingPass(attendee.FullName
                    , attendee.FinalSeating
                    , attendee.FullName
                    , BoardingPassType.DefaultAndGreen);


                _pdfService.CreateBoardingPass(attendee.FullName
                    , attendee.FinalSeating
                    , attendee.FullName
                    , BoardingPassType.YellowAndOrange);
               
                Thread.Sleep(500);

                _backgroundWorker.ReportProgress(5, $"PDF of {counter} of {attendees.Count} created.");

                counter++;

            }
            e.Result = "Done creating PDFs.";
        }

        private void Log(string message)
        {
            rtxtStatus.AppendText($"[{DateTime.Now.ToString("hh:mm:ss:fff tt").ToUpper()}]{message}{Environment.NewLine}");
        }

        private BoardingPassType GetBoardingPassType(string division)
        {
            BoardingPassType output;

            switch (division.ToUpper())
            {
                case "BMI":
                case "CENTURY":
                case "CORPORATE":
                case "INTERNATIONAL":
                    output = BoardingPassType.Default;
                    break;
                case "PERI-PERI":
                    output = BoardingPassType.PeriPeri;
                    break;
                case "POTATO CORNER":
                    output = BoardingPassType.PotatoCorner;
                    break;
                case "SHAKEY'S":
                    output = BoardingPassType.Shakeys;
                    break;
                default:
                    output = BoardingPassType.Default;
                    break;
            }

            return output;
        }

        private void ToggleControls()
        {
            btnBrowse.Enabled = (btnBrowse.Enabled) ? false : true;
            btnStart.Enabled = (btnStart.Enabled) ? false : true;
            rtxtStatus.Enabled = (rtxtStatus.Enabled) ? false : true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilename.Text))
            {
                MessageBox.Show("No file selected.", "Validation Failed"
                    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            if (!Directory.Exists("BoardingPassPDFs"))
            {
                Directory.CreateDirectory("BoardingPassPDFs");
            }

            DialogResult dialogResult = MessageBox.Show("Start PDF creation?", "Confirmation"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            ToggleControls();
            _backgroundWorker.RunWorkerAsync();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            DialogResult result = fileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtFilename.Text = fileDialog.FileName;
            }
        }

    }
}
