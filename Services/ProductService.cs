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
                            ProductArticleNumber = item.ProductArticleNumber,
                            ProductCount = 0
                            
                        }); 
                    }
                }
                catch { }
            });
            return products;
        }
        public async Task<List<Product>> GetCart()
        {
            List<Product> dbProducts = new();
            var currentProducts = await GetProducts();

            foreach (var item in Global.Cart)
            {
                var product = currentProducts.SingleOrDefault(s => s.ProductArticleNumber.Equals(item.ProductArticleNumber));
                if (product != null)
                {
                    product.ProductCount = Global.Cart.Single(s => s.ProductArticleNumber.Equals(product.ProductArticleNumber)).ProductCount;
                    dbProducts.Add(product);
                }
            }
            return dbProducts;
        }

    }
}
