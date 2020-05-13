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
using LokalestyringWS.Models;

namespace LokalestyringWS.Controllers
{
    public class TavleBookingsController : ApiController
    {
        private LokalestyringDBContext db = new LokalestyringDBContext();

        // GET: api/TavleBookings
        public IQueryable<TavleBooking> GetTavleBookings()
        {
            return db.TavleBookings;
        }

        // GET: api/TavleBookings/5
        [ResponseType(typeof(TavleBooking))]
        public IHttpActionResult GetTavleBooking(int id)
        {
            TavleBooking tavleBooking = db.TavleBookings.Find(id);
            if (tavleBooking == null)
            {
                return NotFound();
            }

            return Ok(tavleBooking);
        }

        // PUT: api/TavleBookings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTavleBooking(int id, TavleBooking tavleBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tavleBooking.Tavle_Id)
            {
                return BadRequest();
            }

            db.Entry(tavleBooking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TavleBookingExists(id))
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

        // POST: api/TavleBookings
        [ResponseType(typeof(TavleBooking))]
        public IHttpActionResult PostTavleBooking(TavleBooking tavleBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TavleBookings.Add(tavleBooking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tavleBooking.Tavle_Id }, tavleBooking);
        }

        // DELETE: api/TavleBookings/5
        [ResponseType(typeof(TavleBooking))]
        public IHttpActionResult DeleteTavleBooking(int id)
        {
            TavleBooking tavleBooking = db.TavleBookings.Find(id);
            if (tavleBooking == null)
            {
                return NotFound();
            }

            db.TavleBookings.Remove(tavleBooking);
            db.SaveChanges();

            return Ok(tavleBooking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TavleBookingExists(int id)
        {
            return db.TavleBookings.Count(e => e.Tavle_Id == id) > 0;
        }
    }
}