using BookStoreAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreAdmin.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        public ActionResult Index()
        {
            var sold = db.books.Sum(x => x.sold);
            var revenue = db.books.Sum(x => x.price * x.sold);
            var remain = db.books.Sum(x => x.remain);

            ViewBag.revenue = String.Format("{0:N0}", revenue);
            ViewBag.sold = sold;
            ViewBag.remain = remain;


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
    }
}