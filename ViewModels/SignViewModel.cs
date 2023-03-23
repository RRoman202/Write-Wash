using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Write_Wash;
using System.Windows.Controls;
using DevExpress.Mvvm;
using Write_Wash.Views;
using Write_Wash.Services;
using System.Windows;

namespace Write_Wash.ViewModels
{
    internal class SignViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageButton { get; set; }
        public SignViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () =>
        {
            await Task.Run(async () =>  
            {
                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    await Application.Current.Dispatcher.InvokeAsync(async () => _pageService.ChangePage(new BrowseProduct()));
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            });
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Пустые поля";
            }
            else
                ErrorMessage = string.Empty;

            if (ErrorMessage.Equals(string.Empty))
                return true; return false;
        });
        public DelegateCommand SignUpCommand => new(() => _pageService.ChangePage(new BrowseProduct()));
        public DelegateCommand SignInLaterCommand => new(() => _pageService.ChangePage(new BrowseProduct()));
        




    }
}
