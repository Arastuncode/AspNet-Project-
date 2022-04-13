using AspProject.Services.Interface;
using AspProject.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(string emailTO,string html,string content,string userName)
        {
            var emailModel = _configuration.GetSection("EmailConfig").Get<EmailRequest>();
            var apiKey = emailModel.SecretKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailModel.SenderEmail,emailModel.SenderName);
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(emailTO, userName);
            var plainTextContent = content;
            var htmlContent = html;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, html);
            await client.SendEmailAsync(msg);
        }
    }
}
