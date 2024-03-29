﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.ViewModels
{
    class AddManufactureViewModel
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
        public Order ChangedOrder { get; set; }
        public List<string> statuses { get; set; } = new List<string>();
        public string SelectedStatus { get; set; }

        public string FullName { get; set; } = Global.CurrentUser.Name == string.Empty ? "Гость" : $"{Global.CurrentUser.Surname} {Global.CurrentUser.Name} {Global.CurrentUser.Patronymic}";
        public DateTime selectedEndDate { get; set; }

        public string NameDelivery { get; set; }


        public AddManufactureViewModel(PageService pageService, AdminService adminService, DataContext context)
        {
            _context = context;
            _pageService = pageService;

            _adminService = adminService;

        }
        public DelegateCommand GoProduct => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseProduct());
        });
        public DelegateCommand GoOrders => new(() =>
        {
            _pageService.ChangePage(new AdminBrowseOrder());
        });

        public DelegateCommand AddNewDelivery => new(async () =>
        {
            ManufacturesContext delivery = new();
            delivery.NameManufactures = NameDelivery;
            _adminService.AddManufacture(delivery);
            await Task.Delay(100);
            _pageService.ChangePage(new ManufacturePage());
        });
        public DelegateCommand GoBack => new(() =>
        {
            _pageService.ChangePage(new ManufacturePage());
        });
    }
}
