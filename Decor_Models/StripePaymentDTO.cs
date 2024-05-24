using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Models
{
    public class StripePaymentDTO
    {
        public StripePaymentDTO() {
            SucssesUrl = "OrderConfirmation";
            CancelUrl = "summary";
        }
        public OrderDTO Order{ get; set; }
        public  string SucssesUrl{ get; set; }
        public  string CancelUrl { get; set; }
    }
}
