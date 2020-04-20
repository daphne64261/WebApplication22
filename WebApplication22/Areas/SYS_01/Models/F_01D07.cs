using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication22.Areas.SYS_01.Models
{
    public class TripApplyShow_Model
    {
        public string ty_no { get; set; }       //序號
        public TripApplyShow_REC Rec { get; set; }
        public string ShowYN { get; set; }
        public string GetAllName { get; set; }
        public string SelCarYN
        {
            get
            {
                return Rec.tt_caryn == 0 ? "無" :
                       Rec.tt_caryn == 1 ? "汽車(自備)" :
                       Rec.tt_caryn == 2 ? "機車(自備)" :
                       Rec.tt_caryn == 3 ? "公務車使用" :
                       Rec.tt_caryn == 4 ? "公務車借用" : "";
            }
        }
        public List<TripReportDetail> Rec1 { get; set; }
    }
    public class TripApplyShow_REC
    {
        public string ty_no { get; set; }       //登錄序號
        public string ty_place { get; set; }    //出差地點
        public string ty_emno { get; set; }     //出差人員卡號
        public string ty_name { get; set; }     //出差人員姓名
        public string ty_agent { get; set; }    //代理人員卡號
        public string em_cname { get; set; }    //代理人員姓名
        public string ty_dpno { get; set; }     //部門編號
        public string ty_dpname { get; set; }   //部門名稱
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime ty_date1 { get; set; }  //出差日期(起)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime ty_date2 { get; set; }  //出差日期(迄)
        public string ty_subject { get; set; }  //出差事由
        public string ty_content { get; set; }  //事項
        public string ty_cuno { get; set; }     //客戶編號
        public string ty_cuname { get; set; }   //客戶名稱
        public string ty_cuno1 { get; set; }    //客戶編號1
        public string ty_cuno2 { get; set; }    //客戶編號2
        public string ty_cuno3 { get; set; }    //客戶編號3
        public string ty_cuno4 { get; set; }    //客戶編號4
        public string ty_cuname1 { get; set; }  //客戶名稱1
        public string ty_cuname2 { get; set; }  //客戶名稱2
        public string ty_cuname3 { get; set; }  //客戶名稱3
        public string ty_cuname4 { get; set; }  //客戶名稱4
        public decimal ty_yn { get; set; }      //國外出差否
        public decimal ty_salYN { get; set; }   //不轉薪資(1:不轉薪資  0:轉薪資)
        //2012/11/10 by souleye 新增修改出差日期
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime ty_date3 { get; set; }  //出差日期(起)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime ty_date4 { get; set; }  //出差日期(迄)

        //2012/11/16 by souleye 增加出差時遇到的會議 
        public Nullable<decimal> ty_meetNo { get; set; }   //略過會議編號
        public string ty_meetName { get; set; }  //略過會議名稱
        public Nullable<decimal> ty_meetNo1 { get; set; }  //略過會議編號(時間修改)
        public string ty_meetName1 { get; set; } //略過會議名稱(時間修改)

        public string ty_agentName { get; set; } //代理人姓名
        public decimal tt_verify { get; set; }   //出差主管核准否
        public decimal tt_verify1 { get; set; }  //出差費用主管核准否

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public Nullable<DateTime> rc_bdate { get; set; }    //略過會議時間(起)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public Nullable<DateTime> rc_edate { get; set; }    //略過會議時間(迄)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public Nullable<DateTime> rc_bdate1 { get; set; }    //略過會議時間(起)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public Nullable<DateTime> rc_edate1 { get; set; }    //略過會議時間(迄)

        public string tt_no { get; set; }        //登錄序號
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime tt_date1 { get; set; }    //出差日期(起)
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt hh:mm}")]
        public DateTime tt_date2 { get; set; }    //出差日期(迄)
        public decimal tt_caryn { get; set; }     //交通工具
        public string tt_carno { get; set; }      //車號
        public Nullable<decimal> tt_trip1 { get; set; }     //里程數(去)
        public Nullable<decimal> tt_trip2 { get; set; }     //里程數(回)
        public Nullable<decimal> tt_trip { get; set; }      //總里程數
        public decimal tt_tripsub { get; set; }   //油錢補助
        public decimal tt_parksub { get; set; }   //停車費補助
        public string tt_taxi { get; set; }       //搭乘交通工具
        public decimal tt_taxisub { get; set; }   //搭乘交通工具補助
        public decimal tt_livesub { get; set; }   //住宿費補助
        public decimal tt_meal1 { get; set; }     //早餐
        public decimal tt_meal2 { get; set; }     //午餐
        public decimal tt_meal3 { get; set; }     //晚餐
        public decimal tt_mealsub { get; set; }   //膳食費補助
        public decimal tt_othersub { get; set; }  //什支費
        public decimal tt_socialsub { get; set; } //交際費
        public decimal tt_outsub { get; set; }    //國外出差補助津貼
        public decimal tt_totalsub { get; set; }  //補助合計
        public string tt_report { get; set; }     //出差報告
        public decimal tt_close { get; set; }     //結算否
        public string tt_curr { get; set; }       //預支金額幣別  預支幣別1
        public Nullable<decimal> tt_prepay { get; set; } //已使用預支金額(原幣) 預支金額1
        public decimal tt_exrate { get; set; }    //預支金額匯率  預支匯率1
        public decimal tt_prepaysub { get; set; } //預支金額(台幣)
        [DisplayFormat(DataFormatString = "{0:yyyyMMdd}")]
        public Nullable<DateTime> tt_OilDate { get; set; }  //加油日期
        public decimal tt_OilTrip { get; set; }   //加油里程
        public decimal tt_OilCnt { get; set; }    //加油公升數
        public decimal tt_OilCharge { get; set; } //加油金額
        public string tt_OilInv { get; set; }     //加油發票號碼

        //2012/11/10 by souleye 加入加油銷售額、營業稅、過路費
        public Nullable<decimal> tt_OilCharge1 { get; set; } //加油銷售額
        public Nullable<decimal> tt_OilCharge2 { get; set; } //加油營業稅
        public Nullable<decimal> tt_park { get; set; }       //停車費
        public Nullable<decimal> tt_carpass { get; set; }    //過路費

        public Nullable<decimal> tt_prepayB { get; set; }    //歸還金額1
        public string tt_curr2 { get; set; }                 //預支幣別2
        public Nullable<decimal> tt_prepay2 { get; set; }    //預支金額2
        public Nullable<decimal> tt_prepayB2 { get; set; }   //歸還金額2
        public Nullable<decimal> tt_exrate2 { get; set; }    //預支匯率2
        public Nullable<decimal> tt_prepaysub2 { get; set; } //
        public string tt_curr3 { get; set; }                 //預支幣別2
        public Nullable<decimal> tt_prepay3 { get; set; }    //預支金額2
        public Nullable<decimal> tt_prepayB3 { get; set; }   //歸還金額2
        public Nullable<decimal> tt_exrate3 { get; set; }    //預支匯率2
        public Nullable<decimal> tt_prepaysub3 { get; set; } //
        public Nullable<int> tt_finanical { get; set; }      //營業稅財務修改
    }
    public class TripReportDetail
    {
        [DisplayFormat(DataFormatString = "{0:yyyyMMdd}")]
        public DateTime td_date { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:yyyyMMdd}")]
        public Nullable<DateTime> ho_date { get; set; }
        public string td_social { get; set; }
    }

}