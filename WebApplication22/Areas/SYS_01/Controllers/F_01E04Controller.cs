using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_01.Models;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01E04Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01E04_q(string dpno, string CarNo, DateTime? Date1, DateTime? Date2, int? Flag, int? PageSize, int? PageNo)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            ViewBag.dpno = dpno;
            ViewBag.CarNo = CarNo;
            if ((Date1 == null) && (Date2 == null))
            {
                Date1 = DateTime.Today.AddDays(-1);
                Date2 = DateTime.Today.AddMonths(+1).AddDays(-1);
            }
            DateTime d1 = Date1 ?? DateTime.ParseExact("1990/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime d2 = Date2 ?? DateTime.ParseExact("2200/12/31", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            if (d1 > d2)
            {
                DateTime d = d2;
                d1 = d2;
                d2 = d;
            }
            ViewBag.Date1 = d1.ToString("yyyy/MM/dd");
            ViewBag.Date2 = d2.ToString("yyyy/MM/dd");
            ViewBag.Flag = Flag ?? 1;
            int m_PageSize = PageSize ?? 10;
            int m_PageNo = PageNo ?? 1;
            ViewBag.PageSize = m_PageSize;
            ViewBag.PageNo = m_PageNo;
            string lsFieldList = " tt_carno,ty_no,ty_place,ty_emno,ty_name,ty_dpno,ty_dpname,ty_subject,tt_date1,tt_date2,tt_trip1,tt_trip2,tt_trip ";
            string lsTableList = " TripApply , TripReport , Vehicle ";
            string lsWhereList = " ty_no = tt_no and tt_caryn in (3,4) and tt_carno = ve_no ";
            if (!string.IsNullOrEmpty(dpno))
            {
                lsWhereList = lsWhereList + " AND ty_dpno = '" + dpno + "' ";
            }
            if (!string.IsNullOrEmpty(CarNo))
            {
                lsWhereList = lsWhereList + " AND tt_carno = '" + CarNo + "' ";
            }
            string s = "select " + lsFieldList + " from " + lsTableList + " where " + lsWhereList +
                       " and tt_date1 between '" + d1.ToString("yyyy/MM/dd") + "' and '" + d2.ToString("yyyy/MM/dd") + "' " +
                       " order by tt_carno, tt_date1, tt_date2 ";
            List<CarUse_REC> cr = Db.Database.SqlQuery<CarUse_REC>(s).ToList();
            IQueryable<CarUse_REC> x = cr.AsQueryable();
            IOrderedQueryable<CarUse_REC> y = x.OrderBy(t => t.tt_carno).ThenBy(t => t.tt_date1).ThenBy(t => t.tt_date2);
            IPagedList z = y.ToPagedList(m_PageNo, m_PageSize);
            return View(z);
        }
        public string GetCarNoMenu(string ve_no)
        {
            string s = "select ve_no from vehicle where ve_kind = 2 order by ve_no";
            List<string> oRecordset = Db.Database.SqlQuery<string>(s).ToList();
            string sHtml = "<Select id='selCarNo' name='selCarNo' >";
            foreach (string veno in oRecordset)
            {
                string str = "";
                if (veno == ve_no)
                {
                    str = "Selected";
                }
                sHtml = sHtml + "<option value='" + veno + "' " + str + " >" + veno + "</option>";
            }
            sHtml = sHtml + "</Select>";
            return sHtml;
        }
        public string GetSelDep(string dp_no)
        {
            string s = "select dp_no, dp_name from dept where  dp_no not in ( '000' ) Order by dp_no";
            List<dept_REC> oRecordset = Db.Database.SqlQuery<dept_REC>(s).ToList();
            string sHtml = "<Select id=selDep name=selDep >";
            foreach (dept_REC d in oRecordset)
            {
                string dpno = d.dp_no;
                string dpname = d.dp_name;
                string str = "";
                if (dpno == dp_no)
                {
                    str = "Selected";
                }
                sHtml = sHtml + "<option value='" + dpno + "' " + str + " >" + dpname + "</option>";
            }
            sHtml = sHtml + "</Select>";
            return sHtml;
        }
        public ActionResult P_01E04_r()
        {
            return View();
        }
        public ActionResult P_01E04_r1()
        {
            return View();
        }
    }
}