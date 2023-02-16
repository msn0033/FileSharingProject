using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FileSharingProject.Data
{
	public class Contact
	{
		public string Id { get; private set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
		public string Email { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public string ?UserId { get; set; }
		public DateTime DateSend { get; set; } = DateTime.UtcNow;
		public bool Open { get; set; }

		[ForeignKey(nameof(UserId))]
		public ApplicationUser? Users { get; set; } = null;//nevagate

	}
}

