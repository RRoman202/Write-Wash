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
        public static User CurrentUser { get; set; } = new User();
    }
}
