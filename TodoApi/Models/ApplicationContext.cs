using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TodoItem> TodoItemsDb { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User user1 = new User { Id = 2, Email = "mail2@gmail.com", Password = "qwerty", RoleId = 2 };
            User user2 = new User { Id = 3, Email = "mail3@gmail.com", Password = "hardpass", RoleId = 2 };
            User user3 = new User { Id = 4, Email = "mail4@gmail.com", Password = "veryhardpass", RoleId = 2 };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, user1, user2, user3 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
