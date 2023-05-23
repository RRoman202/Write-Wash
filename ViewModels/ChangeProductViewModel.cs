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
    internal class ChangeProductViewModel
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


        public List<Product> Products { get; set; }

        public string? Description { get; set; }


        public int Price { get; set; }


        public int CurrentDiscount { get; set; }


        public Product ChangeProduct { get; set; }

        public int MaxCount { get; set; }



        public string SelectedPath { get; set; } = "picture.png";


        public BitmapImage ImagePath { get; set; } = new(new Uri($"/Resources/Image/null.png", UriKind.Relative));

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";


        public ChangeProductViewModel(PageService pageService, AdminService adminService, DataContext context, ProductService productService)
        {
            _context = context;
            _pageService = pageService;
            _productService = productService;
            _adminService = adminService;
            ChangeProduct = Global.product;
            if(ChangeProduct.ProductPhoto != null && ChangeProduct.ProductPhoto != "")
            {
                SelectedPath= ChangeProduct.ProductPhoto;
                ImagePath = new(new Uri(Path.GetFullPath($"Resources/Image/{ChangeProduct.ProductPhoto}"), UriKind.Absolute));
                
            }
                
            Units = new List<string>() { "шт.", "уп." };
            
            Task.Run(async () =>
            {
                
                ProductCategories = await _productService.GetProductCategoriesAsync();
                Manufacturers = await _productService.GetManufacturersAsync();
                Deliveries = await _productService.GetDeliveriesAsync();
                
                Title = ChangeProduct.ProductName;
                Description = ChangeProduct.ProductDescription;
                Price = ChangeProduct.ProductCost;
                CurrentDiscount = ChangeProduct.CurrentDiscount;
                MaxCount = ChangeProduct.ProductQuantityInStock;
                SelectedUnit = ChangeProduct.ProductUnit;
                foreach (var d in Deliveries)
                {
                    if (ChangeProduct.ProductDelivery == d.idDelivery)
                    {
                        SelectedDelivery = d;
                    }
                }
                foreach (var m in Manufacturers)
                {
                    if (ChangeProduct.ProductManufacturer == m.idManufactures)
                    {
                        SelectedManufacturer = m;
                    }
                }
                foreach (var c in ProductCategories)
                {
                    if (ChangeProduct.ProductCategory == c.idCategoryProduct)
                    {
                        SelectedCategory = c;
                    }
                }

                

            }).WaitAsync(TimeSpan.FromMilliseconds(10))
            .ConfigureAwait(false);



        }
        public DelegateCommand GoBack => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseProduct());
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
        public DelegateCommand ChangeProductCommand => new(async () =>
        {
            if (Title != null && Description != null && SelectedCategory != null && SelectedManufacturer != null && SelectedUnit != null && SelectedDelivery != null)
            {
                if (SelectedPath == null)
                {
                    SelectedPath = "picture.png";
                }
                _productService.ChangeProduct(new ProductContext
                {
                    ProductArticleNumber = ChangeProduct.ProductArticleNumber,
                    ProductName = Title,
                    ProductDescription = Description,
                    ProductCategory = SelectedCategory.idCategoryProduct,
                    ProductPhoto = SelectedPath,
                    ProductManufacturer = SelectedManufacturer.idManufactures,
                    ProductCost = Price,
                    CurrentDiscount = CurrentDiscount,
                    ProductQuantityInStock = MaxCount,
                    ProductUnit = SelectedUnit,
                    ProductDelivery = SelectedDelivery.idDelivery
                });
                Global.product = null;

                await Task.Delay(100);

                _pageService.ChangePage(new AdminBrowseProduct());
            }
            
        });
    }
}
