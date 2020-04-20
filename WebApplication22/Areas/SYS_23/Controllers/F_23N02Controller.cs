using Common.BasicLibrary;
using Common.Menu;
using Data.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23
{
    public class F_23N02Controller : Controller
    {
        //23. 系統權限管理 > N.資訊服務需求單 > 02. 資訊服務需求單回饋作業

        //資料庫物件
        TransartEntities db = new TransartEntities();
        Mail_F23N mail23 = new Mail_F23N();

        // GET: SYS_23/F_23N02
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult P_23N02_l(string Kind, int? PageNumber, int? PageSize)
        {
            //改寫程式碼如下
            /*
             * 
             Transart\MisService\library\MisService_wfls.asp 
             GetMisService2'=	Description: 資訊服務需求單--KIND=1：接單確認  2：完工回饋
             原查詢條件
             lsFieldList = " * "
	         lsTableList = " MisService "
             SELECT CASE KIND
		     CASE "1"	'接單確認
			 lsWhereList = " MS_POST = 5 AND MS_YN = 0 ORDER BY MS_no "
		     CASE "2"	'完工回饋
			 lsWhereList = " MS_POST = 5 AND MS_YN = 1 AND MS_YN1 = 0 ORDER BY MS_SCHDATE , MS_no "
		     CASE "3"	'結單
			 lsWhereList = " MS_POST = 5 AND MS_YN = 1 AND MS_YN1 = 1 AND MS_CLOSE = 0 ORDER BY MS_no "
             */

            //宣告變數
            string opKind=""; //單據操作類別
            //string title = ""; //網頁標題
            IOrderedQueryable<MisService> mis; //資料集合

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定查詢頁面分頁筆數
            int pageSize = PageSize ?? 10;//每頁筆數
            int pageNumber = PageNumber ?? 1;

            //3. 依類別顯示title與按鈕與指定初始查詢網頁資料集合
            if (Kind == null | Kind == "")
            {
                opKind = "1";
            }
            else
            {
                opKind = Kind;
            }      

            if (opKind == "1")
            {
                //title = "資訊服務需求單--接單確認";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 0).OrderBy(s => s.MS_NO);
            }
            else if (opKind == "2")
            {
                //title = "資訊服務需求單--完工回饋";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 1 && s.MS_YN1 == 0).OrderBy(s => s.MS_SCHDATE).OrderBy(n => n.MS_NO);
            }
            else if (opKind == "3")
            {
                //title = "資訊服務需求單--結單";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 1 && s.MS_YN1 == 1 && s.MS_CLOSE == 0).OrderBy(s => s.MS_NO);
            }
            else
            {
                //title = "資訊服務需求單--接單確認";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 0).OrderBy(s => s.MS_NO);
            }
            
            //4. 將資料進行分頁, 並指定傳給前端的變數已設定分頁顯示
            //IPagedList<MisService> misPage = mis.ToPagedList(pageNumber, pageSize);
            IPagedList misPage = mis.ToPagedList(pageNumber, pageSize);

            //5. 其他變數傳遞前端
            ViewBag.kind = opKind;
            //ViewBag.Title = title;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            return View(misPage);
        }

        public ActionResult P_23N02_e(string Kind, string MS_NO)
        {
            /*
            * 
            改寫Transart\MisService\library\MisService_wfls 
            MisServiceEdit2'=	Description: 資訊服務需求單--編輯 --KIND=1：接單確認  2：完工回饋  3：結單
            原查詢條件
            lsFieldList = " * "
            lsTableList = " MisService "
            CASE "1"	'接單確認
			    ShowTitle = "資訊服務需求單--接單確認--編輯"
			    lsWhereList = " MS_POST = 5 AND MS_YN = 0 AND MS_NO = '" & MS_NO & "' ORDER BY MS_no "
		    CASE "2"	'完工回饋
			    ShowTitle = "資訊服務需求單--完工回饋--編輯"
			    lsWhereList = " MS_POST = 5 AND MS_YN = 1 AND MS_YN1 = 0 AND MS_NO = '" & MS_NO & "' ORDER BY MS_no "
		    CASE "3"	'結單
			    ShowTitle = "資訊服務需求單--結單--編輯"
			    lsWhereList = " MS_POST = 5 AND MS_YN = 1 AND MS_YN1 = 1 AND MS_CLOSE = 0 AND MS_NO = '" & MS_NO & "' ORDER BY MS_no 
            */

            //宣告變數
            string opKind = "";//單據類別
            string ms_no = "";//單號
           
            string uid ="";//登入者工號資訊
            string empName = "";//取得報修者姓名資訊

            List<MisService> mis; //資料集合
            employee emp;//員工資料

            //1. 依登入者身分指定權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定變數
            opKind = Kind;
            ms_no = MS_NO;
            if (opKind == null | opKind == "")
            {
                opKind = "1";
            }

           if (ms_no == null | ms_no.Trim() == "")
            {
                //因單號參數傳遞失敗, 故導前頁View顯示無資料
                ViewBag.ms_noSeach = ms_no;
                ViewBag.kind = opKind;
                return View();
            }

            //3. 依類別與單號條件查詢網頁資料集合
            if (opKind == "1")
            {
                ViewBag.Title = "資訊服務需求單--接單確認--編輯";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 0 & s.MS_NO == ms_no).OrderBy(s => s.MS_NO).ToList();
            }
            else if (opKind == "2")
            {
                ViewBag.Title = "資訊服務需求單--完工回饋--編輯";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 1 && s.MS_YN1 == 0 && s.MS_NO == ms_no).OrderBy(s => s.MS_SCHDATE).OrderBy(n => n.MS_NO).ToList();
            }
            else if (opKind == "3")
            {
                ViewBag.Title = "資訊服務需求單--結單--編輯";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 1 && s.MS_YN1 == 1 && s.MS_CLOSE == 0 && s.MS_NO == ms_no).OrderBy(s => s.MS_NO).ToList();
            }
            else
            {
                ViewBag.Title = "資訊服務需求單--接單確認--編輯";
                mis = db.MisService.Where(s => s.MS_POST == 5 && s.MS_YN == 0 && s.MS_NO == ms_no).OrderBy(s => s.MS_NO).ToList();
            }
                     
            //4. 判斷需求單條件是否存在資料, 並據此傳遞資料
            
            ViewBag.kind = opKind; //必傳
            if (mis == null | mis.Count == 0)
            {
                //維持導前頁, 由前頁顯示無資料的訊息!
                ViewBag.ms_noSeach = ms_no;
                return View();
            }
            else
            {
                //5. 取得登入者資訊做接單確認者預設值
                uid = (string)Session[TARGET.user_id];
                if (uid != null)
                {
                    //傳入登入者資訊到前端網頁              
                    if (uid != null & uid != "")
                    {
                        emp = db.employee.Find(uid.Trim());
                        if (emp != null)
                        {                           
                            empName = emp.em_cname.Trim();
                        }
                    }
                    else
                    {
                        //員工資料不存在留白, 前端參數留空
                    }
                }
                else
                {
                    //登入者工號無法取得, 前端參數留空
                }

                //預設帶入接單者資訊
                ViewBag.emno = uid;
                ViewBag.ename = empName;
                ViewBag.msno = ms_no;

                return View(mis);
            }// end if mis == null    
        }

        [HttpPost]
        //public ActionResult P_23N02_e(string Kind, string closeYN, [Bind(Include = "MS_NO, MS_ACDATE, MS_ACEMNO, MS_ACNAME, MS_PCDATE, MS_PCHOUR, MS_SCHDATE, MS_YN, MS_RCDATE, MS_RCHOUR,  MS_MEMO1, MS_MEMO2, MS_CLOSE")] MisService MisService)
        //public ActionResult P_23N02_e(FormCollection post, [Bind(Include = "MS_NO, MS_ACDATE, MS_ACEMNO, MS_ACNAME, MS_PCDATE, MS_PCHOUR, MS_SCHDATE, MS_YN, MS_RCDATE, MS_RCHOUR,  MS_MEMO1, MS_MEMO2, MS_CLOSE")] MisService MisService)
        //上面採用MisService Bind 欄位方法若有未宣告, 則執行db.save時, 原本有值的欄位會因沒bind 所以變成null, 改用update語法
        //public ActionResult P_23N02_e(FormCollection post, List<MisService> mis)
        public ActionResult P_23N02_e(FormCollection post)
        {
            /*           
             改寫
             Transart\MisService\library\MisService_wfls.asp 
             wfl_MisService_Update2
             功能：資訊服務需求單編輯儲存--KIND=1：接單確認  2：完工回饋  3：結單
             */
            //kind=3 的操作應該不存在
            //宣告變數
            string opKind = "";//單據類別
            string opcloseYN = ""; //完結
            string updateStr = "";//Update SQL指令
            int RowsAffected = 0; //SQL執行結果
            string RCDATE = ""; //完工日期是系統確認勾選結案選項後, 抓取當下時間更新到資料庫(非同仁填值)

            // 1. 檢查form 物件是否存在
            if (post != null)
            {
                //1.1 設定變數
                opKind = post["Kind"];
                opcloseYN = post["CloseYN"];//如未勾選, 其值null; 反之為1

                if (opKind == null | opKind == "")
                {
                    opKind = "1";
                }

                //2. 依類別處理資料更新組成update SQL 語法
                if (opKind == "1")
                {
                    //更新
                    //MS_ACDATE   '接單日期
                    //MS_ACEMNO   '接單確認者卡號
                    //MS_ACNAME   '接單確認者姓名
                    //MS_PCDATE   '預計完成日期
                    //MS_PCHOUR   '預計處理工時
                    //MS_YN = 1;  '接單確認
                    //'2008/10/14 By Clark 接單時以預定完成日期為排程日期
                    //dictDataUpdate.Add "MS_SCHDATE"		,  "'"& MS_PCDATE    &"'"

                    updateStr = "Update MisService set " +
                        //"MS_ACDATE = " + DateTime.Parse(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()) + "," +
                        //上述語法執行到ExecuteSqlCommand會因為上午 或下午 關鍵字出錯
                        "MS_ACDATE = '" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'," +
                        "MS_ACEMNO = '" + post["MS_ACEMNO"] + "' ," +
                        "MS_ACNAME = '" + post["MS_ACNAME"] + "'," +
                        "MS_PCDATE = '" + DateTime.Parse(post["item.MS_PCDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "',"; //使用表單欄位值更新, 雖然語法沒錯但是更新後會變成1990錯誤時間
                      

                    //MS_PCHOUR	= Request.Form("MS_PCHOUR")             '預計處理工時
                    //IF MS_PCHOUR = "" THEN MS_PCHOUR = 0

                    if (post["MS_PCHOUR"].ToString().Trim() == "")
                    {
                        updateStr = updateStr + "MS_PCHOUR = 0, ";
                    }
                    else
                    {
                        updateStr = updateStr + "MS_PCHOUR = " + post["MS_PCHOUR"] + ",";
                    }

                    updateStr = updateStr + "MS_YN = 1,";

                    //'2008/10/14 By Clark 接單時以預定完成日期為排程日期
                    //dictDataUpdate.Add "MS_SCHDATE"		,  "'"& MS_PCDATE    &"'"
                    updateStr = updateStr + "MS_SCHDATE = '" + DateTime.Parse(post["item.MS_PCDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'";
                    updateStr = updateStr + " where  MS_NO = '" + post["MS_NO"] + "'";

                    /*
                    '發出處理進度通知信
                    '2012/12/20 by souleye 接單不需要發信件
                    'CALL SendProcessMail( MS_NO , MS_SUBJECT , MS_ACEMNO , MS_ACNAME , MS_EMNO , MS_NAME , KIND , "" , MS_PCDATE ,MS_CONTENT1 ,MS_CONTENT2)
                    */
                }
                else if (opKind == "2")
                {
                    //更新
                    //MS_PCDATE    '預計完成日期
                    //MS_SCHDATE   '排程日期 //與kind=1 直接採用接單時以預定完成日期為排程日期, 差異?
                    //MS_MEMO1     '延誤原因說明
                    //MS_MEMO2     '完工說明

                    updateStr = "Update MisService set " +
                        //"MS_PCDATE = " + post["item.MS_PCDATE"] + "," +
                        //"MS_SCHDATE = " + post["item.MS_SCHDATE"] + "," +
                        "MS_PCDATE = '" + DateTime.Parse(post["item.MS_PCDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'," +
                        "MS_SCHDATE = '" + DateTime.Parse(post["item.MS_SCHDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'," +
                        "MS_MEMO1 = '" + post["MS_MEMO1"] + "',";
                    /*
                    MS_RCDATE	= FormatDateTime(now(),vbShortDate) & " " & FormatDateTime(now(),vbShortTime)			'完工日期：2012/12/17 by souleye 改加入時間
			        MS_RCHOUR	= Request.Form("MS_RCHOUR")             '實際處理工時
			        MS_YN1		= 1            							'完工回饋
			        MS_MEMO1 	= Request.Form("MS_MEMO1")				'延誤原因說明
			        MS_MEMO2 	= Request.Form("MS_MEMO2")				'完工說明
			        CloseYN		= Request.Form("CloseYN")            	'完結否
                     */

                    /* CloseYN = Request.Form("CloseYN      '完結否
                     '2008/10/14 By Clark 勾選完結，才轉入結單處理
                     '2012/12/20 by souleye 勾選完結，即結單
                     * 
                     */
                    if (opcloseYN == "1")
                    {
                        RCDATE = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        updateStr = updateStr +
                            "MS_MEMO2 = '" + post["MS_MEMO2"] + "'," + //後面有其他更新條件, 需加逗點
                                                                       //"MS_RCDATE = '" + post["MS_RCDATE"] + "' ," +
                            "MS_RCDATE = '" + RCDATE + "' ," +
                            "MS_RCHOUR = " + post["MS_RCHOUR"] + "  ," +
                            "MS_YN1 = 1 ," +
                            "MS_CLOSE = 1 ";
                    }
                    else
                    {
                        updateStr = updateStr + "MS_MEMO2 = '" + post["MS_MEMO2"] + "'"; //後面無其他更新條件, 不加逗點
                    }
                    updateStr = updateStr + " where  MS_NO = '" + post["MS_NO"] + "'";
                }
                else if (opKind == "3")
                {
                    //原程式無需處理
                } //end of if (opKind

                //3. 如果資料列更新筆數>0, 執行存檔動作
                if (opKind == "1" | opKind == "2")
                {
                    try
                    {
                        RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                        if (RowsAffected > 0)
                        {
                            db.SaveChanges();

                            //3.1 如果存檔成功, 設定不同訊息
                            if (opKind == "1")
                            {
                                TempData["message"] = "接單確認儲存成功";
                            }
                            else if (opKind == "2")
                            {
                                TempData["message"] = "完工回饋儲存成功";
                            }
                            else if (opKind == "3")
                            {
                                TempData["message"] = "結單儲存成功";
                            }

                            if (opKind == "2" & opcloseYN == "1")
                            {
                                //'發出處理進度通知信
                                //CALL SendProcessMail(MS_NO , MS_SUBJECT , MS_ACEMNO , MS_ACNAME , MS_EMNO , MS_NAME , 2 , MS_MEMO2 , MS_RCDATE ,MS_CONTENT1 ,MS_CONTENT2)
                                mail23.SendProcessMail(post["MS_NO"], post["MS_SUBJECT"], post["MS_ACEMNO"], post["MS_ACNAME"], post["MS_EMNO"], post["MS_NAME"], "2", post["MS_MEMO2"], RCDATE, post["MS_CONTENT1"], post["MS_CONTENT2"]);
                            }

                            if (opKind == "2" & opcloseYN != "1")
                            {
                                return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"] });
                            }
                            else
                            {
                                return RedirectToAction("P_23N02_l", "F_23N02", new { Kind = opKind });
                            }
                        }
                        else
                        {
                            TempData["message"] = "更新資料庫筆數0筆, 儲存失敗";
                            //return View(mis);
                            return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                        } // end of roweffected
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "儲存失敗" + ex;
                        //return View(mis);
                        return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                    }
                }
                else
                {
                    //確認原asp 針對kind=3者, 不做任何資料更新
                    return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                }
            }
            else
            {
                TempData["message"] = "取得表單資訊失敗, 更新失敗!";
                //return View(mis);
                return RedirectToAction("P_23N02_1", "F_23N02", new { Kind = opKind });
            } //end of post is null
        }

        [HttpPost]
        //public ActionResult wfl_MisService_Back(FormCollection post, [Bind(Include = "MS_NO, MST_SUBJECT, MST_CONTENT1, MS_ACEMNO, MS_ACBACKMEMO, MS_ACDATE, MS_ACEMNO, MS_ACNAME")] MisService MisService)
        public ActionResult wfl_MisService_Back(FormCollection post)
        {

            // 改寫 Transart\MisService\library\MisService_wfls.asp
            // wfl_MisService_Back
            // 功能：資訊服務需求單編輯儲存--KIND=1：接單確認  2：完工回饋  3：結單
            // 點選"接單退件" 或是 "完工退件"

            //宣告變數
            string opKind = "";//單據類別
            MisSerReject msreject = new MisSerReject(); //新增退件物件
            string updateStr = ""; //Update SQL字串
            int RowsAffected = 0; //資料更新筆數
            string nowStr = "";

            //1. 檢查表單資料是否可以成功存取
            if (post != null)
            {
                //1.1 接收需求單種類資訊
                opKind = post["Kind"];
                if (opKind == null | opKind.Trim() == "")
                {
                    opKind = "1";
                }

                //2. 依種類設定資料狀態            
                if (opKind == "1")
                {
                    //'退件；可輸入退件說明                   

                    //新增需求單退件紀錄
                    msreject.MST_MSNO = post["MS_NO"];
                    msreject.MST_SUBJECT = post["MS_SUBJECT"];
                    msreject.MST_CONTENT1 = post["MS_CONTENT1"];
                    msreject.MST_ACBACKMEMO = post["MS_ACBACKMEMO"];
                    msreject.MST_DATE = DateTime.Now;
                    msreject.MST_ACEMNO = post["MS_ACEMNO"];
                    msreject.MST_ACNAME = post["MS_ACNAME"];

                    //db.Entry(msreject).State = EntityState.Modified; //需先將值都設好再一起檢查, 不需要?!
                    try
                    {                        
                        db.MisSerReject.Add(msreject);
                        db.SaveChanges();
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "新增需求單退件紀錄失敗:" + ex;
                        //此Action 不含對應View, 以下轉導不會帶出原更改資料
                        return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                    }

                    updateStr = "Update MisService set " +
                        "MS_POST = 7 ," +
                        "MS_ACBACKMEMO = '" + post["MS_ACBACKMEMO"].Trim() + "'" +
                        " where  MS_NO = '" + post["MS_NO"] + "'";
                }
                else if (opKind == "2") {
                    //'退回接單確認
                    updateStr = "Update MisService set " +
                       "MS_YN = 0" +
                       " where  MS_NO = '" + post["MS_NO"] + "'";
                } else if (opKind == "3") {
                    //'退回完工回饋；完工日期、處理工時歸零
                    updateStr = "Update MisService set " +
                      "MS_YN1 = 0," +
                      "MS_RCDATE = null," +
                      "MS_RCHOUR = 0" +
                      " where  MS_NO = '" + post["MS_NO"] + "'";
                }

                //3. 如果資料列更新筆數>0, 執行存檔動作
                try
                {
                    RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                    if (RowsAffected > 0)
                    {
                        db.SaveChanges();

                        if (opKind == "1")
                        {
                            //'發出處理進度通知信
                            //CALL SendProcessMail(MS_NO , MS_SUBJECT , MS_ACEMNO , MS_ACNAME , MS_EMNO , MS_NAME , 0 , MS_ACBACKMEMO , CDate4YYYYMMDD(Date()) ,MS_CONTENT1 ,MS_CONTENT2)
                            nowStr = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                            mail23.SendProcessMail(post["MS_NO"], post["MS_SUBJECT"], post["MS_ACEMNO"], post["MS_ACNAME"], post["MS_EMNO"], post["MS_NAME"], "0", post["MS_ACBACKMEMO"], nowStr, post["MS_CONTENT1"], post["MS_CONTENT2"]);

                            TempData["message"] = "退件成功！單號：" + post["MS_NO"].Trim() + "已退回！";
                        }
                        else if (opKind == "2")
                        {
                            TempData["message"] = "退件成功！單號：" + post["MS_NO"].Trim() + "  已退回接單確認！";
                        }
                        else if (opKind == "3")
                        {
                            TempData["message"] = "退件成功！單號：" + post["MS_NO"].Trim() + "  已退回完工回饋！";
                        }

                        return RedirectToAction("P_23N02_l", "F_23N02", new { Kind = opKind });
                    }
                    else
                    {
                        TempData["message"] = "更新資料庫筆數0筆, 儲存失敗";
                        return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                    }//end of roweffect > 0
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "需求單退件失敗:" + ex;
                    //此Action 不含對應View, 以下轉導不會帶出原更改資料
                    return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                }
            }
            else
            {
                TempData["message"] = "取得表單資訊失敗, 更新失敗!";
                return RedirectToAction("P_23N02_1", "F_23N02", new { Kind = opKind });
            }//end of post form == null
        }

        [HttpPost]
        public ActionResult wfl_MisService_Trun(FormCollection post)
        {
            /*
            //點選<轉件>按鈕
            //'功能：資訊服務需求單轉件為系統程式異動申請單
            // 改寫 Transart\MisService\library\MisService_wfls.asp
            // wfl_MisService_Trun
             * 
             * 
             */

            //宣告變數
            string md_no = ""; //取得程式異動申請單單號
            string opKind = "";//種類
            string updateStr = ""; //Update SQL
            int RowsAffected = 0; //執行SQL 更新的筆數
            string nowStr = "";
            
            //1. 確認表單資訓是否有資料
            if (post != null)
            {
                //1.1 設定變數
                opKind = post["Kind"];
                if (opKind == null | opKind.Trim() == "")
                {
                    opKind = "1";
                }

                //調整資料異動順序: 異動單先建好後, 需求單再改狀態
                //2. //新增一筆系統程式異動申請單
                try
                {
                    MisModify mm = new MisModify();
                    md_no = getNumber.getAutoNumber("ER", 0, true, 1);//      '取得單號
                    mm.MM_NO = md_no;
                    mm.MM_EMNO = post["MS_EMNO"].Trim();
                    mm.MM_NAME = post["MS_NAME"].Trim();
                    mm.MM_DPNO = post["MS_DPNO"].Trim();
                    mm.MM_DPNAME = post["MS_DPNAME"].Trim();
                    mm.MM_ATTACH = short.Parse(post["MS_ATTACH"].Trim());
                    mm.MM_DATE1 = DateTime.Parse(post["MS_DATE1"]);
                    //mm.MM_DATE2 = DateTime.Parse(post["MS_DATE2"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    mm.MM_DATE2 = DateTime.Parse(post["MS_DATE2"]);
                    mm.MM_SUBJECT = "資訊室轉件：" + post["MS_SUBJECT"].Trim();
                    mm.MM_CONTENT1 = post["MS_CONTENT1"].Trim();
                    mm.MM_CONTENT2 = post["MS_CONTENT2"].Trim();
                    mm.MM_UPDATE = DateTime.Now;
                    //確認原測試環境沒有此設定, 但是轉件後資料庫預設為0, 非null, 故在此補上
                    //insert 與Add 差異?! 但部分欄位mm_U1~U5 未宣告即預設帶0
                    mm.MM_Confirm = 0; 
                    mm.mm_sub = 0;
                    mm.MM_RCHOUR = 0; //DB 儲存會變0.0?
                    mm.MM_PCHOUR = 0; //DB 儲存會變0.0?
                    mm.MM_POST = 0;
                    mm.MM_YN = 0;
                    mm.MM_YN1 = 0;
                    mm.MM_CLOSE = 0;
                    mm.MM_ACBACKMEMO = "";
                    mm.MM_MEMO1 = "";
                    mm.MM_MEMO2 = "";
                    mm.MM_MEMO3 = "";

                    //儲存MisModify 資料異動
                    db.MisModify.Add(mm);
                    db.SaveChanges();               
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "轉件失敗- 新增程式異動單失敗:\n" + ex;
                    //此Action 不含對應View, 以下轉導不會帶出原更改資料
                    return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                }

                //3. 變更資訊服務需求單
                try
                {
                    updateStr = "Update MisService set " +
                           "MS_POST = 8" +
                           " where  MS_NO = '" + post["MS_NO"] + "'";
                    RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                    
                    if (RowsAffected > 0)
                    {
                        //儲存MisModify 資料異動
                        db.SaveChanges();

                        //'發出處理進度通知信
                        //CALL SendProcessMail(MS_NO , MS_SUBJECT , MS_ACEMNO , MS_ACNAME , MS_EMNO , MS_NAME , 8 , MS_ACBACKMEMO , CDate4YYYYMMDD(Date()) ,MS_CONTENT1 ,MS_CONTENT2)
                        // Response.Redirect("../mser02_l.asp?KIND=" & KIND & "&message=" & message)

                        nowStr = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        mail23.SendProcessMail(post["MS_NO"], post["MS_SUBJECT"], post["MS_ACEMNO"], post["MS_ACNAME"], post["MS_EMNO"], post["MS_NAME"], "8", post["MS_ACBACKMEMO"], nowStr, post["MS_CONTENT1"], post["MS_CONTENT2"]);

                        TempData["message"] = "轉件成功！單號：" + post["MS_NO"].Trim() + "  已轉件，轉件後單號為" + md_no + "!";

                        return RedirectToAction("P_23N02_l", new { KIND = opKind });
                    }
                    else
                    {
                        TempData["message"] = "更新資料庫筆數0筆, 儲存失敗";
                        return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });

                    } //end of RowsAffected > 0
                }
                catch (System.Exception ex)
                {
                    TempData["message"] = "轉件失敗- 新增程式異動單失敗:\n" + ex;
                    //此Action 不含對應View, 以下轉導不會帶出原更改資料
                    return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                }                   
            }
            else
            {
                TempData["message"] = "取得表單資訊失敗, 更新失敗!";
                return RedirectToAction("P_23N02_1", "F_23N02", new { Kind = opKind });
            } //end of post != null
        }

        [HttpPost]
        public ActionResult wfl_MisService_Update3(FormCollection post)
        {
            /*
            //點選<發送延誤通知>按鈕
            //'功能：資訊服務需求單轉件為系統程式異動申請單
            // 改寫 Transart\MisService\library\MisService_wfls.asp
            // wfl_MisService_Update3
             * 
             * 
             */

            //宣告變數
            //string md_no = ""; //取得程式異動申請單單號
            string opKind = "";//種類
            string updateStr = "";// SQL update 字串
            int RowsAffected = 0; //SQL 更新筆數

            //1. 確認表單資訊是否可取得資料
            if (post != null)
            {
                //1.1 設定變數
                opKind = post["Kind"];
                if (opKind == null | opKind.Trim() == "")
                {
                    opKind = "1";
                }

                //2. 針對Kind = 2的狀態才可異動資料
                if (opKind == "2")
                {
                    /*
                     * 
                    已Bind MisService 物件, 故不需再設定欄位值
                    ms.MS_SCHDATE ' 排程日期
                    ms.MS_PCDATE '預定完成日期
                    ms.MS_MEMO1 '完工說明
			         */
                    //2.1 設定欄位資訊並存檔
                    updateStr = "Update MisService set ";
                    if (post["item.MS_SCHDATE"] != null & post["item.MS_SCHDATE"].ToString() != "")
                    {
                        updateStr = updateStr + "MS_SCHDATE = '" + DateTime.Parse(post["item.MS_SCHDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "',";
                    }
                    else
                    {
                        updateStr = updateStr + "MS_SCHDATE = '', ";
                    }

                    if (post["item.MS_PCDATE"] != null & post["item.MS_PCDATE"].ToString() != "")
                    {
                        updateStr = updateStr + "MS_PCDATE = '" + DateTime.Parse(post["item.MS_PCDATE"]).ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "',";
                    }
                    else
                    {
                        updateStr = updateStr + "MS_PCDATE = '', ";
                    }

                    updateStr = updateStr + "MS_MEMO1 = '" + post["MS_MEMO1"] + "'" +
                        " where  MS_NO = '" + post["MS_NO"] + "'";
                    try
                    {
                        RowsAffected = db.Database.ExecuteSqlCommand(updateStr);
                        if (RowsAffected > 0)
                        {
                            db.SaveChanges();
                            //3.1 資料更新成功後, 發出處理進度通知信   
                            //CALL SendProcessMail(MS_NO , MS_SUBJECT , MS_ACEMNO , MS_ACNAME , MS_EMNO , MS_NAME , 2 , MS_MEMO1 , MS_PCDATE ,MS_CONTENT1 ,MS_CONTENT2)
                            mail23.SendProcessMail(post["MS_NO"], post["MS_SUBJECT"], post["MS_ACEMNO"], post["MS_ACNAME"], post["MS_EMNO"], post["MS_NAME"], "2", post["MS_MEMO1"] , post["item.MS_PCDATE"], post["MS_CONTENT1"], post["MS_CONTENT2"]);
                            
                            TempData["message"] = "延誤通知發出成功！";
                            //此Action 不含對應View, 以下轉導不會帶出原更改資料
                            //Response.Redirect("../mser02_e.asp?KIND=" & KIND & "&MS_NO=" & MS_NO & "&message=" & message)
                            return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                        }
                        else
                        {
                            TempData["message"] = "更新資料庫筆數0筆, 儲存失敗";
                            return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });

                        }// end of RowsAffected > 0

                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "轉件失敗- 新增程式異動單失敗:\n" + ex;
                        //此Action 不含對應View, 以下轉導不會帶出原更改資料
                        return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post["MS_NO"].Trim() });
                    }
                }
                else
                {
                    return RedirectToAction("P_23N02_e", "F_23N02", new { Kind = opKind, MS_NO = post[".MS_NO"].Trim() });
                }
            }
            else
            {
                TempData["message"] = "取得表單資訊失敗, 更新失敗!";
                return RedirectToAction("P_23N02_1", "F_23N02", new { Kind = opKind });
            }
        }
        public string checkData(string Kind, string CloseYN, string MS_ACEMNO, int? MS_PCHOUR, DateTime? MS_PCDATE, int? MS_RCHOUR)
        {
            //當編輯儲存時, 檢查欄位填值是否合適
            string checkresult = ""; //檢查到的錯誤字串
            bool IsNumber = false;//數字檢查
            string opKind = ""; //接收傳入變數

            if (opKind == "1")
            {
                //接單者欄位檢查
                if (MS_ACEMNO == null | MS_ACEMNO.Trim() == "")
                {
                    checkresult = "請輸入接單確認者！" + "\r\n";
                }

                //預計處理工時檢查
                if (MS_PCHOUR == null | MS_PCHOUR.ToString().Trim() == "")
                {
                    checkresult = checkresult + "請輸入預計處理工時！" + "\r\n";
                }
                else
                {
                    IsNumber = int.TryParse(MS_PCHOUR.ToString(), out int a);
                    if (!IsNumber)
                    {
                        checkresult = checkresult + "預計處理工時請輸入整數數字！" + "\r\n";
                    }
                }

                //檢查預計完成日期填值
                if (MS_PCDATE == null | MS_PCDATE.ToString() == "")
                {
                    checkresult = checkresult + "請輸入預計完成日期！";
                }
                else if (!DateTime.TryParse(MS_PCDATE.ToString(), out DateTime dtDate))
                {
                    checkresult = checkresult + "預計完成日期格式錯誤, 請填寫YYYY/MM/DD格式！" + "\r\n";
                }
                else if (MS_PCDATE < DateTime.Today)
                {
                    checkresult = checkresult + "預計完成完成日期不可以選過去日期！" + "\r\n";
                }
                else
                {

                }
            }
            else if (opKind == "2")
            {
                //實際處理工時檢查
                if (MS_RCHOUR == null | MS_RCHOUR.ToString().Trim() == "")
                {
                    checkresult = checkresult + "請輸入實際處理工時！" + "\r\n";
                }
                else
                {
                    IsNumber = int.TryParse(MS_RCHOUR.ToString(), out int a);
                    if (!IsNumber)
                    {
                        checkresult = checkresult + "實際處理工時請輸入數字！" + "\r\n";
                    }
                }
            }
            return checkresult;
        }

    }
}


 
 
 
 