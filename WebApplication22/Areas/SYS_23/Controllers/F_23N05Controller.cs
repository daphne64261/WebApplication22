using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23N05Controller : Controller
    {
        //改寫功能
        //23. 系統權限管理 > N.資訊服務需求單 > 05. 資訊服務需求單退件明細查印
        //預設有權限的同仁才可看到此選項

        //資料庫物件
        TransartEntities db = new TransartEntities();

        // GET: SYS_23/F_23N05
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult P_23N05_q(string The_No, string Date1, string Date2, string QryKind, int? PageNumber, int? PageSize)
        {
            //改寫程式碼如下
            /*Transart\MisService\mser05_q.asp
            Transart\MisService\library\MisService_wfls.asp
            EPaperRejectList'=	Description: 退件明細表
            */

            //宣告變數
            string selectStr = ""; //查詢SQL字串

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定查詢頁面分頁筆數
            int pageSize = PageSize ?? 10;//每頁筆數
            int pageNumber = PageNumber ?? 1;

            //3. 指定初始查詢網頁資料集合
            // 確認原網頁隱藏只有一項選項的下拉選單
            //'資訊服務需求單
            
            if (QryKind == "2") 
            {
                /*
                 lsFieldList = " * "
                 lsTableList = " MisSerReject Left Join MisService on MST_MSNO = MS_NO "
                 lsWhereList = " 1 = 1 "
                 */

                //因後面網頁不須帶出Mis 
                //selectStr = "select msr.MST_MSNO, msr.MST_SUBJECT, msr.MST_CONTENT1, msr.MST_ACBACKMEMO, msr.MST_DATE, msr.MST_ACEMNO, msr.MST_ACNAME ";
                selectStr = "select * ";
                selectStr = selectStr + " from MisSerReject msr ";
                selectStr = selectStr + " left join MisService mis ";
                selectStr = selectStr + " on msr.MST_MSNO = mis.MS_No ";
                selectStr = selectStr + " where 1 = 1 ";

                /*
                 *
                 IF Date1 <> "" AND Date2 <> "" THEN
				    lsWhereList = lsWhereList & " AND Convert( char(10),MST_DATE,111 ) between '" & Date1 & "' AND '" & Date2 & "' "
			    ELSEIF Date1 <> "" AND Date2 ="" THEN
				    lsWhereList = lsWhereList & " AND Convert( char(10),MST_DATE,111 ) >= '" & Date1 & "' "
			    ELSEIF Date1 <> "" AND Date2 ="" THEN
				    lsWhereList = lsWhereList & " AND Convert( char(10),MST_DATE,111 ) <= '" & Date2 & "' "
			    END IF
                
                 */
                if ((Date1 != null && Date1.Trim() != "") && (Date2 != null && Date2.Trim() != ""))
                {
                    selectStr = selectStr + " AND Convert( char(10),MST_DATE,111 ) between '" + Date1 + "' AND '" + Date2 + "' ";
                }
                else if ((Date1 != null && Date1.Trim() != "") && (Date2 != null && Date2.Trim() != ""))
                {
                    selectStr = selectStr + " AND Convert( char(10),MST_DATE,111 ) >= '" + Date1 + "'";
                }
                else if ((Date1 != null && Date1.Trim() != "") && (Date2 != null && Date2.Trim() != ""))
                {
                    //雖然else if 條件相同, 但仍不可省略, 測試系統在上個else if 組出程式後, 就不會再進此
                    selectStr = selectStr + " AND Convert( char(10),MST_DATE,111 ) <= '" + Date2 + "'";
                }

                if (The_No != null & The_No != "")
                {
                    selectStr = selectStr + " AND MST_MSNO = '" + The_No + "'  ";
                }

                selectStr = selectStr + " order by MST_DATE , MST_MSNO ";

                try
                {
                    List<MisServiceReject> msr = db.Database.SqlQuery<MisServiceReject>(selectStr).ToList();
                    IQueryable<MisServiceReject> msrIQ = msr.AsQueryable();

                    //4. 將資料進行分頁, 並指定傳給前端的變數已設定分頁顯示
                    //IPagedList<MisService> misPage = mis.ToPagedList(pageNumber, pageSize);
                    IPagedList msrPage = msrIQ.ToPagedList(pageNumber, pageSize);

                    //5. 傳遞變數給前端


                    ViewBag.The_No = The_No;
                    ViewBag.Date1 = Date1;
                    ViewBag.Date2 = Date2;
                    ViewBag.QryKind = QryKind;

                    ViewBag.PageSize = pageSize;
                    ViewBag.PageNumber = pageNumber;

                    return View(msrPage);
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "查詢資訊需求單退件明細失敗:" + ex;
                    return RedirectToAction("P_23N05_q", "F_23N05");
                }
            }            
            else
            {
                //TempData["message"] = "查詢資訊需求單退件明細失敗: 類別須為2";
                //return RedirectToAction("P_23N05_q", "F_23N05", );//此寫法會因為網頁項目尚未loading 而為空值, 導致反覆轉到最後失敗
                return View();
            }
        }

        public ActionResult P_23N05_l(string The_No)
        {
            //改寫Transart\MisService\mser05_1.asp
            //Transart\MisService\library\MisService_wfls.asp
            //MisServiceShow1'=	Description: 資訊服務需求單--內容

            //宣告變數
            string selectStr = ""; // select SQL 字串
            string the_no = ""; //接收參數

            //1. 接收參數
            the_no = The_No;
            if (the_no != null && the_no.Trim() != "")
            {
                /*
                 * 
                lsFieldList = " * "
                lsTableList = " MisService Left Join MisSerReject ON MS_NO = MST_MSNO "
                lsWhereList = " MS_NO = '" & MS_NO & "' ORDER BY MS_no "
                 */
                selectStr = "select * ";
                selectStr = selectStr + " from MisSerReject msr ";
                selectStr = selectStr + " left join MisService mis ";
                selectStr = selectStr + " on msr.MST_MSNO = mis.MS_No ";
                selectStr = selectStr + " where mis.MS_No ='" + the_no + "'";
                selectStr = selectStr + " order by mis.MS_No";

                try
                {
                    List<MisServiceReject> msr = db.Database.SqlQuery<MisServiceReject>(selectStr).ToList();

                    if (msr.Count == 0)
                    {
                        //確認測試環境在http://192.168.1.233:8000/MisService/mser05_q.asp?The_No=IS041301&Date1=&Date2=&QryKind=2
                        //沒對應資料會維持在mser05_q.asp頁面, 不會轉導mser05_l.asp
                        ViewBag.ms_noSeach = the_no;
                    }
                    return PartialView(msr);
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "查詢資訊需求單" + the_no + "資料失敗!" + ex;
                    return RedirectToAction("P_23N05_q", "F_23N05");
                }
            }
            else
            {
                TempData["message"] = "查詢資訊需求單資料失敗: 單號資訊空值!";
                return RedirectToAction("P_23N05_q", "F_23N05");
            }
        }
    }
}