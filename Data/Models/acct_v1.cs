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
    
    public partial class acct_v1
    {
        public string act_no { get; set; }
        public string act_name { get; set; }
        public string act_bank { get; set; }
        public string act_sbank { get; set; }
        public string act_cuno { get; set; }
        public Nullable<decimal> act_amount { get; set; }
        public Nullable<decimal> act_rece { get; set; }
        public Nullable<decimal> act_pay { get; set; }
        public Nullable<decimal> act_rtake { get; set; }
        public Nullable<decimal> act_ptake { get; set; }
        public Nullable<decimal> act_rcash { get; set; }
        public Nullable<decimal> act_pcash { get; set; }
        public Nullable<decimal> act_10 { get; set; }
        public string act_macno { get; set; }
        public Nullable<decimal> act_kind { get; set; }
        public Nullable<decimal> act_type { get; set; }
        public string act_mykno { get; set; }
        public Nullable<decimal> act_11 { get; set; }
        public Nullable<decimal> act_12 { get; set; }
        public string compid { get; set; }
        public string updid { get; set; }
        public Nullable<System.DateTime> upddate { get; set; }
        public byte[] timestamp_column { get; set; }
    }
}
