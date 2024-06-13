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
            int pageSize = 5; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1

            var accounts = db.accounts
                                 .OrderBy(b => b.account_id) // Sắp xếp theo book_id hoặc bất kỳ cột nào khác
                                 .ToPagedList(pageNumber, pageSize);

            return View(accounts);
        }
        // GET: accounts/Details/5
        public ActionResult Details(int? id)
        {
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
            return View();
        }

        // POST: accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,username,email,password,role")] account account)
        {
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
            return db.accounts.FirstOrDefault(x=> x.email == email) != null;
        }

        // GET: accounts/Edit/5
        public ActionResult Edit(int? id)
        {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,username,email,password,role,created_at")] account account)
        {
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
    }
}
