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
    
    public partial class LCM
    {
        public string The_YM { get; set; }
        public string db_no { get; set; }
        public System.DateTime db_date { get; set; }
        public string db_pdno { get; set; }
        public string db_cupdno { get; set; }
        public string db_orno { get; set; }
        public Nullable<decimal> db_amount { get; set; }
        public Nullable<decimal> db_free { get; set; }
        public string db_curr { get; set; }
        public Nullable<double> db_uprice { get; set; }
        public string up_no { get; set; }
        public Nullable<int> up_qty { get; set; }
        public string up_curr { get; set; }
        public Nullable<double> up_price { get; set; }
        public Nullable<double> The_amt { get; set; }
        public Nullable<double> The_uprice1 { get; set; }
        public Nullable<double> The_uprice2 { get; set; }
        public double Bom_Price { get; set; }
        public decimal The_Flag { get; set; }
    }
}
