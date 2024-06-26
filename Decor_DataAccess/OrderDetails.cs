﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required] 
        public int OrderHeaderId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [NotMapped]
        public virtual Product Product{ get; set; }
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
