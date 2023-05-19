using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class OrderContext
    {
        [Key]
        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
        public int OrderPickupPoint { get; set; }
        public string FIO { get; set; }
        public int OrderCode { get; set; }
        public string OrderDate1 { get; set; }
        public string OrderDate2 { get; set; }
        public int OrderList { get; set; }
        public int UserId { get; set; }
    }
}
