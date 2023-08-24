using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils
{
    public class ParkingSpaceModel
    {
        public int Parking_Space_Id { get; set; }
        public string Parking_Space_Title { get; set; }
        public int Parking_Zone_Id { get; set; }
        public string IsBooked { get; set; }
        public string BookedRegistrationNumber { get; set; }
        public int BookedId { get; set; }

        public string ReleaseTime { get; set; }
    }
}
