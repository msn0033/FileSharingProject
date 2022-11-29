using System;
using System.Net.Mail;

namespace FileSharingProject.Helpers.Mail
{
	public class MailHelper:IMailHelper
	{
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }
	
        public void SendMail(InputEmailMessage model)
        {

            try
            {
                string? host = _config.GetValue<string>("Mail:Host");
                int port = _config.GetValue<int>("Mail:Port");
                string? from = _config.GetValue<string>("Mail:From");
                string? Sender = _config.GetValue<string>("Mail:Sender");
                string? pwd = _config.GetValue<string>("Mail:Pwd");




                using (SmtpClient smtpClient = new SmtpClient(host,port))
                {
                    var msg = new MailMessage();
                    msg.To.Add(model.Email);
                    msg.Body = model.Body;
                    msg.Subject = model.Subject;
                    msg.From = new MailAddress(from,Sender,System.Text.Encoding.UTF8);
                    msg.IsBodyHtml = true;

                    smtpClient.Credentials = new System.Net.NetworkCredential(from, pwd);

                    smtpClient.Send(msg);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

