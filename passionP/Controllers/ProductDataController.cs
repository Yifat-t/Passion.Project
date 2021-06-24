using System;
using System.IO;
using System.Web;
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
        public IHttpActionResult ListProducts(string SearchKey = null)
        {
            List<Product> Products;

            if (SearchKey != null)
            {
                Products = db.Products.Where(p => p.ProductName.Contains(SearchKey)).ToList();
            } else
            {
                Products = db.Products.ToList();
            }
            
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(a => ProductDtos.Add(new ProductDto(){
                ProductID = a.ProductID,
                ProductName = a.ProductName,
                Discontinued = a.Discontinued,
                ProductHasPic = a.ProductHasPic,
                PicExtension = a.PicExtension,
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
        [Authorize]
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


            SelectedProduct.Retailers.Add(new RetailerProduct()
            {
                ProductID = productid,
                RetailerID = retailerid,
                Price = 0,
            });
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
        [Authorize]

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

            RetailerProduct rp = SelectedProduct.Retailers.Where(r => r.ProductID == productid && r.RetailerID == retailerid).First();

            if (rp != null)
            {
                SelectedProduct.Retailers.Remove(rp);
                db.SaveChanges();
            }

            return Ok();
        }

        /// <summary>
        /// Returns all products in the system.
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
            ProductDto ProductDto = new ProductDto()
            {
                ProductID = Product.ProductID,
                ProductName = Product.ProductName,
                Discontinued = Product.Discontinued,
                ProductHasPic = Product.ProductHasPic,
                PicExtension = Product.PicExtension,
                BrandName = Product.Brand.BrandName

            };

            if (Product == null)
            {
                return NotFound();
            }           

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
        [Authorize]
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
            // Picture update is handled by another method
            db.Entry(product).Property(a => a.ProductHasPic).IsModified = false;
            db.Entry(product).Property(a => a.PicExtension).IsModified = false;

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
        /// Receives product picture data, uploads it to the webserver and updates the product's HasPic option
        /// </summary>
        /// <param name="id">the product id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// POST: api/productData/UpdateproductPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        

        [HttpPost]
        public IHttpActionResult UploadProductPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var productPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (productPic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(productPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/products/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Products/"), fn);

                                //save the file
                                productPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the product haspic and picextension fields in the database
                                Product Selectedproduct = db.Products.Find(id);
                                Selectedproduct.ProductHasPic = haspic;
                                Selectedproduct.PicExtension = extension;
                                db.Entry(Selectedproduct).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("product Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

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
        [Authorize]
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
        [Authorize]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.ProductHasPic && product.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Products/" + id + "." + product.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
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