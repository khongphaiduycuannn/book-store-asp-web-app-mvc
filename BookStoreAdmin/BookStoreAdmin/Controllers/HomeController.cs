using BookStoreAdmin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BookStoreAdmin.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        List<account> listAdmin = new List<account>();

        public ActionResult Index(int? page)
        {
            var sold = db.books.Sum(x => x.sold);
            var revenue = db.books.Sum(x => x.price * x.sold);
            var remain = db.books.Sum(x => x.remain);

            // Số lượng đơn hàng
            var cntCart = db.orders.Count();

            listAdmin = db.accounts.Where(x => x.role.ToLower() == "admin").ToList();


            ViewBag.revenue = String.Format("{0:N0}", revenue);
            ViewBag.sold = sold;
            ViewBag.remain = remain;
            ViewBag.cntCart = cntCart;
            ViewBag.listAdmin = listAdmin;

            int pageSize = 5; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1

            var accounts = db.accounts.Where(x => x.role.ToLower() == "admin")
                                 .OrderBy(b => b.account_id) // Sắp xếp theo book_id hoặc bất kỳ cột nào khác
                                 .ToPagedList(pageNumber, pageSize);

            return View(accounts);
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