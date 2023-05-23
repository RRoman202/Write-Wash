using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using iTextSharp.xmp.impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Write_Wash.Models;

namespace Write_Wash.ViewModels
{
    internal class AddProductViewModel : BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PageService _pageService;
        public int SelectedProduct { get; set; }
        private readonly AdminService _adminService;
        private readonly ProductService _productService;
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




        

        
        public List<CategoryProductContext> ProductCategories { get; set; }

        
        
        public CategoryProductContext SelectedCategory { get; set; }


        public List<ManufacturesContext> Manufacturers { get; set; }

        
        
        public ManufacturesContext SelectedManufacturer { get; set; }


        public List<DeliveryContext> Deliveries { get; set; }

        public DeliveryContext SelectedDelivery { get; set; }

        public List<string> Units { get; set; }

        
        public string SelectedUnit { get; set; }

        
        
        public string? Title { get; set; }


        
        
        public string? Description { get; set; }

        
        public int Price { get; set; }

        
        public int CurrentDiscount { get; set; }



        
        public int MaxCount { get; set; }



        public string SelectedPath { get; set; } = "picture.png";


        public BitmapImage ImagePath { get; set; } = new(new Uri($"/Resources/Image/null.png", UriKind.Relative));

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";


        public AddProductViewModel(PageService pageService, AdminService adminService, DataContext context, ProductService productService)
        {
            _context = context;
            _pageService = pageService;
            _productService = productService;
            _adminService = adminService;
            Units = new List<string>(){ "шт", "уп" };
            Task.Run(async () =>
            {
                ProductCategories = await _productService.GetProductCategoriesAsync();
                Manufacturers = await _productService.GetManufacturersAsync();
                Deliveries = await _productService.GetDeliveriesAsync();
            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);
            


        }
        public DelegateCommand GoBack => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseProduct());
        });
        public DelegateCommand AddNewProduct => new(async () =>
        {
            if (Title != null && Description != null && SelectedCategory != null && SelectedManufacturer != null && SelectedUnit != null && SelectedDelivery != null)
            {
                _productService.AddNewProduct(new ProductContext
                {
                    ProductArticleNumber = await _productService.GenerateArticle(),
                    ProductName = Title,
                    ProductDescription = Description,
                    ProductCategory = SelectedCategory.idCategoryProduct,
                    ProductPhoto = SelectedPath,
                    ProductManufacturer = SelectedManufacturer.idManufactures,
                    ProductCost = Price,
                    CurrentDiscount = CurrentDiscount,
                    ProductQuantityInStock = MaxCount,
                    ProductUnit = SelectedUnit,
                    ProductDelivery = SelectedDelivery.idDelivery,
                });
                await Task.Delay(100);
                _pageService.ChangePage(new AdminBrowseProduct());
            }
            else
            {
                MessageBox.Show("Неправильные данные");
            }
        });
        public DelegateCommand AddPhoto => new(() =>
        {
            try
            {
                SelectedPath = _adminService.ImageSaveFileDialog();

                ImagePath = new(new Uri(Path.GetFullPath($"Resources/Image/{SelectedPath}"), UriKind.Absolute));
            }
            catch { }
        });
    }
}
