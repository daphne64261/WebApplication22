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
    
    public partial class chstatist
    {
        public string ct_no { get; set; }
        public System.DateTime ct_date { get; set; }
        public string ct_cuno { get; set; }
        public Nullable<decimal> ct_amt { get; set; }
        public Nullable<decimal> ct_price { get; set; }
        public Nullable<decimal> ct_fee { get; set; }
        public string ct_memo { get; set; }
        public string ct_prono { get; set; }
        public Nullable<decimal> ct_bqty { get; set; }
        public decimal ct_seqno { get; set; }
    }
}