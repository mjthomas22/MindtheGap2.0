using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MindTheGap.Models;

namespace MindTheGap.Controllers
{
    public class InterestsController : Controller
    {
        private MindTheGapEntities db = new MindTheGapEntities();

        // GET: Interests
        public ActionResult Index()
        {
            var interests = db.Interests.Include(i => i.Progress).Include(i => i.AspNetUser);
            return View(interests.ToList());
        }

        // GET: Interests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // GET: Interests/Create
        public ActionResult Create()
        {
            ViewBag.interestId = new SelectList(db.Progresses, "progressId", "userId");
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Interests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "interestId,userId,title,weeklygoal,minTime,priority,colorId")] Interest interest)
        {
            if (ModelState.IsValid)
            {
                db.Interests.Add(interest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.interestId = new SelectList(db.Progresses, "progressId", "userId", interest.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", interest.userId);
            return View(interest);
        }

        // GET: Interests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            ViewBag.interestId = new SelectList(db.Progresses, "progressId", "userId", interest.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", interest.userId);
            return View(interest);
        }

        // POST: Interests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "interestId,userId,title,weeklygoal,minTime,priority,colorId")] Interest interest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.interestId = new SelectList(db.Progresses, "progressId", "userId", interest.interestId);
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", interest.userId);
            return View(interest);
        }

        // GET: Interests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // POST: Interests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interest interest = db.Interests.Find(id);
            db.Interests.Remove(interest);
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
