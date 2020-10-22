using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Deneme2.Models;

namespace Deneme2.Controllers
{
    public class AdminKatagoriController : Controller
    {
        private mvcDb db = new mvcDb();

        // GET: AdminKatagori
        public ActionResult Index()
        {
            return View(db.Katagoris.ToList());
        }

        // GET: AdminKatagori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var katagori = db.Makales.Where(m => m.KategoriId == id).ToList();
            if (katagori == null)
            {
                return HttpNotFound();
            }
            return View(katagori);
        }

        // GET: AdminKatagori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminKatagori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriId,KategoriAdi")] Katagori katagori)
        {
            if (ModelState.IsValid)
            {
                db.Katagoris.Add(katagori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(katagori);
        }

        // GET: AdminKatagori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Katagori katagori = db.Katagoris.Find(id);
            if (katagori == null)
            {
                return HttpNotFound();
            }
            return View(katagori);
        }

        // POST: AdminKatagori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriId,KategoriAdi")] Katagori katagori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(katagori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(katagori);
        }

        // GET: AdminKatagori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Katagori katagori = db.Katagoris.Find(id);
            if (katagori == null)
            {
                return HttpNotFound();
            }
            return View(katagori);
        }

        // POST: AdminKatagori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Katagori katagori = db.Katagoris.Find(id);
            db.Katagoris.Remove(katagori);
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
