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
    
    public partial class Poster
    {
        public long pr_id { get; set; }
        public string pr_subject { get; set; }
        public string pr_content { get; set; }
        public Nullable<System.DateTime> pr_Sdt { get; set; }
        public Nullable<System.DateTime> pr_Edt { get; set; }
        public string pr_Contact { get; set; }
        public string pr_emno { get; set; }
        public string pr_name { get; set; }
        public Nullable<int> pr_count { get; set; }
        public System.DateTime pr_update { get; set; }
    }
}