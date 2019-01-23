using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> items;
        string className;

        public MockContext()
        {
            items=new List<T>();
        }

        //Save the Item in cache
        public void Commit()
        {
            return;
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

        //Returns a list(that can be queried) of items
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

    

