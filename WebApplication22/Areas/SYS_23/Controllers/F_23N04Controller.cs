using Data.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23N04Controller : Controller
    {
        //23. 系統權限管理 > N.資訊服務需求單 > 04. 資訊服務需求單每月結單統計表查

        //資料庫物件
        TransartEntities db = new TransartEntities();

        // GET: SYS_23/F_23N04
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult P_23N04_q(string The_Year)
        {
            //改寫程式碼如下 Transart\MisService\mser04_q.asp
            //Transart\MisService\library\MisService_wfls.asp 下的
            //EPaperMonthClose'=	Description: 每月結單統計表查印
            /*
             * 查詢條件
            lsFieldList = " ym , isNull( Count1 , 0 ) as  MisSer_Count "
	        lsTableList = " "
	        lsTableList = lsTableList & " (                                                                             "
	        lsTableList = lsTableList & " 	select Distinct Convert( char(7) , ho_date , 111 ) as ym from holiday     	"
	        lsTableList = lsTableList & " ) H 																			"
	        lsTableList = lsTableList & " Left Join (      					                                            "
	        lsTableList = lsTableList & " 	select Convert( char(7) , MS_rcdate , 111 ) as ym1 , Count( * ) as Count1 	"
	        lsTableList = lsTableList & " 		from MisService where MS_close = 1                                     	"
	        lsTableList = lsTableList & " 				group by Convert( char(7) , MS_rcdate , 111 )                 	"
	        lsTableList = lsTableList & " ) A on H.ym = A.ym1                                                           "

	        lsWhereList = " Left( ym , 4 ) = '" & The_Year & "'  "
	        lsWhereList = lsWhereList & " Order by ym "
             */

            //接收變數
            string the_year = The_Year ?? "";//開單年度

            //宣告變數
            string SQL = ""; //SQL 查詢字串

            //1. 預設年度為執行日當下的年度
            if (the_year == "")
            {
                the_year = DateTime.Today.Year.ToString();
            }

            //2. 組SQL
            SQL = "SELECT  ym , isNull( Count1 , 0 ) as  MisSer_Count ";
            SQL = SQL + " FROM ( ";
            SQL = SQL + "    select Distinct Convert( char(7) , ho_date , 111 ) as ym from holiday ";
            SQL = SQL + " ) H";
            SQL = SQL + " Left Join ( ";
            SQL = SQL + "   select Convert( char(7) , MS_rcdate , 111 ) as ym1 , Count( * ) as Count1 ";
            SQL = SQL + "       from MisService where MS_close = 1";
            SQL = SQL + "       group by Convert( char(7) , MS_rcdate , 111 )";
            SQL = SQL + " ) A on H.ym = A.ym1";
            SQL = SQL + " WHERE Left(ym, 4) = '" + the_year + "' ";
            SQL = SQL + " Order by ym ";

            //3. 執行查詢作業
            try
            {
                List<MisServiceStatistics> misStatistic = db.Database.SqlQuery<MisServiceStatistics>(SQL).ToList();
                ViewBag.the_year = the_year;

                return View(misStatistic);
            }
            catch (System.Exception ex)
            {
                TempData["message"] = "每月結單統計表查印查詢失敗:" + ex;
                return View();
            }//end of try           
        }

        public ActionResult P_23N04_r(string ym)
        {
            //改寫程式碼如下 Transart\MisService\mser04_r.asp
            /*
             * 
            查詢條件
	        strSQL = ""
	        select  Convert( char(7) ,ms_rcdate, 111 ) as YM ,'台灣廠' as Factory ,         "
	        	  	ms_dpno , ms_dpname ,  Count(*) as Cnt , Sum(ms_rchour) as DoHour       "
	        	from MisService                                                             "
	        	Where ms_close = 1 and Convert( char(7) ,ms_rcdate, 111 ) = '" & YM & "'    "
	        		Group By Convert( char(7) ,ms_rcdate, 111 ) ,ms_dpno ,ms_dpname     	"

             */

            //接收變數
            string YM = ym?? ""; //前端傳來的ym不含斜線(格式為yyyyMM)
            string YMNew = "";

            //宣告變數
            string SQL;

            if (YM != "")
            {
                YMNew = YM.Substring(0, 4) + "/" + Right(YM, 2);
                //1. 組成SQL
                SQL = "select  Convert( char(7) ,ms_rcdate, 111 ) as YM ,'台灣廠' as Factory , ";
                SQL = SQL + " ms_dpno , ms_dpname ,  Count(*) as Cnt , Sum(ms_rchour) as DoHour ";
                SQL = SQL + " from MisService ";
                SQL = SQL + " Where ms_close = 1 and Convert( char(7) ,ms_rcdate, 111 ) = '" + YMNew + "' ";
                SQL = SQL + " Group By Convert( char(7) ,ms_rcdate, 111 ) ,ms_dpno ,ms_dpname ";

                //2. 查詢SQL
                try
                {
                    List<MisServiceExport> MisServiceExport = db.Database.SqlQuery<MisServiceExport>(SQL).ToList();
                    if (MisServiceExport.Count == 0 | MisServiceExport == null)
                    {
                        ////空資料仍會顯示報表標題? 轉導?  維持舊ASP 顯示空報表?
                        TempData["message"] = "0筆資訊需需要列印, 請重新選取年月資料";
                        return RedirectToAction("P_23N04_q", "F_23N04");
                    }
                    else
                    {
                        byte[] byteExcel = Export_Excel(MisServiceExport, YMNew);

                        return File(byteExcel,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    Url.Encode("每月結單統計表查印" +
                                               DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));
                    }                   
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "每月結單統計表查印查詢失敗:" + ex;
                    return RedirectToAction("P_23N04_q", "F_23N04");
                }//end of try 
            }
            else
            {
                TempData["message"] = "每月結單統計表查印匯出指定年月報表失敗:無法取得指定年月資料";
                return RedirectToAction("P_23N04_q", "F_23N04");
            }
        }

        byte[] Export_Excel(List<MisServiceExport> mse, string YM)
        {
            decimal totalDoHour = 0;
            long totalCnt = 0;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorkbook workbook = Ep.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Column(1).Width = 20;
            sheet.Column(2).Width = 18;
            sheet.Column(3).Width = 15;
            sheet.Column(4).Width = 15;
            sheet.Column(4).Style.Numberformat.Format = "###0.00";

            sheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int i = 1;
            sheet.Cells[i, 1].Value = YM + " 資訊服務需求單 結單統計表";
            sheet.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[i, 1, i, 4].Merge = true;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            sheet.Row(i).Style.Font.Bold = true;

            i = i + 2; ;
            sheet.Cells[i, 4].Value = "印表日期：" + DateTime.Now.ToString("yyyy/MM/dd");
            sheet.Cells[i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;           
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 10;
            sheet.Row(i).Style.Font.Bold = true;

            i = i + 2; ;
            sheet.Cells[i, 1].Value = "廠別：台灣廠";
            sheet.Cells[i, 2].Value = "部門";
            sheet.Cells[i, 3].Value = "件數";
            sheet.Cells[i, 4].Value = "處理工時";
            sheet.Cells[i, 1, i, 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[i, 1, i, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            //sheet.Row(i).Style.Font.Bold = true;
            i++;

            foreach (MisServiceExport r in mse)
            {
                sheet.Cells[i, 2].Value = r.ms_dpname;
                sheet.Cells[i, 3].Value = r.Cnt;
                sheet.Cells[i, 4].Value = r.DoHour;

                totalDoHour = totalDoHour + r.DoHour;
                totalCnt = totalCnt + r.Cnt;

                sheet.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Cells[i, 3, i, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Row(i).Style.Font.Name = "標楷體";
                sheet.Row(i).Style.Font.Size = 12;
                i++;
            }

            sheet.Cells[i, 2].Value = "合計:";
            sheet.Cells[i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            sheet.Cells[i, 3].Value = totalCnt;
            sheet.Cells[i, 4].Value = totalDoHour;
            sheet.Cells[i, 3, i, 4].Style.Font.Bold = true;
            sheet.Cells[i, 2, i, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Row(i).Style.Font.Name = "標楷體";
            //sheet.Row(i).Style.Font.Size = 14;
            //sheet.Row(i).Style.Font.Bold = true;

            byte[] byteExcel = Ep.GetAsByteArray();
            return byteExcel;
        }

        public static string Right(string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }
    }
}