using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils
{
    public class VehicleParkingModel
    {
        public int Vehicle_Parking_Id { get; set; }
        public int Parking_Zone_Id { get; set; }
        public int Parking_Space_Id { get; set; }
        public string Booking_Date_Time { get; set; }
        public string Release_Date_Time { get; set; }
        public string Registration_No { get; set; }
        public string TimeLeft { get; set; }    
    }
}
