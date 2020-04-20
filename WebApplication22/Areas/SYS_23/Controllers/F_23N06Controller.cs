using Common.BasicLibrary;
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
    public class F_23N06Controller : Controller
    {
        //23. 系統權限管理 > N.資訊服務需求單 > 06. 資訊服務需求單部門主管審核作業
        //預設有權限的同仁才可看到此選項

        //資料庫物件
        TransartEntities db = new TransartEntities();
        Mail_F23N mail23 = new Mail_F23N();

        // GET: SYS_23/F_23N06
        public ActionResult Index()
        {
            return View();
        }

        //http://192.168.1.233:8000/MisService/mser01_q1.asp?The_dpno=094&loginID=785
        //<% = GetMisService3(Request.QueryString("loginID") ,Request.QueryString("The_dpno") ,Request.QueryString("PageNo") ,Request.QueryString("PageSize") ) %>

        //public ActionResult P_23N06_q(string loginID, string the_dpno, int? PageNumber, int? PageSize)
        public ActionResult P_23N06_q(string loginID, string loginName, string the_dpno, int? PageNumber, int? PageSize)
        {
            //參數與原程式碼不同, 新增傳入登入者名稱; 
            //原程式可直取登入者名稱, 新系統未設定, 故新增變數string loginName供重複使用

            //改寫程式碼如下
            /*
             * 
             Transart\MisService\library\MisService_wfls.asp 
             GetMisService3'=	Description: 資訊服務需求單--部門主管審核
             原查詢條件
                lsTableList = " MisService "
	            lsTableList = lsTableList & " Inner Join dbo.GetAllEmpList() A on MS_EMNO = em_no "

	            lsWhereList = " MS_POST = 1 and MS_YN = 0 "

	            IF loginID <> "" THEN
		            lsWhereList = lsWhereList & VBCrlf & " and MS_EMNO in (														"
		            lsWhereList = lsWhereList & VBCrlf & " 	select vr_emno from VerifyGroupUser Where   						"
		            lsWhereList = lsWhereList & VBCrlf & " 		vr_vpseq in (                           						"
		            lsWhereList = lsWhereList & VBCrlf & "	select vp_seq from VerifyGroup       								"
		            lsWhereList = lsWhereList & VBCrlf & "		where  vp_vdseq = 17                                           	"
		            lsWhereList = lsWhereList & VBCrlf & "			and (                                                   	"
		            lsWhereList = lsWhereList & VBCrlf & "				vp_emno = '" & loginID & "'                         	"
		            lsWhereList = lsWhereList & VBCrlf & "				OR (                                                	"
		            lsWhereList = lsWhereList & VBCrlf & "					vp_agent = '" & loginID & "'                    	"
		            lsWhereList = lsWhereList & VBCrlf & "					and vp_emno in (                                	"
		            lsWhereList = lsWhereList & VBCrlf & "						select vl_emno from [dbo].[v_NotWorkEmp]    	"
		            lsWhereList = lsWhereList & VBCrlf & "					)                                               	"
		            lsWhereList = lsWhereList & VBCrlf & "				)                                                   	"
		            lsWhereList = lsWhereList & VBCrlf & " 				OR (                                                	"
		            lsWhereList = lsWhereList & VBCrlf & "					vp_agent1 = '" & loginID & "'                   	"
		            lsWhereList = lsWhereList & VBCrlf & "						and vp_emno in (                            	"
		            lsWhereList = lsWhereList & VBCrlf & "							select vl_emno from [dbo].[v_NotWorkEmp]	"
		            lsWhereList = lsWhereList & VBCrlf & "						)                                           	"
		            lsWhereList = lsWhereList & VBCrlf & "						and vp_agent in (                           	"
		            lsWhereList = lsWhereList & VBCrlf & "							select vl_emno from [dbo].[v_NotWorkEmp]	"
		            lsWhereList = lsWhereList & VBCrlf & "						)                                           	"
		            lsWhereList = lsWhereList & VBCrlf & "					)                                               	"
		            lsWhereList = lsWhereList & VBCrlf & "			)                                                       	"

		            lsWhereList = lsWhereList & VBCrlf & " 		)                                                           	"
		            lsWhereList = lsWhereList & VBCrlf & " )                                                                	"
	            END IF

	            lsWhereList = lsWhereList & " order by MS_NO,MS_DATE1 "
             */

            //宣告變數
            string uid = ""; //登入者工號
            string uname = ""; //登入者名稱
            string dpno = ""; //登入者部門代號
            string SQL = ""; //SQL 查詢指令
            employee emp; //員工資料表

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定登入者工號與部門資訊
            /*
            document.all.MS_EMNO.value = window.sysTransArtSecurity.loginID;
	        document.all.MS_NAME.value = window.sysTransArtSecurity.loginCName;
	        document.all.MS_DPNO.value = window.sysTransArtSecurity.loginDPNO;
             */
            uid = loginID?? "";

            //uid = "005";//測試用
            uname = loginName?? "";
            dpno = the_dpno??""; //如參數空值, 接收後會變null, 下方判斷dpno == null | dpno.Trim() == "")會異常
            //工號參數為空, 查找登入者資訊並更新所屬部門代碼
            if (uid == null | uid == "")
            {
                uid = (string)Session[TARGET.user_id];
                emp = db.employee.Find(uid.Trim());
                if (emp != null)
                {
                    uname = emp.em_cname.Trim();
                    dpno = emp.em_dpno.Trim();
                }
                else
                {
                    //警示使用者?
                }
            }
            else
            {
                //登入者工號參數不為空, 但部門代碼參數或登入者名稱為空時, 查找登入者資訊並更新所屬部門代碼
                if ((dpno == null | dpno.Trim() == "") | (uname == null | uname.Trim() == ""))
                {
                    emp = db.employee.Find(uid.Trim());
                    if (emp != null)
                    {
                        uname = emp.em_cname.Trim();
                        dpno = emp.em_dpno.Trim();
                    }
                    else
                    {
                        //警示使用者?
                    }
                }
            }

            //3. 設定查詢頁面分頁筆數
            int pageSize = PageSize ?? 10;//每頁筆數
            int pageNumber = PageNumber ?? 1;

            //4. 查詢網頁資料集合
            SQL = " select MisService.*, em_cname, dp_name ";
            SQL = SQL + " from MisService Inner Join dbo.GetAllEmpList() A on MS_EMNO = em_no ";
            SQL = SQL + " where MS_POST = 1 and MS_YN = 0 ";

            if (uid != null & uid != "")
            {
                SQL = SQL + " and MS_EMNO in ( ";
                SQL = SQL + "   select vr_emno from VerifyGroupUser Where ";
                SQL = SQL + "      vr_vpseq in ( ";
                SQL = SQL + "        select vp_seq from VerifyGroup ";
                SQL = SQL + "           where  vp_vdseq = 17 ";
                SQL = SQL + "             and( ";
                SQL = SQL + "                 vp_emno = '" + uid + "' ";
                SQL = SQL + "                 OR( ";
                SQL = SQL + "                   vp_agent = '" + uid + "' ";
                SQL = SQL + "                   and vp_emno in ( ";
                SQL = SQL + "                      select vl_emno from[dbo].[v_NotWorkEmp] ";
                SQL = SQL + "                   ) ";
                SQL = SQL + "                 ) ";
                SQL = SQL + "                 OR( ";
                SQL = SQL + "                   vp_agent1 = '" + uid + "' ";
                SQL = SQL + "                   and vp_emno in ( ";
                SQL = SQL + "                      select vl_emno from[dbo].[v_NotWorkEmp] ";
                SQL = SQL + "                   ) ";
                SQL = SQL + "                   and vp_agent in ( ";
                SQL = SQL + "                      select vl_emno from[dbo].[v_NotWorkEmp] ";
                SQL = SQL + "                   ) ";
                SQL = SQL + "                 ) ";
                SQL = SQL + "              ) ";
                SQL = SQL + "        ) ";
                SQL = SQL + "    ) ";

                try
                {
                    List<MisServiceManager> msr = db.Database.SqlQuery<MisServiceManager>(SQL).ToList();
                    IQueryable<MisServiceManager> msrIQ = msr.AsQueryable();

                    //4.1 將資料進行分頁, 並指定傳給前端的變數已設定分頁顯示
                    IPagedList msrPage = msrIQ.ToPagedList(pageNumber, pageSize);

                    //5. 傳遞參數
                    ViewBag.loginID = uid;
                    ViewBag.loginName = uname;
                    ViewBag.the_dpno = dpno;
                    ViewBag.PageSize = pageSize;
                    ViewBag.PageNumber = pageNumber;

                    return View(msrPage);
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "取得資訊需求單明細失敗:" + ex;
                    return View();
                }
            }
            else
            {
                return View();
            }//end of uid != null
        }

        public ActionResult MisServiceShow1(string MS_NO)
        {
            //改寫程式碼如下
            /*
             * 
             Transart\MisService\mser01_l1.asp
             MisServiceShow1'=	Description: 資訊服務需求單--內容
             */

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
	            lsTableList = " MisService Left Join MisSerReject ON MS_NO = MST_MSNO "
	            lsWhereList = " MS_NO = '" & MS_NO & "' ORDER BY MS_no "
                 */
                selectStr = "select * ";
                selectStr = selectStr + " from MisService mis ";
                selectStr = selectStr + " left join MisSerReject msr ";
                selectStr = selectStr + " on mis.MS_NO = msr.MST_MSNO ";
                selectStr = selectStr + " where mis.MS_No ='" + ms_no + "'";
                selectStr = selectStr + " order by mis.MS_No";

                try
                {
                    List<MisServiceReject> msr = db.Database.SqlQuery<MisServiceReject>(selectStr).ToList();

                    if (msr.Count == 0)
                    {
                        //確認測試環境在http://192.168.1.233:8000/MisService/mser05_q.asp?The_No=IS041301&Date1=&Date2=&QryKind=2
                        //沒對應資料會維持在mser05_q.asp頁面, 不會轉導mser05_l.asp
                        ViewBag.ms_noSeach = ms_no;
                    }
                    return PartialView(msr);
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "查詢資訊需求單" + ms_no + "資料失敗!" + ex;
                    return RedirectToAction("P_23N06_q", "F_23N06");//須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
                }
            }
            else
            {
                TempData["message"] = "查詢資訊需求單資料失敗: 單號資訊空值!";
                return RedirectToAction("P_23N06_q", "F_23N06");//須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
            }
        }

        public ActionResult wfl_MisService_Post1(FormCollection post)
        {
            //改寫程式碼如下
            /*
             * 
             Transart\MisService\library\MisService_wfls.asp
             wfl_MisService_Post1'=	Description: 功能：資訊服務需求單審核--部門主管核准
             */

            //變數宣告
            string updateStr = ""; // 資料更新SQL 字串
            int rowCount = 0; // 前端資料列顯示的總筆數
            int RowsAffected = 0; //資料列更新筆數
            string resultStr = ""; //更新紀錄

            //欄位更新與寄送信件所需變數
            string MS_POST = "";
            string MS_NO = "";
            string MS_SUBJECT = "";
            string EMNO1 = "";
            string EMCNAME1 = "";
            string MS_EMNO = "";
            string MS_NAME = "";
            string MS_VDATE = "";
            string MS_CONTENT1 = "";
            string MS_CONTENT2 = "";
            string MS_DPNOM1 = "";
            string MS_DPNOM2 = "";
            string MS_REASON = "";

            //接收前端Form
            if (post != null)
            {
                rowCount = int.Parse(post["RecCount"]);

                for (int i=0; i < rowCount; i++)
                {
                    //針對被勾選的資料列進行更新(勾選為1 未勾選null)
                    MS_POST = post["MS_POST" + i.ToString()];            //'資訊服務需求審核

                    if (MS_POST == "1")
                    {
                        //確認變數未被應用到, 故不處理
                        //MS_DPNAME = Request.Form("MS_DPNAME" & i & "")   //'部門別
                        //KIND = 3 //不明用途

                        MS_NO = post["MS_NO" + i.ToString()];             //'資訊服務需求單號
                        MS_SUBJECT = post["MS_SUBJECT" + i.ToString()];   //'案件主題                        
                        EMNO1 = post["MS_EMNO"];                           //'案件審核者卡號
                        EMCNAME1 = post["MS_NAME"];                        //'案件審核者姓名
                        MS_EMNO = post["MS_EMNO" + i.ToString()];         //'案件申請者卡號
                        MS_NAME = post["MS_NAME" + i.ToString()];         //'案件申請者姓名
                        //MS_VDATE = post["MS_DATE1"];                       //帶入處理單據的日期, 不是取用此單號的資料列的MS_DATE1 欄位
                        MS_VDATE = DateTime.Parse(post["MS_DATE1"]).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        MS_CONTENT1 = post["MS_CONTENT" + i.ToString()];
                        MS_CONTENT2 = post["MS_CONTENT2" + i.ToString()];
                        MS_DPNOM1 = post["MS_EMNO"];                       //'審核者卡號
                        MS_DPNOM2 = post["MS_NAME"];                       //'審核者姓名
                        MS_REASON = post["MS_REASON" + i.ToString()];     //原因

                        updateStr = " Update MisService set " +
                            " MS_POST = 3, " +
                            " MS_REASON = '" + MS_REASON + "', " +
                            //dictDataUpdate.Add "MS_VDATE"	,  "getdate()"
                            //" MS_VDATE ='" + DateTime.Today.ToLongDateString() + "', " + 格式會變中文年月日
                            //" MS_VDATE ='" + DateTime.Today + "', " + 格式含中文2020/4/16 上午 12:00:00
                            " MS_VDATE ='" + DateTime.Today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "', " +
                            " MS_DPNOM1 ='" + MS_DPNOM1 + "', " +
                            " MS_DPNOM2 ='" + MS_DPNOM2 + "' " +
                            " Where MS_NO='" + MS_NO + "'";                                                
                        try
                        {
                            RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                            //RowsAffected = 0;
                            if (RowsAffected > 0)
                            {
                                db.SaveChanges();
                                mail23.SendProcessMail(MS_NO, MS_SUBJECT, EMNO1, EMCNAME1, MS_EMNO, MS_NAME, "3", MS_REASON, MS_VDATE, MS_CONTENT1, MS_CONTENT2);

                                //TempData["message"] = "單號:" + MS_NO + " 核准成功:";
                                resultStr = resultStr + "單號:" + MS_NO + " 核准成功!\n";
                            }
                            else
                            {
                                resultStr = resultStr + "單號:" + MS_NO + " 核准失敗:更新0筆資料!\n";
                                //TempData["message"] = "單號:" + MS_NO + " 核准失敗:更新0筆資料!";                               
                            }// end of RowsAffected > 0

                        }
                        catch (System.Exception ex)
                        {
                            resultStr = resultStr + "單號:" + MS_NO + " 核准失敗:\n " + ex;
                            //TempData["message"] = "單號:" + MS_NO + " 核准失敗:\n " + ex;
                        }
                    } //end of if post=1                   
                }//end of for
                //表單全部處理後一起轉導!
                if (resultStr != "")
                {
                    TempData["message"] = resultStr;
                }
                return RedirectToAction("P_23N06_q", "F_23N06"); //須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
            }
            else
            {
                TempData["message"] = "核准失敗:表單資料無法取得!";
                return RedirectToAction("P_23N06_q", "F_23N06"); //須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
            }//end of post!=null          
        }

        public ActionResult wfl_MisService_Post2(FormCollection post)
        {
            //改寫程式碼如下
            /*
             * 
             Transart\MisService\library\MisService_wfls.asp
             wfl_MisService_Post2'=	Description: 資訊服務需求單審核--部門主管不核准
             */

            //變數宣告
            string updateStr = ""; // 資料更新SQL 字串
            int rowCount = 0; // 前端資料列顯示的總筆數
            int RowsAffected = 0; //資料列更新筆數
            string resultStr = ""; //更新紀錄

            //欄位更新與寄送信件所需變數
            string MS_POST = "";
            string MS_NO = "";
            string MS_SUBJECT = "";
            string EMNO1 = "";
            string EMCNAME1 = "";
            string MS_EMNO = "";
            string MS_NAME = "";
            string MS_VDATE = "";
            string MS_CONTENT1 = "";
            string MS_CONTENT2 = "";
            string MS_DPNOM1 = "";
            string MS_DPNOM2 = "";
            string MS_REASON = "";

            //接收前端Form
            if (post != null)
            {
                rowCount = int.Parse(post["RecCount"]);

                for (int i = 0; i < rowCount; i++)
                {
                    //針對被勾選的資料列進行更新(勾選為1 未勾選null)
                    MS_POST = post["MS_POST" + i.ToString()];            //'資訊服務需求審核

                    if (MS_POST == "1")
                    {
                        //確認變數未被應用到, 故不處理
                        //MS_DPNAME = Request.Form("MS_DPNAME" & i & "")   //'部門別
                        //KIND = 4 //不明用途

                        MS_NO = post["MS_NO" + i.ToString()];             //'資訊服務需求單號
                        MS_SUBJECT = post["MS_SUBJECT" + i.ToString()];   //'案件主題                        
                        EMNO1 = post["MS_EMNO"];                           //'案件審核者卡號
                        EMCNAME1 = post["MS_NAME"];                        //'案件審核者姓名
                        MS_EMNO = post["MS_EMNO" + i.ToString()];         //'案件申請者卡號
                        MS_NAME = post["MS_NAME" + i.ToString()];         //'案件申請者姓名
                        //MS_VDATE = post["MS_DATE1"];                       //帶入處理單據的日期, 不是取用此單號的資料列的MS_DATE1 欄位
                        MS_VDATE = DateTime.Parse(post["MS_DATE1"]).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        MS_CONTENT1 = post["MS_CONTENT" + i.ToString()];
                        MS_CONTENT2 = post["MS_CONTENT2" + i.ToString()];
                        MS_DPNOM1 = post["MS_EMNO"];                       //'審核者卡號
                        MS_DPNOM2 = post["MS_NAME"];                       //'審核者姓名
                        MS_REASON = post["MS_REASON" + i.ToString()];     //原因

                        updateStr = " Update MisService set " +
                            " MS_POST = 4, " +
                            " MS_REASON = '" + MS_REASON + "', " +
                            //dictDataUpdate.Add "MS_VDATE"	,  "getdate()"
                            " MS_VDATE ='" + DateTime.Today.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "', " +
                            " MS_DPNOM1 ='" + MS_DPNOM1 + "', " +
                            " MS_DPNOM2 ='" + MS_DPNOM2 + "' " +
                            " Where MS_NO='" + MS_NO + "'";
                        try
                        {
                            RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                            
                            if (RowsAffected > 0)
                            {
                                db.SaveChanges();
                                mail23.SendProcessMail(MS_NO, MS_SUBJECT, EMNO1, EMCNAME1, MS_EMNO, MS_NAME, "4", MS_REASON, MS_VDATE, MS_CONTENT1, MS_CONTENT2);
                                
                                resultStr = resultStr + "單號:" + MS_NO + " 不核准成功!\n";
                            }
                            else
                            {
                                resultStr = resultStr + "單號:" + MS_NO + " 不核准失敗:更新0筆資料!\n";                    
                            }// end of RowsAffected > 0

                        }
                        catch (System.Exception ex)
                        {
                            resultStr = resultStr + "單號:" + MS_NO + " 不核准失敗:\n " + ex;
                        }
                    } //end of if post=1                   
                }//end of for
                //表單全部處理後一起轉導!
                if (resultStr != "")
                {
                    TempData["message"] = resultStr;
                }
                return RedirectToAction("P_23N06_q", "F_23N06"); //須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
            }
            else
            {
                TempData["message"] = "不核准失敗:表單資料無法取得!";
                return RedirectToAction("P_23N06_q", "F_23N06"); //須加參數?, 先不用, 程式會判斷欄位值是否有值, 無值會重新抓取
            }//end of post!=null          
        }
    }
}