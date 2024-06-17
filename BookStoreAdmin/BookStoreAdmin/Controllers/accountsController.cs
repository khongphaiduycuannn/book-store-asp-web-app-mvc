using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreAdmin.Models;
using PagedList;

namespace BookStoreAdmin.Controllers
{
    public class accountsController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: accounts
        public ActionResult Index(int? page)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            int pageSize = 5; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1

            var accounts = db.accounts
                                 .OrderBy(b => b.account_id) // Sắp xếp theo account_id hoặc bất kỳ cột nào khác
                                 .ToPagedList(pageNumber, pageSize);

            return View(accounts);
        }

        // GET: accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: accounts/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            return View();
        }

        // POST: accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,username,email,password,role")] account account)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            bool isExist = IsEmailExists(account.email);

            Debug.WriteLine(isExist);

            if (IsEmailExists(account.email))
            {
                ModelState.AddModelError("email", "Email đã tồn tại.");
                return View(account);
            }

            if (ModelState.IsValid)
            {
                if (account.role != "Admin" && account.role != "Client")
                {
                    ModelState.AddModelError("role", "Vui lòng chọn vai trò là 'Admin' hoặc 'Client'.");
                    return View(account);
                }
                account.created_at = DateTime.Now;
                db.accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        private bool IsEmailExists(string email)
        {
            return db.accounts.FirstOrDefault(x => x.email == email) != null;
        }

        // GET: accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,username,email,password,role,created_at")] account account)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            account account = db.accounts.Find(id);
            db.accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var account = db.accounts.FirstOrDefault(x => x.email == email);
                if (account == null)
                {
                    ViewBag.err = "Đăng nhập không thành công, tài khoản không tồn tại.";
                    return View();
                }
                else if (account.password != password)
                {
                    ViewBag.err = "Đăng nhập không thành công, email hoặc mật khẩu không đúng.";
                    return View();
                }
                else if (account.role.ToLower() != "admin")
                {
                    ViewBag.err = "Đăng nhập không thành công, tài khoản không đủ quyền để truy cập trang quản lý.";
                    return View();
                }

                // Đăng nhập thành công
                Session["Username"] = account.username;
                TempData["SuccessMessage"] = "Đăng nhập thành công";
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ sau khi đăng nhập thành công
            }

            return View();
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            Session.Clear(); // Xóa session khi người dùng đăng xuất
            return RedirectToAction("Login", "accounts");
        }
    }
}
