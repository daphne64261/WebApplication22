using Common.Menu;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_01.Models;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01E03Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01E03_q(string The_YM, string The_DPNO, string loginID, string message)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            if (string.IsNullOrEmpty(The_YM))
            {
                if (!string.IsNullOrEmpty(The_DPNO))
                {
                    string SQL = "";
                    SQL = SQL + " Select Top 1 Convert( char(7) ,ty_date1 ,111) as YM  			";
                    SQL = SQL + " 	from TripApply , TripReport , Vehicle                       ";
                    SQL = SQL + " 	Where ty_no = tt_no and tt_caryn = 4 and tt_carverify = 0 	";
                    SQL = SQL + " 		and tt_carno = ve_no and ve_dpno = '" + The_DPNO + "'	";
                    SQL = SQL + " 		Order By ty_date1                                       ";
                    string t = Db.Database.SqlQuery<string>(SQL).FirstOrDefault();
                    if (t != null)
                        The_YM = t;
                    else
                        The_YM = DateTime.Today.ToString("yyyy/MM");
                }
                else
                    The_YM = DateTime.Today.ToString("yyyy/MM");
            }

            string The_LastYM, The_NextYM;
            DateTime dt;
            dt = DateTime.ParseExact(The_YM + "/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            The_LastYM = dt.AddMonths(-1).ToString("yyyy/MM");
            The_NextYM = dt.AddMonths(+1).ToString("yyyy/MM");

            if (string.IsNullOrEmpty(The_DPNO))
            {
                loginID = (string)Session[TARGET.user_id];
                employee e = Db.employee.Find(loginID);
                The_DPNO = e.em_dpno;
            }

            ViewBag.The_YM = The_YM;
            ViewBag.The_LastYM = The_LastYM;
            ViewBag.The_NextYM = The_NextYM;
            ViewBag.The_DPNO = The_DPNO;
            ViewBag.loginID = loginID;
            ViewBag.message = message;

            string s = "select tt_carno, ty_no, ty_place, ty_emno, ty_name, ty_dpno, ty_dpname, " +
                        " ty_date1, ty_date2, ty_subject, ty_content, ty_cuno, ty_cuname, tt_trip, " +
                        " tt_totalsub, tt_carverify " +
                        " from TripApply , TripReport , Vehicle " +
                        " where ty_no = tt_no and Convert( char(7) ,ty_date1 ,111) = '" + The_YM + "' " +

                        " and tt_caryn = 4 and tt_carverify = 0 and tt_carno = ve_no and ve_dpno = '" + The_DPNO + "' " +
                        //" and tt_caryn = 4 and tt_carverify = 0 and tt_carno = ve_no  " +

                        " order by ty_no ";
            List<P_01E03_q_REC> rs = Db.Database.SqlQuery<P_01E03_q_REC>(s).ToList();
            List<P_01E03_q_Model> m = new List<P_01E03_q_Model>();
            foreach (P_01E03_q_REC t in rs)
            {
                P_01E03_q_Model d = new P_01E03_q_Model()
                {
                    Rec = t
                };
                m.Add(d);
            }
            return View(m);
        }
        public ActionResult wfl_TripApply_CarVerify(List<P_01E03_q_Model> m)
        {
            foreach (P_01E03_q_Model t in m)
            {
                string s = "Update  TripReport set tt_carverify = " + t.Rec.tt_carverify +
                           " where  tt_no = '" + t.Rec.ty_no + "'";
                Db.Database.ExecuteSqlCommand(s);
            }
            Db.SaveChanges();
            return RedirectToAction("cmre03_q");
        }
    }
}