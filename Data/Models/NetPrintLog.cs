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
    
    public partial class NetPrintLog
    {
        public string ng_netno { get; set; }
        public System.DateTime ng_NewDate { get; set; }
        public Nullable<System.DateTime> ng_BadDate { get; set; }
        public decimal ng_carqty { get; set; }
        public decimal ng_DayCnt { get; set; }
        public decimal ng_mCnt { get; set; }
        public string ng_reason { get; set; }
        public string ng_brand { get; set; }
    }
}
