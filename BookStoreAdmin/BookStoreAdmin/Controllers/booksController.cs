using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreAdmin.Models;
using PagedList;

namespace BookStoreAdmin.Controllers
{
    public class booksController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: books
        public ActionResult Index(int? page, int? topWell, int? topNotWell, string searchString)
        {
            int pageSize = 5; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1


            IQueryable<book> books;

            if (topWell.HasValue && topWell > 0)
            {
                string sqlQuery = @"
                    select top (@TopCount) WITH TIES * from book order by sold Desc                    
                 ";


                // Thực hiện truy vấn SQL với tham số top.Value
                books = db.Database.SqlQuery<book>(sqlQuery,  new SqlParameter("TopCount", topWell.Value)).ToList().AsQueryable().OrderBy(b => b.book_id);

            }
            else if(topNotWell.HasValue && topNotWell > 0)
            {
                string sqlQuery = @"
                    select top (@TopCount) WITH TIES * from book order by sold ASC                    
                 ";


                // Thực hiện truy vấn SQL với tham số top.Value
                books = db.Database.SqlQuery<book>(sqlQuery, new SqlParameter("TopCount", topNotWell.Value)).ToList().AsQueryable().OrderBy(b => b.book_id);
            } else
            {
                books = db.books.Include(b => b.author).Include(b => b.category)
                                     .OrderBy(b => b.book_id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(p => p.name.Contains(searchString));
            }

            IPagedList<book> pageListBooks = books.ToPagedList(pageNumber, pageSize);

            return View(pageListBooks);
        }


        // GET: books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: books/Create
        public ActionResult Create()
        {
            ViewBag.author_id = new SelectList(db.authors, "author_id", "name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name");
            return View();
        }

        // POST: books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "book_id,author_id,category_id,name,image,description,publish_company,publish_year,price,sold,remain,is_deleted,created_at")] book book)
        {

            if (IsBooksNameExists(book.name))
            {
                ModelState.AddModelError("email", "Tên Sách đã tồn tại.");
                return View(book);
            }
            book.sold = 0;
            book.created_at = DateTime.Now;
            book.is_deleted = 0;
            if (ModelState.IsValid)
            {
                db.books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.author_id = new SelectList(db.authors, "author_id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", book.category_id);
            return View(book);
        }

        private bool IsBooksNameExists(string name)
        {
            return db.books.FirstOrDefault(x => x.name == name) != null;
        }
        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            book book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.AuthorList = new SelectList(db.authors, "author_id", "name", book.author_id);
            ViewBag.CategoryList = new SelectList(db.categories, "category_id", "name", book.category_id);

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,author_id,category_id,name,image,description,publish_company,publish_year,price,sold,remain,is_deleted,created_at")] book book)
        {
            if (ModelState.IsValid)
            {
                // Tìm sách trong cơ sở dữ liệu
                var existingBook = db.books.Find(book.book_id);
                if (existingBook == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các thuộc tính của sách hiện tại
                existingBook.author_id = book.author_id;
                existingBook.category_id = book.category_id;
                existingBook.name = book.name;
                existingBook.image = book.image;
                existingBook.description = book.description;
                existingBook.publish_company = book.publish_company;
                existingBook.publish_year = book.publish_year;
                existingBook.price = book.price;
                existingBook.sold = book.sold;
                existingBook.remain = book.remain;
                existingBook.is_deleted = book.is_deleted;

                // Đánh dấu sách là đã được chỉnh sửa
                db.Entry(existingBook).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, tải lại danh sách Author và Category
            ViewBag.AuthorList = new SelectList(db.authors, "author_id", "name", book.author_id);
            ViewBag.CategoryList = new SelectList(db.categories, "category_id", "name", book.category_id);

            return View(book);
        }


        // GET: books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            book book = db.books.Find(id);
            db.books.Remove(book);
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
