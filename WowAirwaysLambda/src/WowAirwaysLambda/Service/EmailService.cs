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
            string senderEmail = "2024leaderssummit@shakeys.biz";
            string senderPassword = "Jah67081";

            // Create the MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Booking Confirmation - 2024 Leaders Summit";
            mail.Body = CreateEmailBody();
            mail.IsBodyHtml = true;

            // Create the SmtpClient object
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }

    }
}
