using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace FileSharingProject.Helpers.Mail
{
	public class MailHelper:IMailHelper
	{
       

        public async Task<bool> SendMail(MailRequest model)
        {

            MailSettings settings = GetMailSettings();
            var msg = new MimeMessage
            {
                Sender = MailboxAddress.Parse(settings.FromEmail),
                Subject=model.Subject,
            };
            msg.To.Add(MailboxAddress.Parse(model.ToEmail));

            var builder = new BodyBuilder();
            builder.HtmlBody = model.Body;
            msg.Body = builder.ToMessageBody();

            msg.From.Add(new MailboxAddress(settings.DisplayName, settings.FromEmail));

               
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(settings.Host, int.Parse(settings.Port),MailKit.Security.SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(settings.FromEmail, settings.Password);
            await smtpClient.SendAsync(msg);
            await smtpClient.DisconnectAsync(true);
            return true;
        }

        private MailSettings GetMailSettings()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var x = config.GetSection("MailSettings").Get<Dictionary<string, object>>();
            string json = System.Text.Json.JsonSerializer.Serialize(x);
             MailSettings settings = System.Text.Json.JsonSerializer.Deserialize<MailSettings>(json)!;
            return settings;
        }

    }
}

