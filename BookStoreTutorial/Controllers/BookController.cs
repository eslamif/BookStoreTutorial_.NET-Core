using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreTutorial.Models.DataLayer;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DataTransferObjects;
using BookStoreTutorial.Models.DomainModels;
using BookStoreTutorial.Models.Grid;
using BookStoreTutorial.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTutorial.Controllers
{
    public class BookController : Controller
    {
        private BookStoreUnitOfWork data { get; set; }

        public BookController(BookstoreContext context) => data = new BookStoreUnitOfWork(context);

        public IActionResult Index() => RedirectToAction("List");

        public ViewResult List(BookGridDTO values)
        {
            var builder = new BooksGridBuilder(HttpContext.Session, values, defaultSortFilter: nameof(Book.Title));

            var options = new BookQueryOptions
            {
                Includes = "BookAuthors.Author, Genre",
                OrderByDirection = builder.CurrentRoute.SortDirection,
                PageNumber = builder.CurrentRoute.PageNumber,
                PageSize = builder.CurrentRoute.PageSize
            };

            options.SortFilter(builder);

            var vm = new BookListViewModel
            {
                Books = data.Books.List(options),
                Authors = data.Authors.List(new QueryOptions<Author> { OrderBy = a => a.FirstName }),
                Genres = data.Genres.List(new QueryOptions<Genre> { OrderBy = g => g.Name }),
                CurrentRoute = builder.CurrentRoute,
                TotalPages = builder.GetTotalPages(data.Books.Count)
            };

            return View(vm);
        }
    }
}
