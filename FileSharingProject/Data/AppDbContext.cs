using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileSharingProject.Data
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
      
        public AppDbContext( DbContextOptions options):base(options)
        {
        }
        public DbSet<Uploads> Uploads { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
    }
}
