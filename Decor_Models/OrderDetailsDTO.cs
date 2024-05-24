using Decor_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Models
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [NotMapped]
        public virtual ProductDTO Product{ get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string Size{ get; set; }
        public string ProductName { get; set; }
    }
}
