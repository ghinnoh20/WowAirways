using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using WowAirwaysPrinter.Models;
using WowAirwaysPrinter.Services;

namespace WowAirwaysPrinter
{
    public partial class Form1 : Form
    {

        private PdfService _pdfService;
        private ExcelService _excelService;
        public Form1()
        {
            InitializeComponent();

            _pdfService = new PdfService();
            _excelService = new ExcelService();
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
                //_pdfService.CreateBoardingPass("Linda, Blair 2", "23", BoardingPassType.Shakeys);

                var output = _excelService.Read(@"C:\Users\GinoMartinIngreso\Downloads\LS_Final Seat Plan_1.xlsx");

                foreach (var item in output)
                {
                    Log(JsonConvert.SerializeObject(item));
                }

                Log("PDFs created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
