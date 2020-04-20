using Common.Menu;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_01.Models;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01D07Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01D07_l(string ty_no)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            string s = "select ty_no,ty_place,ty_emno,ty_name,ty_agent,em_cname," +
                             "ty_dpno,ty_dpname,ty_date1,ty_date2,ty_subject,ty_content," +
                             "ty_cuno,ty_cuname,ty_cuno1,ty_cuno2,ty_cuno3,ty_cuno4," +
                             "ty_cuname1,ty_cuname2,ty_cuname3,ty_cuname4,ty_yn,ty_salYN," +
                             "ty_date3,ty_date4,ty_meetNo,ty_meetName,ty_meetNo1,ty_meetName1," +
                             "em_cname as ty_agentName, tt_verify,tt_verify1," +
                             "null as rc_bdate,null as rc_edate,null as rc_bdate1,null as rc_edate1," +
                             "tt_no,tt_date1,tt_date2,tt_caryn,tt_carno,tt_trip1,tt_trip2,tt_trip," +
                             "tt_tripsub,tt_parksub,tt_taxi,tt_taxisub,tt_livesub," +
                             "tt_meal1,tt_meal2,tt_meal3,tt_mealsub,tt_othersub,tt_socialsub," +
                             "tt_outsub,tt_totalsub,tt_report,tt_close,tt_curr,tt_prepay,tt_exrate," +
                             "tt_prepaysub,tt_OilDate,tt_OilTrip,tt_OilCnt,tt_OilCharge,tt_OilInv," +
                             "tt_OilCharge1,tt_OilCharge2,tt_park,tt_carpass,tt_prepayB," +
                             "tt_curr2,tt_prepay2,tt_prepayB2,tt_exrate2,tt_prepaysub2," +
                             "tt_curr3,tt_prepay3,tt_prepayB3,tt_exrate3,tt_prepaysub3," +
                             "tt_finanical" +
                       " from TripApply left join dbo.GetAllEmpList() " +
                            " on ty_agent = em_no, TripReport " +
                       " where ty_no = tt_no and ty_no = '" + ty_no + "' " +
                       " ORDER BY ty_no ";
            TripApplyShow_REC t = Db.Database.SqlQuery<TripApplyShow_REC>(s).First();

            decimal ty_meetNo = t.ty_meetNo ?? 0;
            if (ty_meetNo != 0)
            {
                RoomRec r = Db.RoomRec.Find(ty_meetNo);
                t.rc_bdate = r.rc_bdate;
                t.rc_edate = r.rc_edate;
            }
            decimal ty_meetNo1 = t.ty_meetNo1 ?? 0;
            if (ty_meetNo1 != 0)
            {
                RoomRec r = Db.RoomRec.Find(ty_meetNo1);
                t.rc_bdate1 = r.rc_bdate;
                t.rc_edate1 = r.rc_edate;
            }
            string m_ShowYN = "";
            if (t.ty_yn == 1)
            {
                m_ShowYN = "<B><font color=red>國外出差</font><B>";
                //2011/11/07 By Clark 若有出差表檔案，則顯示連結
                string FileName = Server.MapPath("~/Areas/Trip/files/" + ty_no + ".xlsx");
                if (System.IO.File.Exists(FileName))
                {
                    string OpenURL = "Http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/Trip/files/" + ty_no + ".xlsx";
                    m_ShowYN = m_ShowYN + "　<span onmouseover=\"this.style.color='#FF8000'\" onmouseout=\"this.style.color='black'\" onclick=\"window.open('/Trip/files/" + ty_no + ".xlsx','','channelmode, titlebar=no ,resizable=yes ,scrollbars=yes')\"" + "style='cursor:hand' ><img src='../images/ispaper.gif' width='20' height='21' border=0  >出差表</span>";
                }
                m_ShowYN = m_ShowYN + "<br/>";
            }
            s = "select th_name from TripWith inner join employee on th_emno = em_no Where th_tyno = '" + t.ty_no + "' order by th_emno ";
            List<string> loRecordset = Db.Database.SqlQuery<string>(s).ToList();
            string str = "";
            for (int i = 0; i < loRecordset.Count; i++)
            {
                if ((i % 6) == 0)
                {
                    if (i == (loRecordset.Count - 1))
                    {
                        str = str + loRecordset[i];
                    }
                    else
                    {
                        str = str + loRecordset[i] + "、<br>";
                    }
                }
                else
                {
                    if (i == (loRecordset.Count - 1))
                    {
                        str = str + loRecordset[i];
                    }
                    else
                    {
                        str = str + loRecordset[i] + "、<br>";
                    }
                }
            }
            s = "select td_date,td_memo,td_taxisub1,td_taxisub2,td_taxisub3, " +
                " td_livesub,td_meal1,td_meal2,td_meal3," +
                " td_other,td_othersub,td_socialsub," +
                " td_type,td_hour,ho_date,td_social " +
                " from TripReportD Left Join holiday on td_date = ho_date " +
                " where td_no = '" + t.ty_no + "' " +
                " ORDER BY td_date ";
            List<TripReportDetail> rd = Db.Database.SqlQuery<TripReportDetail>(s).ToList();

            TripApplyShow_Model m = new TripApplyShow_Model
            {
                ty_no = t.ty_no,
                Rec = t,
                ShowYN = m_ShowYN,
                GetAllName = str,
                Rec1 = rd
            };
            return PartialView(m);
        }
    }
}