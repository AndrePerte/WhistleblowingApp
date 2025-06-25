using Microsoft.EntityFrameworkCore;
using WhistleblowingApp.Models;

namespace WhistleblowingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Segnalazione> Segnalazioni { get; set; }
    }
}