using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraahvIndividual.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace TraahvIndividual.Controllers
{
    public class HomeController : Controller
    {
        TrahvidContext db = new TrahvidContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Traahv()
        {
            IEnumerable<Traahv> traahvs = db.Traahv.ToList();
            return View(traahvs);
        }
        [HttpGet]
        public ActionResult CreateTraahv()
        {
            return View();
        }


        public ActionResult CreateTraahv(Traahv trahv)
        {
            trahv.CalculateFine(); 
            db.Traahv.Add(trahv);
            db.SaveChanges();
            trahv.SendMessage();
            return RedirectToAction("Traahv");
        }

        [HttpGet]
        public ActionResult EditTraahv(int? id)
        {
            Traahv g = db.Traahv.Find(id);
            if (g == null)
            {
                return HttpNotFound();

            }
            return View(g);
        }
        [HttpPost, ActionName("EditTraahv")]
        public ActionResult EditTraahvConfirmed(Traahv guest)
        {
            guest.CalculateFine(); 
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Traahv");
        }

        public ActionResult DeleteTraahv(int id)
        {
            Traahv d = db.Traahv.Find(id);
            if (d == null)
            {
                return HttpNotFound();

            }
            return View(d);
        }
        [HttpPost, ActionName("DeleteTraahv")]
        public ActionResult DeleteTraahvConfirmed(int id)
        {
            Traahv d = db.Traahv.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }

            db.Traahv.Remove(d);
            db.SaveChanges();
            return RedirectToAction("Traahv");
        }
    }
}