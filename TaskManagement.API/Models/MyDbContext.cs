using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Models
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        // Entities
        public DbSet<MyTask> Tasks { get; set; }

        // Seeding of default roles and admin account
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
                );

            var defaultAdminUser = new User
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Id = Guid.NewGuid().ToString(),
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "Admin123456789")
            };

            builder.Entity<User>().HasData(defaultAdminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = defaultAdminUser.Id,
                });

            // Creates the many-to-many relationship between Users and Tasks
            builder.Entity<UserTask>().HasKey(ut => new { ut.UserId, ut.TaskId });
            builder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany(ut => ut.UserTasks)
                .HasForeignKey(ut => ut.UserId);

            builder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(ut => ut.UserTasks)
                .HasForeignKey(ut => ut.TaskId);

        }
    }
}
