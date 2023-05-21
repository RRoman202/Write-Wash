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
        public string _pattern;
        public int ListOrderSpan { get; set; } = 2;
        public int CodeOrder { get; set; }
        public string OrderProductCount { get; set; }

        

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
            CodeOrder = Orders[SelectedProduct].OrderCode;
            VisibleFullOrder = Visibility.Visible;
            ListOrderSpan = 1;
        });
        public DelegateCommand FullOrderHidden => new(() =>
        {
            VisibleFullOrder = Visibility.Hidden;
            ListOrderSpan = 2;
        });
    }
}
