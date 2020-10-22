using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Deneme2.Models;
using PagedList;
using PagedList.Mvc;


namespace Deneme2.Controllers

{

    public class HomeController : Controller
    {
        mvcDb db = new mvcDb();
        // GET: Home
        public ActionResult Index(int Page = 1 )
        {
            var makale = db.Makales.OrderByDescending(m => m.MakaleId).ToPagedList(Page, 5 );
            return View(makale);
        }

        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }

            return View(makale);
        }
        // GET: KullanıcıMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Katagoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: KullanıcıMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale, string etiketler, HttpPostedFileBase Foto)
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
                    makale.UyeId = Convert.ToInt32(Session["uyeid"]);
                    makale.Tarih = DateTime.Now;
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
        public ActionResult MakaleSil(int id)
        {
            var uyeid = Session["uyeid"];
            var makalegelen = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makalegelen.UyeId == Convert.ToInt32(uyeid))
            {
               
                if (makalegelen == null)
                {
                    return HttpNotFound();
                }
                foreach (var i in makalegelen.Etikets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                if (System.IO.File.Exists(Server.MapPath(makalegelen.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makalegelen.Foto));
                }
                foreach (var i in makalegelen.Yorums.ToList())
                {
                    db.Yorums.Remove(i);
                }
                
                db.Makales.Remove(makalegelen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }

        }

        //public ActionResult YorumPartial()
        //{
           
        //    return View(db.Yorums.OrderByDescending(m => m.YorumId).Take(5));
        //}

        public ActionResult KatagoriPartial()
        {
            return View(db.Katagoris.ToList());
        }

        public JsonResult YorumYap(string yorum,int makaleId)
        {
            var uyeid = Session["uyeid"];

            if (yorum!=null)
            {
                db.Yorums.Add(new Yorum { UyeId = Convert.ToInt32(uyeid), MakaleId = makaleId, Icerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }
            
            return Json(false,JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorums.Where(m => m.YorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(n => n.MakaleId == yorum.MakaleId).SingleOrDefault();

            if (yorum.UyeId == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.MakaleId });
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult OkumaArttir(int makaleid)
        {
            var makale = db.Makales.Where(m => m.MakaleId == makaleid).SingleOrDefault();
            makale.Okunma += 1;
            db.SaveChanges();

            return View();

        }
        public ActionResult KategoriMakale(int id)
        {
            var makaleler = db.Makales.Where(m => m.KategoriId == id).ToList();
            return View(makaleler);
        }

        public ActionResult BlogAra(string ara = null)
        {
            var aranan = db.Makales.Where(m => m.Baslik.Contains(ara)).ToList();
            return View(aranan.OrderByDescending(m=>m.Okunma));
        }
 
    }
}