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
    
    public partial class basset_chg
    {
        public decimal bg_seqno { get; set; }
        public string bg_setno { get; set; }
        public Nullable<decimal> bg_difficulty_old { get; set; }
        public Nullable<decimal> bg_difficulty_new { get; set; }
        public string bg_emno { get; set; }
        public Nullable<System.DateTime> bg_cdate { get; set; }
        public Nullable<decimal> bg_verify { get; set; }
        public string bg_vemno { get; set; }
        public string bg_vname { get; set; }
        public Nullable<System.DateTime> bg_vdate { get; set; }
        public decimal bg_PrtPlus_old { get; set; }
        public decimal bg_PrtPlus_new { get; set; }
    }
}
