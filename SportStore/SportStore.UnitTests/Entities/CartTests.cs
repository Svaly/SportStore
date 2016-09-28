using NUnit.Framework;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Entities;

namespace SportStore.Domain.Entities.Tests
{
    [TestFixture()]
    public class CartTests
    {
        [Test()]
        public void AddItemTest_AddTwoProducts_CanAddNewLinesOfTwoProducts()
        {
            Product[] testProduct = CreateTestProducts();

            Cart target = new Cart();

            target.AddItem(testProduct[0],1);
            target.AddItem(testProduct[1],1);

            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results.Length,2);
            Assert.AreEqual(results[0].Product,testProduct[0]);
            Assert.AreEqual(results[1].Product,testProduct[1]);
        }

        [Test()]
        public void AddItemTest_AddQuantityForExistingLines_Sumary11PiecesOfItem1()
        {
            Product[] testProduct = CreateTestProducts();

            Cart target = new Cart();

            target.AddItem(testProduct[0], 1);
            target.AddItem(testProduct[1], 1);
            target.AddItem(testProduct[0], 10);

            CartLine[] results = target.Lines.OrderBy(c =>c.Product.ProductId).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }


        [Test()]
        public void RemoveLineTest_Add2Product_RemoveProduct1FromCart()
        {
            Product[] testProduct = CreateTestProducts();

            Cart target = new Cart();

            target.AddItem(testProduct[0], 1);
            target.AddItem(testProduct[1], 1);
            target.AddItem(testProduct[0], 10);

            target.RemoveLine(testProduct[1]);

            Assert.AreEqual(target.Lines.Where(c =>c.Product == testProduct[1]).Count(), 0);
            Assert.AreEqual(target.Lines.Count(),1);
        }


        [Test()]
        public void ComputeTotaValueTest_AddProductsValueadAt200_CopmuteTotalValue()
        {
            Product[] testProduct = CreateTestProducts();

            Cart target = new Cart();

            target.AddItem(testProduct[0], 1);
            target.AddItem(testProduct[1], 2);

            decimal result = target.ComputeTotaValue();

            Assert.AreEqual(result,200M);

        }

        [Test()]
        public void ClearTest_Add2Products_ClearCart()
        {
            Product[] testProduct = CreateTestProducts();

            Cart target = new Cart();

            target.AddItem(testProduct[0], 1);
            target.AddItem(testProduct[1], 2);

           target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }

        public Product[] CreateTestProducts()
        {
            return new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Price = 100M},
                new Product {ProductId = 2, Name = "P2", Price = 50M}
            };

        }
    }
}