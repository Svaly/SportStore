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
using Microsoft.CSharp;

namespace SportStore.WebUI.Controllers.Tests
{
    [TestFixture()]
    public class NavControllerTests
    {
        [Test()]
        public void MenuTest_CanCreateCategories()
        {

            IProductRepository fakeRepository = CreateFakeRepository();

            NavController target = new NavController(fakeRepository);

            string[] results = ((IEnumerable<string>) target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length,3);
            Assert.AreEqual(results[0],"Jabłka");
            Assert.AreEqual(results[1], "Pomarańcze");
            Assert.AreEqual(results[2], "Śliwki");
        }



        [Test()]
        public void MenuTest_IndicatesSelectedCategory()
        {

            IProductRepository fakeRepository = CreateFakeRepository();

            NavController target = new NavController(fakeRepository);
            string CategoryToSelect = "Jabłka";
            string result = target.Menu(CategoryToSelect).ViewBag.SelectedCategory;
            

            Assert.AreEqual(CategoryToSelect,result);
           
        }






        public IProductRepository CreateFakeRepository()
        {
            IProductRepository fakeRepository = Substitute.For<IProductRepository>();

            fakeRepository.Products.Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1",Category = "Śliwki"},
                 new Product {ProductId = 2, Name = "P2",Category = "Jabłka"},
                  new Product {ProductId = 3, Name = "P3",Category = "Śliwki"},
                   new Product {ProductId = 4, Name = "P4",Category = "Pomarańcze"},
                    new Product {ProductId = 5, Name = "P5",Category = "Jabłka"}
            });

            return fakeRepository;
        }
    }
}