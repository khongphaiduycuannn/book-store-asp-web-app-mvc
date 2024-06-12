using Book_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Book_Store.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreDB db = new BookStoreDB();
        public ActionResult Index()
        {
            //var booksWithRevenue = db.books.Select(b => new
            //{
            //    Book = b,
            //    Revenue = b.price * b.sold
            //}).ToList();

            //// Lấy giá trị số lượng đã bán
            //var sold = booksWithRevenue.Sum(b => b.Book.sold);

            //if (booksWithRevenue == null )
            //{
            //    ViewBag.Revenue = 0;
            //} else
            //{
            //    ViewBag.Revenue = booksWithRevenue.Sum(b => b.Revenue);

            //}
            //if (sold == null)
            //{
            //    sold = 0;
            //}

            ViewBag.Sold = 0;
            ViewBag.Revenue = 0;
            return View();
        }

    }
}