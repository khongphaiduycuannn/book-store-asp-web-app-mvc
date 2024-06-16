using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreAdmin.Models;
using PagedList;

namespace BookStoreAdmin.Controllers
{
    public class authorsController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: authors
        public ActionResult Index(int? page)
        {
            int pageSize = 5; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1
            var author = db.authors
                                .OrderBy(b => b.author_id) // Sắp xếp theo book_id hoặc bất kỳ cột nào khác
                                .ToPagedList(pageNumber, pageSize);
            return View(author);
        }


        // GET: authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "author_id,name,image,description,is_deleted,created_at")] author author)
        {
            if (ModelState.IsValid)
            {
                author.created_at = DateTime.Now;
                author.is_deleted = 0;
                db.authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "author_id,name,image,description,is_deleted,created_at")] author author)
        {
            if (ModelState.IsValid)
            {
                // Tìm sách trong cơ sở dữ liệu
                var existingBook = db.authors.Find(author.author_id);
                if (existingBook == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các thuộc tính của sách hiện tại
                existingBook.name = author.name;
                existingBook.image = author.image;
                existingBook.description = author.description;
                existingBook.is_deleted = author.is_deleted;

                // Đánh dấu sách là đã được chỉnh sửa
                db.Entry(existingBook).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
               
            }
            return View(author);
        }

        // GET: authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            author author = db.authors.Find(id);
            db.authors.Remove(author);
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
