using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebProjectMechanics.API.Controllers
{
    public partial class StatusController : ApiController
    {

        /// <summary>
        /// Get the Status of the Core API
        /// </summary>
        /// <returns></returns>
        [Route("tm/status")]
        public HttpResponseMessage GetStatus()
        {
            var status = new ServiceStatus();
            try
            {
                status.Tests.Add("Mineral Collection database", "Success");
            }
            catch (Exception EE)
            {
                status.Tests.Add("Mineral Collection database", "Failure");
                status.Messages.Add(EE.ToString());
            }

            // Implement override header parameter to facilitate TM monitoring testing
            var headers = this.Request.Headers;
            string serviceStatusOverride = null;
            try
            {
                // Should only have a single header value
                var overRideHeader = headers?.FirstOrDefault(x => x.Key.Equals("X-STATUS-OVERRIDE", StringComparison.OrdinalIgnoreCase));
                var overrideHeaderValue = overRideHeader?.Value?.ToString();
                if (string.IsNullOrEmpty(overrideHeaderValue) == false)
                {
                    serviceStatusOverride = overrideHeaderValue;
                }
            }
            catch { }
            if (string.IsNullOrEmpty(serviceStatusOverride) == false)
            {
                status.Messages.Add($"Overriding service status as requested by header X-STATUS-OVERRIDE: '{serviceStatusOverride}'");
                status.Status = serviceStatusOverride;
                return Request.CreateResponse(serviceStatusOverride.ToLower() == "online" ? HttpStatusCode.OK : HttpStatusCode.BadRequest, status);
            }
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }
    }
}
