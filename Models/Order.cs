using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public int OrderPickupPoint { get; set; }
        public string fio { get; set; }
        public int OrderCode { get; set; }
        public string OrderDate1 { get; set; }
        public string OrderDate2 { get; set; }
        public int OrderList { get; set; }
        public int UserId { get; set; }
    }
}
