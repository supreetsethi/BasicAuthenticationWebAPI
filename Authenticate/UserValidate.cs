using BasicAuthenticationWebAPI.BL;
using BasicAuthenticationWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicAuthenticationWebAPI.Authenticate
{
    public class UserValidate
    {
        public static bool Login(string username, string password)
        {

            UserBL user = new UserBL();
            var UserList = user.GetUsers();
            return UserList.Any(usr => usr.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && usr.Password == password);

        }
    }
}