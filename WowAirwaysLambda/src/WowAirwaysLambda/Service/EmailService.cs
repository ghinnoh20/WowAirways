using System.Net.Mail;
using System.Net;

namespace WowAirwaysLambda.Service
{
    public class EmailService
    {
        private string CreateEmailBody()
        {
            return File.ReadAllText(@"Templates/BookingEmail.html");
        }

        public void Send(string recipientEmail)
        {
            // Sender's email address and credentials
            string senderEmail = "ginosoftwareengineermanager@gmail.com";
            string senderPassword = "vbpbxxznxodcawhn";

            // Create the MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Hello, this is a test email";
            mail.Body = CreateEmailBody();
            mail.IsBodyHtml = true;

            // Create the SmtpClient object
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }

    }
}
