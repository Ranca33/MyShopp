using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productsCategories;

        //La apelare, cauta in cache sa vadaca daca este un cache numit products, daca nu creaza o lista de products 
        public ProductCategoryRepository()
        {
            productsCategories = cache["productsGategories"] as List<ProductCategory>;
            if (productsCategories == null)
            {
                productsCategories = new List<ProductCategory>();
            }
        }

        //Save the product in cache
        public void Commit()
        {
            cache["productsGategories"] = productsCategories;
        }

        //Insert product in list 
        public void Insert(ProductCategory p)
        {
            productsCategories.Add(p);
        }

        //check for the product by Id and updates it 
        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productsCategories.Find(p => p.Id == productCategory.Id);

            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("product Category not found");
            }

        }

        //Finding product in DataBase
        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productsCategories.Find(p => p.Id == Id);

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("product Category not found");
            }
        }

        //Return a list of Products
        public IQueryable<ProductCategory> Collection()
        {
            return productsCategories.AsQueryable();
        }


        //Checks for Products by Id and deletes it

        public void Delete(string Id)
        {
            ProductCategory productCategoryToDelete = productsCategories.Find(p => p.Id == Id);


            if (productCategoryToDelete != null)
            {
                productsCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("product Category not found");
            }

        }
    }
}
