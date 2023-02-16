using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileSharingProject.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
      
        public AppDbContext( DbContextOptions options):base(options)
        {
        }
        public DbSet<Upload> Uploads { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
    }
}
