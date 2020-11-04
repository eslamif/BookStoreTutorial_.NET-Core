using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookStoreTutorial.Models;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DomainModels;
using BookStoreTutorial.Models.DataLayer;

namespace BookStoreTutorial.Controllers
{
    public class HomeController : Controller
    {
        private Repository<Book> data { get; set; }

        public HomeController(BookstoreContext context) =>
            data = new Repository<Book>(context);

        public IActionResult Index()
        {
            //Display random book
            var random = data.Get(new QueryOptions<Book>
            {
                OrderBy = b => Guid.NewGuid()
            });

            return View(random);
        }

        public ContentResult Register()
        {
            return Content("Registration will be created later");
        }
    }
}
