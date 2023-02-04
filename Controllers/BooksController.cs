using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        // контекст представления БД
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Book Book { get; set; }

        // контекст БД с помощью внедрения зависимостей Dependency Injection
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ссылка на представление
        public IActionResult Index()
        {
            return View();
        }

        // обновление/создание таблицы, id - необязательный параметр
        public IActionResult Upsert(int? id)
        {
            // инициализация новым Book
            Book = new Book();
            if (id == null)
            {
                // создание
                return View(Book);
            }
            // обновление
            // необходимо получить все даннные из книги и отобразить их для редактирования, точка входа по Id или значение по умолчанию
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
            {
                // в БД нет книг
                return NotFound();
            }
            // если книга есть, просмотр ожидает книгу независимо от создания или изменения
            return View(Book);
        }

        // создание таблицы
        // Http Post  и проверка от подделки, встроенная защита от атак, Book по привязке Binding, прямой доступ к объекту Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    // создание
                    _db.Books.Add(Book);
                }
                else
                {
                    // обновление на основе индентификатора
                    _db.Books.Update(Book);
                }
                // обновление БД
                _db.SaveChanges();
                // перенаправить на страницу MVC Index в BookController, снова отобразит весь список книг
                return RedirectToAction("Index");
            }
            return View();
        }

        // вызовы через API
        #region API Calls
        // чтобы вернуть json, api поддержку добавить в startup
        // асинхронные задачи с Entity Framework
        // получить все книги из БД в json
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        // удалить книгу из БД по id
        // для тэга удалить добави
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            // возвращает по id или значение по умолчанию, если элемент не найден
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            // сообщение об ошибке удаления
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
