using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Models
{
    public class Product : ProductContext
    {
        public string DisplayedImage
        {
            get { return Path.GetFullPath($@"Resources\Image\{ProductPhoto}"); }
        }
        public string Manufacturer { get; set; }
        public float? DisplayedPrice
        {
            get
            {
                if (CurrentDiscount != 0)
                {
                    return (float)((float)ProductCost - ((float)ProductCost * (float)CurrentDiscount / 100));
                }
                else { return null; }
            }
        }
        public int ProductCount { get; set; }
        public float? FullPrice
        {
            get
            {
                if(ProductCount != 0)
                {
                    return ProductCost * ProductCount;
                }
                else
                {
                    return ProductCost;
                }
            }
        }
        public float? FullPriceDisc
        {
            get
            {
                if(CurrentDiscount != 0)
                {
                    if (ProductCount != 0)
                    {
                        return DisplayedPrice * ProductCount;
                    }
                    else
                    {
                        return DisplayedPrice;
                    }
                }
                else
                {
                    return ProductCost;
                }
            }
        }
    }
}
