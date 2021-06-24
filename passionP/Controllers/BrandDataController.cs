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
    public class BrandDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all Brands in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all brands in the database, including their associated brand id.
        /// </returns>
        /// <example>
        ///  GET: api/BrandData/ListBrands
        /// </example>


        [HttpGet]
        [ResponseType(typeof(BrandDto))]
        public IHttpActionResult ListBrands()
        {
            List<Brand> Brands = db.Brands.ToList();
            List<BrandDto> BrandDto = new List<BrandDto>();

            Brands.ForEach(a => BrandDto.Add(new BrandDto()
            {
                BrandID = a.BrandID,
                BrandName = a.BrandName
                

            }));
            return Ok(BrandDto);
        }

        /// <summary>
        /// Returns all Brands in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All brands in the system matching up to the brands ID primary key
          //or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the brands</param>
        /// <example>
        // GET: api/BrandData/FindBrand/5
        /// </example>




        [ResponseType(typeof(BrandDto))]
        [HttpGet]
        public IHttpActionResult FindBrand(int id)
        {
            Brand Brand = db.Brands.Find(id);
            BrandDto BrandDto = new BrandDto()
            {
                BrandID = Brand.BrandID,
                BrandName = Brand.BrandName
                
            };

            if (Brand == null)
            {
                return NotFound();
            }

            return Ok(BrandDto);
        }

        /// <summary>
        /// Updates a particular brands in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the brands ID primary key</param>
        /// <param name="brands">JSON FORM DATA of a brand</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/BrandData/UpdateBrand/5
        /// FORM DATA: brand JSON Object
        /// </example>

        // POST: api/BrandData/UpdateBrand/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateBrand(int id, Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brand.BrandID)
            {
                return BadRequest();
            }

            db.Entry(brand).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
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
        /// Adds a Brand to the system
        /// </summary>
        /// <param name="Brand">JSON FORM DATA of a Brand</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Brand ID, Brand Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/BrandData/AddBrand
        /// FORM DATA: Brand JSON Object
        /// </example>

       
        [ResponseType(typeof(Brand))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Brands.Add(brand);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = brand.BrandID }, brand);
        }


        /// <summary>
        /// Deletes a Brand from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Brand</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/BrandData/DeleteBrand/5
        /// FORM DATA: (empty)
        /// </example>

      
        [ResponseType(typeof(Brand))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteBrand(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            db.Brands.Remove(brand);
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

        private bool BrandExists(int id)
        {
            return db.Brands.Count(e => e.BrandID == id) > 0;
        }
    }
}