using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;

namespace Write_Wash
{
    public static class Global
    {
        public static User CurrentUser { get; set; } = new User
        {
            Id = 0,
            Name = string.Empty,
            Surname = string.Empty,
            Patronymic = string.Empty,
            Role = 0
        };
        public static List<OrderProduct> Cart { get; set; } = new List<OrderProduct>();
        public static Order order { get; set; } = new Order();
        public static Points point { get; set; } = new Points();
        public static Product product { get; set; } = new Product();
        
        
    }
}
