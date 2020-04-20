using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication22.Areas.SYS_01.Models
{
    public class dept_REC
    {
        public string dp_no { get; set; }
        public string dp_name { get; set; }
    }
    public class CarUse_REC
    {
        public string tt_carno { get; set; }
        public string ty_no { get; set; }
        public string ty_place { get; set; }
        public string ty_emno { get; set; }
        public string ty_name { get; set; }
        public string ty_dpno { get; set; }
        public string ty_dpname { get; set; }
        public string ty_subject { get; set; }
        public DateTime tt_date1 { get; set; }
        public DateTime tt_date2 { get; set; }
        public Nullable<decimal> tt_trip1 { get; set; }
        public Nullable<decimal> tt_trip2 { get; set; }
        public Nullable<decimal> tt_trip { get; set; }
    }

}