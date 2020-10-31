using BookStoreTutorial.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer.Repositories {
    public interface IBookStoreUnitOfWork {
        Repository<Book> Books { get; }
        Repository<Author> Authors { get; }
        Repository<BookAuthor> BookAuthors { get; }
        Repository<Genre> Genres { get; }

        void DeleteCurrentBookAuthors(Book book);
        void AddNewBookAuthors(Book book, int[] authorIds);
        void Save();
    }
}
