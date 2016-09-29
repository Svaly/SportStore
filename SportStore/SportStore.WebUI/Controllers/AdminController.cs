using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

namespace SportStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository productRepository;

        public AdminController(IProductRepository repositoryParam)
        {
            productRepository = repositoryParam;
        }
   
        public ViewResult Index()
        {
         return View(productRepository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductId == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("Zapisano {0}", product.Name);
                return RedirectToAction("Index");
            }
            else
            {   //invalid data
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = productRepository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Usunięto {0}", deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}