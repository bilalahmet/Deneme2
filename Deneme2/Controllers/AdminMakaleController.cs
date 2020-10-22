using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Deneme2.Models;

namespace Deneme2.Controllers
{
    
    public class AdminMakaleController : Controller
    {
        mvcDb db = new mvcDb();
        // GET: AdminMakale
        public ActionResult Index()
        {
            var makales = db.Makales.ToList();
            return View(makales);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            ViewBag.KategoriId = new SelectList(db.Katagoris, "KategoriId", "KategoriAdi", makale.KategoriId);
            return View(makale);
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Katagoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale,string etiketler,HttpPostedFileBase Foto)
        {
            try
            {
               

                ViewBag.KategoriId = new SelectList(db.Katagoris, "KategoriId", "KategoriAdi");
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makale.Foto = "/Uploads/MakaleFoto/" + newFoto;
                    makale.Okunma = 0;
                    makale.UyeId = 1;
                    db.SaveChanges();


                }
                if (etiketler != null)
                {
                    string[] etiketDizi = etiketler.Split(',');
                    foreach (var i in etiketDizi)
                    {
                        var yeniEtiket = new Etiket { EtiketAdi = i };
                        db.Etikets.Add(yeniEtiket);
                        makale.Etikets.Add(yeniEtiket);
                    }
                }
                db.Makales.Add(makale);
                makale.UyeId = Convert.ToInt32(Session["uyeid"]);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(makale);
            }
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            
            var makale = db.Makales.Where(m => m.MakaleId == id ).SingleOrDefault();
            if (makale == null )
            {
                return HttpNotFound();            
            }

            ViewBag.KategoriId = new SelectList(db.Katagoris, "KategoriId", "KategoriId", makale.KategoriId);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Foto , Makale makale)
        {
            try
            {
                var makalegelen = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
                makalegelen.Baslik = makale.Baslik;
                makalegelen.Icerik = makale.Icerik;   
                makalegelen.Tarih = makale.Tarih;       
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makalegelen.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makalegelen.Foto));
                    }

                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makalegelen.Foto = "/Uploads/MakaleFoto/" + newFoto;
                    


                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makalegelen = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makalegelen==null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(makalegelen);

            }
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var makalegelen = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
                if (makalegelen == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makalegelen.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makalegelen.Foto));
                }
                foreach (var i in makalegelen.Yorums.ToList())
                {
                    db.Yorums.Remove(i);
                }
                foreach (var i in makalegelen.Etikets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                db.Makales.Remove(makalegelen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
