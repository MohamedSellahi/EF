﻿using BabyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BabyStore.ViewModels {
  public class ProductIndexviewModel {

    public IPagedList<Product> Products { get; set; }
    public string Search { get; set; }
    public IEnumerable<CategoryWithCount> CatsWithCount { get; set; }
    public string Category { get; set; }
    public string SortBy { get; set; }
    public Dictionary<string,string> Sorts { get; set; }


    public IEnumerable<SelectListItem> CatFilterItems {
      get {
        var allCats = CatsWithCount.Select(cc => new SelectListItem {
          Value = cc.CategoryName,
          Text = cc.CatNamesWithCount
        });
        return allCats;
      }
    }
  }

  public class CategoryWithCount {
    public int ProductCount { get; set; }
    public string CategoryName { get; set; }
    public string CatNamesWithCount {
      get { return CategoryName + "(" + ProductCount.ToString() + ")"; }
    }
  }
}