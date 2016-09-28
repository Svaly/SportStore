using NUnit.Framework;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers.Tests
{
    [TestFixture()]
    public class ProductControllerTests
    {
        [Test()]
        public void ListCanPaginate_ListOf5Products_DispalyTwoProductsOnSecondPage()
        {
            IProductRepository fakeRepository = CreateFakeRepository();

            ProductController controller = new ProductController(fakeRepository);
            controller.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel)controller.List(null,2).Model;

            Product[] prodArray = result.Products.ToArray();

            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name,"P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }


        [Test()]
        public void ListCanSendPaginationViewModel_ListOf5Products_ThreeItemsPerPage()
        {
            IProductRepository fakeRepository = CreateFakeRepository();
            ProductController controller = new ProductController(fakeRepository);
            controller.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel) controller.List(null,2).Model;
            PagingInfo pageInfo = result.PagingInfo;

            Assert.AreEqual(pageInfo.CurrentPage,2);
            Assert.AreEqual(pageInfo.ItemsPerPage,3);
            Assert.AreEqual(pageInfo.TotalItems,5);
            Assert.AreEqual(pageInfo.TotalPages,2);

        }

        [Test()]
        public void ListCanFilterProducts_ListOf5Products_FilterCategoryCat1()
        {
            IProductRepository fakeRepository = CreateFakeRepository();
            ProductController controller = new ProductController(fakeRepository);
            controller.PageSize = 3;

            Product[] result = ((ProductListViewModel) controller.List("Cat1", 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length,2);
            Assert.IsTrue(result[0].Name=="P1" && result[0].Category=="Cat1");
            Assert.IsTrue(result[1].Name == "P3" && result[1].Category == "Cat1");
        }

        [Test()]
        public void ListTest_ListOf5Products_GenerateCategorySpecificProductCount()
        {
            IProductRepository fakeRepository = CreateFakeRepository();
            ProductController controller = new ProductController(fakeRepository);
            controller.PageSize = 3;

            int result1 = ((ProductListViewModel) controller.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resultall = ((ProductListViewModel)controller.List(null).Model).PagingInfo.TotalItems;


            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
            Assert.AreEqual(resultall, 5);
        }




        public IProductRepository CreateFakeRepository()
        {
            IProductRepository fakeRepository = Substitute.For<IProductRepository>();

            fakeRepository.Products.Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1",Category = "Cat1"},
                 new Product {ProductId = 2, Name = "P2",Category = "Cat2"},
                  new Product {ProductId = 3, Name = "P3",Category = "Cat1"},
                   new Product {ProductId = 4, Name = "P4",Category = "Cat2"},
                    new Product {ProductId = 5, Name = "P5",Category = "Cat3"}
            });

            return fakeRepository;
        }
    }
}