using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebProjectMechanics.API.Controllers
{
    public partial class StatusController
    {
        public class ServiceStatus : IServiceStatus
        {
            public string Status { get; set; } = "online"; // online, degraded, offline
            public Dictionary<string, string> Features { get; set; } = new Dictionary<string, string>();
            public Dictionary<string, string> Tests { get; set; } = new Dictionary<string, string>();
            public List<string> Messages { get; set; } = new List<string>();
            public DateTime LastModifiedDate { get; set; }
            public string Region { get; set; } = System.Environment.GetEnvironmentVariable("Region") ?? System.Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            public BuildVersion BuildVersion { get; set; }

            public static string Online = "online";
            public static string Degraded = "degraded";
            public static string Offline = "offline";

            public ServiceStatus()
            {
                GetBuildVersion();
            }

            private void GetBuildVersion()
            {
                var ass = Assembly.GetExecutingAssembly();
                string filePath = ass.Location;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;
                byte[] b = new byte[2048];
                System.IO.Stream s = null;
                try
                {
                    s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    s.Read(b, 0, 2048);
                }
                finally
                {
                    if (s != null)
                    {
                        s.Close();
                    }
                }
                int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
                int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
                var dt = new DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds(secondsSince1970);
                LastModifiedDate = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);

                System.Version oVer = ass?.GetName().Version;
                BuildVersion = new BuildVersion()
                {
                    MajorVersion = oVer.Major,
                    MinorVersion = oVer.Minor,
                    Build = oVer.Build,
                    Revision = oVer.Revision
                };
            }
        }
    }
}
