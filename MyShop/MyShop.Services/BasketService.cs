using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
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
    }
}
