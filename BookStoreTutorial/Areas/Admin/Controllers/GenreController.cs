using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreTutorial.Areas.Admin.Models;
using BookStoreTutorial.Models.DataLayer;
using BookStoreTutorial.Models.DataLayer.Repositories;
using BookStoreTutorial.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTutorial.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class GenreController : Controller
    {
        private Repository<Genre> data { get; set; }

        public GenreController(BookstoreContext context) => data = new Repository<Genre>(context);

        public IActionResult Index()
        {
            var search = new SearchData(TempData);
            search.Clear();

            var genres = data.List(new QueryOptions<Genre>
            {
                OrderBy = g => g.Name
            });

            return View(genres);
        }

        private RedirectToActionResult GoToBookSearchResults(string id)
        {
            var search = new SearchData(TempData)
            {
                SearchTerm = id,
                Type = "genre"
            };

            return RedirectToAction("Search", "Book");
        }

        public RedirectToActionResult ViewBooks(string id) => GoToBookSearchResults(id);

        [HttpGet]
        public ViewResult Add() => View("Genre", new Genre());

        [HttpPost]
        public IActionResult Add(Genre genre)
        {
            var validate = new Validate(TempData);

            if (!validate.IsGenreChecked)
            {
                validate.CheckGenre(genre.GenreId, data);

                if (!validate.IsValid)
                {
                    ModelState.AddModelError(nameof(genre.GenreId), validate.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                data.Insert(genre);
                data.Save();
                validate.ClearGenre();

                TempData["message"] = $"{genre.Name} was added to the database";
                return RedirectToAction("Index");
            }
            else
            {
                return View("Genre", genre);
            }
        }

        [HttpGet]
        public ViewResult Edit(string id) => View("Genre", data.Get(id));

        [HttpPost]
        public IActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                data.Update(genre);
                data.Save();

                TempData["message"] = $"{genre.Name} was updated";
                return RedirectToAction("Index");
            }
            else
            {
                return View("Genre", genre);
            }
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var genre = data.Get(new QueryOptions<Genre>
            {
                Includes = "Books",
                Where = g => g.GenreId == id
            });

            if (genre.Books.Count > 0)
            {
                TempData["message"] = $"Can't delete genre {genre.Name} because the genre has associated books";
                return GoToBookSearchResults(id);
            }
            else
            {
                return View("Author", genre);
            }
        }

        [HttpPost]
        public RedirectToActionResult Delete(Genre genre)
        {
            data.Delete(genre);
            data.Save();

            TempData["message"] = $"This author {genre.Name} was deleted";
            return RedirectToAction("Index");
        }
    }
}
