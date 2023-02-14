using System.Web;
using System.Web.Mvc;

namespace WebProjectMechanics.API
    {
    public class FilterConfig
        {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
            {
            filters.Add(new HandleErrorAttribute());
            }
        }
    }
