using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Models
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }


        // Seeding of default roles and admin account
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
                );

            var defaultAdminUser = new IdentityUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Id = Guid.NewGuid().ToString(),
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Admin123456789")
            };

            builder.Entity<IdentityUser>().HasData(defaultAdminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = defaultAdminUser.Id,
                });
        }
    }
}
