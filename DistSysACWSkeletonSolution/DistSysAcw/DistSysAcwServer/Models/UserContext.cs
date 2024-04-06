using Microsoft.EntityFrameworkCore;

namespace DistSysAcwServer.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base() { }

        public DbSet<User> Users { get; set; }



        //TODO: Task13
        public DbSet<Log> Logs { get; set; }

       public DbSet<LogArchive> LogArchives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Logs)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DistSysAcw;");
        }
    }
}