using BookStoreTutorial.Models.DataTransferObjects;
using BookStoreTutorial.Models.DomainModels;
using BookStoreTutorial.Models.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.Grid {
    // inherits the general purpose GridBuilder class and adds application-specific 
    // methods for loading and clearing filter route segments in route dictionary.
    // Also adds application-specific Boolean flags for sorting and filtering. 
    public class BooksGridBuilder : GridBuilder {
        // this constructor gets current route data from session
        public BooksGridBuilder(ISession session) : base(session) { }

        // this constructor stores filtering route segments, as well as
        // the paging and sorting route segments stored by the base constructor
        public BooksGridBuilder(ISession session, BookGridDTO bookGrid, string defaultSortFilter) : base(session, bookGrid, defaultSortFilter) {
            // store filter route segments - add filter prefixes if this is initial load
            // of page with default values rather than route values (route values have prefix)
            bool isInitial = bookGrid.Genre.IndexOf(FilterPrefix.Genre) == -1;
            Routes.AuthorFilter = (isInitial) ? FilterPrefix.Author + bookGrid.Author : bookGrid.Author;
            Routes.GenreFilter = (isInitial) ? FilterPrefix.Genre + bookGrid.Genre : bookGrid.Genre;
            Routes.PriceFilter = (isInitial) ? FilterPrefix.Price + bookGrid.Price : bookGrid.Price;

            SaveRouteSegment();
        }

        // load new filter route segments contained in a string array - add filter prefix 
        // to each one. if filtering by author (rather than just 'all'), add author slug 
        public void LoadFilterSegments(string[] filter, Author author) {
            if (author == null)
                Routes.AuthorFilter = FilterPrefix.Author + filter[0];
            else
                Routes.AuthorFilter = FilterPrefix.Author + filter[0] + "-" + author.FullName.Slug();

            Routes.GenreFilter = FilterPrefix.Genre + filter[1];
            Routes.PriceFilter = FilterPrefix.Price + filter[2];
        }

        public void ClearFilterSegments() => Routes.ClearFilters();

        //filter flags
        string def = BookGridDTO.DefaultFilter;
        public bool IsFilterByAuthor => Routes.AuthorFilter != def;
        public bool IsFilterByGenre => Routes.GenreFilter != def;
        public bool IsFilterByPrice => Routes.PriceFilter != def;

        //sort flags
        public bool IsSortByGenre => Routes.SortField.EqualNoCase(nameof(Genre));
        public bool IsSortByPrice => Routes.SortField.EqualNoCase(nameof(Book.Price));
    }
}
