using System.Collections.Generic;

namespace ProjectVideo.Models.ViewsModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }

        public IEnumerable<Product> ProductList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
