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
    public class RoomsViewsController : ApiController
    {
        private LokalestyringDBContext db = new LokalestyringDBContext();

        // GET: api/RoomsViews
        public IQueryable<RoomsView> GetRoomsViews()
        {
            return db.RoomsViews;
        }

        // GET: api/RoomsViews/5
        [ResponseType(typeof(RoomsView))]
        public IHttpActionResult GetRoomsView(string id)
        {
            RoomsView roomsView = db.RoomsViews.Find(id);
            if (roomsView == null)
            {
                return NotFound();
            }

            return Ok(roomsView);
        }

        // PUT: api/RoomsViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoomsView(string id, RoomsView roomsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomsView.City)
            {
                return BadRequest();
            }

            db.Entry(roomsView).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomsViewExists(id))
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

        // POST: api/RoomsViews
        [ResponseType(typeof(RoomsView))]
        public IHttpActionResult PostRoomsView(RoomsView roomsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RoomsViews.Add(roomsView);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RoomsViewExists(roomsView.City))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = roomsView.City }, roomsView);
        }

        // DELETE: api/RoomsViews/5
        [ResponseType(typeof(RoomsView))]
        public IHttpActionResult DeleteRoomsView(string id)
        {
            RoomsView roomsView = db.RoomsViews.Find(id);
            if (roomsView == null)
            {
                return NotFound();
            }

            db.RoomsViews.Remove(roomsView);
            db.SaveChanges();

            return Ok(roomsView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomsViewExists(string id)
        {
            return db.RoomsViews.Count(e => e.City == id) > 0;
        }
    }
}