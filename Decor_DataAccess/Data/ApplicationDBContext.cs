using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_DataAccess.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductPrice> ProductPrices{ get; set; }
        public  DbSet<OrderDetails> OrderDetails { get; set; }
        public  DbSet<OrderHeader> OrderHeader{ get; set; }

    }

}
