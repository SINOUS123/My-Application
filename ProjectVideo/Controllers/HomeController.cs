using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectVideo.Data;
using ProjectVideo.Models;
using ProjectVideo.Models.ViewsModels;
using ProjectVideo.utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectVideo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(f => f.Category).Include(f => f.ApplicationType),
                Categories = _db.Category
            };
            return View(homeVM);
        }


        public IActionResult Details(int? id)
        {
            List<ShoppingCart> shoppingCart = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCart = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u=>u.Category).Include(f=>f.ApplicationType)
                .Where(f => f.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };

            foreach (var shoppingCartItem in shoppingCart)
            {
                if(shoppingCartItem.ProductId == id)
                {
                    detailsVM.ExistsInCart = true; 
                }
            }
            return View(detailsVM);
        }

        //REMOVECART
        public IActionResult RemoveFromCart(int? id)
        {
            List<ShoppingCart> shoppingCart = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCart = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemRemove = shoppingCart.SingleOrDefault(r=>r.ProductId == id);
            if(itemRemove != null)
            {
                shoppingCart.Remove(itemRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCart);
            return RedirectToAction(nameof(Index));
        }

        //ADDTOCART
        [HttpPost, ActionName("Details")]
        public  IActionResult DetailsPost(int id)
        {
          List<ShoppingCart> shoppingCart = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCart = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCart.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCart);
            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
