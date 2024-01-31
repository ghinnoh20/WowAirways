using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace WowAirwaysLambda.Service
{
    public class EmailService
    {
        /// <summary>
        ///     Creates the Itinerary file to be used as attachment
        /// </summary>
        /// <returns></returns>
        public byte[] CreateItineraryFile()
        {
            string path = @"Templates/EditableTemplate.pdf";
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
                    toSet.SetValue("Gino");

                    pdf.Close();

                    output =  outputStream.ToArray();
                }

            }

            return output;

        }
    }
}
