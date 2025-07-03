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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Segnalazione>()
                .HasMany(s => s.MessaggiChat)
                .WithOne(m => m.Segnalazione)
                .HasForeignKey(m => m.SegnalazioneId);
        }

        public DbSet<Segnalazione> Segnalazioni { get; set; }
        public DbSet<MessaggioChat> ChatMessaggi { get; set; }
    }
}