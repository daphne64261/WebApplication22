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
    
    public partial class TripReport_H
    {
        public int trh_dsn { get; set; }
        public string trh_ttno { get; set; }
        public string trh_dpno { get; set; }
        public string trh_dpname { get; set; }
        public string trh_emno { get; set; }
        public string trh_cname { get; set; }
        public Nullable<System.DateTime> trh_date1 { get; set; }
        public Nullable<System.DateTime> trh_date2 { get; set; }
        public string trh_kind { get; set; }
        public Nullable<decimal> trh_money1 { get; set; }
        public Nullable<decimal> trh_money2 { get; set; }
        public string trh_femno { get; set; }
        public string trh_fcname { get; set; }
        public Nullable<System.DateTime> trh_fdate { get; set; }
    }
}
