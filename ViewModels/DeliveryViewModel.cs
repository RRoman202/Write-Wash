using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.ViewModels
{
    internal class DeliveryViewModel
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
        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }
        public int FullOrderColumn { get; set; } = 1;
        public int FullOrderSpan { get; set; } = 1;
        public ObservableCollection<string> Sorts { get; set; }
        public ObservableCollection<string> Filtre { get; set; }
        public ObservableCollection<string> FiltreStatus { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";
        public List<DeliveryContext> Deliveries { get; set; }
        public int SelectedDelivery { get; set; }
        public string NameDelivery { get; set; }
        public DeliveryViewModel(PageService pageService, AdminService adminService, DataContext context)
        {
            _context = context;
            _pageService = pageService;

            _adminService = adminService;

            Task.Run(async () =>
            {
                Deliveries = await _adminService.GetDeliveries();

            }).WaitAsync(TimeSpan.FromMilliseconds(50))
            .ConfigureAwait(false);
            


        }
        public DelegateCommand AddDeliveryPage => new(() =>
        {
            _pageService.ChangePage(new AddDelivery());
        });
        public DelegateCommand GoBack => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseProduct());
        });
        public DelegateCommand ChangeButton => new(() =>
        {
            
        });
    }
}
