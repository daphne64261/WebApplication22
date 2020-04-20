using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.Menu
{
    public class TARGET
    {
        public const string user_id = "user_id";
        public const string sfl_id = "sfl_id";
    }
    public class Main_Menu
    {
        private TransartEntities Db;
        private sysusergroup user_group;
        private List<string> group_menu;
        private readonly List<sysfunclist> function_list;
        public Main_Menu(string user_id)
        {
            if (!string.IsNullOrEmpty(user_id))
            {
                Db = new TransartEntities();
                user_group = Db.sysusergroup
                                   .Where(x => x.sug_emno.Equals(user_id))
                                   .FirstOrDefault();
                if (user_group != null)
                {
                    group_menu = Db.sysgroupmenu
                                .Where(x => x.sgm_sgid.Equals(user_group.sug_sgid))
                                .Select(x => x.sgm_sflid)
                                .ToList();
                    if (group_menu.Count > 0)
                    {
                        function_list = Db.sysfunclist
                                    .Where(x => group_menu.Contains(x.sfl_id))
                                    .OrderBy(x => new { x.sfl_level, x.sfl_id })
                                    .ToList();
                    }
                }
            }
        }
        public List<sysfunclist> Get_List()
        {
            return function_list;
        }
    }

    public class Sysfunclist_Info
    {
        public static string Get_sfl_id(Controller c)
        {
            TransartEntities Db;
            sysfunclist my_function;
            string sfl_FUNC = c.ControllerContext.RouteData.Values["controller"].ToString();
            string sfl_PAGE = c.ControllerContext.RouteData.Values["action"].ToString();
            string user_id = (string)c.Session[TARGET.user_id];
            string sfl_id = "";
            if (!string.IsNullOrEmpty(sfl_PAGE))
            {
                Db = new TransartEntities();
                my_function = Db.sysfunclist.Where(x => x.sfl_FUNC == sfl_FUNC && x.sfl_PAGE == sfl_PAGE)
                                .FirstOrDefault();
                if (my_function != null)
                {
                    sysprivilege user_sysprivilege = SysPrivilege.Get_Privilege(user_id, my_function.sfl_id);
                    if (user_sysprivilege != null)
                    {
                        sfl_id = my_function.sfl_id;
                    }
                }
            }
            c.Session[TARGET.sfl_id] = sfl_id;
            return sfl_id;
        }
        public static string Get_Help_Doc(string sfl_id)
        {
            TransartEntities Db;
            string helpdoc = "welcome.pdf";
            if (!string.IsNullOrEmpty(sfl_id) && sfl_id != "undefined")
            {
                Db = new TransartEntities();
                helpdoc = Db.sysfunclist
                            .Where(x => x.sfl_id == sfl_id)
                            .Select(x => x.sfl_helpdoc)
                            .FirstOrDefault();
            }
            return helpdoc;
        }
    }
    public class SysPrivilege
    {
        public static sysprivilege Get_Privilege(string user_id, string sfl_id)
        {
            TransartEntities Db;
            sysusergroup user_group = null;
            sysgrouppriv group_sysprivilege = null;
            sysprivilege user_sysprivilege = null;
            if (!string.IsNullOrEmpty(user_id) && !string.IsNullOrEmpty(sfl_id))
            {
                Db = new TransartEntities();
                user_group = Db.sysusergroup
                                   .Where(x => x.sug_emno.Equals(user_id))
                                   .FirstOrDefault();
                if (user_group != null)
                {
                    group_sysprivilege = Db.sysgrouppriv
                                            .Where(x => x.sgp_sgid == user_group.sug_sgid && x.sgp_sflid == sfl_id)
                                            .FirstOrDefault();
                    if (group_sysprivilege != null)
                    {
                        user_sysprivilege = Db.sysprivilege
                                                 .Where(x => x.spl_sflid == sfl_id)
                                                 .FirstOrDefault();
                        if (user_sysprivilege != null)
                        {
                            user_sysprivilege.spl_add = (int)(user_sysprivilege.spl_add ?? 0) & (int)(group_sysprivilege.sgp_add ?? 0);
                            user_sysprivilege.spl_edit = (int)(user_sysprivilege.spl_edit ?? 0) & (int)(group_sysprivilege.sgp_edit ?? 0);
                            user_sysprivilege.spl_del = (int)(user_sysprivilege.spl_del ?? 0) & (int)(group_sysprivilege.sgp_del ?? 0);
                            user_sysprivilege.spl_qry = (int)(user_sysprivilege.spl_qry ?? 0) & (int)(group_sysprivilege.sgp_qry ?? 0);
                            user_sysprivilege.spl_save = (int)(user_sysprivilege.spl_save ?? 0) & (int)(group_sysprivilege.sgp_save ?? 0);
                            user_sysprivilege.spl_cancel = (int)(user_sysprivilege.spl_cancel ?? 0) & (int)(group_sysprivilege.sgp_cancel ?? 0);
                            user_sysprivilege.spl_f1 = (int)(user_sysprivilege.spl_f1 ?? 0) & (int)(group_sysprivilege.sgp_f1 ?? 0);
                            user_sysprivilege.spl_f2 = (int)(user_sysprivilege.spl_f2 ?? 0) & (int)(group_sysprivilege.sgp_f2 ?? 0);
                            user_sysprivilege.spl_f3 = (int)(user_sysprivilege.spl_f3 ?? 0) & (int)(group_sysprivilege.sgp_f3 ?? 0);
                            user_sysprivilege.spl_f4 = (int)(user_sysprivilege.spl_f4 ?? 0) & (int)(group_sysprivilege.sgp_f4 ?? 0);
                            user_sysprivilege.spl_f5 = (int)(user_sysprivilege.spl_f5 ?? 0) & (int)(group_sysprivilege.sgp_f5 ?? 0);
                            user_sysprivilege.spl_f6 = (int)(user_sysprivilege.spl_f6 ?? 0) & (int)(group_sysprivilege.sgp_f6 ?? 0);
                            user_sysprivilege.spl_f7 = (int)(user_sysprivilege.spl_f7 ?? 0) & (int)(group_sysprivilege.sgp_f7 ?? 0);
                            user_sysprivilege.spl_f8 = (int)(user_sysprivilege.spl_f8 ?? 0) & (int)(group_sysprivilege.sgp_f8 ?? 0);
                            user_sysprivilege.spl_f9 = (int)(user_sysprivilege.spl_f9 ?? 0) & (int)(group_sysprivilege.sgp_f9 ?? 0);
                        }
                    }
                }
            }
            return user_sysprivilege;
        }
    }
}