using System;
namespace FileSharingProject.Helpers.Mail
{
	public interface IMailHelper
	{
		public Task<bool> SendMail(MailRequest mailRequest);
	}
}

