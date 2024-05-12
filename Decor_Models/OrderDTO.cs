using Decor_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Models
{
    public class OrderDTO
    {
        public OrderHeaderDTO OrderHeader { get; set; }
        public List<OrderDetailsDTO>OrderDetails{ get; set; }
    }
}
