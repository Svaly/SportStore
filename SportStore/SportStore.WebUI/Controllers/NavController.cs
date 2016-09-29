using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  SportStore.Domain.Abstract;


namespace SportStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository productRepository;

        public NavController(IProductRepository repositoryParam)
        {
            productRepository = repositoryParam;
        }

        public PartialViewResult Menu(string category=null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = productRepository.Products
                                            .Select(x => x.Category)
                                              .Distinct()
                                         .OrderBy(x => x);

            return PartialView(categories);
        }
    }
}