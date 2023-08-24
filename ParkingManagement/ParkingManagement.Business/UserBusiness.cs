using ParkingManagement.DataAccess;
using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Business
{
    public class UserBusiness
    {
        /// <summary>
        /// It takes the user login details as input and cheks whether the email or password is valid or not 
        /// id it's valid it returns the user Id
        /// else it return 0 as userId.
        /// </summary>
        /// <param name="userLoginDetails"></param>
        /// <returns></returns>
        public static int ValidateUser(UserModel userLoginDetails)
        {
            return UserData.ValidateUser(userLoginDetails);
        }
        /// <summary>
        /// It takes userId as input and check s whether the user is booking agent or not and returns true or false .
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsUserBookingAgent(int userId)
        {
            return UserData.IsUserBookingAgent(userId);
        }
        /// <summary>
        /// It takes userId as input and return the name of the user if id exist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetUserName(int userId)
        {
            return UserData.GetUserName(userId);
        }
    }
}
