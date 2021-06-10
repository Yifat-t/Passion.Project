using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace passionP.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }
        public bool Discontinued { get; set; }

        [ForeignKey("Brand")]
        public int BrandID { get; set; }
        public virtual Brand Brand { get; set; }

        //A product can have many reviews
        //A product can have many retailers
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Retailer> Retailers { get; set; }


    }
    
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }

        public bool Discontinued { get; set; }
    }

}
      