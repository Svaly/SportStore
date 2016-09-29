using NUnit.Framework;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NSubstitute;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers.Tests
{
    [TestFixture()]
    public class AdminControllerTests
    {
        [Test()]
        public void IndexTest_Returns_All_Products()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            AdminController target = new AdminController(fakeRepository);

            Product[] result = ((IEnumerable<Product>) target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length,3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }


        [Test()]
        public void EditTest_CanEditProduct()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            AdminController target = new AdminController(fakeRepository);

            Product p1 =target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductId);
            Assert.AreEqual(2, p2.ProductId);
            Assert.AreEqual(3, p3.ProductId);
        }

        [Test()]
        public void EditTest_CanNotEditNonexistentProduct()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            AdminController target = new AdminController(fakeRepository);

            Product result = target.Edit(4).ViewData.Model as Product;

            Assert.IsNull(result);
        }

        [Test]
        public void EditTest_CanSaveValidChanges()
        {
            IProductRepository fakeRepository = Substitute.For<IProductRepository>();
            AdminController target = new AdminController(fakeRepository);
            Product product = new Product{Name="Test"};

            ActionResult result = target.Edit(product);

            fakeRepository.ReceivedWithAnyArgs(1);
            Assert.IsNotInstanceOf<ViewResult>(result);
        }

        [Test]
        public void EditTest_CanNotSaveInvalidChanges()
        {

            IProductRepository fakeRepository = Substitute.For<IProductRepository>();
            AdminController target = new AdminController(fakeRepository);
            Product product = new Product { Name = "Test" };

            target.ModelState.AddModelError("error","error");

            ActionResult result = target.Edit(product);

            fakeRepository.DidNotReceiveWithAnyArgs();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void DeleteTest_CanDeleteValidProducts()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            AdminController target = new AdminController(fakeRepository);

            target.Delete(1);

            fakeRepository.Received().DeleteProduct(1);

        }

        public IProductRepository CreateFakeRepository()
        {
            IProductRepository fakeRepository = Substitute.For<IProductRepository>();

            fakeRepository.Products.Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Śliwki"},
                new Product {ProductId = 2, Name = "P2", Category = "Jabłka"},
                new Product {ProductId = 3, Name = "P3", Category = "Śliwki"},
            });

            return fakeRepository;
        }
    }
}