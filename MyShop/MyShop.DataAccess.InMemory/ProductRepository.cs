using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        //La apelare, cauta in cache sa vadaca daca este un cache numit products, daca nu creaza o lista de products 
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        //Save the product in cache
        public void Commit()
        {
            cache["products"] = products;
        }

        //Insert product in list 
        public void Insert(Product p)
        {
            products.Add(p);
        }

        //check for the product by Id and updates it 
        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("product not found");
            }

        }

        //Finding product in DataBase
        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("product not found");
            }
        }

        //Return a list of Products
        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }


        //Checks for Products by Id and deletes it
       
        public void Delete(string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);


            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("product not found");
            }
           
        }
    
    }
}
