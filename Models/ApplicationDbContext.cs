using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    // контекст БД приложения, наследуется от контекста БД
    public class ApplicationDbContext : DbContext
    {
        // конструктор, необходимо передать БД, контекст приложения в базовый класс
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // добавление книг в контекст приложения БД
        public DbSet<Book> Books { get; set; }
    }
}
