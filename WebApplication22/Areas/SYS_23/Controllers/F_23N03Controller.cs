using Common.Menu;
using Data.Models;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23N03Controller : Controller
    {
        //23. 系統權限管理 > N.資訊服務需求單 > 03. 資訊服務需求單資料查印

        //資料庫物件
        TransartEntities db = new TransartEntities();

        // GET: SYS_23/F_23N03
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult P_23N03_q(string dpno, string DateKind, string Date1, string Date2, string QryKind, string The_str, int? PageNumber, int? PageSize)
        {
            //改寫程式碼如下 Transart\MisService\mser03_q.asp
            //Transart\MisService\library\MisService_wfls.asp 下的
            //MisServiceList'=	Description: 資訊服務需求單--查印
            //查詢條件如下
            /*
             * 
            lsFieldList = " * "
	        lsTableList = " MisService "
	        lsWhereList = " " & DateKind & " is not Null "

	        IF dpno <> "" THEN lsWhereList = lsWhereList & " AND MS_DPNO = '" & dpno & "' "

	        '2017/09/22 By Clark 增加關鍵字查詢
	        IF The_str <> "" THEN lsWhereList = lsWhereList & " AND (MS_SUBJECT like '%" & The_str & "%' OR MS_Content1 like '%" & The_str & "%')"

	        '2013/08/12 by souleye 新增刪除、不核准條件，已結單跟未結單前提為已核准
	        SELECT CASE QryKind
		        CASE "1"
			        '已結單
			        lsWhereList = lsWhereList & " AND MS_POST in (3,5) AND MS_CLOSE = 1 "
		        CASE "2"
			        '未結單
			        'lsWhereList = lsWhereList & " AND MS_POST = 3 AND MS_CLOSE = 0 "
			        'lsWhereList = lsWhereList & " AND MS_POST in (0,1,5) AND MS_CLOSE = 0 "
			        lsWhereList = lsWhereList & " AND MS_POST not in (4,6,7,8,9)  AND MS_CLOSE = 0 "
		        CASE "4"
			        '不核准
			        lsWhereList = lsWhereList & " AND MS_POST in (4,6) "
		        CASE "5"
			        '刪除
			        lsWhereList = lsWhereList & " AND MS_POST = 9 "
		        CASE "6"
			        '轉件
			        lsWhereList = lsWhereList & " AND MS_POST = 8 "
		        CASE "7"
			        '退件
			        lsWhereList = lsWhereList & " AND MS_POST = 7 "
		        CASE ELSE
			        'Do Nothing
	        END SELECT

	        IF Date1 <> "" AND Date2 <> "" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " between '" & Date1 & "' AND '" & Date2 & "' "
	        ELSEIF Date1 <> "" AND Date2 ="" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " >= '" & Date1 & "' "
	        ELSEIF Date1 = "" AND Date2 <>"" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " <= '" & Date2 & "' "
	        END IF

	        lsWhereList = lsWhereList & " order by MS_DATE1 ,MS_NO "
             */

            // 接收參數
            string dp_no = dpno ?? "";
            string dateKind = DateKind ?? "";
            string date1 = Date1 ?? ""; //System.NullReferenceException: '並未將物件參考設定為物件的執行個體。'         date1 為 null。
            string date2 = Date2 ?? "";
            string the_str = The_str ?? "";
            string qryKind = QryKind ?? "";

            //宣告變數
            string SQL = "";

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定查詢頁面分頁筆數
            int pageSize = PageSize ?? 10;//每頁筆數
            int pageNumber = PageNumber ?? 1;

            //3. 依據查詢條件組成SQL
            SQL = " select * from MisService ";

            if (dateKind == null | dateKind.Trim() == "")
            {
                //第一次載入頁面, 查詢條件設定為已"申請日期" ms_date1
                //	<option value="MS_DATE1">申請日期</option>
                //  < option value = "MS_RCDATE" > 完工日期 </ option >
                 SQL = SQL + " where MS_DATE1 is not Null ";
            }
            else
            {
                SQL = SQL + " where " + dateKind + " is not Null ";
            }

            if (dp_no != null && dp_no.Trim() != "")
            {
                SQL = SQL + " AND MS_DPNO = '" + dp_no.Trim() + "' ";
            }

            //'2017/09/22 By Clark 增加關鍵字查詢
            if (the_str != null && the_str.Trim() != "")
            {
                SQL = SQL + " AND (MS_SUBJECT like '%" + The_str + "%' OR MS_Content1 like '%" + The_str + "%')";
            }

            //'2013/08/12 by souleye 新增刪除、不核准條件，已結單跟未結單前提為已核准
            switch (qryKind)
            {
                case "1":
                    //'已結單
                    SQL = SQL + " AND MS_POST in (3,5) AND MS_CLOSE = 1 ";
                    break;
                case "2":
                    //'未結單
                    SQL = SQL + " AND MS_POST not in (4,6,7,8,9)  AND MS_CLOSE = 0 ";
                    break;
                case "4":
                    //'不核准
                    SQL = SQL + " AND MS_POST in (4,6) ";
                    break;
                case "5":
                    //'刪除
                    SQL = SQL + " AND MS_POST = 9 ";
                    break;
                case "6":
                    //'轉件
                    SQL = SQL + " AND MS_POST = 8 ";
                    break;
                case "7":
                    //'退件
                    SQL = SQL + " AND MS_POST = 7 ";
                    break;
                default:
                    // 'Do Nothing
                    break;
            }// end of switch

            if ((date1 != null && date1.Trim() != "") && (date2 != null && date2.Trim() != ""))
            {
                SQL = SQL + " AND " + dateKind + " between '" + date1 + "' AND '" + date2 + "' ";
            }else if ((date1 != null && date1.Trim() != "") && (date2 == null | date2.Trim() == ""))
            {
                SQL = SQL + " AND " + dateKind + " >= '" + date1 + "' ";
            }else if ((date1 == null | date1.Trim() == "") && (date2 != null & date2.Trim() != ""))
            {
                SQL = SQL + " AND " + dateKind + " <= '" + date2 + "' ";
            }

            SQL = SQL + " order by MS_DATE1 ,MS_NO ";

            // 4. 執行查詢作業
            try
            {
                IQueryable<MisService> misIQ;
                if (dateKind == "" | qryKind == "")
                {
                    //第一次開啟網頁時, Url 應不會包含這兩變數, 以此判斷不要顯示資料; 待實際點選"查詢"組成查詢條件再待出資料
                    List<MisService> mis = new List<MisService>();
                    misIQ = mis.AsQueryable();
                }
                else
                {
                    List<MisService> mis = db.Database.SqlQuery<MisService>(SQL).ToList();
                    misIQ = mis.AsQueryable();
                }

                //4.1 將資料進行分頁, 並指定傳給前端的變數已設定分頁顯示
                IPagedList misPage = misIQ.ToPagedList(pageNumber, pageSize);

                //5. 傳遞變數給前端
                ViewBag.dp_no = dp_no;
                ViewBag.dateKind = dateKind;
                ViewBag.date1 = date1;
                ViewBag.date2 = date2;
                ViewBag.the_str = the_str;
                ViewBag.qryKind = qryKind;

                ViewBag.PageSize = pageSize;
                ViewBag.PageNumber = pageNumber;

                //設定部門選單(呼叫F23_N Model下類別取得清單)並傳值給前端
                if (dp_no != null & dp_no.Trim() != "")
                {
                    //部門選項須維持在之前選定的項目
                    ViewBag.Select_Dept = new DEPT_LIST(dp_no.Trim()).dept_list;
                }
                else
                {
                    ViewBag.Select_Dept = new DEPT_LIST("").dept_list;
                }

                return View(misPage);
            }
            catch (System.Exception ex)
            {
                TempData["message"] = "查詢資訊需求單失敗:" + ex;
                return RedirectToAction("P_23N03_q", "F_23N03");
            }//end of try
        }

        public ActionResult P_23N03_l (string MS_NO)
        {
            //改寫Transart\MisService\mser03_l.asp
            //Transart\MisService\library\MisService_wfls.asp
            //MisServiceShow'=	Description: 資訊服務需求單--內容

            //宣告變數
            string selectStr = ""; // select SQL 字串
            string ms_no = ""; //接收參數

            //1. 接收參數
            ms_no = MS_NO;
            if (ms_no != null && ms_no.Trim() != "")
            {
                /*
                 * 
                lsFieldList = " * "
	            lsTableList = " MisService "
	            lsWhereList = " MS_NO = '" & MS_NO & "' ORDER BY MS_no "
                  */
                selectStr = "select * ";
                selectStr = selectStr + " from MisService ";
                selectStr = selectStr + " where MS_NO ='" + ms_no + "' ";
                selectStr = selectStr + " order by MS_No";

                try
                {
                    List<MisService> mis = db.Database.SqlQuery<MisService>(selectStr).ToList();

                    if (mis.Count == 0)
                    {
                        ViewBag.ms_noSeach = ms_no;
                    }
                    return PartialView(mis);
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "查詢資訊需求單" + ms_no + "資料失敗!" + ex;
                    return RedirectToAction("P_23N03_q", "F_23N03");
                }
            }
            else
            {
                TempData["message"] = "查詢資訊需求單資料失敗: 單號資訊空值!";
                return RedirectToAction("P_23N03_q", "F_23N03");
            }
        }

        public ActionResult P_23N03_r(string dpno, string DateKind, string Date1, string Date2, string QryKind, string The_str, string dpname)
        {
            //改寫程式碼如下 Transart\MisService\mser03_r.asp
            //查詢條件如下
            /*
             * 
            '********************************  產生 REPORT  *********************************************
  	        lsFieldList = ""
	        MS_NO,MS_EMNO, dbo.getSpWord(MS_NAME) as MS_NAME,MS_DPNO,case when Len(Rtrim(MS_DPNAME)) = 0 then '駐國外部' else MS_DPNAME end as MS_DPNAME "
	        ,MS_ATTACH,MS_DATE1,MS_DATE2,MS_SUBJECT,MS_CONTENT1 "
	        ,MS_UPDATE,MS_ACDATE,MS_ACEMNO,dbo.getSpWord(MS_ACNAME) as MS_ACNAME "
	        ,MS_PCDATE,MS_PCHOUR,MS_RCDATE,MS_RCHOUR,MS_POST,MS_YN,MS_YN1,MS_CLOSE"
	        ,MS_ACBACKMEMO "

	        lsTableList = " MisService "
	        lsWhereList = " " & DateKind & " is not Null "

	        IF dpno <> "" THEN lsWhereList = lsWhereList & " AND MS_DPNO = '" & dpno & "' "

	        '2017/09/22 By Clark 增加關鍵字查詢
	        IF The_str <> "" THEN lsWhereList = lsWhereList & " AND (MS_SUBJECT like '%" & The_str & "%' OR MS_Content1 like '%" & The_str & "%')"

	        title = title & "部門：[" & dpname & "]    "

	        IF Date1 <> "" AND Date2 <> "" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " between '" & Date1 & "' AND '" & Date2 & "' "
	        ELSEIF Date1 <> "" AND Date2 ="" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " >= '" & Date1 & "' "
	        ELSEIF Date1 <> "" AND Date2 ="" THEN
		        lsWhereList = lsWhereList & " AND " & DateKind & " <= '" & Date2 & "' "
	        END IF
	        title = title & "時間：[" & Date1 & "]～[" & Date2 & "]"

	        SELECT CASE QryKind
		        CASE "1"
			        '已結單
			        lsWhereList = lsWhereList & " AND MS_POST = 3 AND MS_CLOSE = 1 "
			        title = title & "狀態：[已結單]"
		        CASE "2"
			        '未結單
			        lsWhereList = lsWhereList & " AND MS_POST = 3 AND MS_CLOSE = 0 "
			        title = title & "狀態：[未結單]"
		        CASE "4"
			        '不核准
			        lsWhereList = lsWhereList & " AND MS_POST = 4 "
			        title = title & "狀態：[不核准]"
		        CASE "5"
			        '刪除
			        lsWhereList = lsWhereList & " AND MS_POST = 2 "
			        title = title & "狀態：[刪除]"
		        CASE "6"
			        '刪除
			        lsWhereList = lsWhereList & " AND MS_POST = 8 "
			        title = title & "狀態：[已轉件]"
		        CASE "7"
			        '刪除
			        lsWhereList = lsWhereList & " AND MS_POST = 7 "
			        title = title & "狀態：[已退件]"
		        CASE ELSE
			        'Do Nothing
			        title = title & "狀態：[全部]"
	        END SELECT

	        lsWhereList = lsWhereList & " order by MS_NO "

	        strSQL = "Select " & lsFieldList & " From " & lsTableList & " Where " & lsWhereList
             */

            // 接收參數
            string dp_no = dpno ?? "";
            string dp_name = dpname ?? "";
            string dateKind = DateKind ?? "";
            string date1 = Date1 ?? ""; //System.NullReferenceException: '並未將物件參考設定為物件的執行個體。'         date1 為 null。
            string date2 = Date2 ?? "";
            string the_str = The_str ?? "";
            string qryKind = QryKind ?? "";

            //宣告變數
            string SQL = "";    //資料查詢SQL指令
            string title = ""; //報表標題

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 依據查詢條件組成SQL
            /*
             * 下述指令會因為部分欄位未宣告而跳錯
            SQL = " SELECT MS_NO,MS_EMNO, dbo.getSpWord(MS_NAME) as MS_NAME,MS_DPNO,case when Len(Rtrim(MS_DPNAME)) = 0 then '駐國外部' else MS_DPNAME end as MS_DPNAME ";
            SQL = SQL + " ,MS_ATTACH,MS_DATE1,MS_DATE2,MS_SUBJECT,MS_CONTENT1 ";
            SQL = SQL + " ,MS_UPDATE,MS_ACDATE,MS_ACEMNO,dbo.getSpWord(MS_ACNAME) as MS_ACNAME ";
            SQL = SQL + " ,MS_PCDATE,MS_PCHOUR,MS_RCDATE,MS_RCHOUR,MS_POST,MS_YN,MS_YN1,MS_CLOSE";
            SQL = SQL + " ,MS_ACBACKMEMO ";
            */
            //以下中文字無法成功轉碼覆寫
            //SQL = "SELECT *, dbo.getSpWord(MS_NAME) as MS_NAME, dbo.getSpWord(MS_ACNAME) as MS_ACNAME, case when Len(Rtrim(MS_DPNAME)) = 0 then '駐國外部' else MS_DPNAME end as MS_DPNAME ";
            //組出查詢所有欄位值, 中文名字欄位要處理
            SQL = " SELECT MS_NO, MS_EMNO, dbo.getSpWord(MS_NAME) as MS_NAME, MS_DPNO,case when Len(Rtrim(MS_DPNAME)) = 0 then '駐國外部' else MS_DPNAME end as MS_DPNAME, ";
            SQL = SQL + " MS_ATTACH,MS_DATE1,MS_DATE2,MS_SUBJECT,MS_CONTENT1,MS_UPDATE,MS_ACDATE, ";
            SQL = SQL + " MS_ACEMNO,dbo.getSpWord(MS_ACNAME) as MS_ACNAME,MS_PCDATE,MS_PCHOUR, ";
            SQL = SQL + " MS_RCDATE, MS_RCHOUR,MS_POST,MS_YN,MS_YN1,MS_CLOSE,MS_ACBACKMEMO,MS_MEMO1,MS_MEMO2, ";
            SQL = SQL + " MS_FANO,MS_SCHDATE,MS_CONTENT2,MS_U1,MS_U2,MS_U3,MS_U4,MS_REASON,MS_DPNOM1, ";
            SQL = SQL + " MS_DPNOM2,MS_VDATE,MS_DPNOM3,MS_DPNOM4,MS_VDATE1";
            SQL = SQL + " FROM MisService ";
            SQL = SQL + " WHERE " + dateKind + " is not Null ";

            if (dp_no != null && dp_no.Trim() != "")
            {
                SQL = SQL + " AND MS_DPNO = '" + dp_no.Trim() + "' ";
            }
            
            //'2017/09/22 By Clark 增加關鍵字查詢
            if (the_str != null && the_str.Trim() != "")
            {
                SQL = SQL + " AND (MS_SUBJECT like '%" + The_str + "%' OR MS_Content1 like '%" + The_str + "%')";
            }

            title = title + "部門：[" + dp_name + "]    ";

            if ((date1 != null && date1.Trim() != "") && (date2 != null && date2.Trim() != ""))
            {
                SQL = SQL + " AND " + dateKind + " between '" + date1 + "' AND '" + date2 + "' ";
            }
            else if ((date1 != null && date1.Trim() != "") && (date2 == null | date2.Trim() == ""))
            {
                SQL = SQL + " AND " + dateKind + " >= '" + date1 + "' ";
            }
            else if ((date1 == null | date1.Trim() == "") && (date2 != null & date2.Trim() != ""))
            {
                SQL = SQL + " AND " + dateKind + " <= '" + date2 + "' ";
            }

            title = title + "時間：[" + date1 + "]～[" + date2 + "]";

            //(待解)
            //部分查詢與列印語法不一致(上為查詢, 下為列印).....
            //暫定對齊"查詢指令"
            /*
               MS_POST 狀態註記:
               0: 新資料   1: 轉審核
               3: 資訊服務需求單審核--部門主管核准       4: 資訊服務需求單審核--部門主管不核准
               5: 資訊服務需求單審核--資訊主管核准       6: 資訊服務需求單審核--資訊主管不核准
               7: 退件            8: 轉件從MisServcice > MisModify    9: 刪除
            */
            switch (qryKind)
            {
                case "1":
                    //'已結單                    
                    //lsWhereList = lsWhereList & " AND MS_POST in (3,5) AND MS_CLOSE = 1 "
                    //lsWhereList = lsWhereList & " AND MS_POST = 3 AND MS_CLOSE = 1 "
                    SQL = SQL + " AND MS_POST in (3,5) AND MS_CLOSE = 1 ";
                    title = title + "狀態：[已結單]";
                    break;
                case "2":
                    //'未結單                    
                    //lsWhereList = lsWhereList & " AND MS_POST not in (4,6,7,8,9)  AND MS_CLOSE = 0 "
                    //lsWhereList = lsWhereList & " AND MS_POST = 3 AND MS_CLOSE = 0 "
                    SQL = SQL + " AND MS_POST not in (4,6,7,8,9)  AND MS_CLOSE = 0 ";
                    title = title + "狀態：[未結單]";
                    break;
                case "4":
                    //'不核准
                    //lsWhereList = lsWhereList & " AND MS_POST in (4,6) "
                    //lsWhereList = lsWhereList & " AND MS_POST = 4 " >>有分部門主管與資訊主管
                                
                    SQL = SQL + " AND MS_POST in (4,6) ";
                    title = title + "狀態：[不核准]";
                    break;
                case "5":
                    //'刪除
                    //lsWhereList = lsWhereList & " AND MS_POST = 9 "
                    //lsWhereList = lsWhereList & " AND MS_POST = 2 ">>> 無此狀態
                    SQL = SQL + " AND MS_POST = 9 ";
                    title = title + "狀態：[刪除]";
                    break;
                case "6":
                    //'轉件
                    SQL = SQL + " AND MS_POST = 8 ";
                    title = title + "狀態：[已轉件]";
                    break;
                case "7":
                    //'退件
                    SQL = SQL + " AND MS_POST = 7 ";
                    title = title + "狀態：[已退件]";
                    break;
                default:
                    // 'Do Nothing
                    break;
            }// end of switch
            
            SQL = SQL + " order by MS_NO ";

            // 3. 執行查詢作業
            try
            {
                List<MisService> mis = db.Database.SqlQuery<MisService>(SQL).ToList();

                //4. 執行匯出EXCEL 作業
                
                if (mis.Count == 0 | mis == null)
                {
                    ////空資料仍會顯示報表標題? 轉導?  維持舊ASP 顯示空報表?
                    TempData["message"] = "0筆資訊需需要列印, 請重新輸入查詢條件";
                    return RedirectToAction("P_23N03_q", "F_23N03", new { dp_no, dateKind, date1, date2, qryKind, the_str });
                }
                else
                {
                    byte[] byteExcel = Export_Excel(mis, title);

                    return File(byteExcel,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                Url.Encode("資訊服務需求單資料查印" +
                                           DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));
                }
            }
            catch (System.Exception ex)
            {
                TempData["message"] = "查印資訊需求單失敗:" + ex;
                return RedirectToAction("P_23N03_q", "F_23N03", new { dp_no, dateKind, date1, date2, qryKind, the_str });
            }//end of try
        }

        byte[] Export_Excel(List<MisService> mis, string title)
        {
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorkbook workbook = Ep.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Column(1).Width = 12;
            sheet.Column(2).Width = 18;
            sheet.Column(3).Width = 15;
            sheet.Column(4).Width = 15;
            sheet.Column(5).Width = 60;            
            sheet.Column(6).Width = 18;
            sheet.Column(7).Width = 15;
            sheet.Column(8).Width = 15;
            sheet.Column(9).Width = 10;
            sheet.Column(7).Style.Numberformat.Format = "###0.00";
            sheet.Column(8).Style.Numberformat.Format = "###0.00";

            sheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            int i = 1;
            sheet.Cells[i, 1].Value = " 資 訊 服 務 需 求 單 資 料 查 印";
            sheet.Cells[i, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[i, 1, i, 9].Merge = true;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            sheet.Row(i).Style.Font.Bold = true;

            i = i + 2; ;
            sheet.Cells[i, 1].Value = title;
            sheet.Cells[i, 6].Value = "印表日期：" + DateTime.Now.ToString("yyyy/MM/dd");
            sheet.Cells[i, 1, i, 5].Merge = true;
            sheet.Cells[i, 6, i, 9].Merge = true;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 10;
            sheet.Row(i).Style.Font.Bold = true;

            i = i + 2; ;
            sheet.Cells[i, 1].Value = "單號";
            //同網頁一致改為申請日期與申請人
            //sheet.Cells[i, 2].Value = "提案日期";
            //sheet.Cells[i, 3].Value = "提案人";
            sheet.Cells[i, 2].Value = "申請日期";
            sheet.Cells[i, 3].Value = "申請人";            
            sheet.Cells[i, 4].Value = "部門";
            sheet.Cells[i, 5].Value = "主題";
            sheet.Cells[i, 6].Value = "預定完成日";
            sheet.Cells[i, 7].Value = "預定工時";
            sheet.Cells[i, 8].Value = "實際工時";
            sheet.Cells[i, 9].Value = "結案否";
            sheet.Cells[i, 1, i, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[i, 1, i, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 13;
            sheet.Row(i).Style.Font.Bold = true;
            i++;

            foreach (MisService r in mis)
            {               
                sheet.Cells[i, 1].Value = r.MS_NO;
                if (r.MS_DATE1 != null)
                {
                    sheet.Cells[i, 2].Value = r.MS_DATE1.Value.ToString("yyyy/MM/dd");
                }
                else
                {
                    sheet.Cells[i, 2].Value = "";
                }
                sheet.Cells[i, 3].Value = r.MS_NAME;
                sheet.Cells[i, 4].Value = r.MS_DPNAME;
                sheet.Cells[i, 5].Value = r.MS_SUBJECT;
                sheet.Cells[i, 5].Style.WrapText = true;//自動換行
                if (r.MS_PCDATE != null)
                {
                    sheet.Cells[i, 6].Value = r.MS_PCDATE.Value.ToString("yyyy/MM/dd");
                }
                else
                {
                    sheet.Cells[i, 6].Value = "";
                }
                sheet.Cells[i, 7].Value = r.MS_PCHOUR;
                sheet.Cells[i, 8].Value = r.MS_RCHOUR;

                //結案否
                //AND MS_POST in (3,5) AND MS_CLOSE = 1 ";
                if ((r.MS_POST == 3 | r.MS_POST == 5) & (r.MS_CLOSE == 1)){
                    sheet.Cells[i, 9].Value = "Y";
                }
                else
                {
                    sheet.Cells[i, 9].Value = "N";
                }
                sheet.Cells[i, 1, i, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Row(i).Style.Font.Name = "標楷體";
                sheet.Row(i).Style.Font.Size = 12;
                i++;
            }           
            sheet.Row(i).Style.Font.Name = "標楷體";
            sheet.Row(i).Style.Font.Size = 14;
            sheet.Row(i).Style.Font.Bold = true;

            byte[] byteExcel = Ep.GetAsByteArray();
            return byteExcel;
        }
    }
}
 