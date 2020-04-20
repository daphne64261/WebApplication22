using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23M01Controller : Controller
    {
        TransartEntities db = new TransartEntities();
        public ActionResult P_23M01_q()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            ViewBag.select_com_kind = new COM_KIND().com_kind;
            return View();
        }
        public ActionResult P_23M01_l(string com_no1, string com_no2, int com_kind, string com_ip,
                          string com_cname, string com_no3, int? page_number, int? page_size)
        {
            string m_com_no1 = (com_no1 == null) ? "" : com_no1.Trim();
            string m_com_no2 = (com_no2 == null) ? "" : com_no2.Trim();
            int m_com_kind = com_kind;
            string m_com_ip = (com_ip == null) ? "" : com_ip.Trim();
            string m_com_cname = (com_cname == null) ? "" : com_cname.Trim();
            string m_com_no3 = (com_no3 == null) ? "" : com_no3.Trim();
            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 10;
            ViewBag.com_no1 = m_com_no1;
            ViewBag.com_no2 = m_com_no2;
            ViewBag.com_kind = m_com_kind;
            ViewBag.com_ip = m_com_ip;
            ViewBag.com_cname = m_com_cname;
            ViewBag.com_no3 = m_com_no3;
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IQueryable<Data.Models.computer> x = db.computer;
            if (!String.IsNullOrEmpty(m_com_no1))
                x = x.Where(y => y.com_no.Trim().CompareTo(m_com_no1) >= 0);
            if (!String.IsNullOrEmpty(m_com_no2))
                x = x.Where(y => y.com_no.Trim().CompareTo(m_com_no2) <= 0);
            x = x.Where(y => y.com_kind == m_com_kind);
            if (!String.IsNullOrEmpty(m_com_ip))
                x = x.Where(y => y.com_ip.Trim().Contains(m_com_ip));
            if (!String.IsNullOrEmpty(m_com_cname))
                x = x.Where(y => y.com_cname.Trim().Contains(m_com_cname));
            if (!String.IsNullOrEmpty(m_com_no3))
                x = x.Where(y => y.com_no1.Trim().Contains(m_com_no3));
            x = x.Where(y => y.com_del == 0);

            IOrderedQueryable<Data.Models.computer> w = x.OrderBy(y => y.com_no);
            IPagedList z = w.ToPagedList(m_page_number, m_page_size);
            return PartialView(z);
        }
        public ActionResult P_23M01_a()
        {
            Sysfunclist_Info.Get_sfl_id(this);
            ViewBag.select_com_kind = new COM_KIND().com_kind;
            ViewBag.select_mon_kind = new MONEY_KIND().money_kind;
            return View();
        }
        public ActionResult P_23M01_e(int com_dsn)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            Data.Models.computer c = db.computer.Find(com_dsn);
            int m_com_kind = c.com_kind ?? 1;
            string m_mon_kind = c.mon_kind ?? "NT";
            ViewBag.select_com_kind = new COM_KIND(m_com_kind).com_kind;
            ViewBag.select_mon_kind = new MONEY_KIND(m_mon_kind).money_kind;

            Edit_Model e = new Edit_Model
            {
                Computer = c
            };
            return View(e);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Edit_Model e)
        {
            if (ModelState.IsValid)
            {
                db.Entry(e.Computer).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("P_23M01_e");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Edit_Model e)
        {
            if (ModelState.IsValid)
            {
                db.computer.Add(e.Computer);
                db.SaveChanges();
            }
            return RedirectToAction("P_23M01_e");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Edit_Model e)
        {
            if (ModelState.IsValid)
            {
                db.computer.Remove(e.Computer);
                db.SaveChanges();
            }
            return RedirectToAction("P_23M01_e");
        }
        [HttpPost]
        public ActionResult DoUpload1(int com_dsn, HttpPostedFileBase picture_file)
        {
            if (picture_file != null)
            {
                string file_ext = Path.GetExtension(picture_file.FileName);
                string filename = Url.Encode("picture_" + DateTime.Now.ToString("yyyyMMddHHmmss") + file_ext);
                string my_path = Server.MapPath("~/File_Store");
                string full_path_filename = Path.Combine(my_path, filename);
                picture_file.SaveAs(full_path_filename);
                Data.Models.computer c = db.computer.Find(com_dsn);
                c.com_picture = "~/File_Store/" + filename;
                c.com_udate = DateTime.Now;
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("P_23M01_e", "F_23M01", new { com_dsn });
        }
        public ActionResult ShowList_computer(string field_id, int com_kind, int? page_number, int? page_size)
        {
            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 10;
            ViewBag.field_id = field_id;
            ViewBag.com_kind = com_kind;
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IOrderedQueryable<Data.Models.computer> w = db.computer
                                              .Where(x => x.com_del == 0 && x.com_kind == com_kind)
                                              .OrderBy(x => x.com_no);
            IPagedList z = w.ToPagedList(m_page_number, m_page_size);
            if (z.TotalItemCount > 0)
                return PartialView(z);
            else
                return RedirectToAction("P_23M01_q", "F_23M01");
        }
        public ActionResult ShowList_employee(int? page_number, int? page_size)
        {
            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 10;
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IOrderedQueryable<employee> w = db.employee
                                              .OrderBy(x => x.em_no);
            IPagedList z = w.ToPagedList(m_page_number, m_page_size);
            if (z.TotalItemCount > 0)
                return PartialView(z);
            else
                return RedirectToAction("P_23M01_q", "F_23M01");
        }
    }
}