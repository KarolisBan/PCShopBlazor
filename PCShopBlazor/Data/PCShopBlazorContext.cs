using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCShopBlazor.Data;

namespace PCShopBlazor.Data
{
    public class PCShopBlazorContext : DbContext
    {
        public PCShopBlazorContext (DbContextOptions<PCShopBlazorContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Computer> Computers { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
