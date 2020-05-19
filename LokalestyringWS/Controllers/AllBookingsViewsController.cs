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
    public class AllBookingsViewsController : ApiController
    {
        private LokalestyringDBContext db = new LokalestyringDBContext();

        // GET: api/AllBookingsViews
        public IQueryable<AllBookingsView> GetAllBookingsViews()
        {
            return db.AllBookingsViews;
        }

        // GET: api/AllBookingsViews/5
        [ResponseType(typeof(AllBookingsView))]
        public IHttpActionResult GetAllBookingsView(int id)
        {
            AllBookingsView allBookingsView = db.AllBookingsViews.Find(id);
            if (allBookingsView == null)
            {
                return NotFound();
            }

            return Ok(allBookingsView);
        }

        // PUT: api/AllBookingsViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAllBookingsView(int id, AllBookingsView allBookingsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != allBookingsView.Room_Id)
            {
                return BadRequest();
            }

            db.Entry(allBookingsView).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllBookingsViewExists(id))
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

        // POST: api/AllBookingsViews
        [ResponseType(typeof(AllBookingsView))]
        public IHttpActionResult PostAllBookingsView(AllBookingsView allBookingsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AllBookingsViews.Add(allBookingsView);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AllBookingsViewExists(allBookingsView.Room_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = allBookingsView.Room_Id }, allBookingsView);
        }

        // DELETE: api/AllBookingsViews/5
        [ResponseType(typeof(AllBookingsView))]
        public IHttpActionResult DeleteAllBookingsView(int id)
        {
            AllBookingsView allBookingsView = db.AllBookingsViews.Find(id);
            if (allBookingsView == null)
            {
                return NotFound();
            }

            db.AllBookingsViews.Remove(allBookingsView);
            db.SaveChanges();

            return Ok(allBookingsView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AllBookingsViewExists(int id)
        {
            return db.AllBookingsViews.Count(e => e.Room_Id == id) > 0;
        }
    }
}