﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Models
{
    public class ProductDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public bool ShopFavorites { get; set; }
        public bool CustommerFavorites { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Please select a category")]
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public ICollection<ProductPriceDTO> ProductPrices { get; set; }

    }
}
