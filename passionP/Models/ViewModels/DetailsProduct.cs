using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionP.Models.ViewModels
{
    public class DetailsProduct
    {

        public ProductDto SelectedProduct { get; set; }

        //all of the Unvailable Retailers (Responsible Retailers) to that particular product
        public IEnumerable<RetailerProductDto> ResponsibleRetailers { get; set; }

        //all of the Available Retailers to that particular product
        public IEnumerable<RetailerDto> AvailableRetailers { get; set; }

    }
}