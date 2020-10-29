using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DomainModels
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Range(0.0, 1000000.0, ErrorMessage = "Price must be between 0 and 1,000,000")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please select a genre")]
        public string GenreId { get; set; }     //foreign key

        public Genre Genre { get; set; }        //nav property
        public ICollection<BookAuthor> BookAuthors { get; set; }    //nav property

    }
}
