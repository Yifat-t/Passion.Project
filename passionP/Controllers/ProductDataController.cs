using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using passionP.Models;
using System.Diagnostics;


namespace passionP.Controllers
{
    public class ProductDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        /// <summary>
        /// Returns list of all products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all products in the database, including their associated id and brand name and if it's discontinued.
        /// </returns>
        /// /// <example>
        // GET: api/ProductData/ListProducts
        /// </example>


        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProducts()
        {
            List<Product> Products = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(a => ProductDtos.Add(new ProductDto(){
                ProductID = a.ProductID,
                ProductName = a.ProductName,
                Discontinued = a.Discontinued,
                BrandName = a.Brand.BrandName

            }));
            return Ok(ProductDtos);
        }


        /// <summary>
        /// Gathers information about all products related to a particular brand ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all products in the database, including their associated brand name and discontinued status that matched with a particular brand ID.
        /// </returns>
        /// <param name="id">Brand ID</param>
        /// <example>
        /// GET: api/ProductData/ListProductsForBrand/3
        /// </example>
        /// 

        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProductsForBrand(int id)
        {
            List<Product> Products = db.Products.Where(a => a.BrandID == id).ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(a => ProductDtos.Add(new ProductDto()
            {
                ProductID = a.ProductID,
                ProductName = a.ProductName,
                Discontinued = a.Discontinued,
                BrandName = a.Brand.BrandName

            }));

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Gathers information about products related to a particular retailer
        /// </summary>
        /// <returns>
        /// Header: 200 (OK)
        /// CONTENT: all products in the database, including their associated brand name, discontinued status and product name that match to a particular retailer id
        /// </returns>
        /// <param name="id">Retailer ID.</param>
        /// <example>
        /// GET: api/ProductData/ListProductsForRetailer/1
        /// </example>
        /// 
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProductsForRetailer(int id)
        {
            //all products that have retailers which match with our ID
            List<Product> Products = db.Products.Where(
                a => a.Retailers.Any(
                    r => r.RetailerID == id
                )).ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(a => ProductDtos.Add(new ProductDto()
            {
                ProductID = a.ProductID,
                ProductName = a.ProductName,
                Discontinued = a.Discontinued,
                BrandName = a.Brand.BrandName
            }));

            return Ok(ProductDtos);
        }


        /// <summary>
        /// Associates a particular retailer with a particular product
        /// </summary>
        /// <param name="productid">The product ID primary key</param>
        /// <param name="retailerid">The retailer ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/ProductData/AssociateProductWithRetailer/3/1
        /// </example>
        [HttpPost]
        [Route("api/ProductData/AssociateProductWithRetailer/{productid}/{retailerid}")]
        public IHttpActionResult AssociateProductWithRetailer(int productid, int retailerid)
        {

            Product SelectedProduct = db.Products.Include(a => a.Retailers).Where(a => a.ProductID == productid).FirstOrDefault();
            Retailer SelectedRetailer = db.Retailers.Find(retailerid);

            if (SelectedProduct == null || SelectedRetailer == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input product id is: " + productid);
            Debug.WriteLine("selected product name is: " + SelectedProduct.ProductName);
            Debug.WriteLine("input retailer id is: " + retailerid);
            Debug.WriteLine("selected retailer name is: " + SelectedRetailer.RetailerName);


            SelectedProduct.Retailers.Add(SelectedRetailer);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular retailer and a particular product
        /// </summary>
        /// <param name="productid">The product ID primary key</param>
        /// <param name="retailerid">The retailer ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/ProductData/UnAssociateProductWithRetailer/2/1
        /// </example>
        [HttpPost]
        [Route("api/ProductData/UnAssociateProductWithRetailer/{productid}/{retailerid}")]
        
        public IHttpActionResult UnAssociateProductWithRetailer(int productid, int retailerid)
        {

            Product SelectedProduct = db.Products.Include(a => a.Retailers).Where(a => a.ProductID == productid).FirstOrDefault();
            Retailer SelectedRetailer = db.Retailers.Find(retailerid);

            if (SelectedProduct == null || SelectedRetailer == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input product id is: " + productid);
            Debug.WriteLine("selected product name is: " + SelectedProduct.ProductName);
            Debug.WriteLine("input retailer id is: " + retailerid);
            Debug.WriteLine("selected retailer name is: " + SelectedRetailer.RetailerName);


            SelectedProduct.Retailers.Remove(SelectedRetailer);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all animals in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A product in the system matching up to the product ID primary key, showing brand name , discontinued status, Product name and Product id
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the product</param>
        /// <example>
        /// GET: api/ProductData/FindProduct/5
        /// </example>


        [ResponseType(typeof(ProductDto))]
        [HttpGet]
        public IHttpActionResult FindProduct(int id)
        {
            Product Product = db.Products.Find(id);
                
            if (Product == null)
            {
                return NotFound();
            }

            ProductDto ProductDto = new ProductDto()
            {
                ProductID = Product.ProductID,
                ProductName = Product.ProductName,
                Discontinued = Product.Discontinued,
                BrandName = Product.Brand.BrandName

            };

            return Ok(ProductDto);
        }

        /// <summary>
        /// Updates a particular product in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Product ID primary key</param>
        /// <param name="product">JSON FORM DATA of a product</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/3
        /// FORM DATA: Product JSON Object
        /// </example>


        
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a product to the system
        /// </summary>
        /// <param name="product">JSON FORM DATA of a product</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Product ID, Product Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/AddProduct
        /// FORM DATA: Product JSON Object
        /// </example>


     
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        /// <summary>
        /// Deletes a product from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the product</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/3
        /// FORM DATA: (empty)
        /// </example>


        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}