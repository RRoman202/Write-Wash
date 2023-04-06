using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Write_Wash.Models;

namespace Write_Wash.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<ProductContext> product = await _context.Product.ToListAsync();
                    
                    List<ManufacturesContext> pmanufactures = await _context.Manufactures.ToListAsync();

                    foreach (var item in product)
                    {
                        products.Add(new Product
                        {
                            ProductPhoto = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            ProductName = item.ProductName,
                            ProductDescription = item.ProductDescription,
                            Manufacturer = pmanufactures.SingleOrDefault(pm => pm.idManufactures == item.ProductManufacturer).NameManufactures,
                            ProductCost = item.ProductCost,
                            CurrentDiscount = item.CurrentDiscount,
                            ProductArticleNumber = item.ProductArticleNumber
                            
                        }); 
                    }
                }
                catch { }
            });
            return products;
        }
        
    }
}
