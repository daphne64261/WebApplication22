using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication22.Areas.SYS_01.Models
{
    public class P_01E05_l_REC
    {
        public decimal vm_seqno { get; set; }
        public string vm_carno { get; set; }
        public Nullable<System.DateTime> vm_date { get; set; }
        public Nullable<decimal> vm_item { get; set; }
        public Nullable<decimal> vm_trip2 { get; set; }
    }
}