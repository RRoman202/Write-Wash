using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    
                    foreach (var item in product)
                    {
                        products.Add(new Order
                        {
                            OrderId = item.OrderID,
                            OrderList = item.OrderList,
                            OrderDate2 = item.OrderDate2,
                            OrderDate1 = item.OrderDate1,
                            OrderCode = item.OrderCode,
                            OrderPickupPoint = item.OrderPickupPoint,
                            OrderStatus = item.OrderStatus,
                            fio = item.FIO,
                            UserId= item.UserId,

                        });
                        
                    }
                }
                catch {
                    
                }
            });
            return products;
        }
        
    }
}
