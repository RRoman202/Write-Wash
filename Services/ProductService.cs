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
                            ProductQuantityInStock = item.ProductQuantityInStock,
                            ProductUnit = item.ProductUnit,
                            ProductDelivery = item.ProductDelivery,
                            ProductManufacturer = item.ProductManufacturer,
                            ProductCategory= item.ProductCategory,
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
        public async Task<List<CategoryProductContext>> GetProductCategoriesAsync()
        {
            return await _context.Categoryproduct.ToListAsync();
        }

        public async Task<List<ManufacturesContext>> GetManufacturersAsync()
        {
            return await _context.Manufactures.ToListAsync();
        }


        public async Task<List<DeliveryContext>> GetDeliveriesAsync()
        {
            return await _context.Delivery.ToListAsync();
        }
        public async Task<string> GenerateArticle()
        {
            string article = "";
            List<ProductContext> articles = await _context.Product.ToListAsync();

            await Task.Run(() =>
            {
                string[] symbolsArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Z" };

                bool isArticle = false;

                while (isArticle == false)
                {
                    Random rnd = new();
                    for (int i = 0; i < 5; i = i + 1)
                    {
                        article = article + symbolsArray[rnd.Next(0, symbolsArray.Length)];
                    }

                    if (articles.All(a => a.ProductArticleNumber != article))
                    {
                        isArticle = true;
                    }
                }

            });

            return article;
        }
        public async void AddNewProduct(ProductContext product)
        {
            
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async void ChangeProduct(ProductContext productDB)
        {
            ProductContext? product = await _context.Product.Where(p => p.ProductArticleNumber == productDB.ProductArticleNumber).SingleOrDefaultAsync();

            if (product != null)
            {
                product.ProductManufacturer = productDB.ProductManufacturer;
                product.ProductPhoto = productDB.ProductPhoto;
                product.CurrentDiscount = productDB.CurrentDiscount;
                product.ProductDelivery = productDB.ProductDelivery;
                product.ProductCategory = productDB.ProductCategory;
                product.ProductDescription = productDB.ProductDescription;
                product.ProductName = productDB.ProductName;
                product.ProductUnit = productDB.ProductUnit;
                product.ProductCost = productDB.ProductCost;
                product.ProductQuantityInStock = productDB.ProductQuantityInStock;
                _context.Product.Update(product);
                _context.SaveChanges();
                _context.Entry<ProductContext>(product).State = EntityState.Detached;
            }
        }
    }
}
