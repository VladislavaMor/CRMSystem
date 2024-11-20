using Microsoft.EntityFrameworkCore;
using CRM_Helper;
using CRM_API.Models;

namespace CRM_API.Data
{
    public class WebApiDBContext : DbContext
    {
        public WebApiDBContext(DbContextOptions<WebApiDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Appeal> Appeals { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Appeal>().HasData(new Appeal[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Сергей",
                    Description="Хочу обсудить с вами детали проекта",
                    EMail="serg@yandex.ru",
                    Status = AppealStatus.Received,
                    Created = new(2023,5,12)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Марк",
                    Description="Расскажите про сервисы подробнее",
                    EMail="mark1989@gmail.com",
                    Status = AppealStatus.Received,
                    Created = new(2025,12,09)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "А как написать вам в поддержку?",
                    Description="Ольга",
                    EMail="olga@yandex.ru",
                    Status = AppealStatus.Completed,
                    Created = new(2024,12,18)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Иван",
                    Description="Хочу записаться",
                    EMail="ivan@yandex.ru",
                    Status = AppealStatus.Completed,
                    Created = DateTime.Now
                }

            });

            modelBuilder.Entity<Project>().HasData(new Project[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Новый проект на языке С#",
                    Description="Мы применили последние новшества языка и фреймворка для создания программы мечты!",
                    ImageName= "97EF6ADF-C3F3-4051-8681-44D0DD046B4B",
                    Created = DateTime.Now
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Игра Sims 5",
                    Description="Постройте жизнь мечты в новой версии всеми любимой игры.",
                    ImageName = "E6E9CF54-60B8-4BC1-9530-2DD888F1545D",
                    Created = DateTime.Now
                }
            });

            modelBuilder.Entity<Blog>().HasData(new Blog[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Презентация компании Tesla",
                    Description="Компания Tesla и её руководитель Илон Маск представили новые беспилотные транспортные решения. Это роботакси Cybercab и микроавтобус Robovan. Презентация прошла в рамках мероприятия We, Robot на студии Warner Bros. в калифорнийском городе Бербанке.",
                    ImageName="C374DCF4-C397-4827-9DE4-1BFBA3096D57",
                    Created = DateTime.Now
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Новые вакансии в геймдеве — в России и других странах",
                    Description="Обновлённая подборка актуальных вакансий в игровой индустрии.",
                    ImageName= "F8BE18A3-DB85-4172-A9DE-4A0F83D8DC04",
                    Created = DateTime.Now
                }
            });

            modelBuilder.Entity<Service>().HasData(new Service[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Сервис развития навыков совместно с ИИ",
                    Description="Обучайтесь, развивайтесь, опережайте.",
                    Created = DateTime.Now
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Сервис подбора вакансий",
                    Description="Получите помощь в нахождении своего места под солнцем",
                    Created = DateTime.Now
                }
           });

            modelBuilder.Entity<UserAccount>().HasData(new UserAccount[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Login = "admin",
                    Password= "admin",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Login = "user",
                    Password= "user",
                }
            });
        }
    }
}
