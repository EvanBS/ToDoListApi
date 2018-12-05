using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // ...
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<TodoItem>()
            .HasOne(s => s.User)
            .WithMany(g => g.TodoItems)
            .HasForeignKey(s => s.UserId);



            //modelBuilder.Entity<User>().HasMany(u => u.TodoItems).WithOne(t => t.User).IsRequired();


            //modelBuilder.UsePropertyAccessMode.items0()

            //modelBuilder.Entity<TodoItem>().HasRequired(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId);


            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };

            List<TodoItem> items0 = new List<TodoItem>()
            {
                new TodoItem() { Id = 1, Name = "Drink beer", IsComplete = false },
                new TodoItem() { Name = "Make kursach", IsComplete = true }
            };

            List<TodoItem> items1 = new List<TodoItem>()
            {
                new TodoItem() { Name = "Drink beer", IsComplete = false },
                new TodoItem() { Name = "Make kursach", IsComplete = true }
            };

            List<TodoItem> items2 = new List<TodoItem>()
            {
                new TodoItem() { Name = "Make test taks", IsComplete = false },
                new TodoItem() { Name = "Call mom", IsComplete = true }
            };

            List<TodoItem> items3 = new List<TodoItem>()
            {
                new TodoItem() { Name = "Be smart", IsComplete = false },
                new TodoItem() { Name = "Buy PS4", IsComplete = false }
            };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id, TodoItems = items0 };
            User user1 = new User { Id = 2, Email = "mail2@gmail.com", Password = "qwerty", RoleId = 2, TodoItems = items1 };
            User user2 = new User { Id = 3, Email = "mail3@gmail.com", Password = "hardpass", RoleId = 2, TodoItems = items2 };
            User user3 = new User { Id = 4, Email = "mail4@gmail.com", Password = "veryhardpass", RoleId = 2, TodoItems = items3 };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<TodoItem>().HasData(new TodoItem[] { new TodoItem() { Id = 1, Name = "Drink beer", IsComplete = false, UserId = 1 } });
            modelBuilder.Entity<User>().HasData(new User[] { new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id } });


        }
    }
}
