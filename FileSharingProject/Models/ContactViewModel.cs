using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharingProject.Models
{
	public class ContactViewModel
	{
		
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? UserId { get; set; }
   
  

    }
}

