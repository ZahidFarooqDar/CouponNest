using CouponNestAPI.CouponNest.DM;
using CouponNestAPI.CouponNest.SM;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CouponNestAPI.CouponNest.DAL
{
    public class ApiDbContext : IdentityDbContext<AuthenticUserSM>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserDM> Users { get; set; }
        public DbSet<ApplicationUserDM> ApplicationUsers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CouponNext;Trusted_Connection=True;MultipleActiveResultSets=true");
            //optionsBuilder.UseSqlServer("Server=192.168.29.71;Database=HeroAPIDB;User Id=sa;Password=123@Reno;MultipleActiveResultSets=true;Encrypt=False;");
            base.OnConfiguring(optionsBuilder);
            //For deployment we use below method for database creation
            /*optionsBuilder.UseSqlServer("Server=192.168.29.71;Database=HeroDB;User Id=sa;Password=123@Reno;MultipleActiveResultSets=true;Encrypt=False;");
            base.OnConfiguring(optionsBuilder);*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure unique constraint on Email and UserName
            modelBuilder.Entity<UserDM>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserDM>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            modelBuilder.Entity<ApplicationUserDM>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ApplicationUserDM>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }

    }
}
