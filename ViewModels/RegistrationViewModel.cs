using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.ViewModels
{
    internal class RegistrationViewModel : BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DataContext _context;
        private readonly PageService _pageService;
        
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        

        public RegistrationViewModel(PageService pageService, DataContext context)
        {
            _context = context;
            _pageService = pageService;
            

        }
        public DelegateCommand Back => new(() =>
        {
            _pageService.ChangePage(new SignView());
        });
        public DelegateCommand Reg => new(() =>
        {
            try
            {
                if(password == password2)
                {
                    _context.User.Add(new UserContext
                    {
                        UserName = name,
                        UserSurName = surname,
                        UserPatronymic = patronymic,
                        UserLogin = login,
                        UserPassword = password,
                        UserRole = 2,
                        UserId = _context.User.Count() + 1
                    });
                    _context.SaveChanges();
                    _pageService.ChangePage(new SignView());
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }
            }
            catch
            {
                MessageBox.Show("Неправильные данные");
            }
        });
    }
}
