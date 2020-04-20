//檢查是否為合法日期，必須配合Date.vbs
function isDate(strDate) {
	if (strDate.search(/^[0-9]{4}\/((0{0,1}[1-9])|(1[0-2]))\/((0{0,1}[1-9])|([1-2][0-9])|(3[0-1]))$/) == -1) {
		return false;
	}

	//2017/11/28 By Clark 相容性測試：改以 javascript 替換日期相關處理，取消 vbscript
	var t = Date.parse(strDate);

	if (isNaN(t)) {
		//alert('你輸入的不是日期');
		return false;
	} else {
		return true
	}

	//if (strDate.search(/^[0-9]{4}\/((0{0,1}[1-9])|(1[0-2]))\/((0{0,1}[1-9])|([1-2][0-9])|(3[0-1]))$/)==-1){
	//	return false;
	//}
	//
	//return VBIsDate__(strDate);
}

//傳回兩日期之差異，使用方法與VBScript中的datediff相同
function datediff(sPart, date1, date2) {

	//2017/11/28 By Clark 相容性測試：改以 javascript 替換日期相關處理，取消 vbscript
	var SDate, EDate;

	if (typeof (date1) == "object")
		SDate = date1;
	else
		SDate = new Date(date1);

	if (typeof (date2) == "object")
		EDate = date2;
	else
		EDate = new Date(date2);

	if (isNaN(SDate)) return undefined;
	if (isNaN(EDate)) return undefined;

	switch (sPart) {
		case "s": return parseInt((EDate - SDate) / 1000);
		case "n": return parseInt((EDate - SDate) / 60000);
		case "h": return parseInt((EDate - SDate) / 3600000);
		case "d": return parseInt((EDate - SDate) / 86400000);
		case "w": return parseInt((EDate - SDate) / (86400000 * 7));
		case "m": return (EDate.getMonth() + 1) + ((EDate.getFullYear() - SDate.getFullYear()) * 12) - (SDate.getMonth() + 1);
		case "y": return EDate.getFullYear() - SDate.getFullYear();
	}

	//var vt_date1, vt_date2;
	//
	//if (typeof(date1)=="object")
	//  vt_date1 = date1.getVarDate();
	//else
	//  vt_date1 = date1;
	//
	//if (typeof(date2)=="object")
	//  vt_date2 = date2.getVarDate();
	//else
	//  vt_date2 = date2;
	//
	//return VB_is_fn_DateDiff(sPart, vt_date1, vt_date2);
}

//傳回內容為某個基準日期加上數個時間間隔單位後的日期
//使用方法與VBScript中的dateAdd相同
function dateAdd(interval, number, date) {
	//2017/11/28 By Clark 相容性測試：改以 javascript 替換日期相關處理，取消 vbscript
	var d;

	if (typeof (date) == "object") {
		d = date;
	} else {
		d = new Date(date);
	}

	switch (interval) {
		case "d":
			d.setDate(d.getDate() + number);
			break;
		case "m":
			d.setMonth(d.getMonth() + number);
			break;
		case "y":
			d.setYear(d.getYear() + number);
			break;
		default:
			break;
	}

	return d;

	//	var vt_date;
	//	if (typeof(date)=="object"){
	//	  vt_date = date.getVarDate();
	//	  //vt_date= date.getFullYear()+"/"+("0"+(date.getMonth()+1)).right(2)+"/"+("0"+(date.getDate())).right(2)
	//	}else{
	//	  vt_date = date;
	//	}

	//return (new Date(VB_is_fn_DateAdd(interval, number, vt_date)));
}

//2017/11/28 By Clark 相容性測試：改以 javascript 替換日期相關處理，取消 vbscript
function CDate4YYYYMMDD(The_date) {
	var The_Len, d, strDate;

	if (typeof (The_date) == "object") {
		The_Len = The_date.value.length;

		//以字串長度判斷字串為月份或日期
		if (The_Len <= 7) {
			d = new Date(The_date.value + "/01");
		} else {
			d = The_date;
		}

	} else {
		The_Len = The_date.length;

		if (The_Len == 0) return "";

		//以字串長度判斷字串為月份或日期
		if (The_Len <= 7) {
			The_date = The_date + "/01";
		}

		d = new Date(The_date);
	}

	//組成日期十碼字串
	//strDate = d.getFullYear() + "/" + ("0" + (d.getMonth() + 1)).right(2) + "/" + ("0" + (d.getDate())).right(2);
	
	var month = "0" + (d.getMonth() + 1);
	var day = "0" + (d.getDate());

	strDate = d.getFullYear() + "/" + month.substring(month.length - 2, month.length) + "/" + day.substring(day.length - 2, day.length);


	if (The_Len <= 7) {
		strDate = strDate.substr(0, 7);
	}

	return strDate;
}

//2005/06/15 By Clark 排程系統時間判斷改放在 Date.js 以方便取用及修改
function ChkServerTime() {

	var dateObj = new Date();
	var checkHr = dateObj.getHours();
	var checkMin = dateObj.getMinutes();
	var ChkMin = checkHr * 60 + checkMin;

	//排程處理時間為 8:55 ~ 9:10，以分鐘數作比較
	//2006/07/21 By Clark 排程處理時間為 7:40 ~ 7:45，以分鐘數作比較
	//2007/07/12 By Clark 排程處理改於 7:45 執行
	if (ChkMin >= 465 && ChkMin <= 470) {
		alert("系統正在進行資料轉移，請於07:50以後再作業。");
		return false;
	} else {
		return true;
	}
}

//2005/06/15 By Clark 排程系統時間判斷改放在 Date.js 以方便取用及修改，不顯示訊息
function ChkServerTime2() {

	var dateObj = new Date();
	var checkHr = dateObj.getHours();
	var checkMin = dateObj.getMinutes();
	var ChkMin = checkHr * 60 + checkMin;

	//排程處理時間為 8:55 ~ 9:10，以分鐘數作比較
	//2006/07/21 By Clark 排程處理時間為 7:40 ~ 7:45，以分鐘數作比較
	//2007/07/12 By Clark 排程處理改於 7:45 執行
	if (ChkMin >= 465 && ChkMin <= 470) {
		return false;
	} else {
		return true;
	}
}