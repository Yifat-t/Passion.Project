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
    public class RetailerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all Retailers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Retailers in the database, including their associated id.
        /// </returns>
        /// <example>
        /// GET: api/RetailerData/ListRetailers
        /// </example>
        /// 

        [HttpGet]
        [ResponseType(typeof(RetailerDto))]
        public IHttpActionResult ListRetailers()
        {
            List<Retailer> Retailers = db.Retailers.ToList();
            List<RetailerDto> RetailerDtos = new List<RetailerDto>();

            Retailers.ForEach(a => RetailerDtos.Add(new RetailerDto()
            {
                RetailerID = a.RetailerID,
                RetailerName = a.RetailerName


            }));
            return Ok(RetailerDtos);
        }


        /// <summary>
        /// Returns all Retailers in the system associated with a particular product.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Retailers in the database that selling a particular product/s.
        /// </returns>
        /// <param name="id">Product Primary Key</param>
        /// <example>
        /// GET: api/RetailerData/ListRetailersForProduct/3
        /// </example>


        [HttpGet]
        [ResponseType(typeof(RetailerDto))]
        public IHttpActionResult ListRetailersForProduct(int id)
        {
            List<Retailer> Retailers = db.Retailers.Where(
                r=>r.Products.Any(
                    a=>a.ProductID==id)
                ).ToList();
            List<RetailerProductDto> RetailerDto = new List<RetailerProductDto>();

            Retailers.ForEach(r => RetailerDto.Add(new RetailerProductDto()
            {
                RetailerID = r.RetailerID,
                RetailerName = r.RetailerName,
                ProductID = id,
                ProductPrice = db.RetailerProducts.Where(rp => rp.ProductID == id && rp.RetailerID == r.RetailerID).Single().Price

            }));
            return Ok(RetailerDto);
        }

        /// <summary>
        /// Returns Retailers in the system not selling perticuler product.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Retailers in the database not selling perticular product
        /// </returns>
        /// <param name="id">Product Primary Key</param>
        /// <example>
        /// GET: api/RetailersData/ListRetailersNotSellingThisProduct/1
        /// </example>



        [HttpGet]
        [ResponseType(typeof(RetailerDto))]
        ///[Authorize]
        public IHttpActionResult ListRetailersNotSellingThisProduct(int id)
        {
            List<Retailer> Retailers = db.Retailers.Where(
                r => !r.Products.Any(
                    a => a.ProductID == id)
                ).ToList();
            List<RetailerDto> RetailerDto = new List<RetailerDto>();

            Retailers.ForEach(r => RetailerDto.Add(new RetailerDto()
            {
                RetailerID = r.RetailerID,
                RetailerName = r.RetailerName


            }));
            return Ok(RetailerDto);
        }

        /// <summary>
        /// Returns all Retailers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Retailer in the system matching up to the Retailer ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Retailer</param>
        /// <example>
        /// GET: api/RetailerData/FindRetailer/1
        /// </example>



        [ResponseType(typeof(RetailerDto))]
        [HttpGet]
        public IHttpActionResult FindRetailer(int id)
        {
            Retailer Retailer = db.Retailers.Find(id);
            RetailerDto RetailerDto = new RetailerDto()
            {
                RetailerID = Retailer.RetailerID,
                RetailerName = Retailer.RetailerName

            };

            if (Retailer == null)
            {
                return NotFound();
            }

            return Ok(RetailerDto);
        }

        /// <summary>
        /// Updates a particular Retailer in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Retailer ID primary key</param>
        /// <param name="Retailer">JSON FORM DATA of an Retailer</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/RetailerData/UpdateRetailer/5
        /// FORM DATA: Retailer JSON Object
        /// </example>


        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateRetailer(int id, Retailer retailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != retailer.RetailerID)
            {
                return BadRequest();
            }

            db.Entry(retailer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetailerExists(id))
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
        /// Adds a Retailer to the system
        /// </summary>
        /// <param name="Retailer">JSON FORM DATA of a Retailer</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Retailer ID, Retailer Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/RetailerData/AddRetailer
        /// FORM DATA: Retailer JSON Object
        /// </example>


        
        [ResponseType(typeof(Retailer))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddRetailer(Retailer retailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Retailers.Add(retailer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = retailer.RetailerID }, retailer);
        }


        /// <summary>
        /// Deletes a Retailer from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Retailer</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/RetailerData/DeleteRetailer/5
        /// FORM DATA: (empty)
        /// </example>



        [ResponseType(typeof(Retailer))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteRetailer(int id)
        {
            Retailer retailer = db.Retailers.Find(id);
            if (retailer == null)
            {
                return NotFound();
            }

            db.Retailers.Remove(retailer);
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

        private bool RetailerExists(int id)
        {
            return db.Retailers.Count(e => e.RetailerID == id) > 0;
        }
    }
}