using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deneme2.Models;


namespace Deneme2.Controllers
{
   
    public class AdminController : Controller
    {
        mvcDb db = new mvcDb();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult uyeList()
        {
            var liste = db.Uyes.ToList();
            return View(liste);
        }

        public ActionResult UyeMakales(int id)
        {
            var makales = db.Makales.Where(m => m.UyeId == id).ToList();
            return View(makales);
        }

        public ActionResult UyeDetay(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            return View(uye);
        }
        public ActionResult UyeDelete(int id)
        {
            var uye = db.Uyes.Where(m => m.UyeId == id).SingleOrDefault();
            if (uye == null || id == 1 )
            {
                return HttpNotFound();
            }
            else
            {
                return View(uye);
            }
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult UyeDelete(int id, FormCollection collection)
        {
            try
            {
                var yorumList = db.Yorums.Where(y => y.UyeId == id).ToList();
                var makale = db.Makales.Where(m => m.UyeId == id).SingleOrDefault();
                var makaleList = db.Makales.Where(m => m.UyeId == id).ToList();
                var uye = db.Uyes.Where(m => m.UyeId == id).SingleOrDefault();
                if (uye == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(uye.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(uye.Foto));
                }
                if (makaleList != null)
                {
                    foreach (var i in makaleList)
                    {

                        if (System.IO.File.Exists(Server.MapPath(i.Foto)))
                        {
                            System.IO.File.Delete(Server.MapPath(i.Foto));
                        }
                        foreach (var j in makale.Yorums.ToList())
                        {
                            db.Yorums.Remove(j);
                        }
                        foreach (var j in makale.Etikets.ToList())
                        {
                            db.Etikets.Remove(j);
                        }
                        db.Makales.Remove(i);
                    }

                }
                foreach (var item in yorumList)
                {
                    db.Yorums.Remove(item);
                }
                db.Uyes.Remove(uye);
                db.SaveChanges();
                return RedirectToAction("uyeList");
            }
            catch
            {
                return View();
            }
        }


    }
}