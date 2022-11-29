using System;
namespace FileSharingProject.Helpers.Mail
{
	public interface IMailHelper
	{
		public void SendMail(InputEmailMessage inputEmailMessage);
	}
}

