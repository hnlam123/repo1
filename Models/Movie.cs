// =====COPYRIGHT=====
// aaaaaaaaaaaaaaabbbbbbbbbb
// =====COPYRIGHT=====
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Genre must be specified")]
        [StringLength(5,ErrorMessage = "dai qua roi")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Price Required")]
        [Range(1, 100, ErrorMessage = "Price must be between $1 and $100")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
    
}
