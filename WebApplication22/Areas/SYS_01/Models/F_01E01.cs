using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication22.Areas.SYS_01.Models
{
    public class P_01E03_q_Model
    {
        public P_01E03_q_REC Rec { get; set; }
        public bool ChkYN
        {
            get
            {
                return (Rec.tt_carverify >= 1) ? true : false;
            }
            set
            {
                if (value == true)
                    Rec.tt_carverify = 1;
                else
                    Rec.tt_carverify = 0;
            }
        }
    }
    public class P_01E03_q_REC
    {
        public string tt_carno { get; set; }
        public string ty_no { get; set; }
        public string ty_place { get; set; }
        public string ty_emno { get; set; }
        public string ty_name { get; set; }
        public string ty_dpno { get; set; }
        public string ty_dpname { get; set; }
        public DateTime ty_date1 { get; set; }
        public DateTime ty_date2 { get; set; }
        public string ty_subject { get; set; }
        public string ty_content { get; set; }
        public string ty_cuno { get; set; }
        public string ty_cuname { get; set; }
        public decimal? tt_trip { get; set; }
        public decimal? tt_totalsub { get; set; }
        public decimal tt_carverify { get; set; }
    }
}