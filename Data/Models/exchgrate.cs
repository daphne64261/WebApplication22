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
    
    public partial class exchgrate
    {
        public string er_yymm { get; set; }
        public string er_ocurr { get; set; }
        public string er_ccurr { get; set; }
        public Nullable<decimal> er_excrate { get; set; }
        public Nullable<System.DateTime> er_cdate { get; set; }
        public string er_nocurr { get; set; }
        public string er_nccurr { get; set; }
        public decimal er_excrate1 { get; set; }
        public decimal er_excrate2 { get; set; }
    }
}
