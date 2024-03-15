using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntityFameworkLab
{
    internal class MyContext:DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public MyContext() :base() { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = EFLab;");
            base.OnConfiguring(optionsBuilder);
        }
      
    }
    
    
}
