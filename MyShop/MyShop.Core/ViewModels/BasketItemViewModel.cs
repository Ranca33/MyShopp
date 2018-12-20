using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    //this will bring together a couple of properties from 2 tables
    public class BasketItemViewModel
    {
        //Id of baschet Item and quantity
        public string Id { get; set; }
        public int Quantity { get; set;  }
        //from product table, prod name, price and image url
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
