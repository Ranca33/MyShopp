using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    //(where T : BaseEntity) whenever we pass in an object " here <T> " it must be the type BaseEntity or at least inherit from base
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            // Using Reflection to get the names of classes(getting our actual name of our class)
            className = typeof(T).Name;

            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        //Save the Item in cache
        public void Commit()
        {
            cache[className] = items;
        }


        //Insert Item in List
        public void Insert(T t)
        {
            items.Add(t);
        }

        //Check for the item by Id and updates item
        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + "Not found");
            }
        }

        //Find item in DataBase
        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);

            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + "Not found");
            }
        }

        //Returns a list of items
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();

        }

        //Check items by Id and deletes it
        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + "Not found");
            }
        }

    }
}
