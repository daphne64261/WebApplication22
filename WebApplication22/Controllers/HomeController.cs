using Data.Models;
using Common.Menu;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApplication22.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Session[TARGET.user_id] = "SA";
            //Session[TARGET.user_id] = "193";
            //Session[TARGET.user_id] = "361";
            //Session[TARGET.user_id] = "004";
            Session[TARGET.user_id] = "785";
            Session[TARGET.sfl_id] = "";
            ViewBag.Message = "公告訊息: Hello World";
            return View();
        }
        public ActionResult MainMenu()
        {
            string uid = (string)Session[TARGET.user_id];
            List<sysfunclist> my_level_123 = new Main_Menu(uid).Get_List();
            return PartialView(my_level_123);
        }
        public ActionResult TopButtons()
        {
            string user_id = (string)Session[TARGET.user_id];
            string sfl_id = (string)Session[TARGET.sfl_id];
            sysprivilege user_sysprivilege = SysPrivilege.Get_Privilege(user_id, sfl_id);
            return PartialView(user_sysprivilege);
        }
        public string Pdf_Url(string pdf_id)
        {
            string pdf_url = Url.Content("~/helpdoc/") + Sysfunclist_Info.Get_Help_Doc(pdf_id);
            return pdf_url;
        }
    }
}