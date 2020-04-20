using System.ComponentModel.DataAnnotations;

namespace WebApplication22.Areas.SYS_23.Models
{
    public class MonthlyClose
    {
        [Display(Name = "統計年")]
        public int Y { get; set; }
        [Display(Name = "月")]
        public int M { get; set; }
        [Display(Name = "系統程式異動申請單結單數")]
        public int Count1 { get; set; }
    }
    public class MisModify_Report
    {
        [Display(Name = "統計年")]
        public int Y { get; set; }
        [Display(Name = "月")]
        public int M { get; set; }
        [Display(Name = "廠別")]
        public decimal? mm_fano { get; set; }
        [Display(Name = "廠別名稱")]
        public string mm_Factory { get; set; }
        [Display(Name = "申請部門代號")]
        public string mm_dpno { get; set; }
        [Display(Name = "申請部門名稱")]
        public string mm_dpname { get; set; }
        [Display(Name = "件數")]
        public int Cnt { get; set; }
        [Display(Name = "處理工時")]
        public decimal? DoHour { get; set; }
    }
}