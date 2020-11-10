using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreTutorial.Areas.Admin.Models;
using BookStoreTutorial.Models.DataLayer;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace BookStoreTutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private BookStoreUnitOfWork data { get; set; }

        public BookController(BookstoreContext context) => data = new BookStoreUnitOfWork(context);

        public IActionResult Index()
        {
            var search = new SearchData(TempData);
            search.Clear();

            return View();
        }

        [HttpPost]
        public RedirectToActionResult Search(SearchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var search = new SearchData(TempData)
                {
                    SearchTerm = vm.SearchTerm,
                    Type = vm.Type
                };
         
                return RedirectToAction("Search");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ViewResult Search()
        {
            var search = new SearchData(TempData);
            
            if (search.HasSearchTerm)
            {
                var vm = new SearchViewModel
                {
                    SearchTerm = search.SearchTerm
                };

                var options = new QueryOptions<Book>
                {
                    Includes = "Genre, BookAuthors.Author"
                };

                if (search.IsBook)
                {
                    options.Where = b => b.Title.Contains(vm.SearchTerm);
                    vm.Header = $"Search results for the book title '{vm.SearchTerm}'";
                }

                if (search.IsAuthor)
                {
                    //Check for space
                    int index = vm.SearchTerm.LastIndexOf(' ');

                    if (index == -1)
                    {
                        options.Where = b => b.BookAuthors.Any(
                            ba => ba.Author.FirstName.Contains(vm.SearchTerm) ||
                            ba.Author.LastName.Contains(vm.SearchTerm));
                    }
                    else
                    {
                        string first = vm.SearchTerm.Substring(0, index);
                        string last = vm.SearchTerm.Substring(index + 1);

                        options.Where = b => b.BookAuthors.Any(
                            ba => ba.Author.FirstName.Contains(first) &&
                            ba.Author.LastName.Contains(last)
                        );
                    }

                    vm.Header = $"Search results for author '{vm.SearchTerm}'";
                }

                if (search.IsGenre)
                {
                    options.Where = b => b.GenreId.Contains(vm.SearchTerm);
                    vm.Header = $"Search results for the genre id '{vm.SearchTerm}'";
                }

                vm.Books = data.Books.List(options);
                return View("SearchResults", vm);
            }
            else
            {
                return View("Index");
            }
        } 

        [HttpGet]
        public ViewResult Add(int id) => GetBook(id, "Add");

        [HttpPost]
        public IActionResult Add(BookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                data.AddNewBookAuthors(vm.Book, vm.SelectedAuthors);
                data.Books.Insert(vm.Book);
                data.Save();

                TempData["message"] = $"{vm.Book.Title} was added to Books";
                return RedirectToAction("Index");
            }
            else
            {
                Load(vm, "Add");
                return View("Book", vm);
            }
        }

        [HttpGet]
        public ViewResult Edit(int id) => GetBook(id, "Edit");

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                data.DeleteCurrentBookAuthors(vm.Book);
                data.AddNewBookAuthors(vm.Book, vm.SelectedAuthors);
                data.Books.Update(vm.Book);
                data.Save();

                TempData["message"] = $"{vm.Book.Title} was updated";
                return RedirectToAction("Search");
            }
            else
            {
                Load(vm, "Edit");
                return RedirectToAction("Book", vm);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id) => GetBook(id, "Delete");

        [HttpPost]
        public IActionResult Delete(BookViewModel vm)
        {
            data.Books.Delete(vm.Book);
            data.Save();

            TempData["message"] = $"{vm.Book.Title} was deleted";
            return RedirectToAction("Search");
        }

        private void Load(BookViewModel vm, string operation, int? id = null)
        {
            if (Operation.IsAdd(operation))
            {
                vm.Book = new Book();
            }
            else
            {
                vm.Book = data.Books.Get(new QueryOptions<Book>
                {
                    Includes = "BookAuthors.Author, Genre",
                    Where = b => b.BookId == (id ?? vm.Book.BookId)
                });
            }

            vm.SelectedAuthors = vm.Book.BookAuthors?.Select(ba => ba.Author.AuthorId).ToArray();
            vm.Authors = data.Authors.List(new QueryOptions<Author> { OrderBy = a => a.FirstName });
            vm.Genres = data.Genres.List(new QueryOptions<Genre> { OrderBy = g => g.Name });
        }

        private ViewResult GetBook(int id, string operation)
        {
            var book = new BookViewModel();
            Load(book, operation, id);

            return View("Book", book);
        }
    }
}
