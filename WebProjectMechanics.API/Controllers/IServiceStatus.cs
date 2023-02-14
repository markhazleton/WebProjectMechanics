using System.Collections.Generic;

namespace WebProjectMechanics.API.Controllers
{
    public partial class StatusController
    {
        public interface IServiceStatus
        {
            BuildVersion BuildVersion { get; set; }
            Dictionary<string, string> Features { get; set; }
            List<string> Messages { get; set; }
            string Region { get; set; }
            string Status { get; set; }
            Dictionary<string, string> Tests { get; set; }
        }
    }
}
