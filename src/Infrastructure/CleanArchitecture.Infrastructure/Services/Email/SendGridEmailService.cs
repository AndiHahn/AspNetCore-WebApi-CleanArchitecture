using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Email;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanArchitecture.Infrastructure.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridConfiguration sendGridConfig;

        public SendGridEmailService(IOptions<SendGridConfiguration> sendGridConfig)
        {
            this.sendGridConfig = sendGridConfig?.Value ?? throw new ArgumentNullException(nameof(sendGridConfig));
        }

        public async Task SendEmailAsync(EmailSendModel email)
        {
            var emailMessage = BuildEmail(email);
            var apiKey = sendGridConfig.ApiKey;
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(emailMessage);
            if (!response.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                throw new HttpRequestException($"Could not send email. HttpStatusCode: {response.StatusCode}, Error: {response.Body}");
            }
        }

        private SendGridMessage BuildEmail(EmailSendModel emailData)
        {
            var senderEmail = BuildEmailAddress(emailData.SenderEmail, emailData.SenderName);
            var receiverEmail = BuildEmailAddress(emailData.ReceiverEmail, emailData.ReceiverName);
            var mailTextContent = emailData.Message;
            var emailMessage = MailHelper.CreateSingleEmail(
                senderEmail,
                receiverEmail,
                emailData.Subject,
                mailTextContent,
                null);
            emailMessage.TrackingSettings = CreateTrackingSettings();
            AddSubstitutionsToEmailMessage(emailMessage, emailData.Substitutions);

            return emailMessage;
        }

        private TrackingSettings CreateTrackingSettings()
        {
            return new TrackingSettings
            {
                ClickTracking = new ClickTracking
                {
                    Enable = false
                }
            };
        }

        private EmailAddress BuildEmailAddress(string emailAddress, string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return new EmailAddress(emailAddress);
            }

            return new EmailAddress(emailAddress, name);
        }

        private void AddSubstitutionsToEmailMessage(
            SendGridMessage emailMessage,
            IDictionary<string, string> substitutions)
        {
            if (substitutions != null)
            {
                var substitutionsDict = new Dictionary<string, string>(substitutions);
                emailMessage.AddSubstitutions(substitutionsDict);
            }
        }
    }
}
