using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using System.Collections.Generic;
using System.IO;
using WowAirwaysPrinter.Models;

namespace WowAirwaysPrinter.Services
{
    public class PdfService
    {
        /// <summary>
        ///     Creates a boarding pass PDF file
        /// </summary>
        /// <param name="attendeeName">Name of the attendee to be printed. Format: Lastname, Firstname</param>
        /// <param name="seatNo">Seat number to be printed</param>
        /// <returns></returns>
        public byte[] CreateBoardingPass(string attendeeName
            , string seatNo
            , BoardingPassType boardingPassType = BoardingPassType.Default)
        {
            string path = @"Templates/ItineraryTemplate.pdf";
            byte[] output = null;

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

                    fields.TryGetValue("txtBookingReference", out toSet);
                    toSet.SetValue(bookingReference);

                    fields.TryGetValue("txtFlightNo1", out toSet);
                    toSet.SetValue(flightNo);

                    fields.TryGetValue("txtFlightNo2", out toSet);
                    toSet.SetValue(flightNo);

                    fields.TryGetValue("txtName", out toSet);
                    toSet.SetValue($"{lastName},{firstName}");

                    pdf.Close();

                    output = outputStream.ToArray();
                }

            }

            return output;

        }
    }
}
