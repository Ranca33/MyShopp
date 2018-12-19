using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        //load curent product and praductCategory from data base
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        //Constructor that initializes, and tells to accept 2 interfaces
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            //Every time we create an instance of our ProductManagerController it's going to want to inject
            //in a Irepository, a class that implements our Irepository product and a class that implements Irepository product category
            context = productContext;
            productCategories = productCategoryContext;

        }

        //Category = null means nr 1 you can have a null item, 2 if you don't pass anything we asume its null
        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();
            if(Category== null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                //this is why we expose the collection as IQuariable because that will then allow us to create a filter, and in EF world
                //this means the actuall called SQL will not be called untill the filter has been enumerated, in other words it would convert it 
                //into a SQL statement that will filter this category list for us rather than sending everything back
                products = context.Collection().Where(p => p.Category == Category).ToList();
            }
            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.productCategories = categories;
            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}