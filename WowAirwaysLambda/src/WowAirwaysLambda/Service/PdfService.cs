using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace WowAirwaysLambda.Service
{
    public class PdfService
    {
        /// <summary>
        ///     Creates the Itinerary file to be used as attachment
        /// </summary>
        /// <returns></returns>
        public byte[] CreateItineraryFile(string bookingReference, string flightNo
            , string firstName, string lastName)
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
