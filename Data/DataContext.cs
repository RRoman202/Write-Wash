using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;
using Write_Wash.Models.DBContext;

namespace Write_Wash.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserContext> User { get; set; }

        public DbSet<ProductContext> Product { get; set; }

        public DbSet<ManufacturesContext> Manufactures { get; set; }

        public DbSet<PointContext> Point { get; set; }

        public DbSet<OrderProductContext> orderproduct { get; set; }

        public DbSet<OrderContext> order1 { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
    }
}
