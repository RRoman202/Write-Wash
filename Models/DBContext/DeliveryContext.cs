using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class DeliveryContext
    {
        [Key]
        public int idDelivery { get; set; }
        public string NameDelivery { get; set; }
    }
}
