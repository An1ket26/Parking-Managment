using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils.Models
{
    public class ReportParkingSpaceModel
    {
        public string ParkingSpaceName { get; set; }
        public int NoOfBooking { get; set; }
        public string IsCurrentlyVehicleparked { get; set; }
    }
}
