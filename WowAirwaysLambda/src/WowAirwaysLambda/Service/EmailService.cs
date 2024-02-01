using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace WowAirwaysLambda.Service
{
    public class EmailService
    {
        private PdfService _pdfService;
        private byte[] _bytes;

        public EmailService(PdfService pdfService)
        {
            _pdfService = pdfService;
        }

        private string CreateEmailBody(string attendeeName, string bookingReference)
        {
            var output = File.ReadAllText(@"Templates/BookingEmail.html");

            output = output.Replace("[Guest Name]", attendeeName);

            output = output.Replace(" [Booking Reference]", bookingReference);

            return output;
        }

        private AlternateView CreateAlternateView(string htmlBody)
        {
            var imageResource = new LinkedResource(@"Templates/banner.png");

            imageResource.ContentId = Guid.NewGuid().ToString();
            imageResource.ContentType.MediaType = MediaTypeNames.Image.Jpeg; ;
            imageResource.TransferEncoding = TransferEncoding.Base64;
            imageResource.ContentType.Name = imageResource.ContentId;
            imageResource.ContentLink = new Uri("cid:" + imageResource.ContentId);

            htmlBody = htmlBody.Replace("banner.png", $"cid:{imageResource.ContentId}");

            var output = AlternateView.CreateAlternateViewFromString(htmlBody
                , Encoding.UTF8
                , MediaTypeNames.Text.Html);

            output.LinkedResources.Add(imageResource);

            return output;
        }

        private Attachment CreateAttachment(string attendeeFirstName, string attendeeLastName
            , string bookingReference, string flightNo)
        {
            _bytes = _pdfService.CreateItineraryFile(bookingReference, flightNo
                , attendeeFirstName, attendeeLastName);

            var output = new Attachment(new MemoryStream(_bytes),
                new ContentType(MediaTypeNames.Application.Octet));

            output.ContentDisposition.FileName = "BookingConfirmation.pdf";

            return output;
        }

        public void Send(string recipientEmail, string attendeeFirstName, string attendeeLastName
            , string bookingReference, string flightNo)
        {
            // Sender's email address and credentials
            string senderEmail = "2024leaderssummit@shakeys.biz";
            string senderPassword = "Hrsummit2024*@";

            // Create the MailMessage object
            var mail = new MailMessage(new MailAddress(senderEmail, "Leaders Summit 2024")
                , new MailAddress(recipientEmail));

            mail.Subject = "Booking Confirmation - 2024 Leaders Summit";
            mail.IsBodyHtml = true;

            var htmlBody = CreateEmailBody($"{attendeeLastName}, {attendeeFirstName}"
                , bookingReference);

            mail.AlternateViews.Add(CreateAlternateView(htmlBody));

            mail.Attachments.Add(CreateAttachment(attendeeFirstName, attendeeLastName, bookingReference, flightNo));

            // Create the SmtpClient object
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }

    }
}
