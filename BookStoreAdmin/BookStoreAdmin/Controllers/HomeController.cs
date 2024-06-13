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

        List<account> listAdmin = new List<account>();

        public ActionResult Index()
        {
            var sold = db.books.Sum(x => x.sold);
            var revenue = db.books.Sum(x => x.price * x.sold);
            var remain = db.books.Sum(x => x.remain);
            var cntCart = db.cart_book.Count();

            listAdmin = db.accounts.Where(x => x.role.ToLower() == "admin").ToList();


            ViewBag.revenue = String.Format("{0:N0}", revenue);
            ViewBag.sold = sold;
            ViewBag.remain = remain;
            ViewBag.cntCart = cntCart;
            ViewBag.listAdmin = listAdmin;

            return View();
        }

        public ActionResult Delete(int accountId)
        {
            try
            {
                var account = db.accounts.Find(accountId);
                if (account != null)
                {
                    db.accounts.Remove(account);
                    db.SaveChanges();
                    // Chuyển hướng người dùng đến trang Index sau khi xóa thành công
                    return RedirectToAction("Index");
                }
                else
                {
                    // Trả về một trạng thái 404 Not Found nếu không tìm thấy tài khoản
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi xảy ra trong quá trình xóa và trả về một trạng thái lỗi 500 Internal Server Error
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
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