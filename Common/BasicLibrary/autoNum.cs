using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace Common.BasicLibrary
{
    public class autoNum
    {
    }

	public class getNumber
	{
		public static string getAutoNumber(string sCodeNo, int nDpNo, Boolean bNeedYear, int recursiveCount)
		{
			string au_codeno, strSQL, strSeqNo;
			string returnValue;
			string regExPattern;
			RegexOptions options;
			Regex regEx;
			string getAutoNumber;
			string todayYear, todayMonth, todayDay, todayMonthShort, todayDayShort;
			string ChkSQL;
			int ErrFlag = 0;
			//要連接資料庫的位置
			string sqlConnectionStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TransartEntities"].ConnectionString;

			strSeqNo = "";
			returnValue = "";
			getAutoNumber = "";

			ChkSQL = "";

			//ASP
			//Year(Now) '' Year in 4 digits
			//Month(Now) '' Month without leading zero
			//Day(Now) '' Day of the month without leading zero
			//today = DateTime.Today.ToString(); // 存在一~九月 一~九日資料格式變一碼的問題
			//以下指定格式會將1~9前補0
			todayYear = DateTime.Now.ToString("yyyy");
			todayMonth = DateTime.Now.ToString("MM");
			todayDay = DateTime.Now.ToString("dd");
			todayMonthShort = DateTime.Now.Month.ToString();
			todayDayShort = DateTime.Now.Day.ToString();

			/*
             * 以下ASP 不改寫
            '2007/04/26 By Clark 異常單在每月25日開的單要以下個月的單號編碼
	        '2015/12/30 By Clark 2016年起取消此項程序
	        IF YEAR(Date()) < 2016 THEN
		        IF (sCodeNo = "EA" OR sCodeNo = "EB") AND Day( today ) >= 25 THEN
			        today = DateAdd( "m" , 1 , today )
		        END IF
	        END IF

             * 
             */

			//判斷需要加入事業部簡碼
			if (nDpNo > 0)
			{
				//au_codeno = CStr(Left(Trim(sCodeNo), 2) & nDpNo);
				au_codeno = sCodeNo.Trim().Substring(0, 2) + nDpNo.ToString();
			}
			else
			{
				//au_codeno = Left(Trim(sCodeNo),2)
				au_codeno = sCodeNo.Trim().Substring(0, 2);
			}

			//判斷是否需要使用年份編碼
			/*
			Set regEx = New RegExp
			regEx.Pattern = "^(SC|PM|PC|PE|PB|A|B|F|H|U|P|Z|QI|OI|QR|QS|PI|CI|IN|EA|EB|RD|RP|BA|BB|BC|BD|BE|BF)$"
			regEx.IgnoreCase = False
			retVal = regEx.Test(au_codeno)
			*/

			// Step 1: create new Regex.
			regExPattern = "^(SC|PM|PC|PE|PB|A|B|F|H|U|P|Z|QI|OI|QR|QS|PI|CI|IN|EA|EB|RD|RP|BA|BB|BC|BD|BE|BF)$";
			options = RegexOptions.IgnoreCase; //RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
			regEx = new Regex(regExPattern, options);

			// Step 2: call Match on Regex instance.
			Match match = regEx.Match(au_codeno);

			if (bNeedYear == true)
			{

				//If(retVal = False) {
				// Step 3: test for Success.
				if (!match.Success)
				{
					//returnValue = au_codeno & Right(CStr(Year(today)), 1) & CStr(Hex(Month(today))) & Right("0" + CStr(Day(today)), 2) 
					//月份短碼, 日期補0+right 處理兩碼, => 改寫完整兩位數日期
					returnValue = au_codeno + stringProcess.Right(todayYear, 1) + stringProcess.ConvertHexToString(stringProcess.ConvertStringToHex(todayMonthShort, System.Text.Encoding.Unicode), System.Text.Encoding.Unicode) + todayDay;
				}
				else
				{
					//returnValue = au_codeno & Right(CStr(Year(today)), 1) & Right(String(1, "0") + CStr(Month(today)), 2)
					//STRING產生字串函數：產生N個字元。 	string(5, "*") * ****
					//月份補0+right 處理, 改寫完整兩位數月份
					returnValue = au_codeno + stringProcess.Right(todayYear, 1) + todayMonth;
				}
				//'2008/01/15 By Clark 另外處理新產品開發需求單號 EX:RD8001
				//'2014/10/27 By Clark RD RP 年度碼為 2 碼(太誇張了！已經用了快兩年，現在才發現單號沒連號！)
				if (au_codeno == "RD" | au_codeno == "RP")
				{
					//returnValue = au_codeno & Right(CStr(Year(today)), 2)
					returnValue = au_codeno + stringProcess.Right(todayYear, 2);
				}
			}
			else
			{
				if (au_codeno == "I" | au_codeno == "C" | au_codeno == "RC" | au_codeno == "RM" | au_codeno == "RB")
				{
					//returnValue = au_codeno & Right(String(1, "0")+CStr(Month(today)), 2) & Right("0"+CStr(Day(today)), 2)
					//月份& 日期補0+right 處理, 改寫完整兩位數月份&日期
					returnValue = au_codeno + todayMonth + todayDay;
				}
				else if (au_codeno == "T")
				{
					if (DateTime.Today >= DateTime.Parse("2014/12/26"))
					{
						//returnValue = au_codeno & Right(CStr(YEAR(today)), 2) & CStr(Hex(Month(today))) & Right("0" + CStr(Day(today)), 2)
						//日期補0+right 處理兩碼, => 改寫完整兩位數日期
						returnValue = au_codeno + stringProcess.Right(todayYear, 2) + stringProcess.ConvertHexToString(stringProcess.ConvertStringToHex(todayMonthShort, System.Text.Encoding.Unicode), System.Text.Encoding.Unicode) + todayDay;

					}
					else
					{
						//returnValue = au_codeno & CStr(Hex(Month(today))) & Right("0" + CStr(Day(today)), 2)
						//日期補0+right 處理兩碼, => 改寫完整兩位數日期
						returnValue = au_codeno + stringProcess.ConvertHexToString(stringProcess.ConvertStringToHex(todayMonthShort, System.Text.Encoding.Unicode), System.Text.Encoding.Unicode) + todayDay;
					}
				}
				else if (au_codeno == "TQ")
				{
					//returnValue = au_codeno & CStr(Hex(Month(today))) & Right("0" + CStr(Day(today)), 2)
					//日期補0+right 處理兩碼, => 改寫完整兩位數日期
					returnValue = au_codeno + stringProcess.ConvertHexToString(stringProcess.ConvertStringToHex(todayMonthShort, System.Text.Encoding.Unicode), System.Text.Encoding.Unicode) + todayDay;
				}
				else if (au_codeno == "PI" | au_codeno == "CI" | au_codeno == "IN")
				{
					//'2004/11/22 By Clark 增加報價單流水號 PI (PI-YMM001)
					//returnValue = au_codeno & "-" & Right(CStr(Year(today)), 1) & Right(String(1, "0") + CStr(Month(today)), 2)
					////月份補0+right 處理, 改寫完整兩位數月份
					returnValue = au_codeno + "-" + stringProcess.Right(todayYear, 1) + todayMonth;
				}
				else
				{
					//returnValue = au_codeno & CStr(Hex(Month(today))) & Right("0" + CStr(Day(today)), 2)
					//日期補0+right 處理兩碼, => 改寫完整兩位數日期
					returnValue = au_codeno + stringProcess.ConvertHexToString(stringProcess.ConvertStringToHex(todayMonthShort, System.Text.Encoding.Unicode), System.Text.Encoding.Unicode) + todayDay;
				}
			}

			//'//利用au_codeno以及today取得目前編碼檔的狀況
			//If(retVal = False) Then
			if (!match.Success)
			{
				//原寫法" where au_date='"&today&"' and au_codeno='"&au_codeno&"' And au_id='R' " + _
				strSQL = " select min(au_no) as au_no, 'R' as seqType " +
						 " from auto " +
						 " where convert(char(10),au_date,111)='" + todayYear + "/" + todayMonth + "/" + todayDay + "' and au_codeno='" + au_codeno + "' And au_id='R' " +
						 " UNION " +
						 " select min(au_no) as au_no, 'L' as seqType " +
						 " from auto " +
						 " where convert(char(10),au_date,111)='" + todayYear + "/" + todayMonth + "/" + todayDay + "' and au_codeno='" + au_codeno + "' And au_id='L' ";
			}
			//ElseIf(retVal = True) Then
			else
			{
				//'2008/01/15 By Clark 另外處理新產品開發需求單號 EX:RD8001
				if (au_codeno == "RD" | au_codeno == "RP")
				{
					strSQL = " select min(au_no) as au_no, 'R' as seqType  " +
							 "from auto  " +
							 "where convert(char(4),au_date,111) = '" + todayYear + "' and au_codeno='" + au_codeno + "' And au_id='R' " +
							 "UNION  " +
							 "select min(au_no) as au_no, 'L' as seqType " +
							 "from auto  " +
							 "where convert(char(4),au_date,111) = '" + todayYear + "' and au_codeno='" + au_codeno + "' And au_id='L' ";
				}
				else
				{
					//"where Left(convert(char(10),au_date,111), 7)='" &Year(today)&"/"&Right(String(1,"0")+CStr(Month(today)),2)& "' and au_codeno='"&au_codeno&"' And au_id='R' " + _
					//月份補0+right 處理, 改寫完整兩位數月份
					strSQL = " select min(au_no) as au_no, 'R' as seqType  " +
						"from auto  " +
						"where Left(convert(char(10),au_date,111), 7)='" + todayYear + "/" + todayMonth + "' and au_codeno='" + au_codeno + "' And au_id='R' " +
						"UNION  " +
						"select min(au_no) as au_no, 'L' as seqType " +
						"from auto  " +
						"where Left(convert(char(10),au_date,111), 7)='" + todayYear + "/" + todayMonth + "' and au_codeno='" + au_codeno + "' And au_id='L' ";
				}
			}
			/*
			bSucess = dal_getRecordset(oRecordset, strConnect, strSQL)

			If(bSucess = False) Then
				 getAutoNumber = ""
			End If

			'2015/01/19 By Clark 增加異常處理，因 01 A7 常同時轉S單，而發生通知單明細歸屬到錯誤的客戶下，若發生錯誤則重新取號
			'2018/07/19 By Clark 針對 S 單做檢查，若單號已存在於 inform ，則重新取號
			ErrFlag = 0

			'//看看是否存在可供回收利用的流水號
			oRecordset.Filter = "seqType='R'"

			If(IsNull(oRecordset("au_no")) = True) Then
				'//判斷是否存在最後可用編號
				oRecordset.Filter = "seqType='L'"

				If(IsNull(oRecordset("au_no")) = True) Then
					 strSeqNo = "0001"
					'//新增表單編號
					strSQL = "Insert Into auto (au_date,au_codeno,au_no,au_id) values ('" + today + "','" + au_codeno + "','0002','L')"
					bSucess = dal_ExecQuery(strConnect, strSQL)
				Else
					strSeqNo = oRecordset("au_no")

					'//將可用編號遞增
					If(retVal = False) Then
						 strSQL = "Update auto Set au_no='" + (Right("000" + CStr(CInt(strSeqNo) + 1), 4)) + "' where au_date='" + today + "' and au_codeno='" + au_codeno + "' And au_id='L'"
					Else
						'2008/03/19 By Clark 另外處理新產品開發需求單號 EX:RD8001
						IF au_codeno = "RD" OR au_codeno = "RP" THEN
							strSQL = "Update auto Set au_no='" + (Right("000" + CStr(CInt(strSeqNo) + 1), 4)) + "' where Left(convert(char(10),au_date,111), 4)='" + Year(today) + "' and au_codeno='" + au_codeno + "' And au_id='L'"
						ELSE
							strSQL = "Update auto Set au_no='" + (Right("000" + CStr(CInt(strSeqNo) + 1), 4)) + "' where Left(convert(char(10),au_date,111), 7)='" + Year(today) + "/" + Right(String(1, "0") + CStr(Month(today)), 2) + "' and au_codeno='" + au_codeno + "' And au_id='L'"
						END IF
					End If

					bSucess = dal_ExecQuery(strConnect, strSQL)
				End If
			Else
				strSeqNo = oRecordset("au_no")

				'//刪除回收編號
				If(retVal = False) Then
					 strSQL = "Delete auto where au_date='" + today + "' and au_codeno='" + au_codeno + "' And au_id='R' and au_no='" + strSeqNo + "'"
				Else
					'2008/01/15 By Clark 另外處理新產品開發需求單號 EX:RD8001
					IF au_codeno = "RD" OR au_codeno = "RP" THEN
						strSQL = "Delete auto where convert(char(4),au_date,111) = '" + Year(today) + "' and au_codeno='" + au_codeno + "' And au_id='R' And au_no='" + strSeqNo + "'"
					ELSE
						strSQL = "Delete auto where Left(convert(char(10),au_date,111), 7)='" + Year(today) + "/" + Right(String(1, "0") + CStr(Month(today)), 2) + "' and au_codeno='" + au_codeno + "' And au_id='R' And au_no='" + strSeqNo + "'"
					END IF
				End If
				bSucess = dal_ExecQuery(strConnect, strSQL)
			End If
			*/

			//改寫SQL 連線
			try
			{
				//註記直接取用web.config 設定的connectionstring 會出錯 >> 不支持关键字“metadata" 
				//using (SqlConnection sqlConn = new SqlConnection(sqlConnectionStr))
				var entityConnectionStringBuilder = new EntityConnectionStringBuilder(sqlConnectionStr);
				var sqlConnectionConnectionString = entityConnectionStringBuilder.ProviderConnectionString;
				//Console.WriteLine($"EF Connection String: {sqlConnectionStr}"); 
				//Console.WriteLine($"SqlConnection Connection String: {sqlConnectionConnectionString}");
				using (SqlConnection sqlConn = new SqlConnection(sqlConnectionConnectionString))
				{
					sqlConn.Open();

					SqlCommand sqlComm = new SqlCommand();
					sqlComm.Connection = sqlConn;
					sqlComm.CommandTimeout = 3000;
					sqlComm.CommandText = strSQL;

					SqlDataReader dr = sqlComm.ExecuteReader();
					if (dr.HasRows)
					{
						//上述SQL執行結果如下兩列資料:
						//au_no seqType
						//Null	R
						//002	L
						if (dr.Read()) //取得第一列R 回收的流水號
						{
							//MessageBox.Show(dr["au_no"].ToString() + " " + dr["seqType"].ToString(), "");

							if (dr["au_no"].Equals(DBNull.Value))
							{
								if (dr.Read())// 取得第二筆L 資料
								{
									//MessageBox.Show(dr["au_no"].ToString() + " " + dr["seqType"].ToString(), "");
									//取得第二列L並避免為空白行
									if (dr["au_no"].Equals(DBNull.Value))
									{
										strSeqNo = "0001";
										//新增表單號碼紀錄
										//處理紀錄: 從字元字串轉換成日期及/或時間時，轉換失敗。
										//strSQL = "Insert Into auto(au_date, au_codeno, au_no, au_id) values('" + DateTime.Today + "', '" + au_codeno + "', '0002', 'L')";
										strSQL = "Insert Into auto(au_date, au_codeno, au_no, au_id) values('" + todayYear + "/" + todayMonth + "/" + todayDay + "', '" + au_codeno + "', '0002', 'L')";
									}
									else
									{
										strSeqNo = dr["au_no"].ToString();
										//將可用編號遞增
										//If(retVal = False) Then
										if (!match.Success)
										{
											// strSQL = "Update auto Set au_no='" & (Right("000" + CStr(CInt(strSeqNo) + 1), 4)) & "' where au_date='" & today & "' and au_codeno='" & au_codeno & "' And au_id='L'"
											strSQL = "Update auto Set au_no='" + stringProcess.Right("000" + Convert.ToString(int.Parse(strSeqNo) + 1), 4) + "' where convert(char(10),au_date,111)='" + todayYear + "/" + todayMonth + "/" + todayDay + "' and au_codeno='" + au_codeno + "' And au_id='L'";
										}
										else
										{
											//'2008/03/19 By Clark 另外處理新產品開發需求單號 EX:RD8001
											if (au_codeno == "RD" | au_codeno == "RP")
											{
												//strSQL = "Update auto Set au_no='"&(Right("000"+CStr(CInt(strSeqNo)+1), 4))&"' where Left(convert(char(10),au_date,111), 4)='" & Year(today) & "' and au_codeno='"&au_codeno&"' And au_id='L'"
												strSQL = "Update auto Set au_no='" + (stringProcess.Right("000" + Convert.ToString(int.Parse(strSeqNo) + 1), 4)) + "' where Left(convert(char(10),au_date,111), 4)='" + todayYear + "' and au_codeno='" + au_codeno + "' And au_id='L'";
											}
											else
											{
												//strSQL = "Update auto Set au_no='" & (Right("000" + CStr(CInt(strSeqNo) + 1), 4)) & "' where Left(convert(char(10),au_date,111), 7)='" & Year(today) & "/" & Right(String(1, "0") + CStr(Month(today)), 2) & "' and au_codeno='" & au_codeno & "' And au_id='L'"
												strSQL = "Update auto Set au_no='" + (stringProcess.Right("000" + Convert.ToString(int.Parse(strSeqNo) + 1), 4)) + "' where Left(convert(char(10),au_date,111), 7)='" + todayYear + "/" + todayMonth + "' and au_codeno='" + au_codeno + "' And au_id='L'";
											}
										}
									}//End of 第二列L DBNull 判斷
								}// End of 第二列L 判斷
							}
							else
							{
								strSeqNo = dr["au_no"].ToString();
								//刪除回收編號
								//If(retVal = False) Then
								if (!match.Success)
								{
									//strSQL = "Delete auto where au_date='" & today & "' and au_codeno='" & au_codeno & "' And au_id='R' and au_no='" & strSeqNo & "'"
									strSQL = "Delete auto where au_date='" + todayYear + "/" + todayMonth + "/" + todayDay + "' and au_codeno='" + au_codeno + "' And au_id='R' and au_no='" + strSeqNo + "'";
								}
								else
								{
									//'2008/01/15 By Clark 另外處理新產品開發需求單號 EX:RD8001
									if (au_codeno == "RD" | au_codeno == "RP")
									{
										//strSQL = "Delete auto where convert(char(4),au_date,111) = '" & Year(today) & "' and au_codeno='" & au_codeno & "' And au_id='R' And au_no='" & strSeqNo & "'"
										strSQL = "Delete auto where convert(char(4),au_date,111) = '" + todayYear + "' and au_codeno='" + au_codeno + "' And au_id='R' And au_no='" + strSeqNo + "'";
									}
									else
									{
										//strSQL = "Delete auto where Left(convert(char(10),au_date,111), 7)='" & Year(today) & "/" & Right(String(1, "0") + CStr(Month(today)), 2) & "' and au_codeno='" & au_codeno & "' And au_id='R' And au_no='" & strSeqNo & "'"
										strSQL = "Delete auto where Left(convert(char(10),au_date,111), 7)='" + todayYear + "/" + todayMonth + "' and au_codeno='" + au_codeno + "' And au_id='R' And au_no='" + strSeqNo + "'";
									}
								}
							} // End of 第一列 R 空白列檢查
						}//End if 第一列資料
					}// End of dr.hasRows

					//更新單號紀錄表資料
					try
					{
						SqlCommand sqlComm2 = new SqlCommand();
						sqlComm2.Connection = sqlConn;
						sqlComm2.CommandTimeout = 3000;
						sqlComm2.CommandText = strSQL;

						int dr2 = sqlComm2.ExecuteNonQuery();
						if (dr2 > 0)
						{
							//update or insert successfully
						}
						else
						{
							//抓錯誤訊息
							MessageBox.Show("執行單號插入或更新失敗", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return "fail";
						}

					}
					catch (Exception e)
					{
						//抓錯誤訊息
						MessageBox.Show(e.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return "";
					}
					//關閉讀取資料庫資料的元件
					dr.Close();
					//關閉資料庫
					sqlConn.Close();
				}
			}
			catch (Exception e)
			{
				//抓錯誤訊息
				MessageBox.Show(e.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return "fail";
			}

			//組合出完整編號
			//'2014/12/18 By Clark 2015年開始，送貨單號增加兩位年度碼
			if (au_codeno == "T")
			{
				if (DateTime.Today >= DateTime.Parse("2014/12/26"))
				{
					returnValue = returnValue + stringProcess.Right(strSeqNo, 9 - returnValue.Length);
				}
				else
				{
					returnValue = returnValue + stringProcess.Right(strSeqNo, 7 - returnValue.Length);
				}
			}
			else if (au_codeno == "PI" | au_codeno == "CI" | au_codeno == "IN")
			{
				returnValue = returnValue + stringProcess.Right(strSeqNo, 9 - returnValue.Length);
			}
			else if (au_codeno == "RD" | au_codeno == "RP")
			{
				returnValue = returnValue + stringProcess.Right(strSeqNo, 6 - returnValue.Length);
			}
			else
			{
				returnValue = returnValue + stringProcess.Right(strSeqNo, 8 - returnValue.Length);
			}

			//regEx = "";

			//'2016/06/22 By Clark 領料單號跳號原因不明，增加紀錄取號履歷至 2016/07/31
			//'2016/06/23 By Clark 因天津廠發生送貨單跳號，改為紀錄所有取號履歷
			//'IF DateDiff("d" ,Date() ,"2016/12/31") >= 0 THEN
			//'	'IF au_codeno = "W" THEN
			//'		CALL dal_ExecQuery(strConnect, "Insert AutoNum_log ( ag_date ,ag_no ,ag_ip ,ag_pro ) values ( getdate() ,'" + returnValue + "' ,'" + Request.ServerVariables("REMOTE_ADDR") + "','" + Request.ServerVariables("URL") + "' ) ")
			//'	'END IF
			//'END IF

			//'2018/07/19 By Clark 針對 S 單做檢查，若單號已存在於 inform ，則重新取號
			//'2020/01/06 By Clark 增加對 M 單做檢查，若單號已存在於 Bommo ，則重新取號 (因為同部門通知單過帳自動產生製令單與手開製令單發生取號異常的狀況)
			if (sCodeNo == "S" | sCodeNo == "M")
			{
				if (sCodeNo == "S")
				{
					ChkSQL = "select Count(*) as Cnt from inform where im_inno = '" + returnValue + "'";
				}
				else if (sCodeNo == "M")
				{
					ChkSQL = "select Count(*) as Cnt from bommo where mo_mkno = '" + returnValue + "'";
				}

				try
				{
					using (SqlConnection sqlConn = new SqlConnection(sqlConnectionStr))
					{
						sqlConn.Open();
						SqlCommand sqlComm = new SqlCommand();
						sqlComm.Connection = sqlConn;
						sqlComm.CommandTimeout = 3000;
						sqlComm.CommandText = ChkSQL;

						SqlDataReader dr = sqlComm.ExecuteReader();

						if (dr.HasRows)
						{
							ErrFlag = Convert.ToInt32(dr["Cnt"]);
						}
					}
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return "fail";
				}
			}

			if (ErrFlag > 0)
			{
				if (recursiveCount == 1)
				{
					getAutoNumber = getNumber.getAutoNumber(sCodeNo, nDpNo, bNeedYear, 2);
				}
				else
				{
					getAutoNumber = "fail";
				}
			}
			else
			{
				getAutoNumber = returnValue;
			}
			return getAutoNumber;
		}
	}

	public static class stringProcess
	{
		public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
		{
			Byte[] stringBytes = encoding.GetBytes(input);
			StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
			foreach (byte b in stringBytes)
			{
				sbBytes.AppendFormat("{0:X2}", b);
			}
			return sbBytes.ToString();
		}

		public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
		{
			int numberChars = hexInput.Length;
			byte[] bytes = new byte[numberChars / 2];
			for (int i = 0; i < numberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
			}
			return encoding.GetString(bytes);
		}

		public static string Left(string param, int length)
		{
			string result = param.Substring(0, length);
			return result;
		}


		public static string Right(string s, int length)
		{
			length = Math.Max(length, 0);

			if (s.Length > length)
			{
				return s.Substring(s.Length - length, length);
			}
			else
			{
				return s;
			}
		}

		private static string Mid(string param, int startIndex, int length)
		{
			string result = param.Substring(startIndex, length);
			return result;
		}

		private static string Mid(string param, int startIndex)
		{

			string result = param.Substring(startIndex);
			return result;
		}
	}
}