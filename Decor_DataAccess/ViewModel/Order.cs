﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess.ViewModel
{
    public class Order
    {
        public OrderHeader OrderHeader{ get; set; }
        public IEnumerable<OrderDetails> OrderDetails{ get; set; }
    }
}
