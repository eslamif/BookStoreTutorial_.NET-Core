using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DomainModels
{
    public class Genre
    {
        [Required(ErrorMessage = "Please enter a genre ID.")]
        [StringLength(10, ErrorMessage = "Genre ID must be <= 10 characters.")]
        public string GenreId { get; set; }

        [Required(ErrorMessage = "Please enter a genre name.")]
        [StringLength(25, ErrorMessage = "Genre name must <= 25 characters.")]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }    //nav property
    }
}
