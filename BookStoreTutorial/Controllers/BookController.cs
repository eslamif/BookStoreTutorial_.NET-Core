﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreTutorial.Models.DataLayer;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DataTransferObjects;
using BookStoreTutorial.Models.DomainModels;
using BookStoreTutorial.Models.ExtensionMethods;
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

        public ViewResult Details(int id)
        {
            var book = data.Books.Get(new QueryOptions<Book>
            {
                Includes = "BookAuthors.Author, Genre",
                Where = b => b.BookId == id
            });

            return View(book);
        }

        [HttpPost]
        public RedirectToActionResult Filter(string[] filter, bool clear = false)
        {
            var builder = new BooksGridBuilder(HttpContext.Session);

            if (clear)
            {
                builder.ClearFilterSegments();
            }
            else
            {
                var author = data.Authors.Get(filter[0].ToInt());
                builder.CurrentRoute.PageNumber = 1;
                builder.LoadFilterSegments(filter, author);
            }

            builder.SaveRouteSegment();
            return RedirectToAction("List", builder.CurrentRoute);
        }

        [HttpPost]
        public RedirectToActionResult PageSize(int pageSize)
        {
            var builder = new BooksGridBuilder(HttpContext.Session);
            builder.CurrentRoute.PageSize = pageSize;
            builder.SaveRouteSegment();

            return RedirectToAction("List", builder.CurrentRoute);
        }
    }
}
