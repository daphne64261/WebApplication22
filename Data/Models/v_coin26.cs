//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class v_coin26
    {
        public string ar_dpno { get; set; }
        public string dp_name { get; set; }
        public string ar_year { get; set; }
        public string ar_month { get; set; }
        public string ar_cuno { get; set; }
        public string ar_cusname { get; set; }
        public Nullable<decimal> v_sumnotpay { get; set; }
        public decimal v_sumpay { get; set; }
        public Nullable<decimal> v_notpay { get; set; }
        public Nullable<decimal> v_sumnotpay_NT { get; set; }
        public decimal v_sumpay_NT { get; set; }
        public Nullable<decimal> v_notpay_NT { get; set; }
        public string cu_emno { get; set; }
        public string em_cname { get; set; }
        public string ar_mykno { get; set; }
        public string myk_name { get; set; }
        public string cu_pay { get; set; }
        public decimal cu_payday { get; set; }
        public Nullable<decimal> cu_date1 { get; set; }
        public decimal cu_tradelimit { get; set; }
        public Nullable<decimal> NxtAmt { get; set; }
        public Nullable<decimal> NxtAmt_NT { get; set; }
        public Nullable<decimal> NxtFees { get; set; }
        public Nullable<decimal> NxtFees_NT { get; set; }
        public Nullable<int> OverDay { get; set; }
        public Nullable<System.DateTime> GetCheckDate { get; set; }
    }
}