using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;
using DatingApp.Domain.Entities;

namespace DatingApp.Data
{
    public class ProiectColectivContext : DbContext
    {
        public ProiectColectivContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
