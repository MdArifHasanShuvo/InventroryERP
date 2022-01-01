using InventoryERP_Project.Models;
using InventoryERP_Project.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryERP_Project.Controllers
{
    public class ProductTypesController : Controller
    {
        readonly InventoryDbContext db=null;
        public ProductTypesController(InventoryDbContext db) { this.db = db; }
        public IActionResult Index(string search="")
        {
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(search))
            {
                var data = this.db.ProductTypes
                    .Where(x => x.Name.ToLower().StartsWith(search.ToLower()))
                    .Where(p => p.Is_Deleted == 0)
                    .ToList();
                return View(data);
            }
            else
                return View(this.db.ProductTypes.Where(p=> p.Is_Deleted == 0).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                productType._Key = Util.RandomString(32);
                productType.Is_Deleted = 0;
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productType);
        }
        public IActionResult DeleteSelected(int[] ids)
        {
            foreach(int i in ids)
            {
                var existing = db.ProductTypes.FirstOrDefault(x => x.Id == i);
                if (existing != null)
                    existing.Is_Deleted = 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete (int Id)
        {
            var existing = db.ProductTypes.FirstOrDefault(x => x.Id == Id);
            if (existing != null)
                existing.Is_Deleted = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id, string _Key)
        {
            var data = db.ProductTypes.FirstOrDefault(x => x.Id == id);
            if (data == null)
                return NotFound();
            else
            {
                if (data._Key != _Key)
                    return BadRequest();
            }
            
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                var data = db.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                if (data == null)
                    return NotFound();
                else
                {
                    if (data.Id != productType.Id)
                        return BadRequest();
                    else
                         if (data._Key != productType._Key)
                        return BadRequest();

                }
                db.Entry(productType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productType);
        }
    }
}
