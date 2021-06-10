using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionP.Models.ViewModels
{
    public class DetailsRetailer
    {

        public RetailerDto SelectedRetailer { get; set; }

        //all of the product that are sold with this Retailer

        public IEnumerable<ProductDto> SoldProducts { get; set; }
    }
}