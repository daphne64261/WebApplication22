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
    
    public partial class OutDeliveb
    {
        public string odb_no { get; set; }
        public Nullable<System.DateTime> odb_date { get; set; }
        public string odb_cuno { get; set; }
        public string odb_emno { get; set; }
        public string odb_pdno { get; set; }
        public string odb_spec { get; set; }
        public Nullable<decimal> odb_cardamt { get; set; }
        public Nullable<decimal> odb_amount { get; set; }
        public string odb_curr { get; set; }
        public Nullable<decimal> odb_uprice { get; set; }
        public Nullable<decimal> odb_total { get; set; }
        public string odb_item { get; set; }
        public Nullable<decimal> odb_post { get; set; }
    }
}