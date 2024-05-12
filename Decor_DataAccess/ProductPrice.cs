using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess
{
    public class ProductPrice
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product{ get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
    }
}
