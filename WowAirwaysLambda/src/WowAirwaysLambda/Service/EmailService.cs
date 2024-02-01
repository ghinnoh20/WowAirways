using System.Net.Mail;
using System.Net;

namespace WowAirwaysLambda.Service
{
    public class EmailService
    {
        public void Send(string recipientEmail)
        {
            // Sender's email address and credentials
            string senderEmail = "ginosoftwareengineermanager@gmail.com";
            string senderPassword = "Cheese1!";

            // Create the MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Hello, this is a test email";
            mail.Body = "This is the body of the email.";

            // Create the SmtpClient object
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

        }

    }
}
