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
    
    public partial class fapricchg
    {
        public string fc_mtno { get; set; }
        public string fc_suno { get; set; }
        public decimal fc_price1 { get; set; }
        public string fc_memo1 { get; set; }
        public decimal fc_price2 { get; set; }
        public string fc_memo2 { get; set; }
        public string fc_reason { get; set; }
        public string fc_emno { get; set; }
        public Nullable<System.DateTime> fc_update { get; set; }
        public string fc_curr1 { get; set; }
        public string fc_curr2 { get; set; }
        public Nullable<int> fc_verify { get; set; }
        public string fc_vemno { get; set; }
        public Nullable<System.DateTime> fc_vdate { get; set; }
        public string fc_vname { get; set; }
    }
}
