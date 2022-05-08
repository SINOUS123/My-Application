using Microsoft.AspNetCore.Mvc;
using ProjectVideo.Data;
using ProjectVideo.Models;
using System.Collections.Generic;

namespace ProjectVideo.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }
        //GET- CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST-CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        //GET- EDIT
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var cat = _db.Category.Find(id);
            if(cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }



        //POST-EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        /////////////////////////////////
        ///
         //GET- DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cat = _db.Category.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        //POST-DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteById(int? id)
        {
            Category cat = _db.Category.Find(id);
            if( cat == null)
            {
                return NotFound(cat);
            }
            _db.Category.Remove(cat);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
