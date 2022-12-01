using System;

using System.Net.Mail;

namespace FileSharingProject.Helpers.Mail
{
	public class MailHelper:IMailHelper
	{

        public async Task< bool> SendMail(MailRequest model)
        {

            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                var x = config.GetSection("MailSettings").Get<Dictionary<string, object>>();
                string json = System.Text.Json.JsonSerializer.Serialize(x);
                MailSettings settings = System.Text.Json.JsonSerializer.Deserialize<MailSettings>(json)!;

                using (SmtpClient smtpClient = new SmtpClient(settings.Host, int.Parse(settings.Port)))
                {
                    var msg = new MailMessage();
                    msg.To.Add(model.Email);
                    msg.Body = model.Body;
                    msg.Subject = model.Subject;
                    msg.From = new MailAddress(settings.From, settings.Sender, System.Text.Encoding.UTF8);
                    msg.IsBodyHtml = true;

                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(settings.From, settings.Password);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await smtpClient.SendMailAsync(msg);    
                    return true;
                }
                return false;
            }
         
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

