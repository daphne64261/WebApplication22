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
    
    public partial class SchPrepareRec
    {
        public decimal The_seq { get; set; }
        public Nullable<System.DateTime> HandleTime { get; set; }
        public string brg_emno { get; set; }
        public Nullable<System.DateTime> brg_date { get; set; }
        public string brg_mkno { get; set; }
        public string brg_setno { get; set; }
        public string first_mkno { get; set; }
        public string first_setno { get; set; }
        public Nullable<System.DateTime> first_date { get; set; }
        public string first_bompre { get; set; }
        public string first_paper { get; set; }
        public string first_reason { get; set; }
        public string NotBySchReason { get; set; }
    }
}