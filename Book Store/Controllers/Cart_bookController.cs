using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Book_Store.Models;
using BookStoreAdmin.Models;

namespace Book_Store.Controllers
{
    public class Cart_bookController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        [HttpGet]
        // GET: cart_book
        public ActionResult Index()
        {
            ViewBag.Category = db.categories;
            if (Session["account_id"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int account_id = (int) Session["account_id"];
            int cart_id = db.carts.Where(c => c.account_id == account_id).First().cart_id;
            var cart_Book = db.cart_book.Include(c => c.book).Where(cb => cb.card_id == cart_id).ToList();

            long total = 0;
            for (int i = 0; i < cart_Book.Count; i++)
            {
                cart_book cb = cart_Book[i];
                total += (int)cb.total_amount;
            }
            ViewBag.Total = total;
            return View(cart_Book.ToList());
        }

        [HttpPost]
        public ActionResult Index(string address, int? phone_number)
        {
            ViewBag.Category = db.categories;
            if (Session["account_id"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            int account_id = (int)Session["account_id"];
            int cart_id = db.carts.Where(c => c.account_id == account_id).First().cart_id;
            var cart_Book = db.cart_book.Include(c => c.book).Where(cb => cb.card_id == cart_id).ToList();
            if (cart_Book.Count == 0)
            {
                ViewBag.Success = "";
                ViewBag.Error = "Không có sản phẩm nào trong giỏ hàng.";
            }
            else
            {
                order o = new order();
                o.account_id = account_id;
                o.created_at = DateTime.Now;
                o.address = address;
                o.phone_number = phone_number.ToString();
                o.status = "Đang giao hàng";
                db.orders.Add(o);
                db.SaveChanges();

               int id = db.orders.Local.First().order_id;
                for (int i = 0; i < cart_Book.Count; i++)
                {
                    cart_book cb = cart_Book[i];
                    book b = db.books.Find(cb.book_id);
                    b.remain -= cb.quantity;
                    b.sold += cb.quantity;

                    order_book ob = new order_book();
                    ob.order_id = id;
                    ob.book_id = cb.book_id;
                    ob.total_amount = cb.total_amount;
                    ob.quantity = cb.quantity;
                    db.order_book.Add(ob);
                    db.SaveChanges();
                }
                db.cart_book.RemoveRange(db.cart_book);
                db.SaveChanges();
                ViewBag.Total = 0;
                ViewBag.Success = "Mua hàng thành công, vui lòng theo dõi đơn hàng.";
                ViewBag.Error = "";
            }
            return View(new List<cart_book>());
        }

        // GET: cart_book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart_book cart_book = db.cart_book.Find(id);
            if (cart_book == null)
            {
                return HttpNotFound();
            }
            return View(cart_book);
        }

        // GET: cart_book/Create
        public ActionResult Create()
        {
            ViewBag.card_id = new SelectList(db.books, "book_id", "name");
            ViewBag.card_id = new SelectList(db.carts, "cart_id", "cart_id");
            return View();
        }

        // POST: cart_book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "card_id,book_id,quantity,status,total_amount")] cart_book cart_book)
        {
            if (ModelState.IsValid)
            {
                db.cart_book.Add(cart_book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.card_id = new SelectList(db.books, "book_id", "name", cart_book.card_id);
            ViewBag.card_id = new SelectList(db.carts, "cart_id", "cart_id", cart_book.card_id);
            return View(cart_book);
        }

        // GET: cart_book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart_book cart_book = db.cart_book.Find(id);
            if (cart_book == null)
            {
                return HttpNotFound();
            }
            ViewBag.card_id = new SelectList(db.books, "book_id", "name", cart_book.card_id);
            ViewBag.card_id = new SelectList(db.carts, "cart_id", "cart_id", cart_book.card_id);
            return View(cart_book);
        }

        // POST: cart_book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "card_id,book_id,quantity,status,total_amount")] cart_book cart_book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart_book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.card_id = new SelectList(db.books, "book_id", "name", cart_book.card_id);
            ViewBag.card_id = new SelectList(db.carts, "cart_id", "cart_id", cart_book.card_id);
            return View(cart_book);
        }

        // GET: cart_book/Delete/5
        public ActionResult Delete(int? cardId, int? bookId)
        {
            if (cardId == null || bookId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart_book cart_book = db.cart_book.Where(cb => cb.card_id == cardId && cb.book_id == bookId).First();
            db.cart_book.Remove(cart_book);
            db.SaveChanges();
            if (cart_book == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Cart_Book");
        }

        // POST: cart_book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cart_book cart_book = db.cart_book.Find(id);
            db.cart_book.Remove(cart_book);
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
