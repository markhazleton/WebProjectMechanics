namespace WebProjectMechanics.API.Controllers
{
    public partial class StatusController
    {
        /// <summary>
        /// Build Version
        /// </summary>
        public class BuildVersion
        {
            /// <summary>
            /// Major Version
            /// </summary>
            public int MajorVersion { get; set; }
            /// <summary>
            /// Minor Version
            /// </summary>
            public int MinorVersion { get; set; }
            /// <summary>
            /// Revision
            /// </summary>
            public int Revision { get; set; }
            /// <summary>
            /// Build
            /// </summary>
            public int Build { get; set; }
            /// <summary>
            /// Date of Build
            /// </summary>
            public override string ToString()
            {
                return MajorVersion.ToString() + "." + MinorVersion.ToString() + "." + Build.ToString() + "." + Revision.ToString();
            }
        }
    }
}
