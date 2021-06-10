using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace passionP.Models
{
    public class Retailer

    {
        [Key]
        public int RetailerID { get; set; }

        public string RetailerName { get; set; }

        //A retailer can have many products
        public ICollection<Product> Products { get; set; }


    }
    public class RetailerDto
    {
        public int RetailerID { get; set; }
        public string RetailerName { get; set; }
    }
}