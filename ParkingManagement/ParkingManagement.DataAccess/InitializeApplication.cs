using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.DataAccess
{
    public class InitializeApplication
    {
        /// <summary>
        /// It removes all the transactional data from database and reset the no of Parking Zones to three named as A,B and C 
        /// and each zone consisting of ten parking spaces,
        /// reset the identity of each table.
        /// </summary>
        public static void InitializeAllDetails()
        {
            using(var dbContext = new ParkingManagementEntities())
            {
                
                dbContext.Database.ExecuteSqlCommand("Truncate table [Vehicle_Parking]");
                dbContext.Database.ExecuteSqlCommand("delete from [Parking_Space]");
                dbContext.Database.ExecuteSqlCommand("dbcc checkident (Parking_Space,reseed,0)");
                dbContext.Database.ExecuteSqlCommand("delete from [Parking_Zone]");
                dbContext.Database.ExecuteSqlCommand("dbcc checkident (Parking_Zone,reseed,0)");
                dbContext.Database.ExecuteSqlCommand("insert into [Parking_Zone] values('A'),('B'),('C')");
                for (char c = 'A'; c <= 'C'; c++)
                {
                    int zoneId=ParkingZoneData.GetParkingZoneIdByName(c.ToString());
                    for (int i = 1; i <= 10; i++)
                    {
                        string number;
                        if(i<10)
                        {
                            number = "0" + i.ToString();
                        }
                        else
                        {
                            number=i.ToString();
                        }
                        dbContext.Database.ExecuteSqlCommand($@"insert into [Parking_Space] values('{c}{number}',{zoneId})");
                    }
                }

            }
        }

    }
}
