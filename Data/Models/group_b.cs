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
    
    public partial class group_b
    {
        public string gh_id { get; set; }
        public string gkey { get; set; }
        public string seqno { get; set; }
        public Nullable<decimal> region { get; set; }
        public bool menu { get; set; }
        public string menudes { get; set; }
        public string menukey { get; set; }
        public string prog_id { get; set; }
        public Nullable<decimal> add_priv { get; set; }
        public Nullable<decimal> upt_priv { get; set; }
        public Nullable<decimal> del_priv { get; set; }
        public Nullable<decimal> rpt_priv { get; set; }
        public string parentkey { get; set; }
        public Nullable<decimal> gb_priv { get; set; }
        public string bmppath { get; set; }
        public Nullable<decimal> gb_left { get; set; }
        public Nullable<decimal> gb_top { get; set; }
        public Nullable<decimal> gb_width { get; set; }
        public Nullable<decimal> gb_height { get; set; }
        public byte[] timestamp_column { get; set; }
    }
}
