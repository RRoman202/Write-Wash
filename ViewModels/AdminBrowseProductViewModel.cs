using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Write_Wash.Models;
using Write_Wash.Services;
using Write_Wash.Views;

namespace Write_Wash.ViewModels
{
    internal class AdminBrowseProductViewModel : BindableBase
    {



        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Product> Products { get; set; }
        private readonly DataContext _context;
        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }

        public ObservableCollection<string> Sorts { get; set; }
        public ObservableCollection<string> Filtre { get; set; }

        public string _pattern;

        public Visibility NullProduct { get; set; }
        public Visibility OrderEllipse { get; set; }
        public string OrderProductCount { get; set; }

        public Visibility menuitems { get; set; } = Global.CurrentUser.Role == 1 || Global.CurrentUser.Role == 3 ? Visibility.Visible : Visibility.Collapsed;

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        public Visibility BasketVisible { get; set; }

        public AdminBrowseProductViewModel(PageService pageService, ProductService productService, DataContext context)
        {
            _context = context;
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () =>
            {
                Products = await _productService.GetProducts();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
                OrderEllipseCheck();
                if (OrderEllipse == Visibility.Visible) { OrderProductCount = Global.Cart.Count().ToString(); }
                
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
        void CheckNullProduct()
        {
            if (CurrentProd > 0) { NullProduct = Visibility.Hidden; } else { NullProduct = Visibility.Visible; }
        }
        void OrderEllipseCheck()
        {
            if (Global.Cart.Count() > 0) { OrderEllipse = Visibility.Visible; BasketVisible = Visibility.Visible; } else { OrderEllipse = Visibility.Collapsed; BasketVisible = Visibility.Collapsed; }
        }
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                RaisePropertyChanged(nameof(Pattern));
                UpdateProduct();
            }
        }

        public int SelectedProduct { get; set; }

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
            var currentProduct = await _productService.GetProducts();
            MaxProd = currentProduct.Count;

            if (!string.IsNullOrEmpty(SelectedFiltre))
            {
                switch (SelectedFiltre)
                {
                    case "0 - 9,99%":
                        currentProduct = currentProduct.Where(p => p.CurrentDiscount >= 0 && p.CurrentDiscount < 10).ToList();
                        break;
                    case "10 - 14,99%":
                        currentProduct = currentProduct.Where(p => p.CurrentDiscount >= 10 && p.CurrentDiscount < 15).ToList();
                        break;
                    case "15% и более":
                        currentProduct = currentProduct.Where(p => p.CurrentDiscount >= 15).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(Pattern))
                currentProduct = currentProduct.Where(p => p.ProductName.ToLower().Contains(Pattern.ToLower())).ToList();
            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentProduct = currentProduct.OrderBy(p => p.ProductCost).ToList();
                        break;
                    case "По убыванию":
                        currentProduct = currentProduct.OrderByDescending(p => p.ProductCost).ToList();
                        break;
                }
            }
            CurrentProd = currentProduct.Count;
            Products = currentProduct;
            CheckNullProduct();

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
        public DelegateCommand AdminOrder => new(() => { _pageService.ChangePage(new AdminBrowseOrder()); });
        public DelegateCommand AddProductPage => new(() => { _pageService.ChangePage(new AddProduct()); });
        public DelegateCommand ChangeProductPage => new(() =>
        {
            Global.product = Products[SelectedProduct];
            _pageService.ChangePage(new ChangeProduct()); 
        });
        public DelegateCommand DeliveryPage => new(() => { _pageService.ChangePage(new Views.Delivery()); });
        public DelegateCommand ManufacturePage => new(() => { _pageService.ChangePage(new ManufacturePage()); });
    }
}
