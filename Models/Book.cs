using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Models
{
    // Модель книги
    public class Book
    {
        // идентификатор, аннотация ключ
        [Key]
        public int Id { get; set; }

        // обязательное свойство
        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }

    }
}
