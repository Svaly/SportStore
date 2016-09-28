using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository productRepository;

        public CartController(IProductRepository reopsitoryParam)
        {
            productRepository = reopsitoryParam;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel {Cart = GetCart(), ReturnUrl = returnUrl});
        }

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new {returnUrl});
        }


        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductId == productId);


            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];

            if (cart==null)
             {
                cart = new Cart();
                 Session["Cart"] = cart;
             }
            return cart;
        }

    }
}