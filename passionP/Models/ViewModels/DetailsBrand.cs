using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionP.Models.ViewModels
{
    public class DetailsBrand
    {
       
        public BrandDto SelectedBrand { get; set; }


        //all of the related products to that particular brand
        public IEnumerable<ProductDto> RelatedProducts { get; set; }
    }
}