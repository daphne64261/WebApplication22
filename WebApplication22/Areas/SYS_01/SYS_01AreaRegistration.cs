using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_01
{
    public class SYS_01AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SYS_01";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SYS_01_default",
                "SYS_01/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}