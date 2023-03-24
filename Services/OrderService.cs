using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            _context.order1.Add(new OrderContext
            {
                OrderID = _context.order1.Count() + 1,
                OrderStatus = "Новый",
                FIO = Global.CurrentUser.Surname + " " + Global.CurrentUser.Name + " " + Global.CurrentUser.Patronymic,
                OrderCode= _context.order1.Count(),
                OrderDate1= DateTime.Now,
                OrderDate2= DateTime.Now.AddDays(6),
                OrderList = _context.order1.Count() + 1,
                UserId = Global.CurrentUser.Id,
            });
            
            for(int i =0;i < Global.OrderProductList.ToList().Count(); i++)
            {
                _context.orderproduct.Add(new OrderProductContext
                {
                    OrderID= _context.order1.Count() + 1,
                    ProductArticleNumber = Global.OrderProductList[i].ProductArticleNumber,
                    ProductCount = Global.OrderProductList[i].ProductCount,

                });
                
            }
            _context.SaveChanges();
        }
    }
}
