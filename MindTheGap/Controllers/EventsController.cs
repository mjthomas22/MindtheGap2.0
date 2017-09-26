using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MindTheGap.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;

namespace MindTheGap.Controllers
{
    public class EventsController : Controller
    {
        private MindTheGapEntities db = new MindTheGapEntities();
        public string EventSummary { get; set; }
        public string EventLocation { get; set; }
        public string EventStart { get; set; }
        public string EventEnd { get; set; }
        public string EventDescription { get; set; }
        public string EventRecurrence { get; set; }
        public bool EventReminders { get; set; }
        public string EventColor { get; set; }


        //Grabs user data from Google API and creates a DB with that information.
        [HttpPost]
        public JsonResult GetUserEventInfo(EventsController events)
        {
            //var today = DateTime.Now.ToString();

            DateTime startDateResult;
            DateTime endDateResult;


            string summary = events.EventSummary;
            string location = events.EventLocation;
            string description = events.EventDescription;
            string startTime = events.EventStart;

            string endTime = events.EventEnd;
            string recurrence = events.EventRecurrence;
            bool reminders = false;
            string color = events.EventColor;

            Event newEvent = new Event();
            newEvent.userId = "c03d4c0a-ee82-4980-ad00-bb4cb16f99ca";
            newEvent.summary = summary;
            newEvent.location = location;
            if (description != null && description.Length > 500)
            {
                newEvent.description = description.Substring(0, 500);
            }
            else
            {
                newEvent.description = description;
            }
            DateTime.TryParse(startTime, out startDateResult);
            newEvent.starttime = startDateResult;
            DateTime.TryParse(endTime, out endDateResult);
            newEvent.endtime = endDateResult;
            newEvent.recurrence = recurrence;
            newEvent.reminders = reminders;
            newEvent.colorId = color;
            db.Events.Add(newEvent);
            db.SaveChanges();


            return new JsonResult()
            {
            };

        }
        //END OF TEST


        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.Interest).Include(e => e.AspNetUser);
            return View(events.ToList());
        }

        public ActionResult Daily()
        {

            return View();
        }

        //Deletes Google events from our DB when we grab them so there are no duplicates
        public void EventDuplicates()
        {
            db.Database.ExecuteSqlCommand("DELETE Event Where reminders = 0");
        }

        //Pulls DB information to display on the calendar
        public ActionResult GetCalendarDatabase()
        {
            {
                var events = db.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        //SQL runs through and finds the gaps in the schedule
        public ActionResult Gaps()
        {
            var today = DateTime.Now.ToString("dd-MM-yyyy");
            db.Database.ExecuteSqlCommand("DROP TABLE Gap");
            /*AND starttime LIKE 'today'*/
            db.Database.ExecuteSqlCommand("SELECT* INTO Gap FROM (SELECT MAX(endtime) OVER(ORDER BY starttime) GapStart, LEAD(starttime) OVER(ORDER BY starttime) GapEnd FROM[Event] WHERE starttime < endtime) AS Gaps");

            //We don't have anything we need to return to the view. We could replace this with a redirect action if we want.
            return View();
        }

        //Displays the gaps found
        public JsonResult TestJson()
        {

            var Gaps = (from Gap in db.Gaps
                        orderby
                        Gap.GapStart ascending
                        select new
                        {
                            Gap.GapStart,
                            Gap.GapEnd,
                        }).Take(10);

            var output = JsonConvert.SerializeObject(Gaps.ToList());

            //Finally, we return Json to the view
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.interestId = new SelectList(db.Interests, "interestId", "userId");
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "eventId,userId,interestId,summary,description,location,colorId,starttime,endtime,recurrence,reminders,completed")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.userId = "c03d4c0a-ee82-4980-ad00-bb4cb16f99ca";
                @event.colorId = "36c4e8";
                @event.reminders = true;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Daily");
            }

            ViewBag.interestId = new SelectList(db.Interests, "interestId", "userId", @event.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", @event.userId);
            return RedirectToAction("Daily");
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.interestId = new SelectList(db.Interests, "interestId", "userId", @event.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", @event.userId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "eventId,userId,interestId,summary,description,location,colorId,starttime,endtime,recurrence,reminders,completed")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.userId = "c03d4c0a-ee82-4980-ad00-bb4cb16f99ca";
                @event.colorId = "36c4e8";
                @event.reminders = true;
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.interestId = new SelectList(db.Interests, "interestId", "userId", @event.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", @event.userId);
            return RedirectToAction("Index");
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
