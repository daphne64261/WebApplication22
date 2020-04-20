using Common.BasicLibrary;
using Common.Menu;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication22.Areas.SYS_23.Models;

namespace WebApplication22.Areas.SYS_23.Controllers
{
    public class F_23N01Controller : Controller
    {
        //23. 系統權限管理 > N.資訊服務需求單 > 01. 資訊服務需求單建立
     
        /*
        MS_POST 狀態註記:
        0: 新資料
        1: 轉審核
        3: 資訊服務需求單審核--部門主管核准
        4: 資訊服務需求單審核--部門主管不核准
        5: 資訊服務需求單審核--資訊主管核准
        6: 資訊服務需求單審核--資訊主管不核准
        7: 退件
        8: 轉件從MisServcice > MisModify        
        9: 刪除
         */

        //資料庫物件
        TransartEntities db = new TransartEntities();
        GetList getList = new GetList();
        List<SelectListItem> dp_list;//設定部門選單 (為避免更新失敗重返網頁下拉選單值空掉)

        // GET: SYS_23/F_23N01
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult P_23N01_l(int? PageNumber, int? PageSize)
        {
            //改寫程式碼如下
            /*
             * 
             Transart\MisService\library\MisService_wfls.asp 
             GetMisService '=	Description: 資訊服務需求單--清單
             原查詢條件
             lsFieldList = " * "
	         lsTableList = " MisService "
	         lsWhereList = " MS_POST = 0 ORDER BY MS_no "
            */

            //1. 依登入者指派可用系統權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 設定查詢頁面分頁筆數
            int pageSize = PageSize ?? 10;//每頁筆數
            int pageNumber = PageNumber ?? 1;
            

            //3. 指定初始查詢網頁資料集合
            IOrderedQueryable<MisService> mis = db.MisService.Where(s => s.MS_POST == 0).OrderBy(s => s.MS_NO);

            //4. 將資料進行分頁, 並指定傳給前端的變數已設定分頁顯示
            //IPagedList<MisService> misPage = mis.ToPagedList(pageNumber, pageSize);
             IPagedList misPage = mis.ToPagedList(pageNumber, pageSize);

            //5. 傳遞變數給前端
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;


            return View(misPage);
        }

        public ActionResult P_23N01_a()
        {
            //改寫Transart\MisService\mser01_a.asp
            //Transart\MisService\library\MisService_wfls.asp 

            //宣告變數
            string uid="";//登入者資訊
            string deptInfoStr = "";//取得報修者隸屬部門字串資訊
            string[] deptInfo = new string[3];//切割部門字串資訊

            //1. 設定按鈕權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 帶入登入者資訊做預設申請人           
            uid = (string)Session[TARGET.user_id];
            if (uid != null)
            {
                //傳入登入者資訊到前端網頁

                deptInfoStr = updateDept(uid);
                deptInfo = deptInfoStr.Split('!');
                if (deptInfo != null)
                {
                    ViewBag.emno = uid;
                    ViewBag.ename = deptInfo[0];
                    ViewBag.dpno = deptInfo[1];
                    ViewBag.dpname = deptInfo[2];

                    //設定部門選單(呼叫F23_N Model下類別取得清單)
                    ViewBag.Select_Dept = new DEPT_LIST(deptInfo[1]).dept_list;//錯誤new用法? 結果null

                    //部門清單檢查
                    if (ViewBag.Select_Dept == null)
                    {
                        dp_list = getList.deptList(deptInfo[1]);
                        if (dp_list == null | dp_list.Count == 0)
                        {
                            TempData["message"] = "部門清單匯入網頁異常, 請洽資訊!";
                        }
                        else
                        {
                            ViewBag.Select_Dept = dp_list;
                        }
                    }
                }//end of if (deptInfo != null)
            }//end of if (deptInfo != null)            
            return View();
        }

        public ActionResult P_23N01_e(string MS_NO)
        {
            /*
            * 
            Transart\MisService\library\MisService_wfls 
            MisServiceEdit'=	Description: 資訊服務需求單--編輯
            原查詢條件
            lsFieldList = " * "
            lsTableList = " MisService "
            lsWhereList = " MS_POST = 0 AND MS_NO = '" & MS_NO & "' ORDER BY MS_no "

            */

            //宣告變數
            string ms_no = "";
            string dp_no = ""; //用於查找所屬部門
            MisService MisService;

            //1. 依登入者身分指定權限
            Sysfunclist_Info.Get_sfl_id(this);

            //2. 依單號回傳需求單資訊
            ms_no = MS_NO;
            if (ms_no == null | ms_no.Trim() == "")
            {                
                //維持導致前頁, 由前頁判斷是否有資料!
                ViewBag.ms_noSeach = ms_no;
                return View();
            }
            else
            {
                MisService = db.MisService.Find(ms_no.Trim());
                if (MisService == null)
                {
                    //return HttpNotFound();
                    //維持導致前頁, 由前頁判斷是否有資料!
                    ViewBag.ms_noSeach = ms_no;
                    return View();
                }
                else
                {
                    //2.1 依需求單需求者資訊比對隸屬部門
                    dp_no = MisService.MS_DPNO;
                    ViewBag.Select_Dept = new DEPT_LIST(dp_no).dept_list;

                    //部門清單檢查
                    if (ViewBag.Select_Dept == null)
                    {
                        dp_list = getList.deptList(dp_no);
                        if (dp_list == null | dp_list.Count == 0)
                        {
                            TempData["message"] = "部門清單匯入網頁異常, 請洽資訊!";
                        }
                        else
                        {
                            ViewBag.Select_Dept = dp_list;
                        }
                    }

                    return View(MisService);
                } //end of MisService == null)
            }//end of ms_no == null)  
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //View 未包含@Html.AntiForgeryToken(), 故出錯
        //public ActionResult wfl_MisService_insert([Bind(Include = "MS_DPNO, MS_DPNAME, MS_EMNO, MS_NAME, MS_ATTACH,  MS_DATE1, MS_DATE2, MS_SUBJECT, MS_CONTENT2, MS_CONTENT1")] MisService MisService)
        public ActionResult P_23N01_a([Bind(Include = "MS_DPNO, MS_DPNAME, MS_EMNO, MS_NAME, MS_ATTACH, MS_DATE1, MS_DATE2, MS_SUBJECT, MS_CONTENT2, MS_CONTENT1")] MisService MisService)
        {
            //改寫Transart\MisService\mser01_a.asp 點選儲存觸發以下程式
            //Transart\MisService\library\MisService_wfls.asp wfl_MisService_insert
            //接續P_23N01_a 儲存後呼叫

            //wfl_MisService_insert, 改用P_23N01_a原因:
            //當檢查有務執行return View(MisService);, 會因找不到View : Areas/SYS_23/Views/F_23N01/wfl_MisService_insert.aspx 跳錯, 
            //且畫面不會顯示錯誤訊息, 故維持使用P_23N01_a

            //1. 宣告變數
            string checkResult = "";
            string mis_no = "";
            string mis_no2 = "";

            //2. 設定部門選單 (為避免更新失敗重返網頁下拉選單值空掉)
            dp_list = new List<SelectListItem>();

            if (dp_list == null | dp_list.Count == 0)
            {
                //以下函式參數為部門代號
                dp_list = getList.deptList(MisService.MS_DPNO);
            }

            //3. 填值檢查
            //3.1 未宣告model 填值樣式, 故以下永遠valid
            if (ModelState.IsValid) 
            {
                //3.1 針對填值取出檢查是否合理
                checkResult = checkData(MisService.MS_DPNO.Trim(), MisService.MS_EMNO.Trim(), MisService.MS_ATTACH, MisService.MS_SUBJECT, MisService.MS_CONTENT1, MisService.MS_CONTENT2, MisService.MS_DATE1, MisService.MS_DATE2);
                if (checkResult != "")
                {
                    TempData["message"] = checkResult;
                    ViewBag.Select_Dept = dp_list;
                    return View(MisService);//傳入原編輯物件
                }
                else
                {
                    //4. 填值檢查通過後, 取得單號
                    // 最後參數用於檢查是否第一次取值, 如1表示取值失敗會在嘗試做一次, 否則直接回傳fail
                    mis_no = getNumber.getAutoNumber("IS", 0, true, 1);
                    if (mis_no == "fail")
                    {
                        TempData["message"] = "單號取值失敗, 請洽資訊!";
                        ViewBag.Select_Dept = dp_list;
                        return View(MisService);//傳入原編輯物件
                    }
                    else
                    {
                        //4.1 檢查單號是否已存在, 避免key 重複
                        MisService ms = db.MisService.Find(mis_no);
                        if (ms != null)
                        {
                            //再次嘗試取得單號
                            mis_no2 = getNumber.getAutoNumber("IS", 0, true, 1);
                            MisService ms2 = db.MisService.Find(mis_no);
                            if (ms2 != null)
                            {
                                TempData["message"] = "單號重複取值, 更新失敗!";
                                ViewBag.Select_Dept = dp_list;
                                return View(MisService);//傳入原編輯物件
                            }
                            else
                            {
                                mis_no = mis_no2;
                                //確認單號未重複, 繼續執行存檔動作
                            }
                        }
                        else{
                            //確認單號未重複, 繼續執行存檔動作
                        } //end of ms != null                       
                    }// end of mis_no == "fail")

                    //5. 設定指定欄位值並做存檔
                    //
                    MisService.MS_NO = mis_no;
                    MisService.MS_UPDATE = DateTime.Now;
                    MisService.MS_POST = 0;
                    MisService.MS_YN = 0;
                    MisService.MS_YN1 = 0;
                    MisService.MS_CLOSE = 0;

                    //比對測試系統與新版開發系統資料差異; 測試系統沒看到寫值, 但新增有資料後有值
                    MisService.MS_VDATE = DateTime.Now;
                    MisService.MS_VDATE1 = DateTime.Now;

                    db.Entry(MisService).State = EntityState.Modified; //需先將值都設好再一起檢查
                    try
                    {
                        //進行存檔並轉導 mser01_e.asp?MS_no=IS033001&message=儲存成功    
                        db.MisService.Add(MisService);
                        db.SaveChanges();
                        TempData["message"] = "儲存成功";
                        return RedirectToAction("P_23N01_e", "F_23N01", new { MS_NO = mis_no });
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "儲存失敗" + ex;
                        ViewBag.Select_Dept = dp_list;
                        return View(MisService);
                    }
                } // end of checkResult != "")
            }
            else
            {
                TempData["message"] = "驗證失敗! (Model Validation fail)";
                ViewBag.SelectList = dp_list;
                return View(MisService);
            }// end of ModelState.IsValid) 
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult wfl_MisService_Update([Bind(Include = "MS_NO, MS_DPNO, MS_DPNAME, MS_EMNO, MS_NAME, MS_ATTACH, MS_DATE1, MS_DATE2, MS_SUBJECT, MS_CONTENT2, MS_CONTENT1")] MisService MisService)
        public ActionResult P_23N01_e([Bind(Include = "MS_NO, MS_DPNO, MS_DPNAME, MS_EMNO, MS_NAME, MS_ATTACH, MS_DATE1, MS_DATE2, MS_SUBJECT, MS_CONTENT2, MS_CONTENT1")] MisService MisService)
        {
            //改寫Transart\MisService\mser01_e.asp 點選儲存觸發以下程式
            //Transart\MisService\library\MisService_wfls.asp 下wfl_MisService_Update
            //接續P_23N01_e 儲存後呼叫
            //不用wfl_MisService_Update, 改用P_23N01_e原因:
            //當檢查有務執行return View(MisService);, 會因找不到Areas/SYS_23/Views/F_23N01/wfl_MisService_Update.aspx 跳錯, 故維持使用P_23N01_e

            //1. 宣告變數
            string checkResult = "";//填值檢查

            //2. 設定部門選單 (為避免更新失敗重返網頁下拉選單值空掉)
            dp_list = new List<SelectListItem>();
            if (dp_list == null | dp_list.Count == 0)
            {
                dp_list = getList.deptList(MisService.MS_DPNO);
            }

            //3. 填值檢查
            //3.1 沒設model 格式檢查故不會特別檢查
            if (ModelState.IsValid)
            {
                //3.2 將填值欄位取出檢查檢查
                checkResult = checkData(MisService.MS_DPNO, MisService.MS_EMNO, MisService.MS_ATTACH, MisService.MS_SUBJECT, MisService.MS_CONTENT1, MisService.MS_CONTENT2, MisService.MS_DATE1, MisService.MS_DATE2);
                if (checkResult != "")
                {
                    //填值格式檢查有誤提供錯誤訊息並返回原畫面
                    TempData["message"] = checkResult;
                    ViewBag.Select_Dept = dp_list;
                    return View(MisService); //執行P_23N01_e.chtml須回傳物件資料
                    //return RedirectToAction("P_23N01_e", "F_23N01", MisService);//測試搭配Action 名稱為wfl_MisService_Update, 畫面回去不會秀錯誤訊息
                    //return RedirectToAction("P_23N01_e", "F_23N01", new {MisService.MS_NO}); //測試搭配Action 名稱為wfl_MisService_Update, 畫面回去不會秀錯誤訊息
                }
                else
                {
                    //4. 存檔
                    db.Entry(MisService).State = EntityState.Modified;
                    try
                    {
                        //進行存檔並轉導 mser01_e.asp?MS_no=IS033001&message=儲存成功
                        MisService.MS_UPDATE = DateTime.Now;
                        db.SaveChanges();
                        TempData["message"] = "儲存成功";
                        return RedirectToAction("P_23N01_e", "F_23N01", new { MS_NO = MisService.MS_NO }); //取key值
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "儲存失敗" + ex;
                        ViewBag.Select_Dept = dp_list;
                        return View(MisService);
                    }
                }//end of checkData
            }
            else
            {
                TempData["message"] = "Model Validation fail";
                ViewBag.Select_Dept = dp_list;
                return View(MisService);
            }//end of ModelState.IsValid)
        }

        public ActionResult DoDel(string ms_no)
        {
            // /改寫Z:\Transart\MisService\library\MisService_wfls.asp 下wfls_Action=wfl_MisService_Delete()
            // 非直接刪除資料, 而是更改MS_POST 註記為9

            //宣告資料表物件變數
            MisService ms;

            //1.確認單號參數無效時轉導回編輯網頁, 不過畫面也會跳找不到單號
            if (ms_no == null | ms_no.Trim() == "")
            {
                TempData["message"] = "無法取得單號資訊, 刪除失敗\n";
                return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
            }
            else
            {
                //2. 查找需求單資料表物件
                ms = db.MisService.Find(ms_no);
                if (ms != null)
                {
                    //3. 變更單據狀態並存檔
                    ms.MS_POST = 9;
                    ms.MS_UPDATE = DateTime.Now;//原程式沒寫, 考慮後資料有異動應同時改值
                    try
                    {
                        db.SaveChanges();
                        TempData["message"] = "刪除成功";
                        return RedirectToAction("P_23N01_l");
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = ms_no + "單據刪除失敗\n" + ex;
                        return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
                    }
                }
                else
                {
                    TempData["message"] = "刪除失敗:" + ms_no + "資料不存在刪除失敗\n";
                    return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
                }// end of ms != null)
            }// end of ms_no == "")
        }

        public ActionResult wfl_MisService_Post(string ms_no)
        {
            // /改寫Z:\Transart\MisService\library\MisService_wfls.asp 下wfls_Action=wfl_MisService_Post()
            //點選"轉審核"觸發

            //1. 宣告變數
            MisService ms;

            //2. 檢查單號參數是否有值
            if (ms_no.Trim() == null | ms_no.Trim() == "")
            {
                TempData["message"] = "無法取得單號資訊, 刪除失敗\n";
                return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
            }
            else
            {
                //3. 查找單號對應資料是否存在
                ms = db.MisService.Find(ms_no);
                if (ms != null)
                {
                    ms.MS_POST = 1;
                    ms.MS_UPDATE = DateTime.Now;//原程式沒寫, 考慮後資料有異動應同時改值
                    try
                    {
                        db.SaveChanges();
                        TempData["message"] = "轉審核成功";
                        return RedirectToAction("P_23N01_l");
                    }
                    catch (System.Exception ex)
                    {
                        TempData["message"] = "轉審核失敗\n" + ex;
                        return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
                    }
                }
                else
                {
                    TempData["message"] = "轉審核失敗\n" + "查無此單號資訊:" + ms_no;
                    return RedirectToAction("P_23N01_e", "F_23N01", new { ms_no });
                } // end of ms != null
            }//end of ms_no == ""
        }

        public string checkData(string ms_dpno, string ms_emno, int? ms_attach, string ms_subject, string ms_content1, string ms_content2, DateTime? ms_date1, DateTime? ms_date2)
        {
            //當新增或編輯儲存時, 檢查欄位填值是否合適

            string checkresult = "";
            bool IsNumber = false;

            if (ms_dpno == null | ms_dpno.Trim() == "")
            {
                checkresult = "申請部門不得為空" + "\r\n";
            }

            if (ms_emno == null | ms_emno.Trim() == "")
            {
                checkresult = checkresult + "申請者不得為空" + "\r\n";
            }
            else
            {
                employee emp = db.employee.Find(ms_emno.Trim());
                if (emp == null)
                {
                    checkresult = checkresult + "您輸入的員工編號" + ms_emno.Trim() +" 不存在!!" + "\r\n";
                }
                else
                {
                    if (emp.em_dpno.Trim() != ms_dpno.Trim())
                    {
                        checkresult = checkresult + "您選取的申請部門 " + ms_dpno.Trim() + " 與申請者所屬部門 " + emp.em_dpno.Trim() +" 不一致, 請重新選取申請者修正資料!!" + "\r\n";
                    }
                }
            }

            if (ms_attach == null | ms_attach.ToString().Trim() == "")
            {
                checkresult = checkresult + "附件欄位不得為空" + "\r\n";
            }
            else
            {
                IsNumber = int.TryParse(ms_attach.ToString(), out int a);
                if (!IsNumber)
                {
                    checkresult = checkresult + "張數請輸入整數數字！" + "\r\n";
                }

            }

            if (ms_subject == null | ms_subject.Trim() == "")
            {
                checkresult = checkresult + "請輸入資訊服務主題項目！" + "\r\n";
            }

            if (ms_content1 == null | ms_content1.Trim() == "")
            {
                checkresult = checkresult + "請輸入資訊服務內容！" + "\r\n";
            }

            if (ms_content2 == null | ms_content2.Trim() == "")
            {
                checkresult = checkresult + "請輸入修改依據說明！" + "\r\n";
            }

            //檢查申請日期填值
            if (ms_date1 == null | ms_date1.ToString() == "")
            {
                checkresult = checkresult + "請輸入申請日期！";
            }
            else if (!DateTime.TryParse(ms_date1.ToString(), out DateTime dtDate))
            {
                checkresult = checkresult + "申請日期日期格式錯誤, 請填寫YYYY/MM/DD格式！" + "\r\n";
            }
            else if (ms_date1 < DateTime.Today)
            {
                checkresult = checkresult + "申請日期不可以選過去日期！" + "\r\n";
            }

            //檢查希望日期填值
            if (ms_date2 == null | ms_date2.ToString() == "")
            {
                checkresult = checkresult + "請輸入希望完成日期！";
            }
            else if (!DateTime.TryParse(ms_date2.ToString(), out DateTime dtDate2))
            {
                checkresult = checkresult + "希望完成日期格式錯誤, 請填寫YYYY/MM/DD格式！" + "\r\n";
            }
            else if (ms_date2 < DateTime.Today)
            {
                checkresult = checkresult + "希望完成日期不可以選過去日期！" + "\r\n";
            }

            if (ms_date1 > ms_date2)
            {
                checkresult = checkresult + "希望完成日期不可小於申請日期！" + "\r\n";
            }

            return checkresult;
        }

        //傳入員工工號以查詢姓名所屬部門代號與部門名稱
        public string updateDept(string em_no)
        {
            //宣告變數
            string resultReturn = "";
            employee emp;

            //判斷傳入參數不為空才做查詢部門
            if (em_no != null & em_no.Trim() != "")
            {
                emp = db.employee.Find(em_no.Trim());
                if (emp != null)
                {
                    resultReturn = emp.em_cname.Trim();
                    dept dept = db.dept.Find(emp.em_dpno.Trim());
                    if (dept != null)
                    {
                        resultReturn = resultReturn + "!" + dept.dp_no.Trim() + "!" + dept.dp_name.Trim();
                    }
                }
                else
                {
                    //員工資料不存在留白
                }
            }
            else
            {
                //resultReturn = "";
            }
            return resultReturn;
        }

        /* 轉呼叫CommonCheckController
        public string checkEmp(string emno)
        {
            string resultReturn = "";
            employee emp;
            if (emno != null & emno.Trim() != "")
            {
                emp = db.employee.Find(emno.Trim());
                if (emp != null)
                {
                    resultReturn = emp.em_cname.Trim();
                    dept dept = db.dept.Find(emp.em_dpno.Trim());
                    if (dept != null)
                    {
                        resultReturn = resultReturn + "!" + dept.dp_no.Trim() + "!" + dept.dp_name.Trim();
                    }
                }
            }

            if (resultReturn == "")
            {
                resultReturn = "fail";
            }
            return resultReturn;
        }
        */
    }
}
