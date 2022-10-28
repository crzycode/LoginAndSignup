using LoginAndSignup.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAndSignup.Context
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
