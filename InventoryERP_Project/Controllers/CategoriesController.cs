using InventoryERP_Project.Models;
using InventoryERP_Project.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryERP_Project.Controllers
{
    public class CategoriesController : Controller
    {
        readonly InventoryDbContext db = null;
        public CategoriesController(InventoryDbContext db) { this.db = db; }
        public IActionResult Index(int? typeid)
        {
            ViewBag.TypeId = typeid;
            ViewBag.ProductTypes = db.ProductTypes.ToList();
           
                var data = db.Categories.Where(x=> x.Product_Type_Id==typeid.Value && x.Is_Deleted==0).Where(x => x.Is_Deleted == 0).ToList();
                return View(data);
          
            
        }
        public IActionResult Create()
        {
            ViewBag.ProductTypes = db.ProductTypes.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category._Key = Util.RandomString(32);
                category.Is_Deleted = 0;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult CreateMultiple(int? typeid)
        {
            ViewBag.TypeId = typeid;
            ViewBag.ProductTypes = db.ProductTypes.ToList();
            ViewBag.Categories = typeid.HasValue ? db.Categories.Where(x => x.Product_Type_Id == typeid).ToList() : null;
            return View();
        }
        [HttpPost]
        public ActionResult SaveAll(List<Category> categories)
        {
            if (ModelState.IsValid)
            {
                foreach(var c in categories)
                {
                    c._Key = Util.RandomString(32);
                    c.Is_Deleted = 0;
                    db.Categories.Add(c);
                }
                db.SaveChanges();
                var data  =db.Categories.Where(x => x.Product_Type_Id == categories[0].Product_Type_Id).ToList();
                return Json(data);
            }
            return new EmptyResult();
        }
        public PartialViewResult ProductList(int typeid)
        {
            return PartialView("_ProductList_By_Type");
        }
    }
}
