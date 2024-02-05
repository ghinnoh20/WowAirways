using System;
using System.Windows.Forms;
using WowAirwaysPrinter.Models;
using WowAirwaysPrinter.Services;

namespace WowAirwaysPrinter
{
    public partial class Form1 : Form
    {

        private PdfService _pdfService;

        public Form1()
        {
            InitializeComponent();

            _pdfService = new PdfService();
        }

        private void Log(string message)
        {
            rtxtStatus.AppendText($"[{DateTime.Now.ToString("hh:mm:ss:fff TT")}]{message}{Environment.NewLine}");
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Log("Creating PDFs...");
                _pdfService.CreateBoardingPass("Linda, Blair 2", "23", BoardingPassType.Shakeys);



                Log("PDFs created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
