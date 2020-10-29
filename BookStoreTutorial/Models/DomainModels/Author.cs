﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DomainModels
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        [MaxLength(200)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public ICollection<BookAuthors> BookAuthors { get; set; }    //nav property
    }
}
