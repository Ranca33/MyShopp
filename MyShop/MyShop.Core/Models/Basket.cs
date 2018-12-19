using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket :BaseEntity
    {
        //by setting it "virtual" EF will know that whenever we try to load the basket from the db it will automatically load all the baskets item as well(LAZY LOADING)
        public virtual ICollection<BasketItem> BasketItems { get; set; }

        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }
    }
}
