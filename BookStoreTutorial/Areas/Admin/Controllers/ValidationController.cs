using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using BookStoreTutorial.Areas.Admin.Models;
using BookStoreTutorial.Models.DataLayer;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
        private Repository<Author> authorData { get; set; }
        private Repository<Genre> genreData { get; set; }

        public ValidationController(BookstoreContext context)
        {
            authorData = new Repository<Author>(context);
            genreData = new Repository<Genre>(context);
        }

        public JsonResult CheckGenre(string genreId)
        {
            var validate = new Validate(TempData);
            validate.CheckGenre(genreId, genreData);

            if (validate.IsValid)
            {
                validate.MarkGenreChecked();
                return Json(true);
            }
            else
            {
                return Json(validate.ErrorMessage);
            }
        }

        public JsonResult CheckAuthor(string firstName, string lastName, string operation)
        {
            var validate = new Validate(TempData);
            validate.CheckAuthor(firstName, lastName, operation, authorData);

            if (validate.IsValid)
            {
                validate.MarkAuthorChecked();
                return Json(true);
            }
            else
            {
                return Json(validate.ErrorMessage);
            }
        }
    }
}
