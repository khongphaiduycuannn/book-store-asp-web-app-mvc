using Book_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Store.Controllers
{
    public class AccountsController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: Accounts
        public ActionResult Index()
        {
            if (Session["account_id"] != null)
            {
                return RedirectToAction("Index", "Books");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["account_id"] != null)
            {
                return RedirectToAction("Index", "Books");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(String email, String password)
        {
            if (Session["account_id"] != null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (email == null || email.Length == 0 || email.Trim().Equals(""))
            {
                return View(new ErrorMessage("Tài khoản không đúng định dạng!"));
            }
            if (password == null || password.Length < 8 || password.Trim().Equals(""))
            {
                return View(new ErrorMessage("Mật khẩu không đúng định dạng!")); 
            }

            var account = db.accounts.Where(a => a.email == email);
            if (account.Count() == 0)
            {
                return View(new ErrorMessage("Tài khoản không hợp lệ!"));
            }

            if (account.First().password != password)
            {
                return View(new ErrorMessage("Mật khẩu không hợp lệ!"));
            }

            Session["account_id"] = account.First().account_id;
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(String username, String email, String password, String repassword)
        {
            if (username == null || username.Length == 0 || username.Trim().Equals(""))
            {
                return View(new ErrorMessage("Tên người dùng không đúng định dạng!"));
            }
            if (username == null || username.Length == 0 || username.Trim().Equals(""))
            {
                return View(new ErrorMessage("Tài khoản không đúng định dạng!"));
            }
            if (password == null || password.Length < 8 || password.Trim().Equals(""))
            {
                return View(new ErrorMessage("Mật khẩu không đúng định dạng!"));
            }
            if (repassword == null || repassword.Length < 8 || repassword.Trim().Equals(""))
            {
                return View(new ErrorMessage("Nhật lại khẩu không đúng định dạng!"));
            }
            if (password != repassword)
            {
                return View(new ErrorMessage("Nhập lại mật khẩu chưa đúng!"));
            }

            var account = db.accounts.Where(a => a.email == email);
            if (account.Count() != 0)
            {
                return View(new ErrorMessage("Tài khoản đã tồn tại!"));
            }

            var newAccount = new account();
            var newCart = new cart();

            newAccount.username = username;
            newAccount.email = email;
            newAccount.password = password;

            db.accounts.Add(newAccount);
            db.SaveChanges();

            newCart.account_id = db.accounts.Where(a => a.email == email).First().account_id;
            db.carts.Add(newCart);
            db.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}