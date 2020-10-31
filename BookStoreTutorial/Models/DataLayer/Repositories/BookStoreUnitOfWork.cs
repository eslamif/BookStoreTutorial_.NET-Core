using BookStoreTutorial.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer.Repositories {
    public class BookStoreUnitOfWork : IBookStoreUnitOfWork {
        private BookstoreContext context;

        public BookStoreUnitOfWork(BookstoreContext context) {
            this.context = context;
        }

        private Repository<Book> bookData;
        public Repository<Book> Books {
            get {
                if (bookData == null) {
                    bookData = new Repository<Book>(context);
                }
                return bookData;
            }
        }

        private Repository<Author> authorData;
        public Repository<Author> Authors {
            get {
                if (authorData == null) {
                    authorData = new Repository<Author>(context);
                }
                return authorData;
            }
        }

        private Repository<BookAuthor> bookAuthorData;
        public Repository<BookAuthor> BookAuthors {
            get {
                if (bookAuthorData == null) {
                    bookAuthorData = new Repository<BookAuthor>(context);
                }
                return bookAuthorData;
            }
        }

        private Repository<Genre> genreData;
        public Repository<Genre> Genres {
            get {
                if (genreData == null) {
                    genreData = new Repository<Genre>(context);
                }
                return genreData;
            }
        }

        public void AddNewBookAuthors(Book book, int[] authorIds) {
            book.BookAuthors = authorIds.Select(i => new BookAuthor { Book = book, AuthorId = i }).ToList();
        }

        public void DeleteCurrentBookAuthors(Book book) {
            var currentAuthors = BookAuthors.List(new QueryOptions<BookAuthor> {
                Where = ba => ba.BookId == book.BookId
            }); ;
        }

        public void Save() => context.SaveChanges();
    }
}
