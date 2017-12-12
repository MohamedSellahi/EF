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
using System.Diagnostics;

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
      public ActionResult Upload(HttpPostedFileBase[] files) {
         bool allValid = true;
         string inValidFiles = string.Empty;
         db.Database.Log = sql => Trace.WriteLine(sql);
         // check the user has entered a file 
         if(files[0] != null) {
            // if the user has entered less than 10 files 
            if(files.Length <= 10) {
               foreach(var file in files) {
                  if(!validateFile(file)) {
                     allValid = false;
                     inValidFiles += ", " + file.FileName;
                  }
               }
               // if they are all valid try to save them to disk 
               if(allValid) {
                  foreach(var file in files) {
                     try {
                        saveFileToDisk(file);
                     }
                     catch(Exception) {
                        ModelState.AddModelError("FileName","Sorry, an error occured savig file to the disk, please try again");
                     }
                  }
               }
               else {
                  ModelState.AddModelError("FileName","All files must be gif, png, jpeg or jpg and less than 2MB in size.The following files" + inValidFiles + " are not valid");
               }
            }
            else { // the user has entered more than 10 files 
               ModelState.AddModelError("FileName","Please only uplaod ten files at a time");
            }
         }
         else {
            // no files have been entered 
            ModelState.AddModelError("FileName","Please choose a file");
         }
         if(ModelState.IsValid) {
            // try add files to database 
            bool duplicates = false;
            bool otherDbError = false;
            string duplicateFiles = string.Empty;
            foreach(var file in files) {
               //try and save each file 
               var productToAdd = new ProductImage { FileName = file.FileName };
               try {
                  db.ProductImages.Add(productToAdd);
                  db.SaveChanges();
               }
               catch(DbUpdateException ex) {
                  SqlException innerException = ex.InnerException.InnerException as SqlException;
                  if(innerException != null && innerException.Number == 2601) {
                     duplicateFiles += ", " + file.FileName;
                     duplicates = true;
                     db.Entry(productToAdd).State = EntityState.Detached;
                  }
                  else {
                     otherDbError = true;
                  }
               }
            }
            //add a list of duplicate files to the error message
            if(duplicates) {
               ModelState.AddModelError("FileName","All files uploaded except the files" +
               duplicateFiles + ", which already exist in the system." + " Please delete them and try again if you wish to re - add them");
               return View();
            }
            else if(otherDbError) {
               ModelState.AddModelError("FileName","Sorry an error has occurred saving to the database,please try again");
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
