// JavaScript for ASP.NET Maker 3
// (C) 2004-2006 e.World Technology Ltd.

var ew_DateSep; // default date separator
if (ew_DateSep == "") ew_DateSep = "/";

function ew_DHTMLEditor(name) {
	this.name = name;
	this.create = function() { this.active = true; }
	this.editor = null;
	this.active = false;
}

function ew_CreateEditor(name) {
	if (typeof ew_DHTMLEditors == 'undefined')
		return;
	if (name && name.substring(0,2) == 'r_')
		name = name.replace(/r_/, 'x_');
	for (var i = 0; i < ew_DHTMLEditors.length; i++) {
		var ed = ew_DHTMLEditors[i];
		var cr = !ed.active;
		if (name) cr = cr && ed.name == name;
		if (cr) {
			if (typeof ed.create == 'function')
				ed.create();
			if (name)
				break;
		}
	}
}

// reset text box
function ew_ResetElement(id) {
	var obj = document.getElementById(id);
	obj.value = '';
}

// reset drop down list
function ew_ResetDropDownList(id) {
	var obj = document.getElementById(id);
	if (obj.multiple != true) {
		obj.selectedIndex = '0';
		if (document.createEvent) {
			var onchangeEvent = document.createEvent('HTMLEvents');
			onchangeEvent.initEvent('change', true, false);
			obj.dispatchEvent(onchangeEvent);
		} else if (document.createEventObject) {
			obj.fireEvent('onchange');
		}
	} else {
		for (var i=0; i<obj.options.length; i++)
			obj.options[i].selected = false;
	}
}

// reset radio button
function ew_ResetRadioButton(name, defaultIndex) {
	var rdoBtn = document.getElementsByName(name);
	for (var i=0;i<rdoBtn.length;i++) {
		rdoBtn[i].checked = false;
	}
	if (defaultIndex != null) rdoBtn[defaultIndex].checked = true; // reset search type of basic search to 'Exact phrase'
}

// reset check box
function ew_ResetCheckBox(name) {
	checkboxelems = document.getElementsByTagName("input");
	for (i = 0; i < checkboxelems.length; i++) {
		var elemname = checkboxelems[i].name;
		if (elemname.indexOf(name) >= 0) 
			checkboxelems[i].checked = false;
	}
}

function ew_RemoveSpaces(value) {
	str = value.replace(/^\s*|\s*$/g, "");
	str = str.toLowerCase();
	if (str == "<p />" || str == "<p/>" || str == "<p>" ||
		str == "<br />" || str == "<br/>" || str == "<br>" ||
		str == "&nbsp;" || str == "<p>&nbsp;</p>")
		return ""
	else
		return value;
}

function ew_TextAreaHasValue(source, args) {
	var object_value = args.Value;	
	object_value = object_value.replace(/^\s+|\s+$/g, ""); // trim
	object_value = object_value.toLowerCase();	
	if (object_value == "" ||
		object_value == "<p>&nbsp;</p>" ||
		object_value == "&nbsp;" ||
		object_value == "<p />" ||
		object_value == "<br />") {
		args.IsValid = false;
		//ew_SetFocus(source.controltovalidate);
	}
	return true;
}

function ew_CheckBoxListHasValue(source, args) { 
	if (!document.getElementById) { return true; }
	var ctrl = source.controltovalidate;
	var obj = new Array();
	var i = 0; 
	while (document.getElementById(ctrl + "_" + i)) {
		obj.push(document.getElementById(ctrl + "_" + i));
		i++;
	} 
	if (obj.length == 0) {
		args.IsValid = true;
	} else {
		args.IsValid = false;
		for (i=0; i < obj.length; i++) {
			if (obj[i].checked) {
				args.IsValid = true;
				break;
			}
		} 
	}
	//ew_SetFocus(source.controltovalidate);
	return;
}

function ew_FileHasValue(source, args) {
	if (!document.getElementById) { return true; }
	var radio = source.controltovalidate;
	var obj = new Array();
	if (document.getElementById(radio + "_radio_0") && document.getElementById(radio + "_radio_0").checked) { // keep
			args.IsValid = true;
			return true;
	} else if (document.getElementById(radio + "_radio_1") && document.getElementById(radio + "_radio_1").checked) { // remove
			args.IsValid = true;
			return true;
	} else { // replace
		if (args.Value.length > 0) {
			args.IsValid = true;
		} else {
			args.IsValid = false;
		}
	}
	return;
}

// Date (mm/dd/yyyy)
function ew_CheckUSDate(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	
	var isplit = object_value.indexOf(ew_DateSep);

	if (isplit == -1 || isplit == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	}

	var sMonth = object_value.substring(0, isplit);

	if (sMonth.length == 0) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	}
	var isplit = object_value.indexOf(ew_DateSep, isplit + 1);

	if (isplit == -1 || (isplit + 1 ) == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	}
	var sDay = object_value.substring((sMonth.length + 1), isplit);

	if (sDay.length == 0) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	}
	
	var isep = object_value.indexOf(' ', isplit + 1);
	if (isep == -1) {
		sYear = object_value.substring(isplit + 1);
	} else {
		sYear = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime2(sTime)) {
			args.IsValid = false;
			//ew_SetFocus(source.controltovalidate);
			return;
		}
	}
	
	if (!ew_CheckInt(sMonth)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else if (!ew_NumberRange(sMonth, 1, 12)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else if (!ew_CheckInt(sYear)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else if (!ew_NumberRange(sYear, 0, 9999)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else if (!ew_CheckInt(sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else if (!ew_CheckDay(sYear, sMonth, sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return;
	} else {
		args.IsValid =  true;
		return;
	}
}

	
// Date (yyyy/mm/dd)
function ew_CheckDate(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return;
	}
	var isplit = object_value.indexOf(ew_DateSep);

	if (isplit == -1 || isplit == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}

	var sYear = object_value.substring(0, isplit);

	isplit = object_value.indexOf(ew_DateSep, isplit + 1);

	if (isplit == -1 || (isplit + 1 ) == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	var sMonth = object_value.substring((sYear.length + 1), isplit);
	
	if (sMonth.length == 0) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
		
	isep = object_value.indexOf(' ', isplit + 1);
	if (isep == -1) {
		sDay = object_value.substring(isplit + 1);
	} else {
		sDay = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime2(sTime)) {
			args.IsValid = false;
			//ew_SetFocus(source.controltovalidate);
			return false;
		}
	}
	
	if (sDay.length == 0) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckInt(sMonth)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sMonth, 1, 12)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckInt(sYear)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sYear, 0, 9999)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckInt(sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckDay(sYear, sMonth, sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else {
		args.IsValid =  true;
		return true;
	}
}


// Date (dd/mm/yyyy)
function ew_CheckEuroDate(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}

	var isplit = object_value.indexOf(ew_DateSep);

	if (isplit == -1 || isplit == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	var sDay = object_value.substring(0, isplit);

	var monthSplit = isplit + 1;

	isplit = object_value.indexOf(ew_DateSep, monthSplit);

	if (isplit == -1 ||  (isplit + 1 )  == args.Value.length) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	var sMonth = object_value.substring((sDay.length + 1), isplit);

	var isep = object_value.indexOf(' ', isplit + 1);
	if (isep == -1) {
		sYear = object_value.substring(isplit + 1);
	} else {
		sYear = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime2(sTime)) {
			args.IsValid = false;
			//ew_SetFocus(source.controltovalidate);
			return false;
		}
	}

	if (!ew_CheckInt(sMonth)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sMonth, 1, 12)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckInt(sYear)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sYear, 0, null)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckInt(sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_CheckDay(sYear, sMonth, sDay)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else {
		args.IsValid =  true;
		return true;
	}
}



function ew_CheckDay(checkYear, checkMonth, checkDay) {

	var maxDay = 31;

	if (checkMonth == 4 || checkMonth == 6 || checkMonth == 9 || checkMonth == 11)
		maxDay = 30;
	else if (checkMonth == 2) {
		if (checkYear % 4 > 0)
			maxDay =28;
		else if ((checkYear % 100 == 0) && (checkYear % 400 > 0))
			maxDay = 28;
		else
			maxDay = 29;
	}

	return ew_NumberRange(checkDay, 1, maxDay); 
}


function ew_CheckInteger(source, args) {
	args.IsValid = ew_CheckInt(args.Value);
	if (!args.IsValid) {
		//ew_SetFocus(source.controltovalidate);
	}
}


function ew_CheckInt(object_value) {
	if (object_value.length == 0)
		return true;
	
	var decimal_format = ew_CurrencyDecimalSeparator;
	var check_char;

	check_char = object_value.indexOf(decimal_format);
	return (check_char < 1)? ew_CheckNum(object_value): false;
}

function ew_NumberRange(object_value, min_value, max_value) {
	if (min_value && object_value < min_value) {
		return false;
	}

	if (max_value && object_value > max_value) {
		return false;
	}
	
	return true;
}


function ew_CheckNumber(source,args) {
	if (args.Value.length == 0) {
		args.IsValid = true;
		return true;
	}

	var start_format = " " + ew_CurrencyDecimalSeparator + "+-0123456789";
	var number_format = " " + ew_CurrencyDecimalSeparator + "0123456789";
	var check_char;
	var decimal = false;
	var trailing_blank = false;
	var digits = false;
	var object_value = args.Value;
	check_char = start_format.indexOf(object_value.charAt(0))
	if (check_char == 1) {
		decimal = true;
	} else if (check_char < 1) {
		args.IsValid = false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
		
	for (var i = 1; i < args.Value.length; i++) {
		check_char = number_format.indexOf(object_value.charAt(i))
		if (check_char < 0) {
			args.IsValid = false;
			//ew_SetFocus(source.controltovalidate);
			return false;
		} else if (check_char == 1) {
			if (decimal) {
				args.IsValid = false;
				//ew_SetFocus(source.controltovalidate);
				return false;
			} else {
				decimal = true;
			}
		} else if (check_char == 0) {
			if (decimal || digits)    
				trailing_blank = true;
		} else if (trailing_blank) {
			args.IsValid = false;
			//ew_SetFocus(source.controltovalidate);
			return false;
		} else {
			digits = true;
		}
	}    

	args.IsValid = true;
	return true;
}
	
function ew_CheckNum(object_value) { 
	if (object_value.length == 0) {
		return true;
	}

	var start_format = " " + ew_CurrencyDecimalSeparator + "+-0123456789";
	var number_format = " " + ew_CurrencyDecimalSeparator + "0123456789";
	var check_char;
	var decimal = false;
	var trailing_blank = false;
	var digits = false;
	
	check_char = start_format.indexOf(object_value.charAt(0))
	if (check_char == 1) {
		decimal = true;
	} else if (check_char < 1) {
		return false;
	}
		
	for (var i = 1; i < object_value.length; i++) {
		check_char = number_format.indexOf(object_value.charAt(i))
		if (check_char < 0) {
			return false;
		} else if (check_char == 1) {
			if (decimal) {
				return false;
			} else {		
				decimal = true;
			}
		} else if (check_char == 0) {
			if (decimal || digits)    
				trailing_blank = true;
		} else if (trailing_blank) {
			return false;
		} else {
			digits = true;
		}
	}    

	return true;
}
	

function ew_CheckTime(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	var isplit = object_value.indexOf(':');

	if (isplit == -1 || isplit == args.Value.length) {
		args.IsValid =  false;
		return false;
	}
	var sHour = object_value.substring(0, isplit);
	var iminute = object_value.indexOf(':', isplit + 1);

	if (iminute == -1 || iminute == args.Value.length) {
		var sMin = object_value.substring((sHour.length + 1));
	} else {
		var sMin = object_value.substring((sHour.length + 1), iminute);
	}

	if (!ew_CheckInt(sHour)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sHour, 0, 23)) {
		args.IsValid =  false;
		return false;
	} 
	
	if (!ew_CheckInt(sMin)){
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else if (!ew_NumberRange(sMin, 0, 59)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	
	if (iminute != -1)
	{
		var sSec = object_value.substring(iminute + 1);

		if (!ew_CheckInt(sSec)) {
			args.IsValid =  false;
			//ew_SetFocus(source.controltovalidate);
			return false;
		} else	if (!ew_NumberRange(sSec, 0, 59)) {
			args.IsValid =  false;    
			//ew_SetFocus(source.controltovalidate);
			return false;
		}
	}
	
	args.IsValid =  true;
	return true;
}
	
function ew_CheckTime2(value) {
	if (value.length == 0) {
		return true;
	}
	var isplit = value.indexOf(':');

	if (isplit == -1 || isplit == value.length) {
		return false;
	}
	var sHour = value.substring(0, isplit);
	var iminute = value.indexOf(':', isplit + 1);

	if (iminute == -1 || iminute == value.length) {
		var sMin = value.substring((sHour.length + 1));
	} else {
		var sMin = value.substring((sHour.length + 1), iminute);
	}

	if (!ew_CheckInt(sHour)) {
		return false;
	} else if (!ew_NumberRange(sHour, 0, 23)) {
		return false;
	} 
	
	if (!ew_CheckInt(sMin)){
		return false;
	} else if (!ew_NumberRange(sMin, 0, 59)) {
		return false;
	}
	
	if (iminute != -1)
	{
		var sSec = value.substring(iminute + 1);

		if (!ew_CheckInt(sSec)) {
			return false;
		} else	if (!ew_NumberRange(sSec, 0, 59)) {
			return false;
		}
	}
	
	return true;
}


function ew_CheckPhone(source, args) {
	var object_value = args.Value;
	var tempint; 
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	if (args.Value.length != 12) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckNum(object_value.substring(0,3))) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else {
		tempint = eval(object_value.substring(0,3));
	}	
	if (!ew_NumberRange(tempint, 100, 1000)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(3) != "-" && object_value.charAt(3) != " ") {
		args.IsValid =  false
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckNum(object_value.substring(4,7))) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else {
		tempint = eval(object_value.substring(4,7))
	}
	if (!ew_NumberRange(tempint, 100, 1000)){
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(7) != "-" && object_value.charAt(7) != " ") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(8) == "-" || object_value.charAt(8) == "+") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	} else {
		args.IsValid =  (ew_CheckInt(object_value.substring(8,12)));
		return args.IsValid
	}
}


function ew_CheckZip(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	if (args.Value.length != 5 && args.Value.length != 10) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(0) == "-" || object_value.charAt(0) == "+") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckInt(object_value.substring(0,5))) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (args.Value.length == 5) {
		args.IsValid =  true;
		return true;
	}
	if (object_value.charAt(5) != "-" && object_value.charAt(5) != " ") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(6) == "-" || object_value.charAt(6) == "+") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	args.IsValid =  (ew_CheckInt(object_value.substring(6,10)));
	if (!args.IsValid)
		//ew_SetFocus(source.controltovalidate);
	return args.IsValid
}


function ew_CheckCreditCard(source, args) {
	var object_value = args.Value;
	var white_space = " -";
	var creditcard_string="";
	var check_char;
	var strlen = object_value.length
	if (strlen == 0) {
		args.IsValid = true;
		return true;
	}
	for (var i = 0; i < strlen; i++) {
		check_char = white_space.indexOf(object_value.charAt(i))
		if (check_char < 0)
			creditcard_string += object_value.substring(i, (i + 1));
	}    

	if (creditcard_string.length == 0) {
		args.IsValid =  false;     
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (creditcard_string.charAt(0) == "+") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckInt(creditcard_string)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}

	var doubledigit = (creditcard_string.length % 2 == 1) ? false : true;
	var checkdigit = 0;
	var tempdigit;

	for (var i = 0; i < creditcard_string.length; i++) {
		tempdigit = eval(creditcard_string.charAt(i))
		if (doubledigit) {
			tempdigit *= 2;
			checkdigit += (tempdigit % 10);

			if ((tempdigit / 10) >= 1.0) {
				checkdigit++;
			}

			doubledigit = false;
		} else {
			checkdigit += tempdigit;
			doubledigit = true;
		}
	}    
	args.IsValid = (checkdigit % 10 == 0) ? true : false;
	if (!args.IsValid)
		//ew_SetFocus(source.controltovalidate);
	return args.IsValid;
}


function ew_CheckSSC(source, args) {
	var object_value = args.Value;
	var white_space = " -+.";
	var ssc_string="";
	var check_char;

	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	if (args.Value.length != 11) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(3) != "-" && object_value.charAt(3) != " ") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (object_value.charAt(6) != "-" && object_value.charAt(6) != " ") {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	for (var i = 0; i < args.Value.length; i++) {
		check_char = white_space.indexOf(object_value.charAt(i))
		if (check_char < 0)
			ssc_string += object_value.substring(i, (i + 1));
	}    

	if (ssc_string.length != 9) {
		args.IsValid =  false;     
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	if (!ew_CheckInt(ssc_string)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	args.IsValid =  true;
	return true;
}
	

function ew_CheckEmail(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	if(!(object_value.indexOf("@") > -1 && object_value.indexOf(".") > -1)) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	args.IsValid =  true;
	return true;
}
	

// GUID xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
function ew_CheckGUID(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid =  true;
		return true;
	}
	if (args.Value.length != 36) {
		args.IsValid =  false;
		//ew_SetFocus(source.controltovalidate);
		return false;
	}
	/*
	if (object_value.charAt(0)!="{"){
		args.IsValid =  false;
		return false;
	}
	if (object_value.charAt(37)!="}"){
		args.IsValid =  false;    
		return false;
	}
	*/
	var hex_format = "0123456789abcdefABCDEF";
	var check_char;    

	for (var i = 0; i < 36; i++) {        
		if ((i==8)||(i==13)||(i==18)||(i==23)) {
			if (object_value.charAt(i)!="-") {
				args.IsValid =  false;
				//ew_SetFocus(source.controltovalidate);
				return false;
			}
		} else {
			check_char = hex_format.indexOf(object_value.charAt(i));
			if (check_char < 0) {
				args.IsValid =  false;
				//ew_SetFocus(source.controltovalidate);
				return false;
			}
		}
	}
	args.IsValid = true;
	return true;
}

function ew_CheckFileType(source, args) {
	var object_value = args.Value;
	if (args.Value.length == 0) {
		args.IsValid = true;
		return true;
	}
	var fileTypes = ew_UploadAllowedFileExt.split(",");
	var ext = object_value.substring(object_value.lastIndexOf(".") + 1, object_value.length).toLowerCase();
	for (var i = 0; i < fileTypes.length; i++) {
		if (fileTypes[i] == ext) {
			args.IsValid = true;
			return true;
		}
	}
	args.IsValid = false;
	//ew_SetFocus(source.controltovalidate);
	return false;
}

function ew_IsHiddenTextArea(input_object) {
	return (input_object && input_object.type && input_object.type == "textarea" &&
		input_object.style && input_object.style.display && input_object.style.display == "none");
}

function ew_SetFocus(objectId) {
    var input_object = document.getElementById(objectId);
	if (!input_object || !input_object.type)
		return;
	var type = input_object.type;
	if (type == "radio" || type == "checkbox") {
		if (input_object[0])
			input_object[0].focus();
		else
			input_object.focus();
	} else if (!ew_IsHiddenTextArea(input_object)) {
		input_object.focus();
	}
	if (type == "text" || type == "password" || type == "textarea" || type == "file") {
		if(!ew_IsHiddenTextArea(input_object))
			input_object.select();
	}
}

// Get image width/height
function ew_GetImageSize(file_object, width_object, height_object) {
	if (navigator.appVersion.indexOf("MSIE") != -1) {
		myimage = new Image();
		myimage.onload = function () { width_object.value = myimage.width; height_object.value = myimage.height; }        
		myimage.src = file_object.value;
	}
}

// Get Netscape Version
function ew_GetNNVersionNumber() {
	if (navigator.appName == "Netscape") {
		var appVer = parseFloat(navigator.appVersion);
		if (appVer < 5) {
			return appVer;
		} else {
			if (typeof navigator.vendorSub != "undefined")
				return parseFloat(navigator.vendorSub);
		}
	}
	return 0;
}

// Get Ctrl key for multiple column sort
function ew_Sort(e,form, tablename) {
	var ctrlPressed = 0;    
	if (parseInt(navigator.appVersion) > 3) {
		if (navigator.appName == "Netscape") {
			if (ew_GetNNVersionNumber() >= 6)
				ctrlPressed = e.ctrlKey;
			else
				ctrlPressed = ((e.modifiers+32).toString(2).substring(3,6).charAt(1)=="1");            
		} else {
			ctrlPressed = event.ctrlKey;
		}
		var ctrl = document.getElementById(tablename + "_Ctrl")
		if (ctrlPressed) {
			if (ctrl) ctrl.value = 'yes';
		} else {
			if (ctrl) ctrl.value = 'no';
		}
	}
	return true;
}


// Dynamic Highlight
// Set mouse over color
function ew_MouseOver(row) {
	row.mover = true; // mouse over
	if (!(row.selected || row.master)) {
		if (usecss)
	    	row.className = rowmoverclass;
		else
			row.style.backgroundColor = rowmovercolor;
	}
}

// Set mouse out color
function ew_MouseOut(row, firstrowoffset) {
	row.mover = false; // mouse out
	if (!(row.selected || row.master)) {
		ew_SetColor(row, firstrowoffset);
	}
}

// Set row color
function ew_SetColor(row, firstrowoffset) {
	if (row.selected) {
		if (usecss)
			row.className = rowselectedclass;
		else
			row.style.backgroundColor = rowselectedcolor;
	}
	else if (row.master) {
		if (usecss)
			row.classname = rowmasterclass;
		else
			row.style.backgroundColor = rowmastercolor;
	}
	else if (row.edit) {
		if (usecss)
			row.className = roweditclass;
		else
			row.style.backgroundColor = roweditcolor;
	}
	else if ((row.rowIndex-firstrowoffset)%2) {
		if (usecss)
			row.className = rowaltclass;
		else
			row.style.backgroundColor = rowaltcolor;
	}
	else {
		if (usecss)
			row.className = rowclass;
		else
			row.style.backgroundColor = rowcolor;
	}
}

// Set selected row color
function ew_Click(row, tablename, firstrowoffset, lastrowoffset) {
	if (row.deleteclicked)
		row.deleteclicked = false; // reset delete button/checkbox clicked
	else {
		var bselected = row.selected;
		ew_ClearSelected(tablename, firstrowoffset, lastrowoffset); // clear all other selected rows
		if (!row.deleterow) row.selected = !bselected; // toggle
		ew_SetColor(row, firstrowoffset);		
	}
}

// Clear selected rows color
function ew_ClearSelected(tablename, firstrowoffset, lastrowoffset) {
	var table = document.getElementById(tablename);
	for (var i = firstrowoffset; i < table.rows.length - lastrowoffset; i++) {
		var thisrow = table.rows[i];
		if (thisrow.selected && !thisrow.deleterow) {
			thisrow.selected = false;
			ew_SetColor(thisrow, firstrowoffset);
		}
	}
}

// Clear all row delete status
function ew_ClearDelete(tablename, firstrowoffset, lastrowoffset) {
	var table = document.getElementById(tablename);
	for (var i = firstrowoffset; i < table.rows.length - lastrowoffset; i++) {
		var thisrow = table.rows[i];
		thisrow.deleterow = false;
	}
}

// Click all delete button
function ew_ClickAll(chkbox, tablename, firstrowoffset, lastrowoffset) {
	var table = document.getElementById(tablename);
	for (var i = firstrowoffset; i < table.rows.length - lastrowoffset; i++) {
		var thisrow = table.rows[i];
		thisrow.selected = chkbox.checked;
		thisrow.deleterow = chkbox.checked;
		ew_SetColor(thisrow, firstrowoffset);
	}
}

// Click single delete link
function ew_ClickDelete(tablename, firstrowoffset, lastrowoffset) {
	ew_ClearSelected(tablename, firstrowoffset, lastrowoffset);
	var table = document.getElementById(tablename);
	for (var i = firstrowoffset; i < table.rows.length - lastrowoffset; i++) {
		var thisrow = table.rows[i];
		if (thisrow.mover) {
			thisrow.deleteclicked = true;
			thisrow.deleterow = true;
			thisrow.selected = true;
			ew_SetColor(thisrow, firstrowoffset);
			break;
		}
	}
}

// Click multi delete checkbox
function ew_ClickMultiDelete(chkbox, tablename, firstrowoffset, lastrowoffset) {
	ew_ClearSelected(tablename, firstrowoffset, lastrowoffset);
	var table = document.getElementById(tablename);
	for (var i = firstrowoffset; i < table.rows.length - lastrowoffset; i++) {
		var thisrow = table.rows[i];
		if (thisrow.mover) {
			thisrow.deleteclicked = true;
			thisrow.deleterow = chkbox.checked;
			thisrow.selected = chkbox.checked;
			ew_SetColor(thisrow, firstrowoffset);
			break;
		}
	}
}


function ew_SetChecked(id) {
	if (document.getElementById(id + '_2')) {
		document.getElementById(id + '_2').checked=true;
	}
}

function ew_SelectAll(sCtrl, elem, tablename, firstrowoffset, lastrowoffset, changeColor) {
	for (i=0; i<elem.form.elements.length; i++) {
		var elm = elem.form.elements[i];
		if (elm.type == "checkbox" && elm.name.indexOf(sCtrl) > 0 && elm.id.indexOf(tablename) >= 0) {
			elm.checked = elem.checked;
			if (ew_ClickAll && changeColor) ew_ClickAll(elm, tablename, firstrowoffset, lastrowoffset);
		}
	}
}

// Inline Delete Confirmation
function ew_ConfirmMultiDelete(msg,clientId, tablename, firstrowoffset, lastrowoffset){
	if (confirm(msg)) {
		__doPostBack(clientId, '');
	} else { 
		ew_ClearDelete(tablename, firstrowoffset, lastrowoffset); 
	}
	return false;
}

function ew_ConfirmDelete(msg) {
	return (confirm(msg));
}

// v3.0
var ew = window.ew || {};

ew.Controls = {};

function ew_SetControlAsChecked(id) {
	var elem = document.getElementById(id);	
	if (elem && elem.checked != "undefined") 
		elem.checked = true;
}

