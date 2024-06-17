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
    public class categoriesController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: categories
        public ActionResult Index(int? page)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            int pageSize = 3; // Số lượng item trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1
            var categories = db.categories.OrderBy(b => b.category_id).ToPagedList(pageNumber, pageSize);
            return View(categories);
        }

        // GET: categories/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: categories/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            return View();
        }
        // GET: categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "category_id,name,description,created_at")] category category)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (IsCategoryNameExists(category.name))
            {
                ModelState.AddModelError("name", "Tên danh mục đã tồn tại.");
                return View(category);
            }

            if (ModelState.IsValid)
            {
                category.created_at = DateTime.Now;
                db.categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        private bool IsCategoryNameExists(string name)
        {
            return db.categories.FirstOrDefault(x => x.name == name) != null;
        }

        // GET: categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);

           
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "category_id,name,description,created_at")] category category)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (ModelState.IsValid)
            {
                // Tải thực thể từ cơ sở dữ liệu
                var existingCategory = db.categories.Find(category.category_id);
                if (existingCategory == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra xem tên danh mục đã tồn tại hay chưa (ngoại trừ danh mục hiện tại)
                if (IsCategoryNameExists(category.name) && !existingCategory.name.Equals(category.name, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("name", "Tên danh mục đã tồn tại.");
                    return View(category);
                }

                // Cập nhật các thuộc tính của thực thể hiện tại
                existingCategory.name = category.name;
                existingCategory.description = category.description;
                existingCategory.created_at = category.created_at; // Nếu bạn thực sự cần cập nhật thời gian tạo

                // Đánh dấu thực thể là đã được chỉnh sửa
                db.Entry(existingCategory).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(category);
        }


        // GET: categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "accounts");
            category category = db.categories.Find(id);
            db.categories.Remove(category);
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
