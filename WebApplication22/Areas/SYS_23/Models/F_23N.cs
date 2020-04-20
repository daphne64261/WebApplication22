using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication22.Areas.SYS_23.Models
{
    public class DEPT_LIST
    {
        //宣告資料庫物件
        TransartEntities Db = new TransartEntities();

        public SelectList dept_list { get; set; }
        public DEPT_LIST(string dp_no)
        {
            List<SelectListItem> dp_list = new List<SelectListItem>();
            //設定部門選單            
            dp_list.Add(new SelectListItem() { Text = "- - -", Value = "", Selected = false });
            foreach (var Dept_item in Db.dept.Where(s => s.dp_no != "000").OrderBy(s => s.dp_no).ToList())
            {
                dp_list.Add(new SelectListItem()
                {
                    Text = Dept_item.dp_name.Trim(),
                    Value = Dept_item.dp_no.Trim(),
                    Selected = Dept_item.dp_no.Trim().Equals(dp_no)
                });
            }

            //dept_list = new SelectList(dp_list, "Value", "Text"); //即使上述宣告selected, 但dropdown list仍不會帶出預設值
            dept_list = new SelectList(dp_list, "Value", "Text", dp_no);
        }

        public string updateDept(string em_no)
        {
            //傳入申請者工號以查詢對應部門資訊

            //宣告變數
            string resultReturn = "";
            employee emp;

            if (em_no != null & em_no.Trim() != "")
            {
                emp = Db.employee.Find(em_no.Trim());
                if (emp != null)
                {
                    resultReturn = emp.em_cname.Trim();
                    dept dept = Db.dept.Find(emp.em_dpno.Trim());
                    if (dept != null)
                    {
                        resultReturn = resultReturn + "!" + dept.dp_no.Trim() + "!" + dept.dp_name.Trim();
                    }
                }
            }
            else
            {
                //工號為空則不處理
            }
            return resultReturn;
        }
    }

    public class MisServiceReject
    {
        //整併MisService與MisReject 所有欄位
        public string MST_MSNO { get; set; }
        public string MST_SUBJECT { get; set; }
        public string MST_CONTENT1 { get; set; }
        public string MST_ACBACKMEMO { get; set; }
        public Nullable<System.DateTime> MST_DATE { get; set; }
        public string MST_ACEMNO { get; set; }
        public string MST_ACNAME { get; set; }

        public string MS_NO { get; set; }
        public string MS_EMNO { get; set; }
        public string MS_NAME { get; set; }
        public string MS_DPNO { get; set; }
        public string MS_DPNAME { get; set; }
        public Nullable<short> MS_ATTACH { get; set; }
        public Nullable<System.DateTime> MS_DATE1 { get; set; }
        public Nullable<System.DateTime> MS_DATE2 { get; set; }
        public string MS_SUBJECT { get; set; }
        public string MS_CONTENT1 { get; set; }
        public Nullable<System.DateTime> MS_UPDATE { get; set; }
        public Nullable<System.DateTime> MS_ACDATE { get; set; }
        public string MS_ACEMNO { get; set; }
        public string MS_ACNAME { get; set; }
        public Nullable<System.DateTime> MS_PCDATE { get; set; }
        public Nullable<decimal> MS_PCHOUR { get; set; }
        public Nullable<System.DateTime> MS_RCDATE { get; set; }
        public Nullable<decimal> MS_RCHOUR { get; set; }
        public Nullable<decimal> MS_POST { get; set; }
        public Nullable<decimal> MS_YN { get; set; }
        public Nullable<decimal> MS_YN1 { get; set; }
        public Nullable<decimal> MS_CLOSE { get; set; }
        public string MS_ACBACKMEMO { get; set; }
        public string MS_MEMO1 { get; set; }
        public string MS_MEMO2 { get; set; }
        public Nullable<decimal> MS_FANO { get; set; }
        public Nullable<System.DateTime> MS_SCHDATE { get; set; }
        public string MS_CONTENT2 { get; set; }
        public Nullable<decimal> MS_U1 { get; set; }
        public Nullable<decimal> MS_U2 { get; set; }
        public Nullable<decimal> MS_U3 { get; set; }
        public Nullable<decimal> MS_U4 { get; set; }
        public string MS_REASON { get; set; }
        public string MS_DPNOM1 { get; set; }
        public string MS_DPNOM2 { get; set; }
        public Nullable<System.DateTime> MS_VDATE { get; set; }
        public string MS_DPNOM3 { get; set; }
        public string MS_DPNOM4 { get; set; }
        public Nullable<System.DateTime> MS_VDATE1 { get; set; }
    }

    public class MisServiceManager
    {
        //整併MisService與主管資訊
        public string MS_NO { get; set; }
        public string MS_EMNO { get; set; }
        public string MS_NAME { get; set; }
        public string MS_DPNO { get; set; }
        public string MS_DPNAME { get; set; }
        public Nullable<short> MS_ATTACH { get; set; }
        public Nullable<System.DateTime> MS_DATE1 { get; set; }
        public Nullable<System.DateTime> MS_DATE2 { get; set; }
        public string MS_SUBJECT { get; set; }
        public string MS_CONTENT1 { get; set; }
        public Nullable<System.DateTime> MS_UPDATE { get; set; }
        public Nullable<System.DateTime> MS_ACDATE { get; set; }
        public string MS_ACEMNO { get; set; }
        public string MS_ACNAME { get; set; }
        public Nullable<System.DateTime> MS_PCDATE { get; set; }
        public Nullable<decimal> MS_PCHOUR { get; set; }
        public Nullable<System.DateTime> MS_RCDATE { get; set; }
        public Nullable<decimal> MS_RCHOUR { get; set; }
        public Nullable<decimal> MS_POST { get; set; }
        public Nullable<decimal> MS_YN { get; set; }
        public Nullable<decimal> MS_YN1 { get; set; }
        public Nullable<decimal> MS_CLOSE { get; set; }
        public string MS_ACBACKMEMO { get; set; }
        public string MS_MEMO1 { get; set; }
        public string MS_MEMO2 { get; set; }
        public Nullable<decimal> MS_FANO { get; set; }
        public Nullable<System.DateTime> MS_SCHDATE { get; set; }
        public string MS_CONTENT2 { get; set; }
        public Nullable<decimal> MS_U1 { get; set; }
        public Nullable<decimal> MS_U2 { get; set; }
        public Nullable<decimal> MS_U3 { get; set; }
        public Nullable<decimal> MS_U4 { get; set; }
        public string MS_REASON { get; set; }
        public string MS_DPNOM1 { get; set; }
        public string MS_DPNOM2 { get; set; }
        public Nullable<System.DateTime> MS_VDATE { get; set; }
        public string MS_DPNOM3 { get; set; }
        public string MS_DPNOM4 { get; set; }
        public Nullable<System.DateTime> MS_VDATE1 { get; set; }

        //employee
        public string em_cname { get; set; }

        //dept        
        public string dp_name { get; set; }
    }

    public class MisServiceStatistics
    {
        public string ym { get; set; }
        public int MisSer_Count { get; set; }
    }

    public class MisServiceExport
    {
        public string YM { get; set; }
        public string Factory { get; set; }
        public string ms_dpno { get; set; }
        public string ms_dpname { get; set; }
        public int Cnt { get; set; }
        public decimal DoHour { get; set; }
    }
}