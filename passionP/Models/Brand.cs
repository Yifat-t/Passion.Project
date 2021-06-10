using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace passionP.Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }

        public string BrandName { get; set; }
        public ICollection<Product> Products { get; set; }


    }
    public class BrandDto
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

    }


}