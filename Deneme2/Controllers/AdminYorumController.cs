using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deneme2.Models;

namespace Deneme2.Controllers
{
    public class AdminYorumController : Controller
    {
        mvcDb db = new mvcDb();

        // GET: AdminYorum
        public ActionResult Index()
        {
            var yorums = db.Yorums.ToList();
            return View(yorums);
        }


        public ActionResult Delete(int id)
        {
            var yorum = db.Yorums.Where(y => y.YorumId == id).SingleOrDefault();
            if (yorum == null)
            {
                return HttpNotFound();
            }

            return View(yorum);
        }
        [HttpPost]
        public ActionResult Delete(int id,FormCollection collection)
        {
            var yorum = db.Yorums.Where(y => y.YorumId == id).SingleOrDefault();
            db.Yorums.Remove(yorum);
            db.SaveChanges();
            return View();

        }
    }
    
}