using Microsoft.AspNetCore.Mvc;
using ProjectVideo.Data;
using ProjectVideo.Models;
using System.Collections.Generic;

namespace ProjectVideo.Controllers
{
    public class ApplicationTypeController : Controller
    {

        private ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
                _db = db;
        }
        //GET-LIST APPLICATION TYPE
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> list = _db.ApplicationType;

            return View(list);
        }

        //GET-CREATE APPLICATION TYPE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST-CREATE APPLICATION TYPE
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }
        /* TRAITEMENT EDIT*/


        //GET-EDIT APPLICATION TYPE
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound(id);
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST-EDIT APPLICATION TYPE
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }
    }
}
