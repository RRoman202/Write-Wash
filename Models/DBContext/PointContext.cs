using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class PointContext
    {
        [Key]
        public int idPoint { get; set; }
        public int PointIndex { get; set; }
        public string PointCity { get; set; }
        public string PointStreet { get; set; }
        public int PointHome { get; set; }
    }
}
