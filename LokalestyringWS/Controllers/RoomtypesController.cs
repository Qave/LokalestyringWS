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
    public class RoomtypesController : ApiController
    {
        private LokalestyringDBContext db = new LokalestyringDBContext();

        // GET: api/Roomtypes
        public IQueryable<Roomtype> GetRoomtypes()
        {
            return db.Roomtypes;
        }

        // GET: api/Roomtypes/5
        [ResponseType(typeof(Roomtype))]
        public IHttpActionResult GetRoomtype(int id)
        {
            Roomtype roomtype = db.Roomtypes.Find(id);
            if (roomtype == null)
            {
                return NotFound();
            }

            return Ok(roomtype);
        }

        // PUT: api/Roomtypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoomtype(int id, Roomtype roomtype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomtype.Type_Id)
            {
                return BadRequest();
            }

            db.Entry(roomtype).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomtypeExists(id))
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

        // POST: api/Roomtypes
        [ResponseType(typeof(Roomtype))]
        public IHttpActionResult PostRoomtype(Roomtype roomtype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Roomtypes.Add(roomtype);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = roomtype.Type_Id }, roomtype);
        }

        // DELETE: api/Roomtypes/5
        [ResponseType(typeof(Roomtype))]
        public IHttpActionResult DeleteRoomtype(int id)
        {
            Roomtype roomtype = db.Roomtypes.Find(id);
            if (roomtype == null)
            {
                return NotFound();
            }

            db.Roomtypes.Remove(roomtype);
            db.SaveChanges();

            return Ok(roomtype);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomtypeExists(int id)
        {
            return db.Roomtypes.Count(e => e.Type_Id == id) > 0;
        }
    }
}