using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Repositories
{
    public class LoginRepo
    {
        public static string Check_Credentials(Login info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                // Check if user exists
                var queryUsername = from all in db.Users
                                    where all.userName == info.Username
                                    select all;

                // Check if password is correct
                var queryCredentials = from all in db.Users
                                       where all.passwordHash == info.Password && all.userName == info.Username
                                       select all;

                // Login only success if user exists and password is correct
                if (queryCredentials.Any())
                {
                    return "Login Success";
                }
                // Username exists but wrong password
                else if (queryUsername.Any())
                {
                    return "wrong password has been entered";
                }
                // Username does not exist
                return "user does not exist";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "database query error occured. Contact administrator.";
            }

        }

        public static UserDetails Get_UserIDs(Login info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                // Check if user exists
                var queryUsername = from all in db.Users
                                    where all.userName == info.Username
                                    select all;

                UserDetails IDBucket = new UserDetails();

                IDBucket.CustomerID = queryUsername.FirstOrDefault().userId;
                IDBucket.RoleID = queryUsername.FirstOrDefault().roleId;

                return IDBucket;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }

        }
    }
}