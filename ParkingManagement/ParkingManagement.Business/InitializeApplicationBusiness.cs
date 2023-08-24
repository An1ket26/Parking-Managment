using ParkingManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Business
{
    public class InitializeApplicationBusiness
    {
        /// <summary>
        /// It removes all the transactional data from database and reset the no of Parking Zones to three named as A,B and C 
        /// and each zone consisting of ten parking spaces,
        /// reset the identity of each table.
        /// </summary>
        public static void InitializeAllDetails()
        {
            InitializeApplication.InitializeAllDetails();
        }
    }
}
