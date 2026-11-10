using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Speurzoekers.Common.Config;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Helpers;
using Speurzoekers.Service.Services.Dto;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Speurzoekers.Service.Services
{
    public interface IEmailService
    {
        Task SendEmailBeeInternationalMessage(string email, ContactEmailDto contactEmailDto);
        Task SendEmailConfirmAccount(string email, string token);
        Task SendEmailPasswordReset(string email, string token);

        Task TestEmailBeeInternationalMessage(ContactEmailDto contactEmailDto);
        Task TestEmailConfirmAccount(string token);
        Task TestEmailPasswordReset(string token);
    }

    public class EmailService : IEmailService
    {
        private delegate void AppendLineDel(string line = "");

        private const string TestEmail = "Test@test.com";        

        private readonly DomainOptions _domainOptions;
        private readonly AuthenticationOptions _authOptions;

        public EmailService(IOptions<DomainOptions> domainOptions, IOptions<AuthenticationOptions> authOptions)
        {
            _domainOptions = domainOptions.Value;
            _authOptions = authOptions.Value;
        }

        public Task SendEmailBeeInternationalMessage(string email, ContactEmailDto contactEmailDto)
        {
            MailDto mail = CreateBeeInternationalMessage(email, contactEmailDto);

            return SendSendGridMail(mail);
        }

        public Task TestEmailBeeInternationalMessage(ContactEmailDto contactEmailDto)
        {
            contactEmailDto.CompanyName = contactEmailDto.CompanyName ?? "ACME";
            contactEmailDto.EmailAddress = contactEmailDto.EmailAddress ?? TestEmail;
            contactEmailDto.Name = contactEmailDto.Name ?? "John Doe";
            contactEmailDto.PhoneNumber = contactEmailDto.PhoneNumber ?? "00-DONTCALL";
            contactEmailDto.Question = contactEmailDto.Question ?? "Is it working?";
            contactEmailDto.Subject = contactEmailDto.Subject ?? "Just testing";

            MailDto mail = CreateBeeInternationalMessage(TestEmail, contactEmailDto);

            return SendTestMail(mail);
        }

        private MailDto CreateBeeInternationalMessage(string email, ContactEmailDto contactEmailDto)
        {
            StringBuilder htmlBuilder = new StringBuilder(), plainBuilder = new StringBuilder();
            string subject = "Informatieverzoek van de Speurzoekers App!";

            CreateHtml(htmlBuilder, subject, (b) => CreateBeeInternationalMessageText(b, contactEmailDto, true));
            CreateBeeInternationalMessageText(plainBuilder, contactEmailDto, false);

            return new MailDto
            {
                Subject = subject,
                To = email,
                Body = htmlBuilder.ToString(),
                PlainText = plainBuilder.ToString()
            };
        }

        private void CreateBeeInternationalMessageText(
            StringBuilder builder,
            ContactEmailDto contactEmailDto,
            bool isHtml)
        {
            AppendLineDel appendLine = GetAppendLine(builder, isHtml);

            if (isHtml)
            {
                builder.Append("<b>");
            }

            builder.Append($"Message from {contactEmailDto.Name},");

            if (isHtml)
            {
                appendLine("</b>");
            }
            else
            {
                appendLine();
            }

            appendLine();
            appendLine($"Company name: {contactEmailDto.CompanyName}");
            appendLine($"Email address: {contactEmailDto.EmailAddress}");
            appendLine($"Phonenumber: {contactEmailDto.PhoneNumber}");
            appendLine($"Subject: {contactEmailDto.Subject}");
            appendLine($"Question: {contactEmailDto.Question}");

            AddFooter(appendLine);
        }

        public Task SendEmailConfirmAccount(string email, string token)
        {
            MailDto mail = CreateConfirmAccount(email, token);

            return SendSendGridMail(mail);
        }

        public Task TestEmailConfirmAccount(string token)
        {
            MailDto mail = CreateConfirmAccount(TestEmail, token);

            return SendTestMail(mail);
        }

        private MailDto CreateConfirmAccount(string email, string token)
        {
            StringBuilder htmlBuilder = new StringBuilder(), plainBuilder = new StringBuilder();
            string subject = "Activeer je account bij De Speurzoekers in Actie app";
            
            CreateHtml(htmlBuilder, subject, (b) => CreateConfirmAccountText(b, email, token, true));
            CreateConfirmAccountText(plainBuilder, email, token, false);

            return new MailDto
            {
                Subject = subject,
                To = email,
                Body = htmlBuilder.ToString(),
                PlainText = plainBuilder.ToString()
            };
        }

        private void CreateConfirmAccountText(
            StringBuilder builder, string email, 
            string token, bool isHtml)
        {
            AppendLineDel appendLine = GetAppendLine(builder, isHtml);

            if (isHtml)
            {
                builder.Append("<b>");
            }

            string time = Helper.MinutesToString(_authOptions.MailTokenExpirationMinutes);
            builder.Append($"Bedankt voor je aanmelding bij {_domainOptions.AppName} app." +
                $" Via onderstaande link kun je je account bevestigen binnen {time}.");

            if (isHtml)
            {
                appendLine("</b>");
            }

            appendLine();

            string link = string.Format(_domainOptions.AppUrlAccountConfirmed, 
                HttpUtility.UrlEncode(email), HttpUtility.UrlEncode(token));

            AddLink(appendLine, link, "Klik hier om je account te bevestigen", isHtml);

            AddFooter(appendLine);
        }

        public Task SendEmailPasswordReset(string email, string token)
        {
            MailDto mail = CreatePasswordReset(email, token);

            return SendSendGridMail(mail);
        }

        public Task TestEmailPasswordReset(string token)
        {
            MailDto mail = CreatePasswordReset(TestEmail, token);

            return SendTestMail(mail);
        }

        private MailDto CreatePasswordReset(string email, string token)
        {
            StringBuilder htmlBuilder = new StringBuilder(), plainBuilder = new StringBuilder();
            string subject = "Je nieuwe wachtwoord instellen";
            
            CreateHtml(htmlBuilder, subject, (b) => CreatePasswordResetText(b, token, true));
            CreatePasswordResetText(plainBuilder, token, false);

            return new MailDto
            {
                Subject = subject,
                To = email,
                Body = htmlBuilder.ToString(),
                PlainText = plainBuilder.ToString()
            };
        }

        private void CreatePasswordResetText(StringBuilder builder, string token, bool isHtml)
        {
            AppendLineDel appendLine = GetAppendLine(builder, isHtml);

            if (isHtml)
            {
                builder.Append("<b>");
            }

            builder.Append("Je hebt aangegeven dat je je wachtwoord hebt vergeten." +
                " Via onderstaande link kan je een nieuw wachtwoord kiezen.");

            if (isHtml)
            {
                appendLine("</b>");
            }

            appendLine();

            string link = string.Format(_domainOptions.AppUrlResetPasswordConfirmed, HttpUtility.UrlEncode(token));

            AddLink(appendLine, link, "Klik hier om je wachtwoord opnieuw in te stellen", isHtml);

            AddFooter(appendLine);
        }

        private Task SendSendGridMail(MailDto mail)
        {
            SendGridClient client = new SendGridClient(_domainOptions.SendGridApiKey);
            List<EmailAddress> tos = new List<EmailAddress>
            {
                new EmailAddress(mail.To)
            };
            SendGridMessage msg = new SendGridMessage
            {
                From = new EmailAddress(_domainOptions.MailAddressFrom, _domainOptions.MailAddressNameFrom),
                Subject = mail.Subject,
                HtmlContent = mail.Body,
                PlainTextContent = mail.PlainText
            };

            msg.AddTos(tos);

            return client.SendEmailAsync(msg);
        }

        private Task SendTestMail(MailDto mail)
        {
            // Use Smtp4Dev (V2) or similar to receive
            SmtpClient client = new SmtpClient("localhost", 25);
            MailMessage msg = new MailMessage
            {
                Subject = mail.Subject,
                From = new MailAddress(_domainOptions.MailAddressFrom, _domainOptions.MailAddressNameFrom),
                Body = mail.Body,
                IsBodyHtml = true,
            };
            msg.To.Add(mail.To);
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(mail.Body,
                new ContentType(MediaTypeNames.Text.Html)));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(mail.PlainText,
                new ContentType(MediaTypeNames.Text.Plain)));

            return client.SendMailAsync(msg);
        }

        private void CreateHtml(StringBuilder builder, string subject, Action<StringBuilder> bodyBuilder)
        {
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<title>");
            builder.AppendLine(subject);
            builder.AppendLine("</title>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");
            bodyBuilder(builder);
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
        }

        private AppendLineDel GetAppendLine(StringBuilder builder, bool isHtml)
        {
            if (isHtml)
            {
                return (l) => AppendHtmlLine(builder, l);
            }
            // else
            return (l) => AppendLine(builder, l);
        }

        private void AppendHtmlLine(StringBuilder builder, string line)
        {
            builder.Append(line);
            builder.AppendLine("<br>");
        }

        private void AppendLine(StringBuilder builder, string line)
        {
            builder.AppendLine(line);
        }

        private void AddLink(AppendLineDel appendLine, string url, string htmlText, bool isHtml)
        {
            if (isHtml)
            {
                appendLine($"<a href={url}>{htmlText}</a>");
            }
            else
            {
                appendLine(url);
            }
        }

        private void AddFooter(AppendLineDel appendLine)
        {
            appendLine();
            appendLine();

            foreach (string line in _domainOptions.MailFooter)
            {
                appendLine(line);
            }
        }
    }
}
