using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace passionP.Models
{
    public class RetailerProduct
    {
        public int Price { get; set; }

        [Key, Column(Order = 2), ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Key, Column(Order = 1), ForeignKey("Retailer")]
        public int RetailerID { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}