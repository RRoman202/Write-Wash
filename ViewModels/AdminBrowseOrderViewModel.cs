using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Services;

namespace Write_Wash.ViewModels
{
    internal class AdminBrowseOrderViewModel : BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PageService _pageService;
        public int SelectedProduct { get; set; }
        private readonly AdminService _adminService;
        public List<Order> Orders { get; set; }
        private readonly DataContext _context;
        public List<Product> Fullorder { get; set; }
        public Visibility VisibleFullOrder { get; set; } = Visibility.Hidden;
        public Visibility VisibleOrderList { get; set; } = Visibility.Visible;
        public string _pattern;
        public int ListOrderSpan { get; set; } = 2;
        public int NumberOrder { get; set; }
        public int CodeOrder { get; set; }
        public float? FullPriceOrder { get; set; }
        public string OrderProductCount { get; set; }

        public int FullOrderColumn { get; set; } = 1;
        public int FullOrderSpan { get; set; } = 1;

        

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        

        public AdminBrowseOrderViewModel(PageService pageService, AdminService adminService, DataContext context)
        {
            _context = context;
            _pageService = pageService;
           
            _adminService = adminService;
            
            Task.Run(async () =>
            {
                Orders = await _adminService.GetOrders();
                
                
            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            


        }
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser.Id = 0;
            Global.CurrentUser.Name = string.Empty;
            Global.CurrentUser.Surname = string.Empty;
            Global.CurrentUser.Patronymic = string.Empty;
            Global.CurrentUser.Role = 0;
            Global.Cart = new List<OrderProduct>();
            _pageService.ChangePage(new SignView());
        });
        public DelegateCommand GoProduct => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseProduct());
        });
        public DelegateCommand FullOrder => new(() =>
        {
            Fullorder = Orders[SelectedProduct].products;
            NumberOrder = Orders[SelectedProduct].OrderID;
            CodeOrder = Orders[SelectedProduct].OrderCode;
            FullPriceOrder = Orders[SelectedProduct].discountPrice;
            VisibleFullOrder = Visibility.Visible;
            ListOrderSpan = 1;
            
        });
        public DelegateCommand FullOrderHidden => new(() =>
        {
            VisibleFullOrder = Visibility.Hidden;
            ListOrderSpan = 2;
            FullOrderColumn = 1;
            FullOrderSpan = 1;
            VisibleOrderList = Visibility.Visible;
        });
        public DelegateCommand FullSize => new(() =>
        {
            FullOrderColumn = 0;
            FullOrderSpan = 2;
            VisibleOrderList = Visibility.Hidden;
        });
    }
}
