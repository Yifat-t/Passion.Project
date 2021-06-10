using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace passionP.Models
{
    public class Review

    {
        [Key]
        public int ReviewID { get; set; }

        public string ReviewDesc { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

    }
    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public string ReviewDesc { get; set; }
    }

}