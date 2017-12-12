using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyStore.DAL;
using BabyStore.Models;
using BabyStore.ViewModels;
using PagedList;

namespace BabyStore.Controllers {
   public class ProductsController:Controller {
      private StoreContext db = new StoreContext();

      // GET: Products
      public ActionResult Index(string category,string search,string sortBy,int? page) {
         // instantiate a new view model 
         ProductIndexviewModel viewModel = new ProductIndexviewModel();

         // select the products 
         var products = db.Products.Include(p => p.Category);

         // perfom the search and save the search string to the viewmodel
         if(!String.IsNullOrEmpty(search)) {
            products = products.Where(p => p.Name.Contains(search) ||
            p.Description.Contains(search) ||
            p.Category.Name.Contains(search));
            viewModel.Search = search;
         }

         // group the results into groups and count how many items in each category 
         viewModel.CatsWithCount = from p in products
                                   where
                                   p.CategoryID != null
                                   group p by p.Category.Name into catGroup
                                   select new CategoryWithCount() {
                                      CategoryName = catGroup.Key,
                                      ProductCount = catGroup.Count()
                                   };


         if(!String.IsNullOrEmpty(category)) {
            products = products.Where(p => p.Category.Name == category);
            viewModel.Category = category;
         }

         // sort the results 
         switch(sortBy) {
            case "price_lowest":
               products = products.OrderBy(p => p.Price);
               break;
            case "price_highest":
               products = products.OrderByDescending(p => p.Price);
               break;
            default:
               products = products.OrderBy(p => p.Name);
               break;
         }

         int currentPage = page ?? 1;
         viewModel.Products = products.ToPagedList(currentPage,Constants.pageItems);
         viewModel.SortBy = sortBy;
         viewModel.Sorts = new Dictionary<string,string> {
        { "Price low to high","price_lowest"},
        {"Price high to low","price_highest" }
      };
         return View(viewModel);
      }

      // GET: Products/Details/5
      public ActionResult Details(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Product product = db.Products.Find(id);
         if(product == null) {
            return HttpNotFound();
         }
         return View(product);
      }

      // GET: Products/Create
      public ActionResult Create() {
         ProductViewModel viewModel = new ProductViewModel();
         viewModel.CategoryList = new SelectList(db.Categories,"ID","Name");
         viewModel.ImageLists = new List<SelectList>();
         for(int i = 0;i < Constants.NumberOfProductImages;i++) {
            viewModel.ImageLists.Add(new SelectList(db.ProductImages,"ID","FileName"));
         }
         return View(viewModel);
      }

      // POST: Products/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(ProductViewModel viewModel) {
         Product product = new Product();
         product.Name = viewModel.Name;
         product.Description = viewModel.Description;
         product.Price = viewModel.Price;
         product.CategoryID = viewModel.CategoryID;
         product.ProductImageMappings = new List<ProductImageMapping>();
         // get a list of selected images without any blanks
         string[] productImagesNames = viewModel.ProductImages.Where(pi => !String.IsNullOrEmpty(pi)).ToArray();

         for(int i = 0;i < productImagesNames.Length;i++) {
            product.ProductImageMappings.Add(new ProductImageMapping {
               productImage = db.ProductImages.Find(int.Parse(productImagesNames[i])),
               imageNumber = i,
            });
         }

         if(ModelState.IsValid) {
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         viewModel.CategoryList = new SelectList(db.Categories,"ID","Name");
         viewModel.ImageLists = new List<SelectList>();
         for(int i = 0;i < Constants.NumberOfProductImages;i++) {
            viewModel.ImageLists.Add(new SelectList(db.ProductImages,"ID","FileName",viewModel.ProductImages[i]));
         }

         ViewBag.CategoryID = new SelectList(db.Categories,"ID","Name",product.CategoryID);
         return View(viewModel);
      }

      // GET: Products/Edit/5
      public ActionResult Edit(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Product product = db.Products.Find(id);
         if(product == null) {
            return HttpNotFound();
         }
         ViewBag.CategoryID = new SelectList(db.Categories,"ID","Name",product.CategoryID);
         return View(product);
      }

      // POST: Products/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,Name,Description,Price,CategoryID")] Product product) {
         if(ModelState.IsValid) {
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.CategoryID = new SelectList(db.Categories,"ID","Name",product.CategoryID);
         return View(product);
      }

      // GET: Products/Delete/5
      public ActionResult Delete(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Product product = db.Products.Find(id);
         if(product == null) {
            return HttpNotFound();
         }
         return View(product);
      }

      // POST: Products/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id) {
         Product product = db.Products.Find(id);
         db.Products.Remove(product);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
