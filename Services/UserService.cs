using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Wash.Data;
using Write_Wash.Models;

namespace Write_Wash.Services
{
    internal class UserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                Global.CurrentUser = new User
                {
                    Id = user.UserId,
                    Name = user.UserName,
                    Surname = user.UserSurName,
                    Patronymic = user.UserPatronymic,
                    Role = user.UserRole
                };

                return true;
            }
            return false;
        }
    }
}
