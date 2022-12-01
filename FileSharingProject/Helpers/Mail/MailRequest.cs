using System;
namespace FileSharingProject.Helpers.Mail
{
	public class MailRequest
	{

        public string Email { get; set; } 
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Files { get; set; } = null;

    }
}

