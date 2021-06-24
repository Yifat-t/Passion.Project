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

        //data needed for keeping track of brand images uploaded
        //images deposited into /Content/Images/Brands/{id}.{extension}

        public bool BrandHasPic { get; set; }

        public string PicExtension { get; set; }


    }
    public class BrandDto
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

    }


}