using Common.Menu;
using Data.Models;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23M03Controller : Controller
    {
        TransartEntities db = new TransartEntities();
        public ActionResult P_23M03_q(int? com_kind,
                                        DateTime? com_ddate1, DateTime? com_ddate2,
                                        int? page_number, int? page_size)
        {
            Sysfunclist_Info.Get_sfl_id(this);
            int m_com_kind = com_kind ?? 0;
            ViewBag.select_com_kind = new COM_KIND(m_com_kind).com_kind;
            ViewBag.com_kind = m_com_kind;
            ViewBag.com_ddate1 = com_ddate1.HasValue ? com_ddate1.Value.ToString("yyyy/MM/dd") : "";
            ViewBag.com_ddate2 = com_ddate2.HasValue ? com_ddate2.Value.ToString("yyyy/MM/dd") : "";

            int m_page_number = page_number ?? 1;
            int m_page_size = page_size ?? 10;
            ViewBag.page_number = m_page_number;
            ViewBag.page_size = m_page_size;

            IQueryable<Data.Models.computer> x = GetComputer(m_com_kind, com_ddate1, com_ddate2);
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
        public ActionResult P_23M03_r(int com_kind,
                          DateTime? com_ddate1, DateTime? com_ddate2)
        {
            IQueryable<Data.Models.computer> x = GetComputer(com_kind, com_ddate1, com_ddate2);
            List<Data.Models.computer> rp = x.OrderBy(y => y.com_no).ToList();

            string m_com_ddate1 = com_ddate1.HasValue ? com_ddate1.Value.ToString("yyyy/MM/dd") : "";
            string m_com_ddate2 = com_ddate2.HasValue ? com_ddate2.Value.ToString("yyyy/MM/dd") : "";
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

            int i = 1;
            sheet.Cells[i, 1, i, 9].Style.Font.Size = 16;
            sheet.Cells[i, 1, i, 9].Merge = true;
            sheet.Cells[i, 1].Value = "報廢設備資料";
            i++;
            sheet.Cells[i, 1, i, 9].Style.Font.Size = 14;
            sheet.Cells[i, 1, i, 9].Merge = true;
            sheet.Cells[i, 1].Value = "類別：[" + m_com_kind1 + "] " +
                                      "報廢日期：[" + m_com_ddate1 + "] " +
                                      "～[" + m_com_ddate2 + "] ";
            sheet.Cells[1, 1, i, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[1, 1, i, 9].Style.Fill.BackgroundColor.SetColor(Color.Blue);
            sheet.Cells[1, 1, i, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Cells[1, 1, i, 9].Style.Font.Color.SetColor(Color.White);
            i++;
            sheet.Cells[i, 1, i, 9].Style.Font.Size = 12;
            sheet.Cells[i, 1, i, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[i, 1, i, 9].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            sheet.Cells[i, 1, i, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[i, 1].Value = "報廢日期";
            sheet.Cells[i, 2].Value = "設備編號";
            sheet.Cells[i, 3].Value = "設備名稱";
            sheet.Cells[i, 4].Value = "購買日期";
            sheet.Cells[i, 5].Value = "歸屬部門";
            sheet.Cells[i, 6].Value = "用途";
            sheet.Cells[i, 7].Value = "購買金額";
            sheet.Cells[i, 8].Value = "報廢原因";
            sheet.Cells[i, 9].Value = "提報人";
            i++;
            sheet.Cells[i, 1, i + rp.Count, 9].Style.Font.Size = 12;
            sheet.Cells[i, 1, i + rp.Count, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            foreach (var c in rp)
            {
                sheet.Cells[i, 1].Value = c.com_ddate.HasValue ? c.com_ddate.Value.ToString("yyyy-MM-dd") : "";
                sheet.Cells[i, 2].Value = c.com_no == null ? "" : c.com_no.Trim();
                sheet.Cells[i, 3].Value = c.com_name == null ? "" : c.com_name.Trim();
                sheet.Cells[i, 4].Value = c.com_bdate.HasValue ? c.com_bdate.Value.ToString("yyyy-MM-dd") : "";
                sheet.Cells[i, 5].Value = c.com_dpname == null ? "" : c.com_dpname.Trim();
                sheet.Cells[i, 6].Value = c.com_use == null ? "" : c.com_use.Trim();
                sheet.Cells[i, 7].Value = c.com_money.ToString() + " " + c.mon_kind;
                sheet.Cells[i, 8].Value = c.com_dreason == null ? "" : c.com_dreason.Trim();
                sheet.Cells[i, 9].Value = c.com_dname1 == null ? "" : c.com_dname1.Trim();
                i++;
            }
            byte[] byteExcel = Ep.GetAsByteArray();

            return File(byteExcel,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Url.Encode("報廢設備資料" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));

        }
        private IQueryable<Data.Models.computer> GetComputer(int com_kind,
                                        DateTime? com_ddate1, DateTime? com_ddate2)
        {
            IQueryable<Data.Models.computer> q = db.computer;
            if (com_kind > 0)
                q = q.Where(y => y.com_kind == com_kind);
            if (com_ddate1.HasValue && !com_ddate2.HasValue)
                q = q.Where(y => y.com_ddate >= com_ddate1);
            else if (!com_ddate1.HasValue && com_ddate2.HasValue)
                q = q.Where(y => y.com_ddate <= com_ddate2);
            else if (com_ddate1.HasValue && com_ddate2.HasValue)
            {
                DateTime? d_from = com_ddate1;
                DateTime? d_to = com_ddate2;
                if (com_ddate1 > com_ddate2)
                {
                    d_from = com_ddate2;
                    d_to = com_ddate1;
                }
                q = q.Where(y => y.com_ddate >= d_from &&
                                 y.com_ddate <= d_to);
            }
            q = q.Where(y => y.com_del == 1);
            return q;
        }
    }
}