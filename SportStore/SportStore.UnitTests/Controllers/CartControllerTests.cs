using NUnit.Framework;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NSubstitute;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;
using Microsoft.CSharp;
using NUnit.Framework.Constraints;

namespace SportStore.WebUI.Controllers.Tests
{
    [TestFixture()]
    public class CartControllerTests
    {
        [Test()]
        public void AddToCart_CanAddProductToCart()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            Cart cart= new Cart();

            CartController target = new CartController(fakeRepository,null);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId,1);

        }

        [Test()]
        public void AddToCart_AddProduct_RouteToCartScreen()
        {
            IProductRepository fakeRepository = CreateFakeRepository();
            Cart cart = new Cart();

            CartController target = new CartController(fakeRepository, null);

            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [Test()]
        public void AddToCart_AddProduct_ViewCartContents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null,null);

            CartIndexViewModel result = (CartIndexViewModel) target.Index(cart, "myUrl").ViewData.Model;
        }


        [Test()]
        public void CheckoutTest_EmptyCart_CantProcessEmptyCart()
        {
            IOrderProcessor fakeProcessor = Substitute.For<IOrderProcessor>();

            Cart cart = new Cart();

            ShippingDetails shippingDetails = new ShippingDetails();

            CartController target = new CartController(null, fakeProcessor);

            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert that OrderProcessor don't recive call
            fakeProcessor.DidNotReceive().ProcessOrder(Arg.Any<Cart>(),Arg.Any<ShippingDetails>());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }


        [Test()]
        public void CheckoutTest_InvalidShippingDetails_CantCheckout()
        {
            IOrderProcessor fakeProcessor = Substitute.For<IOrderProcessor>();

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(null, fakeProcessor);

            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(cart, new ShippingDetails());

            fakeProcessor.DidNotReceive().ProcessOrder(Arg.Any<Cart>(), Arg.Any<ShippingDetails>());

            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }

        [Test()]
        public void CheckoutTest_ValidInformation_CanCheckoutAndSendOrder()
        {
            IOrderProcessor fakeProcessor = Substitute.For<IOrderProcessor>();

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(null, fakeProcessor);

            ViewResult result = target.Checkout(cart, new ShippingDetails());

            fakeProcessor.Received(1).ProcessOrder(Arg.Any<Cart>(), Arg.Any<ShippingDetails>());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }


        public IProductRepository CreateFakeRepository()
        {
            IProductRepository fakeRepository = Substitute.For<IProductRepository>();

            fakeRepository.Products.Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1",Category = "Śliwki"},
            });

            return fakeRepository;
        }
    }
}