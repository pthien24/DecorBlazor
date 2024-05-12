using Decor_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        [Required] 
        public string OrderHeaderId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [NotMapped]
        public virtual ProductDTO Product{ get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Size{ get; set; }
        [Required]
        public string ProductName { get; set; }
    }
}
