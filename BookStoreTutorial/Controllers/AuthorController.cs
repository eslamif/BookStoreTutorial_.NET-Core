using System;
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
    public class AuthorController : Controller
    {
        private Repository<Author> Data { get; set; }

        public AuthorController(BookstoreContext context) => Data = new Repository<Author>(context);

        public IActionResult Index() => RedirectToAction("List");

        public ViewResult List(GridDTO values)
        {
            string defaultSort = nameof(Author.LastName);
            var builder = new GridBuilder(HttpContext.Session, values, defaultSort);
            builder.SaveRouteSegment();

            var options = new QueryOptions<Author>
            {
                Includes = "BookAuthors.Book",
                PageNumber = builder.CurrentRoute.PageNumber,
                PageSize = builder.CurrentRoute.PageSize,
                OrderByDirection = builder.CurrentRoute.SortDirection
            };

            if (builder.CurrentRoute.SortField.EqualNoCase(defaultSort))
            {
                options.OrderBy = a => a.LastName;
            }
            else
            {
                options.OrderBy = a => a.FirstName;
            }

            var vm = new AuthorListViewModel
            {
                Authors = Data.List(options),
                CurrentRoute = builder.CurrentRoute,
                TotalPages = builder.GetTotalPages(Data.Count)
            };

            return View(vm);
        }

        public ViewResult Details(int id)
        {
            var author = Data.Get(new QueryOptions<Author>
            {
                Includes = "BookAuthors.Book",
                Where = a => a.AuthorId == id
            });

            return View(author);
        }
    }
}
