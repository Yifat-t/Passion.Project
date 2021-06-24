using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionP.Models.ViewModels
{
    public class UpdateProduct
    {
        

        public ProductDto SelectedProduct { get; set; }

       
        //the assosiated brand with the update product.
        public IEnumerable<BrandDto> BrandOptions { get; set; }
        public IEnumerable<RetailerDto> AvailableRetailers { get; set; }


    }
}