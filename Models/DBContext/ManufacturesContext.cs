using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class ManufacturesContext
    {
        [Key]
        public int idManufactures { get; set; }
        public string NameManufactures { get; set; }
    }
}
