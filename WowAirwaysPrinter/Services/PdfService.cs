using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.Kernel.Pdf;
using System.Collections.Generic;
using System.IO;
using WowAirwaysPrinter.Models;

namespace WowAirwaysPrinter.Services
{
    public class PdfService
    {
        private string GetTempalte(BoardingPassType boardingPassType)
        {
            var output = @"Templates/";

            switch (boardingPassType)
            {
                case BoardingPassType.Shakeys:
                    output += "Shakeys.pdf";
                    break;
                case BoardingPassType.PeriPeri:
                    output += "PeriPeri.pdf";
                    break;
                case BoardingPassType.PotatoCorner:
                    output += "PC-RB.pdf";
                    break;
                case BoardingPassType.RnB:
                    output += "PC-RB.pdf";
                    break;
                default:
                    output += "Default.pdf";
                    break;
            }

            return output;
        }

        private void CreatePdf(byte[] bytes, string fileName)
        {
            if (!Directory.Exists(@"BoardingPassPDFs"))
            {
                Directory.CreateDirectory(@"BoardingPassPDFs");
            }

            using (FileStream fs = File.Create($@"BoardingPassPDFs/{fileName}")) { 
                fs.Write(bytes, 0, (int)bytes.Length); 
            }
        }

        /// <summary>
        ///     Creates a boarding pass PDF file
        /// </summary>
        /// <param name="attendeeName">Name of the attendee to be printed. Format: Lastname, Firstname</param>
        /// <param name="seatNo">Seat number to be printed</param>
        /// <returns></returns>
        public void CreateBoardingPass(string attendeeName
            , string seatNo
            , BoardingPassType boardingPassType = BoardingPassType.Default)
        {
            string path = GetTempalte(boardingPassType);

            if (System.IO.File.Exists(path))
            {
                var soruceFileStream = File.OpenRead(path);
                var outputStream = new MemoryStream();

                var pdf = new PdfDocument(new PdfReader(soruceFileStream)
                    , new PdfWriter(outputStream));

                var form = PdfAcroForm.GetAcroForm(pdf, false);

                if (form != null)
                {
                    IDictionary<string, PdfFormField> fields = form.GetAllFormFields();
                    PdfFormField toSet;

                    fields.TryGetValue("txtAttendee1", out toSet);
                    toSet.SetValue(attendeeName);

                    fields.TryGetValue("txtAttendee2", out toSet);
                    toSet.SetValue(attendeeName);

                    fields.TryGetValue("txtSeat1", out toSet);
                    toSet.SetValue(seatNo);

                    fields.TryGetValue("txtSeat2", out toSet);
                    toSet.SetValue(seatNo);

                    pdf.Close();

                    CreatePdf(outputStream.ToArray(), $"{attendeeName}-{boardingPassType.ToString()}.pdf");
                }

            }

        }
    }
}
