using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Api_Sample.Models
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customer_DBtable { get; set; }

    }

}
