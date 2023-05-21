using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Models;

namespace Write_Wash.Services
{
    internal class OrderService
    {
        private readonly DataContext _context;
        public OrderService(DataContext context)
        {
            _context = context;
        }
        public async void NewOrder()
        {
            
            int orderNumber = _context.Order.Max(o => o.OrderID) + 1;
            int receiptСode = _context.Order.Max(o => o.OrderCode) + 1;

            await _context.Order.AddAsync(new OrderContext
            {
                OrderID = orderNumber,
                OrderStatus = "Новый",
                OrderDate1 = "",
                OrderDate2 = "",
                OrderPickupPoint = 1,
                FIO = Global.CurrentUser != null ? $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}" : null,
                OrderCode = receiptСode
            });


            List<OrderProductContext> orderproductList = new List<OrderProductContext>();
            foreach (OrderProduct cartItem in Global.Cart)
            {
               
                orderproductList.Add(new OrderProductContext
                {
                    OrderID = orderNumber,
                    ProductArticleNumber = cartItem.ProductArticleNumber,
                    ProductCount = cartItem.ProductCount
                });

                
            }

            foreach (OrderProduct cartItem in Global.Cart)
            {
                ProductContext? product = await _context.Product.Where(p => p.ProductArticleNumber == cartItem.ProductArticleNumber).SingleOrDefaultAsync();

                if (product != null)
                {
                    product.ProductQuantityInStock -= cartItem.ProductCount;
                }
            }

            await _context.Orderproduct.AddRangeAsync(orderproductList);
            await _context.SaveChangesAsync();
        }
    }
}
