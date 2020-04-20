using Common.Menu;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_01.Controllers
{
    public class F_01E01Controller : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_01E01_q()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult P_01E01_l(vehicle v)
        {
            Sysfunclist_Info.Get_sfl_id(this);

            if (ModelState.IsValid)
            {
                string VE_DPNAME = string.IsNullOrEmpty(v.VE_DPNAME) ? "" : v.VE_DPNAME.Trim();
                decimal VE_CARTYPE = v.VE_CARTYPE ?? 0;
                decimal VE_KIND = v.VE_KIND;
                string VE_NO = string.IsNullOrEmpty(v.VE_NO) ? "" : v.VE_NO.Trim();
                string VE_PARKNO = string.IsNullOrEmpty(v.VE_PARKNO) ? "" : v.VE_PARKNO.Trim();
                List<vehicle> vvv = Db.vehicle
                                           .Where(x => ((VE_DPNAME.Length == 0) || x.VE_DPNAME.Contains(VE_DPNAME)) &&
                                                       ((VE_CARTYPE == 0) || (x.VE_CARTYPE == VE_CARTYPE)) &&
                                                       ((VE_KIND == 0) || (x.VE_KIND == VE_KIND)) &&
                                                       ((VE_NO.Length == 0) || x.VE_NO.Contains(VE_NO)) &&
                                                       ((VE_PARKNO.Length == 0) || x.VE_PARKNO.Contains(VE_PARKNO))
                                                 )
                                           .ToList();
                return View(vvv);
            }
            return RedirectToAction("P_01E01_q");
        }
        public ActionResult P_01E01_a()
        {
            Sysfunclist_Info.Get_sfl_id(this);

            return View();
        }

        //[HttpPost]
        public ActionResult P_01E01_e(decimal VE_SEQNO)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            vehicle v = Db.vehicle.Find(VE_SEQNO);
            return View(v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(vehicle v)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(v).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("P_01E01_e", new { VE_SEQNO = v.VE_SEQNO });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(vehicle v)
        {
            vehicle vvv = Db.vehicle.Find(v.VE_SEQNO);
            Db.vehicle.Remove(vvv);
            Db.SaveChanges();
            return RedirectToAction("P_01E01_q");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(vehicle v)
        {
            if (ModelState.IsValid)
            {
                Db.vehicle.Add(v);
                Db.SaveChanges();
            }
            return RedirectToAction("P_01E01_e", new { VE_SEQNO = v.VE_SEQNO });
        }
    }
}