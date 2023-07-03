using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class Order : OrderContext
    {
        public string point { get; set; }

        
        public float fullPrice { get; set; }

        
        public float discount { get; set; }


        public float? discountPrice { get; set; }

        
        

        
        public List<Product> products;
        public List<int> productQuantities
        {
            get
            {
                List<int> quan = new List<int>();
                foreach(var item in products)
                {
                    quan.Add(item.ProductQuantityInStock);
                }
                return quan;

            }
        }
    }
}
