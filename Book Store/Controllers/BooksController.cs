using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Book_Store.Models;
using PagedList;

namespace Book_Store.Controllers
{
    public class BooksController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: books
        public ActionResult Index(decimal? minPrice, decimal? maxPrice, int? page)
        {
            var books = db.books.Include(b => b.author).Include(b => b.category);

            if (minPrice.HasValue)
            {
                books = books.Where(b => b.price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                books = books.Where(b => b.price <= maxPrice.Value);
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            ViewBag.Category = db.categories;

            return View(books.OrderBy(b => b.book_id).ToPagedList(pageNumber, pageSize));
        }

        // GET: books/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.books.Include(b => b.author).Include(b => b.category).FirstOrDefault(b => b.book_id == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sách cùng thể loại
            var relatedBooks = db.books
                                 .Where(b => b.category_id == book.category_id && b.book_id != id)
                                 .Take(5) // Lấy 5 sách cùng thể loại
                                 .ToList();

            // Tạo ViewModel để truyền dữ liệu cho View
            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                RelatedBooks = relatedBooks
            };

            ViewBag.Category = db.categories;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Details(int? quantity, int? id)
        {
            if (Session["account_id"] == null)
                return RedirectToAction("Login", "Accounts");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.books.Include(b => b.author).Include(b => b.category).FirstOrDefault(b => b.book_id == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sách cùng thể loại
            var relatedBooks = db.books
                                 .Where(b => b.category_id == book.category_id && b.book_id != id)
                                 .Take(5) // Lấy 5 sách cùng thể loại
                                 .ToList();

            // Tạo ViewModel để truyền dữ liệu cho View
            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                RelatedBooks = relatedBooks
            };

            ViewBag.Category = db.categories;

            int accountId = (int) Session["account_id"];
            int cartId = db.carts.Where(c => c.account_id == (int)accountId).First().cart_id;

            cart_book cart_Book = new cart_book();
            cart_Book.card_id = cartId;
            cart_Book.book_id = (int)id;
            cart_Book.quantity = quantity;
            cart_Book.total_amount = quantity * book.price;

            db.cart_book.Add(cart_Book);
            if (quantity > book.remain)
            {
                ViewBag.Error = "Thêm vào giỏ hàng thất bại!";
            }
            else
            {
                ViewBag.Success = "Thêm vào giỏ hàng thành công!";
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.Success = "";
                ViewBag.Error = "Sản phẩm đã tồn tại trong giỏ hàng!";
            }

            return View(viewModel);
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

        // GET: books/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.author_id = new SelectList(db.authors, "author_id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", book.category_id);
            return View(book);
        }

        // POST: books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,author_id,category_id,name,image,description,publish_company,publish_year,price,sold,remain,is_deleted,created_at")] book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.author_id = new SelectList(db.authors, "author_id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", book.category_id);
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

        // GET: books/TopSelling
        public ActionResult TopSelling()
        {
            var topSellingBooks = db.books.Include(b => b.author).Include(b => b.category)
                .OrderByDescending(b => b.sold).Take(5).ToList();

            ViewBag.Category = db.categories;

            return View(topSellingBooks);
        }

        // GET: books/NewArrivals
        public ActionResult NewArrivals()
        {
            var newArrivals = db.books.Include(b => b.author).Include(b => b.category)
                .OrderByDescending(b => b.publish_year).Take(5).ToList();

            ViewBag.Category = db.categories;

            return View(newArrivals);
        }

        public ActionResult SearchByName(string searchName)
        {
            var searchedList = db.books.Where(b => b.name.Contains(searchName));
            if (searchedList.Count() == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Category = db.categories;

            return View(searchedList);
        }

        public ActionResult Category(int category_id)
        {
            var books = db.books.Where(b => b.category_id == category_id).ToList();

            // Set the CategoryName in ViewBag if the category exists
            var category = db.categories.FirstOrDefault(c => c.category_id == category_id);
            if (category != null)
            {
                ViewBag.CategoryName = category.name;
            }
            else
            {
                ViewBag.CategoryName = "Unknown Category";
            }

            // Count the books in the specified category
            int bookCount = books.Count();
            Console.WriteLine(bookCount);

            // Set the categories in ViewBag
            ViewBag.Category = db.categories.ToList();

            // Return the view with the list of books
            return View(books);
        }
    }
}
