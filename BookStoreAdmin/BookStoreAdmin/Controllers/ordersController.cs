using BookStoreAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookStoreAdmin.Controllers
{

    public class OrderStatistics
    {
        public DateTime? Date { get; set; }
        public String titleShow { get; set; }
        public int OrderCount { get; set; }
    }

    public class ordersController : Controller
    {
        private BookStoreDB db = new BookStoreDB();


        public ActionResult Statistics(string filterType)
        {
            List<OrderStatistics> statistics;

            switch (filterType)
            {
                case "month":
                    statistics = GetOrderStatisticsByMonth();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:MM/yyyy}", item.Date);
                    }
                    break;
                case "year":
                    statistics = GetOrderStatisticsByYear();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:yyyy}", item.Date);
                    }
                    break;
                default:
                    statistics = GetOrderStatisticsByDay();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:dd/MM/yyyy}", item.Date);
                    }
                    break;
            }

            ViewBag.FilterType = filterType;

            return View(statistics);
        }

        private List<OrderStatistics> GetOrderStatisticsByDay()
        {
            var query = from o in db.orders
                        group o by DbFunctions.TruncateTime(o.created_at) into g
                        select new OrderStatistics
                        {
                            Date = g.Key,
                            OrderCount = g.Count()
                        };

            return query.ToList();
        }

        private List<OrderStatistics> GetOrderStatisticsByMonth()
        {
            var query = from o in db.orders
                        group o by new { o.created_at.Value.Year, o.created_at.Value.Month } into g
                        select new
                        {
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            OrderCount = g.Count()
                        };

            var result = query.ToList().Select(x => new OrderStatistics
            {
                Date = new DateTime(x.Year, x.Month, 1),
                OrderCount = x.OrderCount
            }).ToList();

            return result;
        }

        private List<OrderStatistics> GetOrderStatisticsByYear()
        {
            var query = from o in db.orders
                        group o by o.created_at.Value.Year into g
                        select new
                        {
                            Year = g.Key,
                            OrderCount = g.Count()
                        };

            var result = query.ToList().Select(x => new OrderStatistics
            {
                Date = new DateTime(x.Year, 1, 1),
                OrderCount = x.OrderCount
            }).ToList();

            return result;
        }

        // GET: orders
        public ActionResult Index()
        {
            var orders = db.orders.Include(o => o.account);
            return View(orders.ToList());
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "username");
            return View();
        }

        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,account_id,created_at")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account_id = new SelectList(db.accounts, "account_id", "username", order.account_id);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "username", order.account_id);
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,account_id,address,phone_number,shipping_fee,status")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "username", order.account_id);
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
