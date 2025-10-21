using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace DatingApp.Data
{
    public class ProiectColectivContext : DbContext
    {
        public ProiectColectivContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
