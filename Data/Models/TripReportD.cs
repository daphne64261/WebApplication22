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
    
    public partial class TripReportD
    {
        public string td_no { get; set; }
        public System.DateTime td_date { get; set; }
        public string td_memo { get; set; }
        public decimal td_taxisub1 { get; set; }
        public decimal td_taxisub2 { get; set; }
        public decimal td_taxisub3 { get; set; }
        public decimal td_livesub { get; set; }
        public decimal td_meal1 { get; set; }
        public decimal td_meal2 { get; set; }
        public decimal td_meal3 { get; set; }
        public string td_other { get; set; }
        public decimal td_othersub { get; set; }
        public decimal td_socialsub { get; set; }
        public decimal td_type { get; set; }
        public Nullable<decimal> td_hour { get; set; }
        public string td_social { get; set; }
    }
}
