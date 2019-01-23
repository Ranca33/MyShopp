using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> productContex = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            productContex.Insert(new Product());
            HomeController controller = new HomeController(productContex, productCategoryContext);

            var result = controller.Index() as ViewResult;
            //this is called a cast (ProductListViewModel)
            //with this we cann acces the various properties of the viewModel
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            //This test will make sure that there is 1 product in the list of products
            Assert.AreEqual(1, viewModel.Products.Count());
        }
    }
}
