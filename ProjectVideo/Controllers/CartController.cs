using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectVideo.Data;
using ProjectVideo.Models;
using ProjectVideo.Models.ViewsModels;
using ProjectVideo.utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ProjectVideo.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext _db;

        [BindProperty]
        public ProductUserVM ProductUserVm { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> products = _db.Product.Where(x => productCart.Contains(x.Id));
            return View(products);
        }


        [HttpPost]
         [ValidateAntiForgeryToken]
         [ActionName("Index")]
        public IActionResult IndexPost()
        {
           return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> products = _db.Product.Where(x => productCart.Contains(x.Id));

            ProductUserVm = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(f=>f.Id == claims.Value), 
                ProductList = products 

            };

            return View(ProductUserVm);
        }


        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(r=>r.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
