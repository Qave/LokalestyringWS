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
    public class UserBookingsViewsController : ApiController
    {
        private LokalestyringDBContext db = new LokalestyringDBContext();

        // GET: api/UserBookingsViews
        public IQueryable<UserBookingsView> GetUserBookingsViews()
        {
            return db.UserBookingsViews;
        }

        // GET: api/UserBookingsViews/5
        [ResponseType(typeof(UserBookingsView))]
        public IHttpActionResult GetUserBookingsView(string id)
        {
            UserBookingsView userBookingsView = db.UserBookingsViews.Find(id);
            if (userBookingsView == null)
            {
                return NotFound();
            }

            return Ok(userBookingsView);
        }

        // PUT: api/UserBookingsViews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserBookingsView(string id, UserBookingsView userBookingsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userBookingsView.City)
            {
                return BadRequest();
            }

            db.Entry(userBookingsView).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBookingsViewExists(id))
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

        // POST: api/UserBookingsViews
        [ResponseType(typeof(UserBookingsView))]
        public IHttpActionResult PostUserBookingsView(UserBookingsView userBookingsView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserBookingsViews.Add(userBookingsView);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserBookingsViewExists(userBookingsView.City))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userBookingsView.City }, userBookingsView);
        }

        // DELETE: api/UserBookingsViews/5
        [ResponseType(typeof(UserBookingsView))]
        public IHttpActionResult DeleteUserBookingsView(string id)
        {
            UserBookingsView userBookingsView = db.UserBookingsViews.Find(id);
            if (userBookingsView == null)
            {
                return NotFound();
            }

            db.UserBookingsViews.Remove(userBookingsView);
            db.SaveChanges();

            return Ok(userBookingsView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserBookingsViewExists(string id)
        {
            return db.UserBookingsViews.Count(e => e.City == id) > 0;
        }
    }
}