using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public string ProductArticleNumber { get; set; }
        public int ProductCount { get; set; }
    }
}
