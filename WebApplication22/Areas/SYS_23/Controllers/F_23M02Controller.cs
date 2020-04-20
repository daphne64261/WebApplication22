using Common.Menu;
using Data.Models;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23M02Controller : Controller
    {
        TransartEntities db = new TransartEntities();
        public ActionResult P_23M02_q(int? com_kind, string comDep,
                                        DateTime? com_bdate1, DateTime? com_bdate2,
                                        DateTime? com_udate1, DateTime? com_udate2,
                                        int? page_number, int? page_size)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            int m_com_kind = com_kind ?? 0;
            string m_com_dpname = (comDep == null) ? "" : comDep.Trim();
            ViewBag.select_com_kind = new COM_KIND(m_com_kind).com_kind;
            ViewBag.select_com_dpname = new GetComDep(m_com_dpname).ComDep;
            ViewBag.com_kind = m_com_kind;
            ViewBag.com_dpname = m_com_dpname;
            ViewBag.com_bdate1 = com_bdate1.HasValue ? com_bdate1.Value.ToString("yyyy/MM/dd") : "";
            ViewBag.com_bdate2 = com_bdate2.HasValue ? com_bdate2.Value.ToString("yyyy/MM/dd") : "";
            ViewBag.com_udate1 = com_udate1.HasValue ? com_udate1.Value.ToString("yyyy/MM/dd") : "";
            ViewBag.com_udate2 = com_udate2.HasValue ? com_udate2.Value.ToString("yyyy/MM/dd") : "";

            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 10;
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IQueryable<Data.Models.computer> x = GetComputer(m_com_kind, m_com_dpname, com_bdate1, com_bdate2, com_udate1, com_udate2);
            IOrderedQueryable<Data.Models.computer> w = x.OrderBy(y => y.com_no);
            IPagedList z = w.ToPagedList(m_page_number, m_page_size);

            ViewBag.total_money = 0;
            if (z.IsLastPage)
            {
                int total_money = x.Sum(y => y.com_money) ?? 0;
                ViewBag.total_money = total_money;
            }

            return View(z);
        }
        public ActionResult P_23M02_r(int com_kind, string com_dpname,
                                DateTime? com_bdate1, DateTime? com_bdate2,
                                DateTime? com_udate1, DateTime? com_udate2)
        {
            IQueryable<Data.Models.computer> x = GetComputer(com_kind, com_dpname, com_bdate1, com_bdate2, com_udate1, com_udate2);
            List<Data.Models.computer> rp = x.OrderBy(y => y.com_no).ToList();

            string m_com_dpname = (com_dpname == null) ? "" : com_dpname.Trim();
            string m_com_bdate1 = com_bdate1.HasValue ? com_bdate1.Value.ToString("yyyy/MM/dd") : "";
            string m_com_bdate2 = com_bdate2.HasValue ? com_bdate2.Value.ToString("yyyy/MM/dd") : "";
            string m_com_udate1 = com_udate1.HasValue ? com_udate1.Value.ToString("yyyy/MM/dd") : "";
            string m_com_udate2 = com_udate2.HasValue ? com_udate2.Value.ToString("yyyy/MM/dd") : "";
            string m_com_kind1 = new COM_KIND(com_kind).com_kind
                                              .Where(s => s.Selected)
                                              .FirstOrDefault()
                                              .Text;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorkbook workbook = Ep.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Cells.Style.Font.Name = "標楷體";
            sheet.Column(1).Width = 16;
            sheet.Column(2).Width = 16;
            sheet.Column(3).Width = 16;
            sheet.Column(4).Width = 16;
            sheet.Column(5).Width = 16;
            sheet.Column(6).Width = 16;
            sheet.Column(7).Width = 16;
            sheet.Column(8).Width = 16;
            sheet.Column(9).Width = 16;
            sheet.Column(10).Width = 16;
            sheet.Column(11).Width = 16;
            sheet.Column(12).Width = 16;
            sheet.Column(13).Width = 16;
            sheet.Column(14).Width = 16;
            sheet.Column(15).Width = 16;
            int i = 1;
            sheet.Cells[i, 1, i, 15].Style.Font.Size = 16;
            sheet.Cells[i, 1, i, 15].Merge = true;
            sheet.Cells[i, 1].Value = "電腦設備資料";
            i++;
            sheet.Cells[i, 1, i, 15].Style.Font.Size = 14;
            sheet.Cells[i, 1, i, 15].Merge = true;
            sheet.Cells[i, 1].Value = "類別：[" + m_com_kind1 + "] " +
                                      "部門：[" + m_com_dpname + "] " +
                                      "購買日期：[" + m_com_bdate1 + "] " +
                                      "～[" + m_com_bdate2 + "] " +
                                      "異動日期：[" + m_com_udate1 + "] " +
                                      "～[" + m_com_udate2 + "]";
            sheet.Cells[1, 1, i, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[1, 1, i, 15].Style.Fill.BackgroundColor.SetColor(Color.Blue);
            sheet.Cells[1, 1, i, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Cells[1, 1, i, 15].Style.Font.Color.SetColor(Color.White);
            i++;
            sheet.Cells[i, 1, i, 15].Style.Font.Size = 12;
            sheet.Cells[i, 1, i, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[i, 1, i, 15].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            sheet.Cells[i, 1, i, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[i, 1].Value = "設備編號";
            sheet.Cells[i, 2].Value = "設備名稱";
            sheet.Cells[i, 3].Value = "資產編號";
            sheet.Cells[i, 4].Value = "配置IP";
            sheet.Cells[i, 5].Value = "作業系統";
            sheet.Cells[i, 6].Value = "購買日期";
            sheet.Cells[i, 7].Value = "歸屬部門";
            sheet.Cells[i, 8].Value = "使用者";
            sheet.Cells[i, 9].Value = "放置地點";
            sheet.Cells[i, 10].Value = "用途";
            sheet.Cells[i, 11].Value = "購買金額";
            sheet.Cells[i, 12].Value = "購買廠商";
            sheet.Cells[i, 13].Value = "本機使用者";
            sheet.Cells[i, 14].Value = "最後異動日期";
            sheet.Cells[i, 15].Value = "最後清潔日期";
            i++;
            sheet.Cells[i, 1, i + rp.Count, 15].Style.Font.Size = 12;
            sheet.Cells[i, 1, i + rp.Count, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            foreach (var c in rp)
            {
                sheet.Cells[i, 1].Value = c.com_no.Trim();
                sheet.Cells[i, 2].Value = c.com_name == null ? "" : c.com_name.Trim();
                sheet.Cells[i, 3].Value = c.com_no1 == null ? "" : c.com_no1.Trim();
                sheet.Cells[i, 4].Value = c.com_ip == null ? "" : c.com_ip.Trim();
                sheet.Cells[i, 5].Value = c.com_os == null ? "" : c.com_os.Trim();
                sheet.Cells[i, 6].Value = c.com_bdate.HasValue ? c.com_bdate.Value.ToString("yyyy-MM-dd") : "";
                sheet.Cells[i, 7].Value = c.com_dpname == null ? "" : c.com_dpname.Trim();
                sheet.Cells[i, 8].Value = c.com_cname == null ? "" : c.com_cname.Trim();
                sheet.Cells[i, 9].Value = c.com_place == null ? "" : c.com_place.Trim();
                sheet.Cells[i, 10].Value = c.com_use == null ? "" : c.com_use.Trim();
                sheet.Cells[i, 11].Value = c.com_money.ToString() + " " + c.mon_kind;
                sheet.Cells[i, 12].Value = c.com_company == null ? "" : c.com_company.Trim();
                sheet.Cells[i, 13].Value = c.com_account == null ? "" : c.com_account.Trim();
                sheet.Cells[i, 14].Value = c.com_udate.HasValue ? c.com_udate.Value.ToString("yyyy-MM-dd") : "";
                sheet.Cells[i, 15].Value = c.com_ddate.HasValue ? c.com_ddate.Value.ToString("yyyy-MM-dd") : "";
                i++;
            }
            byte[] byteExcel = Ep.GetAsByteArray();

            return File(byteExcel,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Url.Encode("電腦設備資料" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));

        }
        private IQueryable<Data.Models.computer> GetComputer(int com_kind, string com_dpname,
                                        DateTime? com_bdate1, DateTime? com_bdate2,
                                        DateTime? com_udate1, DateTime? com_udate2)
        {
            IQueryable<Data.Models.computer> q = db.computer;
            if (com_kind > 0)
                q = q.Where(y => y.com_kind == com_kind);
            if (!String.IsNullOrEmpty(com_dpname))
                q = q.Where(y => y.com_dpname.Trim().CompareTo(com_dpname) == 0);
            if (com_bdate1.HasValue && !com_bdate2.HasValue)
                q = q.Where(y => y.com_bdate >= com_bdate1);
            else if (!com_bdate1.HasValue && com_bdate2.HasValue)
                q = q.Where(y => y.com_bdate <= com_bdate2);
            else if (com_bdate1.HasValue && com_bdate2.HasValue)
            {
                DateTime? d_from = com_bdate1;
                DateTime? d_to = com_bdate2;
                if (com_bdate1 > com_bdate2)
                {
                    d_from = com_bdate2;
                    d_to = com_bdate1;
                }
                q = q.Where(y => y.com_bdate >= d_from &&
                                 y.com_bdate <= d_to);
            }
            if (com_udate1.HasValue && !com_udate2.HasValue)
                q = q.Where(y => y.com_udate >= com_udate1);
            else if (!com_udate1.HasValue && com_udate2.HasValue)
                q = q.Where(y => y.com_udate <= com_udate2);
            else if (com_udate1.HasValue && com_udate2.HasValue)
            {
                DateTime? d_from = com_udate1;
                DateTime? d_to = com_udate2;
                if (com_udate1 > com_udate2)
                {
                    d_from = com_udate2;
                    d_to = com_udate1;
                }
                q = q.Where(y => y.com_udate >= d_from &&
                                 y.com_udate <= d_to);
            }
            q = q.Where(y => y.com_del == 0);
            return q;
        }
    }
}