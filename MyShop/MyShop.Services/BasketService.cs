using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        //1 try to read the cookie from the user and look for a basket id in that cookie  
        // in order to read cookies we use HttpContext

        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        //we use a string to identify a cookie we are looking for 
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        //first method load basket, read the users cookie
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            //we read the cookie
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();
            //check if cookie exists
            if (cookie != null)
            {
                //if they have the cookie, we try to get its value (cookie.value)
                string basketId = cookie.Value;

                // a further check if the value we read from the cookie is not null or an empty string
                if (!string.IsNullOrEmpty(basketId))
                {
                    //if not then, load the basket from the basketContext
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    //if the basket Id is null we need to check if we want to create the basket, and if we do, we wil create a new basket
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            //create basket, insert it to DB
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            //we create a cookie first, we add value to cookie, we set an expiration to the cookie, we add that cookie to the httpcontext Response(we send it back to the user)
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;

        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if(item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //because we are just retrieving items if the basket doesn't actually exist, we don't want to create one
            //if there is no items in the basket at the moment we will simply return an empty in memory basket
            Basket basket = GetBasket(httpContext, false);

            //however if we have retrieved a basket, we need to querry the product table and the basket items to get the information we need
            //LINQ QUERRY(we generally put it in paranthases)
            if (basket != null)
            {                      //we tell what is the first table we wish to querry whitch in our case is a LIST
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()// we tell it what items from the two collections we want and we do that
                              {                               // by creating a new basketItem ViewModel and then tell it what items within 
                                  Id = b.Id,                  // that viewModel need to come from where
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                              }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();//empty list
            }
        }



        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if(basket != null)
            {
                //this means we can store a null value here( in the int value)
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();// will count up the quantity value of every item in our basket item table

                //we select from basketItems and Products tables that has the same id, the item quantity and the price of that item, and we sum them
                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                //if there is a basketCount return the value of that basket count if however basketCount is null return 0
                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }

        }
    }
}
