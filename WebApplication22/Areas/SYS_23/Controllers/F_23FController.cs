using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23FController : Controller
    {
        TransartEntities db = new TransartEntities();
        //[HttpPost]
        public ActionResult P_23F_q(int? QryKind, DateTime? Date1, DateTime? Date2, string The_No, int? PageNumber, int? PageSize)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            if (QryKind == null)
                return View();

            DateTime d1 = Date1 ?? DateTime.Parse("1980-01-01");
            DateTime d2 = Date2 ?? DateTime.Parse("2200-12-31");
            DateTime d_from, d_to;
            string s;
            if (d1 <= d2)
            {
                d_from = d1;
                d_to = d2;
            }
            else
            {
                d_from = d2;
                d_to = d1;
            }
            QryKind = 2;

            s = "Select MT_DATE as THE_DATE, " +
                           "MT_MMNO as THE_NO, " +
                           "MM_SUBJECT as THE_SUBJECT, " +
                           "MT_ACBACKMEMO as THE_ACBACKMEMO " +
                           "from MisModReject " +
                           " Left Join MisModify on MT_MMNO = MM_NO " +
                           " where MT_DATE >= @m_Date1 and MT_DATE <= @m_Date2 " +
                           " order by MT_DATE , MT_MMNO";
            //}
            var q_RejectReason = db.Database.SqlQuery<RejectReason>(s, new SqlParameter("m_Date1", d_from),
                                                                                  new SqlParameter("m_Date2", d_to));
            List<RejectReason> rr = q_RejectReason.ToList();
            IQueryable<RejectReason> x = rr.AsQueryable();
            IOrderedQueryable<RejectReason> y = x.OrderBy(t => t.THE_DATE);
            int m_pagesize = PageSize ?? 10;
            int m_pagenumber = PageNumber ?? 1;
            IPagedList z = y.ToPagedList(m_pagenumber, m_pagesize);

            ViewBag.QryKind = QryKind;
            ViewBag.Date1 = Date1.HasValue ? Date1.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.Date2 = Date2.HasValue ? Date2.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.The_No = The_No;
            ViewBag.PageNumber = PageNumber;
            ViewBag.PageSize = PageSize;

            return View(z);
        }
        public ActionResult RejectDescription(string MM_NO)
        {
            if (MM_NO != null)
            {
                Data.Models.MisModify reason = db.MisModify.Find(MM_NO.Trim());
                if (reason != null)
                {
                    List<MisModifyHis> history = db.MisModifyHis.Where(x => x.mh_no == MM_NO).ToList();
                    RejectDetail rd = new RejectDetail
                    {
                        m_MisModify = reason,
                        m_MisModifyHis = history
                    };
                    return View(rd);
                }

            }
            return RedirectToAction("P_23F_q");
        }
    }
}