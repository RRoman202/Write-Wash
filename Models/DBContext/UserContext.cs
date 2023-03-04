using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Write_Wash.Models.DBContext
{
    public class UserContext : DbContext
    {
        [Key]
        public int UserId { get; set; }
        public string UserSurName { get; set; }
        public string UserName { get; set; }
        public string UserPatronymic { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public int UserRole { get; set; }


    }
}
