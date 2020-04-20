using Common.Menu;
using Data.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23EController : Controller
    {
        TransartEntities Db = new TransartEntities();
        public ActionResult P_23E_q(int? this_year)
        {
            Sysfunclist_Info.Get_sfl_id(this);

            if (this_year == null)
                this_year = DateTime.Now.Year;

            string s = "Select H.Y as Y, H.M as M, isNull(A.Count1, 0) as Count1 from " +
                       " (Select distinct year(ho_date) as Y, month(ho_date) as M " +
                       "  from holiday where year(ho_date) = @m_year ) H " +
                       " Left Join ( Select year(mm_rcdate) as Y, month(mm_rcdate) as M, Count( * ) as Count1 " +
                       "             from MisModify where mm_close = 1 " +
                       "             group by year(mm_rcdate), month(mm_rcdate) ) A " +
                       " on H.Y = A.Y and H.M = A.M" +
                       " order by H.Y, H.M";
            List<MonthlyClose> mc = Db.Database.SqlQuery<MonthlyClose>(s, new SqlParameter("@m_year", this_year)).ToList();

            ViewBag.this_year = this_year;
            return View(mc);
        }
        public ActionResult Report(int report_year, int report_month)
        {
            string s = "Select year(mm_rcdate) as Y, " +
                             " month(mm_rcdate) as M, " +
                             " mm_fano, " +
                             " Case mm_fano When 0 Then '台灣廠' " +
                                          " When 1 Then '深圳廠' " +
                                          " When 2 Then '太倉廠' " +
                                          " When 3 Then '天津廠' " +
                                          " else '台灣廠' End as mm_Factory, " +
                             " mm_dpno, " +
                             " mm_dpname, " +
                             " Count(*) as Cnt, " +
                             " Sum(mm_rchour) as DoHour " +
                        " from  MisModify  " +
                        " Where mm_close = 1 and " +
                               "year(mm_rcdate) = @m_year and " +
                               "month(mm_rcdate) = @m_month " +
                        " group by year(mm_rcdate), month(mm_rcdate), mm_fano, mm_dpno ,mm_dpname ";
            List<MisModify_Report> rp = Db.Database.SqlQuery<MisModify_Report>(s, new SqlParameter("m_year", report_year),
                                                                                  new SqlParameter("m_month", report_month)).OrderByDescending(x => x.mm_Factory).ToList();
            if (rp.Count == 0)
                return RedirectToAction("Index", "Home", new { this_year = report_year });

            byte[] byteExcel = Export_Excel(rp);

            return File(byteExcel,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Url.Encode(report_year.ToString() + "/" + report_month.ToString() +
                                   "系統程式異動申請單統計表 結單統計表" +
                                   DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));
        }
        byte[] Export_Excel(List<MisModify_Report> rp)
        {
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorkbook workbook = Ep.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Column(1).Width = 25;
            sheet.Column(2).Width = 25;
            sheet.Column(3).Width = 20;
            sheet.Column(4).Width = 20;
            sheet.Column(4).Style.Numberformat.Format = "###0.00";

            sheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            int i = 1;
            sheet.Cells[i, 1].Value = rp.First().Y + "/" + rp.First().M + " 系統程式異動申請單統計表 結單統計表";
            sheet.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[i, 1, i, 4].Merge = true;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 16;
            sheet.Row(i).Style.Font.Bold = true;
            i = i + 2; ;
            sheet.Cells[i, 1].Value = "印表日期：" + DateTime.Now.ToString("yyyy/MM/dd");
            sheet.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Cells[i, 1, i, 4].Merge = true;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 10;
            sheet.Row(i).Style.Font.Bold = true;
            i = i + 2; ;
            sheet.Cells[i, 1].Value = "廠別";
            sheet.Cells[i, 2].Value = "部門";
            sheet.Cells[i, 3].Value = "件數";
            sheet.Cells[i, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Cells[i, 4].Value = "處理工時";
            sheet.Cells[i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Cells[i, 1, i, 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            sheet.Row(i).Style.Font.Bold = true;
            i++;
            string mm_Factory = rp.First().mm_Factory;
            int sum_Cnt = 0;
            decimal? sum_DoHour = 0;

            foreach (MisModify_Report r in rp)
            {
                if (r.mm_Factory != mm_Factory)
                {
                    sheet.Cells[i, 2, i, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    sheet.Cells[i, 2].Value = "合計：";
                    sheet.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    sheet.Cells[i, 3].Value = sum_Cnt;
                    sheet.Cells[i, 4].Value = sum_DoHour;
                    sheet.Row(i).Style.Font.Name = "標楷體";
                    sheet.Row(i).Style.Font.Size = 14;
                    sheet.Row(i).Style.Font.Bold = true;
                    sum_Cnt = 0;
                    sum_DoHour = 0;
                    mm_Factory = r.mm_Factory;
                    i = i + 3;
                    sheet.Cells[i, 1, i, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

                }
                sheet.Cells[i, 1].Value = r.mm_Factory;
                sheet.Cells[i, 2].Value = r.mm_dpname;
                sheet.Cells[i, 3].Value = r.Cnt;
                sheet.Cells[i, 4].Value = r.DoHour;
                sheet.Row(i).Style.Font.Name = "標楷體";
                sheet.Row(i).Style.Font.Size = 12;
                sum_Cnt = sum_Cnt + r.Cnt;
                sum_DoHour = sum_DoHour + r.DoHour;
                i++;
            }
            sheet.Cells[i, 2, i, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[i, 2].Value = "合計：";
            sheet.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Cells[i, 3].Value = sum_Cnt;
            sheet.Cells[i, 4].Value = sum_DoHour;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            sheet.Row(i).Style.Font.Bold = true;

            byte[] byteExcel = Ep.GetAsByteArray();
            return byteExcel;
        }
    }
}