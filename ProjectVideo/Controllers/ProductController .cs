using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectVideo.Data;
using ProjectVideo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectVideo.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
    }
        public IActionResult Index()
        {
            IEnumerable<Product> pro = _db.Product.Include(f => f.Category).Include(f => f.ApplicationType);
           /* foreach (var obj in pro)
            {
                obj.Category = _db.Category.FirstOrDefault(u=>u.Id == obj.CategoryId);
                obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u=>u.Id == obj.ApplicationId);
            }*/
            return View(pro);
        }
        //GET- UPSET
        public IActionResult UpSert(int? id)
        {
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            ViewBag.CategoryDropDown = CategoryDropDown;

            IEnumerable<SelectListItem>ApplicationDropDown = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            ViewBag.ApplicationDropDown = ApplicationDropDown;

            Product product = new Product();
            if(id == null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                if(product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            
        }

        //POST-CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(Product pro)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string weRootPath = _webHostEnvironment.WebRootPath;

                if(pro.Id == 0)
                {
                    //creating
                    string upload = weRootPath + WC.ImagePath;
                    string filname = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);


                    using (var fileStream = new FileStream(Path.Combine(upload, filname+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    pro.Image = filname + extension;
                    _db.Product.Add(pro);

                }
                else
                {
                    //updating

                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(f => f.Id == pro.Id);
                    if(files.Count() > 0)
                    {
                        string upload = weRootPath + WC.ImagePath;
                        string filname = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        } 

                        using (var fileStream = new FileStream(Path.Combine(upload, filname + extension), FileMode.Create))
                        {  
                            files[0].CopyTo(fileStream);
                        }
                        pro.Image = filname + extension;
                    }
                    else
                    {
                        pro.Image = objFromDb.Image;
                    }
                    _db.Product.Update(pro);
                    
                }
                _db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(pro);
        }

   

        /////////////////////////////////
        ///
         //GET- DELETE PRODUCT
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _db.Product.Include(f=>f.Category).FirstOrDefault(u=>u.Id == id);

            product.Category = _db.Category.Find(product.CategoryId);
            product.ApplicationType = _db.ApplicationType.Find(product.ApplicationId);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST-DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteById(int? id)
        {
            Product pro = _db.Product.Find(id);
            if( pro == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, pro.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _db.Product.Remove(pro);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
