using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Write_Wash.Services;
using Write_Wash.Views;

namespace Write_Wash.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public Page PageSource { get; set; }

        public MainWindowViewModel(PageService pageService)
        {
            _pageService = pageService;
            _pageService.onPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new SignView());
        }
    }
}
