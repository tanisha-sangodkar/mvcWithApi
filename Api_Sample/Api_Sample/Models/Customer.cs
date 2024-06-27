using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Api_Sample.Models
{
    [Table("Customer")]
    public class Customer
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email_ID { get; set; }
        public string Password { get; set; }
        public byte[] ImageData { get; set; }
    }
}