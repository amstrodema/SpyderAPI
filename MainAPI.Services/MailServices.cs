using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using MainAPI.Models;

namespace MainAPI.Services
{
    public class MailServices
    {
        // The email address of the sender
        private string _senderEmail;
        // The password for the sender's email account
        private string _senderPassword;
        // The email address of the recipient
        private string _recipientEmail;
        // The subject of the email
        private string _subject;
        // The body of the email
        private string _body;

        public MailServices(Email email)
        {

            _senderEmail = email.senderEmail;
            _senderPassword = email.senderPassword;
            _recipientEmail = email.recipientEmail;
            _subject = email.subject;
            _body = email.body;
        }

        public async Task SendMail()
        {
            // Create a new instance of the MailMessage class
            MailMessage message = new MailMessage();
            // Set the sender's email address
            message.From = new MailAddress(_senderEmail);
            // Set the recipient's email address
            message.To.Add(_recipientEmail);
            // Set the subject of the email
            message.Subject = _subject;
            // Set the body of the email
            message.Body = _body;
            // Set the email's content type to HTML
            message.IsBodyHtml = true;

            // Create a new instance of the SmtpClient class
            SmtpClient client = new SmtpClient();
            // Set the hostname of the mail server
            client.Host = "smtp.gmail.com";
            // Set the port number for the mail server
            client.Port = 587;
            // Enable SSL
            client.EnableSsl = true;
            // Set the credentials for the email account
            client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

            // Send the email
            await client.SendMailAsync(message);
        }
    }
}
