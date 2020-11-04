using BookStoreTutorial.Models.DomainModels;
using BookStoreTutorial.Models.ExtensionMethods;
using BookStoreTutorial.Models.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer
{
    /// <summary>
    /// Extends generic QueryOptions<Book> class to add a SortFilter() method
    /// </summary>
    public class BookQueryOptions : QueryOptions<Book>
    {
        public void SortFilter(BooksGridBuilder builder)
        {
            // Genre filter
            if (builder.IsFilterByGenre)
            {
                Where = b => b.GenreId == builder.CurrentRoute.GenreFilter;
            }

            //Price filter
            if (builder.IsFilterByPrice)
            {
                if (builder.CurrentRoute.PriceFilter == "under7")
                {
                    Where = b => b.Price < 7;
                }
            }
            else if (builder.CurrentRoute.PriceFilter == "7to14") 
            {
                Where = b => b.Price >= 7 && b.Price <= 14;
            }
            else
            {
                Where = b => b.Price > 14;
            }

            //Author filter
            if (builder.IsFilterByAuthor)
            {
                int id = builder.CurrentRoute.AuthorFilter.ToInt();

                if (id > 0)
                {
                    Where = b => b.BookAuthors.Any(ba => ba.Author.AuthorId == id);
                }
            }

            //Sort filter
            if (builder.IsSortByGenre)
            {
                OrderBy = b => b.Genre.Name;
            }
            else if(builder.IsSortByPrice)
            {
                OrderBy = b => b.Price;
            }
            else
            {
                OrderBy = b => b.Title;
            }
        }
    }
}
