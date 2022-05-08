using System.Collections.Generic;

namespace ProjectVideo.Models.ViewsModels
{
    public class HomeVM
    {
        public IEnumerable<Product> Products {get; set;}
        public IEnumerable<Category> Categories {get; set;}
    }
}
