using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp
{
    internal class UserRepository
    {
        List<User> _users = new List<User>()
        {
            { new User { UserName = "ali", Password="ali123", Role="admin"} },
            { new User { UserName = "paresh", Password="paresh123", Role="member"} }
        };

        internal User FindUser(string userName, string password)
        {
            return _users.FirstOrDefault(user => user.UserName == userName && user.Password == password);
        }
    }
}