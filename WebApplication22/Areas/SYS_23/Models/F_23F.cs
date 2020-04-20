using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication22.Areas.SYS_23.Models
{
    public class RejectReason
    {
        [Display(Name = "退件日期")]
        public DateTime THE_DATE { get; set; }
        [Display(Name = "單號")]
        public string THE_NO { get; set; }
        [Display(Name = "主題")]
        public string THE_SUBJECT { get; set; }
        [Display(Name = "退件說明")]
        public string THE_ACBACKMEMO { get; set; }
    }
    public class RejectDetail
    {
        public Data.Models.MisModify m_MisModify { get; set; }
        public List<MisModifyHis> m_MisModifyHis { get; set; }
    }
}