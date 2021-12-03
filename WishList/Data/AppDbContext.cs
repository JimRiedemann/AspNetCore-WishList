
using Microsoft.EntityFrameworkCore;
using WishList.Models;

namespace WishList.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Wish> Wishes { get; set; }

        public virtual DbSet<Wisher> Wishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=NSL-PC1Q4VLN\Sql2019;Initial Catalog=WishList;Integrated Security=SSPI;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wish>().ToTable("Wishes");
            modelBuilder.Entity<Wisher>().ToTable("Wishers");
        }
    }
}

