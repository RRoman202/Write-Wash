using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserSurName { get; set; }
        public string UserName { get; set; }
        public string UserPatronymic { get; set; }
        public int UserRole { get; set; }
    }
}
