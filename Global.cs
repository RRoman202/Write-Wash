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
        public static List<Product> OrderProductList { get; set; } = new List<Product>();
        
    }
}
