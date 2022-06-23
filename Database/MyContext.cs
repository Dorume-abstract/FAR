using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Database
{
    class MyContext : DbContext
    {
        public MyContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<ProductName> ProductNames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=helper;");
           
        }
    }
}
