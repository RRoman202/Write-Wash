using DevExpress.Mvvm.UI.Native;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Write_Wash.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Write_Wash.Services;
using MaterialDesignColors;

namespace Write_Wash.ViewModels
{
    internal class BrowseProductViewModel : BindableBase
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

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        public Visibility BasketVisible { get; set; }

        public BrowseProductViewModel(PageService pageService, ProductService productService, DataContext context)
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
        public DelegateCommand BasketOpen => new(() =>
        {
            _pageService.ChangePage(new ProductOrder());


        });
        public DelegateCommand AddToOrder => new(() =>
        {
            if (Products.Count > 0)
            {
                if (Global.Cart.Count() > 0)
                {
                    int k = 0;
                    foreach (OrderProduct p in Global.Cart.ToList())
                    {
                        if (p.ProductArticleNumber == Products[SelectedProduct].ProductArticleNumber)
                        {
                            p.ProductCount++;

                        }
                        else
                        {
                            k++;
                        }
                        
                    }
                    if (k == Global.Cart.ToList().Count())
                    {
                        Global.Cart.Add(new OrderProduct
                        {
                            OrderId = _context.order1.Max(o => o.OrderID) + 1,
                            ProductArticleNumber = Products[SelectedProduct].ProductArticleNumber,
                            ProductCount = 1
                        });
                        OrderProductCount = Global.Cart.Count().ToString();
                        
                    }
                }
                else
                {
                    Global.Cart.Add(new OrderProduct
                    {
                        OrderId = _context.order1.Max(o => o.OrderID) + 1,
                        ProductArticleNumber = Products[SelectedProduct].ProductArticleNumber,
                        ProductCount = 1
                    });
                    OrderProductCount = Global.Cart.Count().ToString();
                    
                    
                }
                OrderEllipseCheck();

            }
            

        });
        void OrderEllipseCheck()
        {
            if(Global.Cart.Count() > 0) { OrderEllipse = Visibility.Visible; BasketVisible = Visibility.Visible; } else { OrderEllipse = Visibility.Collapsed; BasketVisible = Visibility.Collapsed; }
        }
    }
}
