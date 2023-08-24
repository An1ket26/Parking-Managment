using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils.Models
{
    public class ReportParkingZoneModel
    {
        public string ZoneName { get; set; }
        public List<ReportParkingSpaceModel> ParkingSpaceList { get; set; }
        
    }
}
