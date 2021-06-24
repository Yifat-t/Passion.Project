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
        public ICollection<RetailerProduct> Products { get; set; }

        //data needed for keeping track of retailer images uploaded
        //images deposited into /Content/Images/Retailers/{id}.{extension} like png and jpg
        public bool RetailerHasPic { get; set; }

        public string PicExtension { get; set; }


    }
    public class RetailerDto
    {
        public int RetailerID { get; set; }
        public string RetailerName { get; set; }
    }

    public class RetailerProductDto
    {
        public int RetailerID { get; set; }

        public string RetailerName { get; set; }

        public int ProductID { get; set; }

        public int ProductPrice { get; set; }
    }
}