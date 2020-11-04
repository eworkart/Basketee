using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Common;
using Basketee.API.Services;

namespace Basketee.API.UserAuthentication
{
    public class UserValidate
    {
        public static bool Login(string username, string password)
        {
            CommonUserServices _commonUserServices = new CommonUserServices();
            var UserLists = _commonUserServices.GetUsers();
            return UserLists.Any(user =>
                user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                && user.Password == password);
        }

        public static User GetUserDetails(string username, string password)
        {
            CommonUserServices _commonUserServices = new CommonUserServices();
            return _commonUserServices.GetUsers().FirstOrDefault(user =>
                user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                && user.Password == password);
        }
    }
}