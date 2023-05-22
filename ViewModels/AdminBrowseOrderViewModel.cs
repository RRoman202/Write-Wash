using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Write_Wash.Models;
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
        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }
        public int FullOrderColumn { get; set; } = 1;
        public int FullOrderSpan { get; set; } = 1;
        public ObservableCollection<string> Sorts { get; set; }
        public ObservableCollection<string> Filtre { get; set; }


        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        

        public AdminBrowseOrderViewModel(PageService pageService, AdminService adminService, DataContext context)
        {
            _context = context;
            _pageService = pageService;
           
            _adminService = adminService;
            
            Task.Run(async () =>
            {
                Orders = await _adminService.GetOrders();
                MaxProd = Orders.Count();
                CurrentProd = Orders.Count();

            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            Sorts = new ObservableCollection<string>
            {
                new string("По возрастанию"),
                new string("По убыванию")
            };
            Filtre = new ObservableCollection<string>
            {
                new string("Все диапозоны"),
                new string("0 - 9,99%"),
                new string("10 - 14,99%"),
                new string("15% и более")
            };


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
        public DelegateCommand ChangeOrder => new(() =>
        {
            if(SelectedProduct != null)
            {
                Global.order = Orders[SelectedProduct];
                _pageService.ChangePage(new ChangeOrder());
            }
            
        });
        private string _SelectedSort { get; set; }

        public string SelectedSort
        {
            get { return _SelectedSort; }
            set
            {
                _SelectedSort = value;
                RaisePropertyChanged(nameof(SelectedSort));
                UpdateProduct();
            }
        }
        private string _SelectedFiltre { get; set; }
        public string SelectedFiltre
        {
            get { return _SelectedFiltre; }
            set
            {
                _SelectedFiltre = value;
                RaisePropertyChanged(nameof(SelectedFiltre));
                UpdateProduct();

            }
        }
        public async void UpdateProduct()
        {
            var currentProduct = await _adminService.GetOrders();
            MaxProd = currentProduct.Count;

            if (!string.IsNullOrEmpty(SelectedFiltre))
            {
                switch (SelectedFiltre)
                {
                    case "0 - 9,99%":
                        currentProduct = currentProduct.Where(p => p.discount >= 0 && p.discount < 10).ToList();
                        break;
                    case "10 - 14,99%":
                        currentProduct = currentProduct.Where(p => p.discount >= 10 && p.discount < 15).ToList();
                        break;
                    case "15% и более":
                        currentProduct = currentProduct.Where(p => p.discount >= 15).ToList();
                        break;
                }
            }
            
            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentProduct = currentProduct.OrderBy(p => p.discountPrice).ToList();
                        break;
                    case "По убыванию":
                        currentProduct = currentProduct.OrderByDescending(p => p.discountPrice).ToList();
                        break;
                }
            }
            CurrentProd = currentProduct.Count;
            Orders = currentProduct;
            

        }
    }
}
