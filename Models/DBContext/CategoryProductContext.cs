using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class CategoryProductContext
    {
        [Key]
        public int idCategoryProduct { get; set; }
        public string NameCategory { get; set; }
    }
}
