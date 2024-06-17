using BookStoreAdmin.Models;
using ClosedXML.Excel;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;

namespace BookStoreAdmin.Controllers
{

    public class OrderStatistics
    {
        public DateTime? Date { get; set; }
        public String titleShow { get; set; }
        public int OrderCount { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class ordersController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        static List<OrderStatistics> statistics;

        public ActionResult Statistics(string filterType)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            switch (filterType)
            {
                case "month":
                    statistics = GetOrderStatisticsByMonth();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:MM/yyyy}", item.Date);
                    }
                    ViewBag.theader = "Tháng";
                    break;
                case "year":
                    statistics = GetOrderStatisticsByYear();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:yyyy}", item.Date);
                    }
                    ViewBag.theader = "Năm";
                    break;
                default:
                    statistics = GetOrderStatisticsByDay();
                    foreach (var item in statistics)
                    {
                        item.titleShow = string.Format("{0:dd/MM/yyyy}", item.Date);
                    }
                    ViewBag.theader = "Ngày";
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
                            OrderCount = g.Count(),
                            OrderTotal = g.Sum(x => (decimal?)x.order_book.Sum(ob => ob.total_amount) ?? decimal.Zero) + g.Sum(x => x.shipping_fee)
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
                            OrderCount = g.Count(),
                            TotalRevenue = g.Sum(x => (decimal?)x.order_book.Sum(ob => ob.total_amount) ?? decimal.Zero) + g.Sum(x => x.shipping_fee)
                        };

            var result = query.ToList().Select(x => new OrderStatistics
            {
                Date = new DateTime(x.Year, x.Month, 1),
                OrderCount = x.OrderCount,
                OrderTotal = x.TotalRevenue,

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
                            OrderCount = g.Count(),
                            TotalRevenue = g.Sum(x => (decimal?)x.order_book.Sum(ob => ob.total_amount) ?? decimal.Zero) + g.Sum(x => x.shipping_fee)
                        };

            var result = query.ToList().Select(x => new OrderStatistics
            {
                Date = new DateTime(x.Year, 1, 1),
                OrderCount = x.OrderCount,
                OrderTotal = x.TotalRevenue,

            }).ToList();

            return result;
        }

        // GET: orders
        public ActionResult Index(int? page)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            int pageSize = 3; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1
            var orders = db.orders.Include(o => o.account).OrderBy(b=>b.order_id).ToPagedList(pageNumber, pageSize);
            return View(orders);
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
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
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
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
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
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
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
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
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (ModelState.IsValid)
            {
                var existingOrder = db.orders.Find(order.order_id);
                if (existingOrder == null)
                {
                    return HttpNotFound();
                }
                existingOrder.phone_number = order.phone_number;
                existingOrder.address = order.address;
                existingOrder.shipping_fee = order.shipping_fee;
                existingOrder.status= order.status;
                db.Entry(existingOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account_id = new SelectList(db.accounts, "account_id", "username", order.account_id);
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
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
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            order order = db.orders.Find(id);
            db.orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel()
        {
            string username = Session["Username"] as string;
            TempData["Error"] = username + "sdfsdf";
            try
            {
                var data = statistics.ToList();
                if (data != null && data.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(ToConvertDataTable(data.ToList()));
                        ws.Column(1).Delete();
                        ws.Cell("A1").Value = "Ngày";
                        ws.Cell("B1").Value = "Số đơn hàng";
                        ws.Cell("C1").Value = "Doanh thu";
                        ws.Cell("E1").Value = "Người tạo";
                        ws.Cell("F1").Value = username; 
                        ws.Cell("E2").Value = "Thời gian tạo";
                        ws.Cell("F2").Value = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
                        ws.Columns().AdjustToContents();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            string fileName = $"Report_{DateTime.Now.ToString("dd/MM/yyyy")}.xlsx";
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocuments.spreedsheetml.sheet", fileName);
                        }
                    }
                }
                TempData["Error"] = "Không có dữ liệu";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Lỗi khi xuất dữ liệu: "+ e.Message;
            }
            return RedirectToAction("Statistics");
        }

        public DataTable ToConvertDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] propInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propInfo)
            {
                dt.Columns.Add(prop.Name);

            }
            foreach (var item in items)
            {
                var values = new object[propInfo.Length];
                for (int i = 0; i < propInfo.Length; i++)
                {
                    values[i] = propInfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
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
