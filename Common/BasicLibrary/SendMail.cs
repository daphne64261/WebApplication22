using Data.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Mail;
using System.Web;

namespace Common.BasicLibrary
{
	public class SendMail
	{

	}

	public class Mail_F23N { 
        //資料庫物件
        //TransartEntities db = new TransartEntities();
               
        public void SendProcessMail(string The_No, string The_Subject, string EMNO1, string EMCNAME1, string EMNO2, string EMCNAME2, string KIND, string The_Note, string The_Date, string The_Content, string The_Content1)
	    {
			//      用於23_N- MisService  相關通知信寄送
			//      '2005/08/06 By Clark 發出處理進度通知信
			//      '2013/01/18 by souleye 重新修改

			//宣告變數
			//MailMessage message = new MailMessage();
			var message = new MimeMessage();
			string Subject = ""; //信件主旨
			string HTMLBody = "";//信件內容
			string Recipient = "";//'收信者E-Mail
            string Sender = "";//寄件人E-Mail
            string RecipientCC = ""; //'副本收信者 = 應通知人員 及 寄件者
            string FANAME = ""; //廠區
			string smtpServer = "";
			string smtpPw = "";

            Recipient = GetSenderEMail(EMNO2); //'收信者E-Mail
            Sender = GetSenderEMail(EMNO1); // '寄件人E-Mail            
            RecipientCC = GetRecipientCCEMail(EMNO2, KIND) + Sender; //  '副本收信者 = 應通知人員 及 寄件者


            //'2013/12/12 by souleye 寄件者改為 Computer@transart.com.tw
            Sender = "Computer@transart.com.tw";

            FANAME = "台灣廠";

            //2015/02/16 by souleye 新增取不到E-mail的防呆
            if (Recipient == "")
            {
                //Recipient = "Computer@transart.com.tw";
                //HTMLBodyStr = "※注意：申請者 " + au_cname + " 並無E-Mail信箱，請以電話通知。";
            }
            else
            {
				//'信件內容
				switch (KIND)
				{
					case "0":
						Subject = "資訊服務需求單--退件通知";         //'信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--退件通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >退件</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >退件日期</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >退件原因</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Note, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" +The_Note.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>"
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽退件人員：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					//'2012/12/11 by souleye 接單、進度延誤通知不需要mail
					case "2":
						Subject = "資訊服務需求單--進度通知（完工）";          // '信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--進度通知（完工）</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >廠別</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + FANAME + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >已結單</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >結單日期</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >完工說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Note, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Note.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽處理人員：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					case "3":
						Subject = "資訊服務需求單--部門主管核准通知";                  // '信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--部門主管核准通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >廠別</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + FANAME + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >已核准</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >核准通知</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽部門主管：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					case "4":
						Subject = "資訊服務需求單--部門主管不核准通知";                  //'信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--不核准通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >不核准</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >不核准通知</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >不核准原因</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Note, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Note.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽部門主管：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					case "5":
						Subject = "資訊服務需求單--資訊主管核准通知";                   //'信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--資訊主管核准通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >廠別</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + FANAME + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >已核准</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >核准通知</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽資訊主管：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;

					case "6":
						Subject = "資訊服務需求單--資訊主管不核准通知";                  //'信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--不核准通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >不核准</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >不核准通知</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >不核准原因</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Note, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" +The_Note.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽資訊主管：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					case "8":
						Subject = "資訊服務需求單--資訊室轉件通知";                  //'信件主題

						HTMLBody = "<HTML><BODY>";
						HTMLBody = HTMLBody + "<TABLE border=1 cellspacing=0 cellpadding=0 style='BORDER-COLLAPSE:collapse;FONT-SIZE:12pt'>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=700  bgcolor=#0000ff colspan=2><FONT face=標楷體 size=5 color=white>資訊服務需求單--資訊室轉件通知</font></TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >單號</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_No + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請項目</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Subject + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >申請者</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + EMCNAME2 + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >處理進度</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >資訊室轉件</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >轉件通知</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Date + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >轉件原因</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >此單已轉為申請系統程式異動申請單，請依系統<br/>程式異動申請單申請流程重新進行審核程序</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >修改依據說明</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content1, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content1.Replace("\r\n", "<br/>") + "</TD>";
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "<TR height=25>";
						HTMLBody = HTMLBody + "<TD align=Center width=100  bgcolor=dodgerblue >資訊服務內容</TD>";
						//HTMLBody = HTMLBody + "<TD align=Left width=600 >" + Replace(The_Content, VBCrlf, "<br>") + "</TD>";
						HTMLBody = HTMLBody + "<TD align=Left width=600 >" + The_Content.Replace("\r\n", "<br/>") + "</TD>"; //au_reason.Replace("\r\n", "<br/>")
						HTMLBody = HTMLBody + "</TR>";
						HTMLBody = HTMLBody + "</TABLE><BR><BR>";
						HTMLBody = HTMLBody + "***本信件為系統自動發出，若有疑問，請洽轉件人員：" + EMCNAME1 + "***<BR>";
						HTMLBody = HTMLBody + "***系統相關問題，請洽資訊室。分機：624、625。***<BR>";
						HTMLBody = HTMLBody + "</BODY></HTML>";
						break;
					default:
						break;
				}
				/*

				'Debug 使用
				'HTMLBody = HTMLBody + RecipientCC
				'Recipient = "souleye@transart.com.tw"
				'RecipientCC = ""

				if KIND = 0 then
					Call Send_JMailHTML(Subject,Recipient,RecipientCC,"",Sender,HTMLBody)
				elseif KIND = 2 then
					Call Send_JMailHTML(Subject,Recipient,RecipientCC,"",Sender,HTMLBody)
				elseif KIND = 3 then
					Call Send_JMailHTML(Subject,Recipient,RecipientCC,"",Sender,HTMLBody)
				elseif KIND = 5 then
					Call Send_JMailHTML(Subject,Recipient,RecipientCC,"",Sender,HTMLBody)
				elseif KIND = 8 then
					Call Send_JMailHTML(Subject,Recipient,RecipientCC,"",Sender,HTMLBody)
				else
					Call Send_JMailHTML(Subject,Recipient,"","",Sender,HTMLBody)
				end if
				*/

				Recipient = "slhungf@transart.com.tw";
				RecipientCC = "slhungf@transart.com.tw;";
				//message.IsBodyHtml = true;			
				//message.From = new MailAddress(Sender);   //寄件人E-Mail > Sender = "Computer@transart.com.tw";
				//message.To.Add(new MailAddress(Recipient));               //收信者E-Mail

				message.From.Add(new MailboxAddress(Sender));
				message.To.Add(new MailboxAddress(Recipient));

				if (KIND == "0" | KIND == "2" | KIND == "3" | KIND == "5" | KIND == "8")
				{
					//Call Send_JMailHTML(Subject, Recipient, RecipientCC, "", Sender, HTMLBody)

					//email 分號處理
					foreach (var m_RecipientCC in RecipientCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
					{
						//message.CC.Add(new MailAddress(m_RecipientCC));                 //副本收信者 = 申請者部門主管
						message.Cc.Add(new MailboxAddress(m_RecipientCC));
					}
				}
				else
				{
					//Call Send_JMailHTML(Subject, Recipient,"","",Sender,HTMLBody)
				}

				message.Subject = Subject;                                    //主旨

				//message.Body = HTMLBody;
				message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = HTMLBody };
				//SmtpClient smtp = new SmtpClient();
				//smtp.Send(message);

				using (var client = new SmtpClient())
				{
					//帶改寫為取用webconfig
					//string smtpServer = "";
					//string smtpPw = "";

					//client.Connect("smtp.transart.com.tw", 25, true);
					client.Connect("smtp.transart.com.tw", 25, MailKit.Security.SecureSocketOptions.None);
					//client.Authenticate("computer", "aa123");//失敗
					client.Send(message);
					client.Disconnect(true);
				}
			}// end of recepient ==""
		}

        public string GetSenderEMail(string ln_emno)
        {
			/*
             * 改寫 Transart\MisService\library\MisService_wfls.asp 
             * 
             * 2005/08/06 By Clark 以卡號取得E-Mail
             * Function GetSenderEMail(ln_emno )
             * strSQL = " SELECT TOP 1 isNull(ln_email , '' ) as ln_email FROM login WHERE ln_emno = '" + ln_emno + "'"
             */

			//此變數放在外層會出錯
			//資料庫物件
			TransartEntities db = new TransartEntities();

			//宣告變數
			string ln_emnoStr = ""; //工號
            string emailStr = ""; //email 資訊
            login logininfo; //login 資料表取得信箱資料

            //1. 設定變數
            ln_emnoStr = ln_emno.Trim();
            if (ln_emnoStr != null & ln_emnoStr != "")
            {
                // 2. 依傳入工號取得mail 資訊
                logininfo = db.login.Find(ln_emnoStr);
                if (logininfo != null)
                {
                    emailStr = logininfo.ln_email.Trim();
                }
            }
            else
            {
                //在下方處理email 字串
            }

            if (emailStr == "")
            {
                //2013/05/06 by souleye 若申請者無E-mail，則將信件發給computer自己
                //'str = "computer@transart.com.tw"
                //2013/08/15 by souleye 將信件改為轉寄給Clark
                //'str = "clark@transart.com.tw"
                //2013/12/06 by souleye 將信件改為轉寄給souleye
                //'str = "souleye@transart.com.tw"
                //2014/06/24 by souleye 將信件改為轉寄給souleye跟Clark
                //'str = "souleye@transart.com.tw;clark@transart.com.tw"                       
                //2015/01/06 by souleye 將信件改寄給資訊室
                emailStr = "TW_IT@transart.com.tw";
                //2015/11/26 by souleye 依佳玲要求，將信改寄給computer(當資訊主管核准後，會CC給TW_IT)
                emailStr = "computer@transart.com.tw";
            }

            /* 開發測試期間改用自己mail*/
            emailStr = "slhungf@transart.com.tw";
            return emailStr;
        }

        public static string GetRecipientCCEMail(string ln_emno, string KIND)
        {
			/*
             * 改寫 Transart\MisService\library\MisService_wfls.asp
             * 
            原程式註解掉的部分
            '2012/12/11 by souleye 接件時，僅CC給財務部與資訊組主管
            '	IF KIND = 0 THEN	'退件
            '		str = "computer@transart.com.tw;laura@transart.com.tw;"
            '	ELSE
            '		str = "laura@transart.com.tw;candy@transart.com.tw;"
            '	END IF

	            '2013/08/15 by souleye 將信件改為寄給clark
	            'str = "clark@transart.com.tw;"
	            '2013/12/06 by souleye 將信件改為寄給souleye
	            'str = "souleye@transart.com.tw;"
	            '2014/06/24 by souleye 將信件改為轉寄給souleye跟Clark
	            'str = "souleye@transart.com.tw;clark@transart.com.tw;"
	            '2015/01/06 by souleye 將信件改寄給資訊室
	            '2016/08/01 by souleye 退件也要寄給資訊室
	            '2018/02/06 By Clark 轉件也寄給資訊室
             */

			//資料庫物件
			TransartEntities db = new TransartEntities();

			//宣告變數
			string ln_emnoStr = ""; //工號
            string opKind = ""; //接收工號參數
            login logininfo;
			//List<login> logininfoM; //查找指定工號與其主管在login 資料(此資料類型再查詢會出錯), 故改寫
			List<string> logininfoM; //查找指定工號與其主管在login 資料


			string emailStr = ""; // email 回傳字串
            string SQL = ""; //用於查找指定工號部門主管的mail的SQL字串

            //LinQ 用法繁複, 不採用
            //employee emp; //員工資料表
            //dept dept; //部門資料表

            //1. 設定變數
            ln_emnoStr = ln_emno.Trim();
            opKind = KIND.Trim();

            if (opKind == "0" | opKind == "2" | opKind == "5" | opKind == "8") {
                emailStr = "TW_IT@transart.com.tw;";
            }

            // 2. 依傳入工號取得mail 資訊
            ln_emnoStr = ln_emno.Trim();
            if (ln_emnoStr != null & ln_emnoStr != "")
            {
                //strSQL = " SELECT TOP 1 * FROM login WHERE ln_emno = '" + ln_emno + "'"
                logininfo = db.login.Find(ln_emnoStr);
                if (logininfo != null)
                {
					//3. 登入者工號資料存在時, 取得部門主管mail 資料
					/*
                    SQL = " select ln_email from login												";
                    SQL = SQL + " 	where ln_emno in ( select dp_emno from dept where dp_no in				";
                    SQL = SQL + " 		( select em_dpno from employee where em_no = '" + ln_emno + "' ))  	";
					字串組成格式(空白部分會被解析為\t\t\t\t.....)
					 select ln_email from login												 	where ln_emno in ( select dp_emno from dept where dp_no in				 		( select em_dpno from employee where em_no = '783' ))  	
					*/
					SQL = " select ln_email from login ";
					SQL = SQL + " where ln_emno in ( select dp_emno from dept where dp_no in ";
					SQL = SQL + "   ( select em_dpno from employee where em_no = '" + ln_emno + "' )) ";
					try
					{
						//logininfoM = db.Database.SqlQuery<login>(SQL).ToList();
						logininfoM = db.Database.SqlQuery<string>(SQL).ToList();
						/*
						 * 
						IF NOT Rec.EOF THEN
							IF Trim( Rec("ln_email") ) <> "patt@transart.com.tw" THEN
								DO WHILE NOT Rec.EOF
									str = str + Trim( Rec("ln_email") ) + ";"
									Rec.MoveNext
								LOOP
							END IF
						END IF
						 */
						if (logininfoM != null)
						{
							/*
							foreach (login t in logininfoM)
							{
								if (t.ln_email.Trim() != "patt@transart.com.tw")
								{
									emailStr = emailStr + t.ln_email.Trim() + ";";
								}
							}
							*/
							for (int i = 0; i < logininfoM.Count; i++)
							{
								if (logininfoM[i] != "patt@transart.com.tw") //Patt(李煜培 總經理)
								{
									emailStr = emailStr + logininfoM[i] + ";";
								}
							}
						}//end of logininfoM != null)
					}
					catch (System.Exception ex)
					{
						emailStr = "computer@transart.com.tw;";
					}
                    

                    // 註記, 不採用三個Find 的原因是處理多筆資料待解
                    if (opKind == "0" || opKind == "2" || opKind == "3" || opKind == "4"
                        || opKind == "5" || opKind == "6" || opKind == "6" || opKind == "8")
                    {
                        emailStr = emailStr + "laura@transart.com.tw;";//薛小英主管
                    }
                }
                else
                {
                    //工號無效, 回傳空值
                } // end of if (logininfo != null)
            }
            else
            {
                //工號無效, 回傳空值
            }

			/* 開發測試期間改用自己mail*/
			emailStr = "slhungf@transart.com.tw;";
			return emailStr;
        }
    }
}
 
 