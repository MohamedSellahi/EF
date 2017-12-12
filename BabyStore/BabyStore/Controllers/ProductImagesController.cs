using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyStore.DAL;
using BabyStore.Models;
using System.Web.Helpers;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace BabyStore.Controllers {
   public class ProductImagesController:Controller {
      private StoreContext db = new StoreContext();

      // GET: ProductImages
      public ActionResult Index() {
         return View(db.ProductImages.ToList());
      }

      // GET: ProductImages/Details/5
      public ActionResult Details(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ProductImage productImage = db.ProductImages.Find(id);
         if(productImage == null) {
            return HttpNotFound();
         }
         return View(productImage);
      }

      // GET: ProductImages/Create
      public ActionResult Upload() {
         return View();
      }

      // POST: ProductImages/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Upload(HttpPostedFileBase file) {
         // check if the user has entered a file 
         if(file != null) {
            // check if the file is valid
            if(validateFile(file)) {
               try {
                  saveFileToDisk(file);
               }
               catch(Exception e) {
                  ModelState.AddModelError("FileName","Sorry an error occured when savinf the file to disk, please try again");
               }
            }
            else {
               ModelState.AddModelError("FileName","The file must be gif, png, jpeg or jpg and less than 2MB in size");
            }
         }
         else {// no file has been uploaded 
            ModelState.AddModelError("FileName","Please choose a file");
         }

         if(ModelState.IsValid) {
            db.ProductImages.Add(new ProductImage { FileName = file.FileName });
            try {
               db.SaveChanges();
            }
            catch(DbUpdateException e) {
               SqlException innerExeption = e.InnerException.InnerException as SqlException;
               if(innerExeption != null && innerExeption.Number == 2601) {
                  ModelState.AddModelError("FileName","The file " + file.FileName + "already exists in the system. Please delete it and try to add it gain if you wish to re-add it");
               }
               else {
                  ModelState.AddModelError("FileName","Sorry an error has accured during saving to the database, please try again");
               }
               return View();
            }
            return RedirectToAction("Index");
         }

         return View();
      }

      // GET: ProductImages/Edit/5
      public ActionResult Edit(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ProductImage productImage = db.ProductImages.Find(id);
         if(productImage == null) {
            return HttpNotFound();
         }
         return View(productImage);
      }

      // POST: ProductImages/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,FileName")] ProductImage productImage) {
         if(ModelState.IsValid) {
            db.Entry(productImage).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(productImage);
      }

      // GET: ProductImages/Delete/5
      public ActionResult Delete(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ProductImage productImage = db.ProductImages.Find(id);
         if(productImage == null) {
            return HttpNotFound();
         }
         return View(productImage);
      }

      // POST: ProductImages/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id) {
         ProductImage productImage = db.ProductImages.Find(id);
         db.ProductImages.Remove(productImage);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Helpers
      private bool validateFile(HttpPostedFileBase file) {
         string fileExtension = Path.GetExtension(file.FileName).ToLower();
         string[] allowedFileTypes = { ".gif",".png",".jpeg",".jpg" };
         if(file.ContentLength > 0 && file.ContentLength < Constants.maxFileSize && allowedFileTypes.Contains(fileExtension)) {
            return true;
         }
         return false;
      }

      private void saveFileToDisk(HttpPostedFileBase file) {
         WebImage img = new WebImage(file.InputStream);
         if(img.Width > 190) {
            img.Resize(190,img.Height);
         }
         img.Save(Constants.productImagePath + file.FileName);
         if(img.Width > 100) {
            img.Resize(100,img.Height);
         }
         img.Save(Constants.productThumbnailPath + file.FileName);
      }
      #endregion
   }
}
