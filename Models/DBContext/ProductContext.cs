using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models.DBContext
{
    public class ProductContext
    {
        [Key]
        public string ProductArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public int ProductManufacturer { get; set; }
        public int ProductCost { get; set; }
        public int ProductMaxDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public int CurrentDiscount { get; set; }
        public string ProductUnit { get; set; }
        public int ProductDelivery { get; set; }
        
    }
}
