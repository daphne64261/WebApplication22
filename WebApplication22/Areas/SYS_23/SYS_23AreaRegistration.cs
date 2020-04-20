using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_23
{
    public class SYS_23AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SYS_23";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SYS_23_default",
                "SYS_23/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}