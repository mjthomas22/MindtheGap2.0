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
    public class UserLevelsController : Controller
    {
        private MindTheGapEntities db = new MindTheGapEntities();

        // GET: UserLevels
        public ActionResult Index()
        {
            var userLevels = db.UserLevels.Include(u => u.AspNetUser);
            return View(userLevels.ToList());
        }

        // GET: UserLevels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = db.UserLevels.Find(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            return View(userLevel);
        }

        // GET: UserLevels/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UserLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "levelId,userId,userLevel1,xp,xpNeeded")] UserLevel userLevel)
        {
            if (ModelState.IsValid)
            {
                db.UserLevels.Add(userLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", userLevel.userId);
            return View(userLevel);
        }

        // GET: UserLevels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = db.UserLevels.Find(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", userLevel.userId);
            return View(userLevel);
        }

        // POST: UserLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "levelId,userId,userLevel1,xp,xpNeeded")] UserLevel userLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", userLevel.userId);
            return View(userLevel);
        }

        // GET: UserLevels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLevel userLevel = db.UserLevels.Find(id);
            if (userLevel == null)
            {
                return HttpNotFound();
            }
            return View(userLevel);
        }

        // POST: UserLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserLevel userLevel = db.UserLevels.Find(id);
            db.UserLevels.Remove(userLevel);
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
