using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01E06Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01E06_q(decimal? vm_item, string CarNo, DateTime? Date1, DateTime? Date2, int? Flag, int? PageSize, int? PageNo)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            ViewBag.vm_item = vm_item;
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

            IQueryable<Vehicle_M> q = Db.Vehicle_M;
            if (!(vm_item == null))
            {
                q = q.Where(x => x.vm_item == vm_item);
            }
            if (!string.IsNullOrEmpty(CarNo))
            {
                q = q.Where(x => x.vm_carno == CarNo);
            }
            q = q.Where(x => x.vm_date >= d1 && x.vm_date <= d2);
            IOrderedQueryable<Vehicle_M> y = q.OrderBy(x => new { x.vm_carno, x.vm_trip1 });
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
    }
}