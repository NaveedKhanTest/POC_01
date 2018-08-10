using POC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public interface IUserService
    {
        IQueryable<User> GetUsers();
        User GetUser(long id);
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
