using MainAPI.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _emailConfig = "Config:Email";
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(Email email)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                string username = _configuration.GetSection($"{_emailConfig}:Username").Value;
                string password = _configuration.GetSection($"{_emailConfig}:Password").Value;
                bool isAsync = Convert.ToBoolean(_configuration.GetSection($"{_emailConfig}:IsAsync").Value);
                bool sslEnabled = Convert.ToBoolean(_configuration.GetSection($"{_emailConfig}:EnableSSL").Value);

                string displayName = _configuration.GetSection($"{_emailConfig}:DisplayName").Value;
                string sender = _configuration.GetSection($"{_emailConfig}:Sender").Value;
                string defaultRecipient = _configuration.GetSection($"{_emailConfig}:Recipient").Value;

                if (string.IsNullOrWhiteSpace(email.Sender))
                {
                    email.Sender = sender;
                    email.Message += "<p>This email is an auto-generated email. Please do not reply.</p>";
                }

                if (email.Recipients.Count == 0)
                    email.Recipients.Add(defaultRecipient);

                displayName = string.IsNullOrWhiteSpace(email.DisplayName) ? displayName : email.DisplayName;

                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(email.Sender, displayName);

                    smtpClient.Host = _configuration.GetSection($"{_emailConfig}:Host").Value;
                    smtpClient.Port = Convert.ToInt32(_configuration.GetSection($"{_emailConfig}:Port").Value);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(username, password);
                    smtpClient.EnableSsl = sslEnabled;
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = email.Subject;
                    // Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = email.Message;

                    foreach (string recipient in email.Recipients)
                        message.To.Add(recipient);

                    message.CC.Add("cc.email@company.com");

                    // ignore bad certificates
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    //await smtpClient.SendMailAsync(message);
                    await SendEmail(smtpClient, message, isAsync);
                }
            }

        }

        async Task SendEmail(SmtpClient smtpClient, MailMessage mailMessage, bool sendAsAsync)
        {
            try
            {
                if (sendAsAsync)
                    await smtpClient.SendMailAsync(mailMessage);
                else
                    smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {

            }
        }
    }
}
