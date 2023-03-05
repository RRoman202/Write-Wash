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

namespace Write_Wash.ViewModels
{
    internal class BrowseProductViewModel : BindableBase
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Product> Products { get; set; }

        public int MaxProd { get; set; }
        public int CurrentProd { get; set; }

        public ObservableCollection<string> Sorts { get; set; }
        public ObservableCollection<string> Filtre { get; set; }

        public string _pattern;

        public Visibility NullProduct { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";

        void Search(List<Product> prod)
        {
            try
            {
                if(Pattern != null && Pattern != "")
                {
                    List<Product> searchproducts = new List<Product>();
                    Regex regex = new Regex(Pattern.Trim(), RegexOptions.IgnoreCase);
                    foreach (Product p in prod)
                    {
                        MatchCollection matches = regex.Matches(p.Title);
                        if (matches.Count() > 0)
                        {
                            searchproducts.Add(p);
                        }
                    }
                    Products = searchproducts;
                    CurrentProd = Products.Count();
                }
                
            }
            catch { }
        }
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                RaisePropertyChanged(nameof(Pattern));
                Task.Run(async () =>
                {
                    Products = await _productService.GetProducts();
                    Search(Products);
                    Sort(Products);
                    FiltreProduct(Products);
                }).WaitAsync(TimeSpan.FromMilliseconds(10));
                
            }
        }
        public BrowseProductViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            Task.Run(async () =>
            {
                Products = await _productService.GetProducts();
                MaxProd = Products.Count();
                CurrentProd = Products.Count();
                CheckNullProduct();
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
            SelectedFiltre = Filtre[0];
            
        }
        void CheckNullProduct()
        {
            if(CurrentProd > 0)
            {
                NullProduct = Visibility.Hidden;
            }
            if(CurrentProd == 0)
            {
                NullProduct = Visibility.Visible;
            }
        }
        private string _SelectedSort { get; set; }
        void Sort(List<Product> prod)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(prod);
            if (_SelectedSort == "По возрастанию")
            {
                view.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
            }
            if (_SelectedSort == "По убыванию")
            {
                view.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
            }
            Products = view.Cast<Product>().ToList();
            CurrentProd = Products.Count();
            

        }
        void FiltreProduct(List<Product> prod)
        {
            List<Product> prodfiltre = new List<Product>();
            if(_SelectedFiltre == "0 - 9,99%")
            {
                foreach (Product product in prod)
                {
                    if (product.Discount >= 0 && product.Discount <= 9.99)
                    {
                        prodfiltre.Add(product);
                    }
                }
            }
            if (_SelectedFiltre == "10 - 14,99%")
            {
                foreach (Product product in prod)
                {
                    if (product.Discount >= 10 && product.Discount <= 14.99)
                    {
                        prodfiltre.Add(product);
                    }
                }
            }
            if (_SelectedFiltre == "15% и более")
            {
                foreach (Product product in prod)
                {
                    if (product.Discount >= 15)
                    {
                        prodfiltre.Add(product);
                    }
                }
            }
            if(_SelectedFiltre == "Все диапозоны")
            {
                prodfiltre = prod;
            }
            Products = prodfiltre;
            CurrentProd = Products.Count();
            CheckNullProduct();
        }
        public string SelectedSort
        {
            get { return _SelectedSort; }
            set {
                _SelectedSort = value;
                RaisePropertyChanged(nameof(SelectedSort));
                Task.Run(async () =>
                {
                    Products = await _productService.GetProducts();
                    Search(Products);
                    Sort(Products);
                    FiltreProduct(Products);
                }).WaitAsync(TimeSpan.FromMilliseconds(10));
                
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
                Task.Run(async () =>
                {
                    Products = await _productService.GetProducts();
                    MaxProd = Products.Count();
                    Search(Products);
                    FiltreProduct(Products);
                    Sort(Products);
                    
                }).WaitAsync(TimeSpan.FromMilliseconds(10));
                
            }
        }
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser.Id = 0;
            Global.CurrentUser.Name = string.Empty;
            Global.CurrentUser.Surname = string.Empty;
            Global.CurrentUser.Patronymic = string.Empty;
            Global.CurrentUser.Role = 0;
            _pageService.ChangePage(new SignView());
        });
    }
}
