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
    public class CartController : Controller
    {
        private Repository<Book> Data { get; set; }

        public CartController(BookstoreContext context)
        {
            Data = new Repository<Book>(context);
        }

        public IActionResult Index()
        {
            Cart cart = GetCart();
            var builder = new BooksGridBuilder(HttpContext.Session);

            var vm = new CartViewModel
            {
                List = cart.List,
                Subtotal = cart.Subtotal,
                BookGridRoute = builder.CurrentRoute
            };

            return View(vm);
        }

        [HttpPost]
        public RedirectToActionResult Add(int id)
        {
            var book = Data.Get(new QueryOptions<Book>
            {
                Includes = "BookAuthors.Author, Genre",
                Where = b => b.BookId == id
            });

            if (book == null)
            {
                TempData["message"] = "Unable to add book to cart";
                return RedirectToAction("Index");
            }
            else
            {
                var dto = new BookDTO();
                dto.Load(book);
                
                CartItem item = new CartItem()
                {
                    Book = dto,
                    Quantity = 1
                };

                Cart cart = GetCart();
                cart.Add(item);
                cart.Save();

                TempData["message"] = $"{book.Title} was added to cart";
            }

            var builder = new BooksGridBuilder(HttpContext.Session);

            return RedirectToAction("List", "Book", builder.CurrentRoute);
        }

        private Cart GetCart()
        {
            var cart = new Cart(HttpContext);
            cart.Load(Data);
            return cart;
        }

        [HttpPost]
        public RedirectToActionResult Remove(int id)
        {
            Cart cart = GetCart();
            CartItem item = cart.GetById(id);
            cart.Remove(item);
            cart.Save();

            TempData["message"] = $"{item.Book.Title} was removed from the cart";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToActionResult Clear()
        {
            Cart cart = GetCart();
            cart.Clear();
            cart.Save();

            TempData["message"] = "Cart was cleared";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Cart cart = GetCart();
            CartItem item = cart.GetById(id);

            if (item == null)
            {
                TempData["message"] = "Unable to locate cart item";
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
        }

        [HttpPost]
        public RedirectToActionResult Edit(CartItem item)
        {
            Cart cart = GetCart();
            cart.Edit(item);
            cart.Save();

            TempData["message"] = $"{item.Book.Title} was updated";

            return RedirectToAction("Index");
        }

        public ViewResult Checkout() => View();
    }
}
