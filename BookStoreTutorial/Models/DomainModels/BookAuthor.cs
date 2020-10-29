using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DomainModels
{
    /// <summary>
    /// Many to many relatipnship between Author and Book
    /// </summary>
    public class BookAuthor
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }      //nav property
        public Book Book { get; set; }          //nav property
    }
}
