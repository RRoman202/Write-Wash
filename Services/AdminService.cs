using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Write_Wash.Models;

namespace Write_Wash.Services
{
    internal class AdminService
    {
        private readonly DataContext _context;
        public AdminService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrders()
        {
            List<Order> products = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<OrderContext> product = await _context.Order.ToListAsync();
                    List<PointContext> points = await _context.Point.ToListAsync();
                    
                    foreach (var item in product)
                    {
                        List<Product> pp = await GetProduct(item.OrderID);
                        
                        products.Add(new Order
                        {
                            OrderID = item.OrderID,
                            OrderList = item.OrderList,
                            OrderDate2 = item.OrderDate2,
                            OrderDate1 = item.OrderDate1,
                            OrderCode = item.OrderCode,
                            point = points.SingleOrDefault(pm => pm.idPoint == item.OrderPickupPoint).PointIndex + points.SingleOrDefault(pm => pm.idPoint == item.OrderPickupPoint).PointCity + points.SingleOrDefault(pm => pm.idPoint == item.OrderPickupPoint).PointStreet + points.SingleOrDefault(pm => pm.idPoint == item.OrderPickupPoint).PointHome,
                            OrderStatus = item.OrderStatus,
                            FIO = item.FIO,
                            UserId = item.UserId,
                            products = pp,
                            fullPrice = GetFullPrice(pp),
                            discount = GetDiscount(pp),
                            discountPrice = GetDiscountPrice(pp)
                        }); ;
                        
                    }
                }
                catch {
                    
                }
            });
            return products;
        }
        public async Task<List<Product>> GetProduct(int orderid)
        {
            List<Product> products = new();
            await Task.Run(async () =>
            {
                try
                {
                    List<ProductContext> product = await _context.Product.ToListAsync();

                    List<ManufacturesContext> pmanufactures = await _context.Manufactures.ToListAsync();

                    List<OrderProductContext> orderproducts = await _context.Orderproduct.ToListAsync();

                    foreach(var ord in orderproducts)
                    {
                        if(ord.OrderID == orderid)
                        {
                            foreach(var item in product)
                            {
                                if(item.ProductArticleNumber == ord.ProductArticleNumber)
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
                                        ProductCount = ord.ProductCount,
                                        ProductQuantityInStock = item.ProductQuantityInStock

                                    });
                                }
                            }
                        }
                    }
                }
                catch { }
            });
            return products;
        }
        public float GetFullPrice(List<Product> products)
        {
            float fullprice = 0;
            
            foreach (var item in products)
            {
                if (item.ProductCount != 0)
                    fullprice += item.ProductCost * item.ProductCount;
                else
                    fullprice += item.ProductCost;


            }
            return fullprice;
        }
        public float GetDiscount(List<Product> products)
        {
            float disc = 0;
            float pricenondisc = 0;
            float? pricedisc = 0;
            foreach (Product p in products)
            {
                if (p.CurrentDiscount > 0)
                {
                    pricedisc += p.DisplayedPrice;
                }
                else
                {
                    pricedisc += p.ProductCost;
                }
                pricenondisc += p.ProductCost;
            }
            disc = (float)Math.Truncate((decimal)(100 - (pricedisc * 100 / pricenondisc)));
            return disc;
        }
        public float? GetDiscountPrice(List<Product> products)
        {
            float? discprice = 0;
            foreach(var item in products)
            {
                if(item.CurrentDiscount != 0)
                {
                    if(item.ProductCount != 0)
                        discprice += item.DisplayedPrice * item.ProductCount;
                    else
                    {
                        discprice += item.DisplayedPrice;
                    }
                }
                else
                {
                    if (item.ProductCount != 0)
                        discprice += item.ProductCost * item.ProductCount;
                    else
                    {
                        discprice += item.ProductCost;
                    }
                }   
            }
            return discprice;
        }
        public async void ChangeOrder(Order order)
        {
            
            OrderContext newOrder = await _context.Order.Where(o => o.OrderID == order.OrderID).FirstOrDefaultAsync();
            
            if (newOrder != null)
            {
                
                newOrder.OrderDate2 = order.OrderDate2;
                newOrder.OrderStatus = order.OrderStatus;
                _context.Order.Update(newOrder);
                await _context.SaveChangesAsync();
                _context.Entry<OrderContext>(newOrder).State = EntityState.Detached;

            }
        }
    }
}
