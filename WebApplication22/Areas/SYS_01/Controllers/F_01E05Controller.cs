using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_01.Models;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01E05Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01E05_a()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            return View();
        }
        public ActionResult P_01E05_e(decimal vm_seqno)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            Vehicle_M vm = Db.Vehicle_M.Where(x => x.vm_seqno == vm_seqno).First();
            return View(vm);
        }
        public ActionResult P_01E05_l(string vm_carno, DateTime? Date1, DateTime? Date2, int? PageSize, int? PageNo)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            ViewBag.vm_carno = vm_carno;
            int m_PageSize = PageSize ?? 8;
            int m_PageNo = PageNo ?? 1;
            ViewBag.PageSize = m_PageSize;
            ViewBag.PageNo = m_PageNo;

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

            string s = "select vm_seqno, vm_carno, vm_date, vm_item, vm_trip2 " +
                       " from Vehicle_M " +
                       " where 1 = 1 ";
            if (!string.IsNullOrEmpty(vm_carno))
            {
                s = s + " and vm_carno = '" + vm_carno + "' ";
            }
            s = s + " and vm_date between '" + d1.ToString("yyyy/MM/dd") + "' and '" + d2.ToString("yyyy/MM/dd") + "' " +
                    " " +
                    " Order By vm_carno , vm_trip2 ";
            List<P_01E05_l_REC> cr = Db.Database.SqlQuery<P_01E05_l_REC>(s).ToList();
            IQueryable<P_01E05_l_REC> x = cr.AsQueryable();
            IOrderedQueryable<P_01E05_l_REC> y = x.OrderBy(t => t.vm_carno).ThenBy(t => t.vm_trip2);
            IPagedList z = y.ToPagedList(m_PageNo, m_PageSize);
            return PartialView(z);
        }
        public ActionResult P_01E05_q()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            return View();
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
        public string GetCarNoMenu2(string ve_no)
        {
            string s = "select ve_no from vehicle where ve_kind = 2 order by ve_no";
            List<string> oRecordset = Db.Database.SqlQuery<string>(s).ToList();
            string sHtml = "<Select id='selCarNo' name='vm_carno' onchange='fn_LastTrip_Server()' >";
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
        public ActionResult wfl_vehicle_M_insert(Vehicle_M vm)
        {
            Db.Vehicle_M.Add(vm);
            Db.SaveChanges();
            return RedirectToAction("P_01E05_e", "F_01E05", new { vm_seqno = vm.vm_seqno });
        }
        public JsonResult qryBroker(string strSQL)
        {
            List<string> rec = Db.Database.SqlQuery<string>(strSQL).ToList();
            return new JsonResult { Data = rec };
        }
        public ActionResult wfl_vehicle_M_update(Vehicle_M vm)
        {
            //Db.Entry(vm).State = EntityState.Modified;
            //Db.SaveChanges();
            return RedirectToAction("P_01E05_e", new { vm_seqno = vm.vm_seqno });
        }
        public ActionResult wfl_vehicle_M_delete(Vehicle_M vm)
        {
            //Db.Vehicle_M.Remove(vm);
            //Db.SaveChanges();
            return RedirectToAction("P_01E05_q");
        }
    }
}