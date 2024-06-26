﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShopFavorites { get; set; }
        public bool CustommerFavorites { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set;}
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category{ get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
