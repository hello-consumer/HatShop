using FluentEmail.Mailgun;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Services
{
    public class EmailService : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private MailgunSender _mailgunSender;
        private ISendGridClient _sendGridClient;
        private ILogger<EmailService> _logger;

        public EmailService(MailgunSender mailgunSender, ISendGridClient sendGridClient, ILogger<EmailService> logger)
        {
            _mailgunSender = mailgunSender;
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("owner@hatshop.codingtemple.com", "HatShop Owner"),
                    Subject = subject,
                    PlainTextContent = htmlMessage,
                    HtmlContent = htmlMessage
                };
                msg.AddTo(new EmailAddress(email));
                msg.SetClickTracking(false, false);
                Response response = await _sendGridClient.SendEmailAsync(msg);
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    throw new ApplicationException("SendGrid error: " + await response.Body.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                try
                {
                    FluentEmail.Core.Email mgEmail = new FluentEmail.Core.Email("owner@hatshop.codingtemple.com", "HatShop Owner");
                    mgEmail
                        .To(email)
                        .Subject(subject)
                        .Body(htmlMessage);
                    var r2 = await _mailgunSender.SendAsync(mgEmail);
                    if (!r2.Successful)
                    {
                        throw new ApplicationException("MailGun error: " + string.Join(",", r2.ErrorMessages));
                    }
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, ex2.Message);
                }
            }
        }
    }
}
