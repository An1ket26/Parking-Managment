using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.DataAccess
{
    public class UserData
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
            int userId = 0;
            using(var dbContext = new ParkingManagementEntities())
            {
                if(dbContext.User.Where(i=>i.Password== userLoginDetails.Password && i.Email == userLoginDetails.Email).Any())
                {
                    userId = dbContext.User.
                        Where(i => i.Password == userLoginDetails.Password && i.Email == userLoginDetails.Email).
                        Select(i => i.User_Id).
                        SingleOrDefault();
                }
            }
            return userId;
        }
        /// <summary>
        /// It takes userId as input and check s whether the user is booking agent or not and returns true or false .
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsUserBookingAgent(int userId)
        {
            bool flag = false;
            using(var dbContext=new ParkingManagementEntities())
            {
                if (!dbContext.User.Where(i => i.User_Id == userId).Any())
                {
                    throw new Exception("User Id does not exist to check booking Agent");
                }
                var roleIdList = dbContext.User_Roles.Where(i => i.User_Id == userId).Select(i => i.Role_Id);
                foreach(var roleid in roleIdList)
                {
                    if(dbContext.Roles.Where(i => i.Role_Id == roleid).Select(i => i.Role_Name).Single().ToString()== "Booking_Counter_Agent")
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// It takes userId as input and return the name of the user if id exist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetUserName(int userId)
        {
            string userName= "";
            using(var dbContext=new ParkingManagementEntities())
            {
                if (!dbContext.User.Where(i => i.User_Id == userId).Any())
                {
                    throw new Exception("User Id does not exist");
                }
                userName = dbContext.User.Where(i=>i.User_Id==userId).Select(i=>i.Name).SingleOrDefault();
            }
            return userName;
        }
    }
}
