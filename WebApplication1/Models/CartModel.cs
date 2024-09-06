using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Context;

namespace WebApplication1.Models

{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}