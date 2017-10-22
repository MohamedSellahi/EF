using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyStore.Models;
using BabyStore.DAL;
using System.Net;
using System.Data.Entity;

namespace BabyStore.Controllers {
   public class CategoryController : Controller {
      private StoreContext db = new StoreContext();
      
      // GET: Category
      public ActionResult Index() {

         return View(db.Categories.OrderBy(c=>c.Name).ToList());
      }


      public ActionResult Details(int? id) {
         if (id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         Category category = db.Categories.Find(id);
         if (category ==null) {
            return HttpNotFound();
         }
         return View(category);
      }


      // GET: Category Create
      public ActionResult Create() {
         return View();
      }


      // POST: Create category 
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include ="ID,Name")] Category category) {
         if (ModelState.IsValid) {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         // 
         return View(category);
      }

      // GET: Edit Category 
      public ActionResult Edit(int? id) {
         if (id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Category category = db.Categories.Find(id);
         if (category == null) {
            return HttpNotFound();
         }
         return View(category);
      }

      // POST: Categories/Edit/5
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include ="ID,Name")] Category category) {
         if (ModelState.IsValid) {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(category);
      }

      //GET: Categories/Delete/5
      public ActionResult Delete(int? id) {
         if (id==null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Category category = db.Categories.Find(id);
         if (id==null) {
            return HttpNotFound();
         }
         return View(category);
      }


      //POST: Categories/Delete/5
      [HttpPost,ActionName("Delete")]
      public ActionResult Delete(int id) {
         Category category = db.Categories.Find(id);
         db.Categories.Remove(category);
         db.SaveChanges();
         return RedirectToAction("Index");
      }


      protected override void Dispose(bool disposing) {
         if (disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }

   }
}