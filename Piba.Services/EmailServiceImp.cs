﻿using Piba.Data.Dto;
using Piba.Services.Interfaces;
using System.Net.Mail;

namespace Piba.Services
{
    public class EmailServiceImp : EmailService
    {
        private readonly SmtpClientWrapper _smtpClientWrapper;
        private readonly EnvironmentVariables _environmentVariables;

        public EmailServiceImp(SmtpClientWrapper smtpPibaClient, EnvironmentVariables environmentVariables)
        {
            _smtpClientWrapper = smtpPibaClient;
            _environmentVariables = environmentVariables;
        }

        public void SendEmailToDeveloper(SendEmailDto dto)
        {

            var email = new MailMessage(_environmentVariables.FromEmail, _environmentVariables.DeveloperEmail)
            {
                Subject = dto.Subject,
                Body = dto.Body,
            };

            _smtpClientWrapper.Send(email);
        }

        public void SendEmailToDeveloper(SendEmailDto dto, byte[] attachment)
        {
            var email = new MailMessage(_environmentVariables.FromEmail, _environmentVariables.DeveloperEmail)
            {
                Subject = dto.Subject,
                Body = dto.Body
            };

            using var memoryStream = new MemoryStream(attachment);

            email.Attachments.Add(new (memoryStream, dto.FileName));

            _smtpClientWrapper.Send(email);
        }
    }
}
