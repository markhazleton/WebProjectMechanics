// JavaScript for ASP.NET Maker 7+
// (C)2009 e.World Technology Ltd.
// Page properties

var ewDialog;
var ewUtil = YAHOO.util;
var ewDom = YAHOO.util.Dom;
var ewEvent = YAHOO.util.Event;
var ewLang = YAHOO.lang;
var ewConnect = YAHOO.util.Connect;
var ewWidget = YAHOO.widget;
var EW_TABLE_CLASS = "ewTable";
var EW_TABLE_ROW_CLASSNAME = "ewTableRow";
var EW_TABLE_ALT_ROW_CLASSNAME = "ewTableAltRow";
var EW_ITEM_TEMPLATE_CLASSNAME = "ewTemplate";
var EW_ITEM_TABLE_CLASSNAME = "ewItemTable";
var EW_REPORT_CONTAINER_ID = "ewContainer";  

// ew_Page class
// Page Object
function ew_Page(name) {
	this.Name = name;
	this.PageID = "";

	// search highlight properties
	this.ShowHighlightText = "Show highlight";
	this.HideHighlightText = "Hide highlight";
	this.SearchPanel = name + "_SearchPanel";
	this.SearchButton = name + "_SearchImage";

	// validate function
	this.ValidateRequired = true;

	// multi page properties
	this.MultiPage = null;
	this.TabView = null;
}

// Include another client script
function ew_ClientScriptInclude(path) {
	document.write('<' + 'script');
	document.write(' language="JavaScript"');
	document.write(' type="text/javascript"');
	document.write(' src="' + path + '">');
	document.write('</' + 'script' + '>');
}

//  Check if element value changed
function ew_ValueChanged(fobj, infix, fld) {
	var nelm = fobj.elements["x" + infix + "_" + fld];
	var oelm = fobj.elements["o" + infix + "_" + fld];
	if (oelm && nelm && (ew_GetValue(oelm) == ew_GetValue(nelm)))
		return false;
	else
		return true;
}

// Get form element value
function ew_GetValue(obj) {
	if (!obj)
		return "";
	if (!ew_HasValue(obj))
		return "";
	var type = (!obj.type && obj[0]) ? obj[0].type : obj.type;
	if (type == "text" || type == "password" || type == "textarea" ||
		type == "file" || type == "hidden") {
		return (obj.value);
	} else if (type == "select-one") {
		return (obj.options[obj.selectedIndex].value);
	} else if (type == "select-multiple") {
		var selwrk = "";
		for (var i=0; i < obj.options.length; i++) {
			if (obj.options[i].selected) {
				if (selwrk != "") selwrk += ", ";
				selwrk += obj.options[i].value;
			}
		}
		return selwrk;
	} else if (type == "checkbox") {
		if (obj[0]) {
			var chkwrk = "";
			for (var i=0; i < obj.length; i++) {
				if (obj[i].checked) {
					if (chkwrk != "") chkwrk += ", ";
					chkwrk += obj[i].value;
				}
			}
			return chkwrk;
		}
	} else if (type == "radio") {
		if (obj[0]) {
			var rdowrk = "";
			for (var i=0; i < obj.length; i++) {
				if (obj[i].checked) {
					if (rdowrk != "") rdowrk += ", ";
					rdowrk += obj[i].value;
				}
			}
			return rdowrk;
		} else {
			return obj.value;
		}
	}
	return "";
}

// Handle search operator changed
function ew_SrchOprChanged(id) {
	var elem = document.getElementById(id);
	var f = elem.form;
	var isBetween = (elem.options[elem.selectedIndex].value == "BETWEEN");
	var arEl;
	arEl = document.getElementsByName("btw0_" + id.substr(2));
	for (var i=0; i < arEl.length; i++)
		arEl[i].style.display = (isBetween) ? "none" : "";
	arEl = document.getElementsByName("btw1_" + id.substr(2));
	for (var i=0; i < arEl.length; i++) {
		arEl[i].style.display = (isBetween) ? "" : "none";
		var arChildren = ewDom.getChildrenBy(arEl[i], function(El) { return typeof(El.disabled) != "undefined"; });
		for (var j=0; j < arChildren.length; j++)
			arChildren[j].disabled = !isBetween; 
	}
}

// DHTML editor
function ew_DHTMLEditor(name, f) {
	this.name = name;
	this.create = (f) ? f : function() { this.active = true; };
	this.editor = null;
	this.active = false;
}

// Create DHTML editor
function ew_CreateEditor(name) {
	if (typeof ew_DHTMLEditors == 'undefined')
		return;
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

// Submit form
function ew_SubmitForm(p, fobj) {
	if (typeof ew_UpdateTextArea == 'function')
		ew_UpdateTextArea();
	var s = (p.ValidateRequired) ? p.ValidateForm(fobj) : true;
	if (s) fobj.submit();
}

// Submit search
function ew_SubmitSearch(p, fobj) {
	if (typeof ew_UpdateTextArea == 'function')
		ew_UpdateTextArea();
	var s = (p.ValidateRequired) ? p.ValidateSearch(fobj) : true;
	if (s) fobj.submit();
}

// Remove spaces
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

// Check if hidden text area
function ew_IsHiddenTextArea(input_object) {
	return (input_object && input_object.type && input_object.type == "textarea" &&
		input_object.style && input_object.style.display &&
		input_object.style.display == "none");
}

// Set focus
function ew_SetFocus(input_object) {
	if (!input_object)
		return;	
	if (!input_object.type && input_object[0]) {	
		for (var i=0; i < input_object.length; i++) {
			if (input_object[i].value != "{value}") {
				input_object = input_object[i];
				break;
			}
		}
	}
	if (!input_object || !input_object.type)
		return;
	var type = input_object.type;
	if (type == "textarea") {
		if (ew_IsHiddenTextArea(input_object)) { // DHTML editor
			if (typeof ew_FocusDHTMLEditor == "function")
				setTimeout("ew_FocusDHTMLEditor('" + input_object.id + "')", 500);
		} else { // textarea
			input_object.focus();
			input_object.select();
		}	
		return;
	} else if (type == "hidden") {
		var asEl = ew_GetElements("sv_" + input_object.id); // Auto-Suggest
		if (asEl && asEl.type && asEl.type == "text") {
			asEl.focus();
			asEl.select();
		}
		return; 
	}
	input_object.focus();
	if (type == "text" || type == "password" || type == "file")
		input_object.select();
}

// Show error message
function ew_OnError(page, input_object, error_message) {
	alert(error_message); 
	if (page && page.MultiPage) // check if multi-page
		page.MultiPage.GotoPageByElement(input_object);
	ew_SetFocus(input_object);
	return false;
}

// Check if object has value
function ew_HasValue(obj) {
	if (!obj)
		return true;
	var type = (!obj.type && obj[0]) ? obj[0].type : obj.type;
	if (type == "text" || type == "password" || type == "textarea" ||
		type == "file" || type == "hidden") {
		return (obj.value.length != 0);
	} else if (type == "select-one") {
		return (obj.selectedIndex > 0);
	} else if (type == "select-multiple") {
		return (obj.selectedIndex > -1);
	} else if (type == "checkbox") {
		if (obj[0]) {
			for (var i=0; i < obj.length; i++) {
				if (obj[i].value != "{value}" && obj[i].checked)
				return true;
			}
			return false;
		}
	} else if (type == "radio") {
		if (obj[0]) {
			for (var i=0; i < obj.length; i++) {
				if (obj[i].value != "{value}" && obj[i].checked)
				return true;
			}
			return false;
		} else {
			return (obj.value != "{value}" && obj.checked);
		}
	}
	return true;
}

// Get image width/height
function ew_GetImageSize(file_object, width_object, height_object) {
	if (document.all)	{ // IE
		myimage = new Image();
		myimage.onload = function() {
			width_object.value = myimage.width;
			height_object.value = myimage.height;
		}		
		myimage.src = file_object.value;
	}
}

// Get Ctrl key for multiple column sort
function ew_Sort(e, url, type) {
	var newUrl = url
	if (type == 2 && e.ctrlKey)
		newUrl +=	"&ctrl=1";
	location = newUrl;
	return true;
}

// Confirm message
function ew_Confirm(msg) {
	return confirm(msg);
}

// Confirm Delete Message
function ew_ConfirmDelete(msg, el) {
	var del = confirm(msg);
	if (!del)
		ew_ClearDelete(el); // Clear delete status
	return del;
}

// Check if any key selected
function ew_KeySelected(f) {
	if (!f.key_m) return false;
	if (f.key_m[0]) {
		for (var i=0; i<f.key_m.length; i++)
			if (f.key_m[i].checked) return true;
	} else {
		return f.key_m.checked;
	}
	return false;
}

// Select all related checkboxes
function ew_SelectAll(obj)	{
	var f = obj.form;
	var i, elm
	for (i=0; i<f.elements.length; i++) {
		elm = f.elements[i];
		if (elm.type == "checkbox" && elm.name.substr(0, obj.name.length+1) == obj.name + "_") {
			elm.checked = obj.checked;
		}
	}
}

// Update selected checkbox
function ew_UpdateSelected(f) {
	var pfx = "u";
	for (i=0; i<f.elements.length; i++) {
		var elm = f.elements[i];
		if (elm.type == "checkbox" && elm.name.substr(0, pfx.length+1) == pfx + "_") {
			if (elm.checked) return true;
		}
	}
	return false;
}

// Set mouse over color
function ew_MouseOver(ev, row) {
	var tbl = ewDom.getAncestorByClassName(row, EW_TABLE_CLASS);
	row.mouseover = true; // Mouse over
	if (typeof(row.oClassName) == "undefined")
		row.oClassName = row.className;
	if (typeof(row.oCssText) == "undefined")
		row.oCssText = row.style.cssText;
	if (!row.selected) {
		row.className = tbl.getAttribute("rowhighlightclass");
		row.style.cssText = "";
	}
}

// Set mouse out color
function ew_MouseOut(ev, row) {
	row.mouseover = false; // Mouse out
	if (!row.selected)
		ew_SetColor(row);
}

// Set row color
function ew_SetColor(row) {
	var tbl = ewDom.getAncestorByClassName(row, EW_TABLE_CLASS);
	if (row.selected) {
		if (typeof(row.oClassName) == "undefined")
			row.oClassName = row.className;
		if (typeof(row.oCssText) == "undefined")
			row.oCssText = row.style.cssText;
		row.className = tbl.getAttribute("rowselectclass");
	} else if (row.edit) {
		row.className = tbl.getAttribute("roweditclass");
	} else {
		if (typeof(row.oClassName) != "undefined")
			row.className = row.oClassName;
		if (typeof(row.oCssText) != "undefined")
			row.style.cssText = row.oCssText;
	}
}

// Set selected row color
function ew_Click(ev, row) {
	var tbl = ewDom.getAncestorByClassName(row, EW_TABLE_CLASS);
	if (row.deleteclicked) {
		row.deleteclicked = false; // Reset delete button/checkbox clicked
	} else {
		var bselected = row.selected;
		ew_ClearSelected(tbl); // Clear all other selected rows
		if (!row.deleterow)
			row.selected = !bselected; // Toggle
		ew_SetColor(row);
	}
}

// Clear selected rows color
function ew_ClearSelected(tbl) {
	var row;
	var cnt = tbl.rows.length;	
	for (var i=0; i<cnt; i++) {
		row = tbl.rows[i];
		if (row.selected && !row.deleterow) {
			row.selected = false;
			ew_SetColor(row);
		}
	}
}

// Clear all row delete status
function ew_ClearDelete(el) {
	var row;
	var tbl = ewDom.getAncestorByClassName(el, EW_TABLE_CLASS);
	var cnt = tbl.rows.length;
	for (var i=0; i<cnt; i++) {
		row = tbl.rows[i];
		row.deleterow = false;
	}
}

// Click all delete button
function ew_ClickAll(chkbox) {
	var row;
	var tbl = ewDom.getAncestorByClassName(chkbox, EW_TABLE_CLASS);
	var cnt = tbl.tBodies[0].rows.length;
	for (var i=0; i<cnt; i++) {
		row = tbl.tBodies[0].rows[i];
		row.selected = chkbox.checked;
		row.deleterow = chkbox.checked;
		ew_SetColor(row);
	}
}

// Click single delete link
function ew_ClickDelete(a) {
    var row;
    var tbl = ewDom.getAncestorByClassName(a, EW_TABLE_CLASS);
    ew_ClearSelected(tbl);
    var cnt = tbl.rows.length;
    for (var i=0; i<cnt; i++) {
        row = tbl.rows[i];
        if (row.mouseover) {
            row.deleteclicked = true;
            row.deleterow = true;
            row.selected = true;
            ew_SetColor(row);
            break;
        }
    }
}

// Click multiple checkbox
function ew_ClickMultiCheckbox(chkbox) {
	var row;
	var tbl = ewDom.getAncestorByClassName(chkbox, EW_TABLE_CLASS);
	ew_ClearSelected(tbl);
	var cnt = tbl.rows.length;
	for (var i=0; i<cnt; i++) {
		row = tbl.rows[i];
		if (row.mouseover) {
			row.deleteclicked = true;
			row.deleterow = chkbox.checked;
			row.selected = chkbox.checked;
			ew_SetColor(row);
			break;
		}
	}
}

// Get main form
function ew_GetForm(id) {
	var f = document.getElementById(id);
	if (!f) f = document.getElementById("aspnetForm"); 
	return f; 
}

// Setup table
function ew_SetupTable(tbl) {
	if (!tbl)
		return;
	var isReport = ewDom.getAncestorBy(tbl, function(node) { return (node.id == EW_REPORT_CONTAINER_ID)});
	var r, cnt;
	cnt = tbl.rows.length;
	for (var i=0; i < cnt; i++) {
		r = tbl.rows[i];
		r.cells[r.cells.length-1].style.borderRight = "0"; // last column
	}
	if (cnt > 0) {
		r = tbl.rows[tbl.rows.length-1]; // last row
		cnt = r.cells.length;
		for (var i=0; i < cnt; i++)
			r.cells[i].style.borderBottom = "0";
	}
	if (!isReport) {
		cnt = tbl.tBodies[0].rows.length;
		for (var i=0; i < cnt; i++) {
			r = tbl.tBodies[0].rows[i];
			if (r.className == "")
				r.className = (i % 2 == 0) ? EW_TABLE_ROW_CLASSNAME : EW_TABLE_ALT_ROW_CLASSNAME; // row color
		}
	}
}

// Toggle highlight
function ew_ToggleHighlight(p, lnk, name) {
	if (!lnk || !document.getElementsByName)
		return;
	var elems = document.getElementsByName(name);
	var i, el;
	for (i=0; i<elems.length; i++) {
		elem = elems[i];
		elem.className = (elem.className == "") ? "ewHighlightSearch" : "";
	}
	lnk.innerHTML = (lnk.innerHTML == p.HideHighlightText) ? p.ShowHighlightText : p.HideHighlightText;
}

// Html encode text
function ew_HtmlEncode(text) {
	var str = text;
	str = str.replace(/&/g, '&amp');
	str = str.replace(/\"/g, '&quot;');
	str = str.replace(/</g, '&lt;');
	str = str.replace(/>/g, '&gt;'); 
	return str;
}

// Get element from form
function ew_GetFormElement(f, name) {
	for(var i=0; i<f.elements.length; i++) {
		if(f.elements[i].name == name) {
			return f.elements[i];
		}
	}
}

// Extended basic search clear form
function ew_ClearForm(objForm){
	with (objForm) {
		for (var i=0; i<elements.length; i++){
			var tmpObj = eval(elements[i]);
			if (tmpObj.type == "checkbox" || tmpObj.type == "radio"){
				tmpObj.checked = false;
			} else if (tmpObj.type == "select-one"){
				tmpObj.selectedIndex = 0;
			} else if (tmpObj.type == "select-multiple") {
				for (var j=0; j<tmpObj.options.length; j++)
					tmpObj.options[j].selected = false;
            } else if (tmpObj.type == "text" || tmpObj.type == "textarea"){
				tmpObj.value = "";
			}
		}
	}
}

// Toggle search panel
function ew_ToggleSearchPanel(p) {
	if (!document.getElementById)
		return;
	var img = document.getElementById(p.SearchButton);
	var p = document.getElementById(p.SearchPanel);
	if (!p || !img)
		return;
	if (p.style.display == "") {
		p.style.display = "none";
		if (img.tagName == "IMG")
			img.src = "images/expand.gif";
	} else {
		p.style.display = "";
		if (img.tagName == "IMG")
			img.src = "images/collapse.gif";
	}
}

// Functions for multi page
function ew_MultiPage() {
	if (!(document.getElementById || document.all))
		return;		
	this.PageIndex = 1;
	this.MaxPageIndex = 0;
	this.MinPageIndex = 0;
	this.Elements = new Array();
	this.AddElement = ew_MultiPageAddElement;
	this.Init = ew_InitMultiPage;
	this.ShowPage = ew_ShowPage;
	this.EnableButtons = ew_EnableButtons;
	this.GetPageIndexByElementId = ew_GetPageIndexByElementId;
	this.GotoPageByIndex = ew_GotoPageByIndex;
	this.GotoPageByElement = ew_GotoPageByElement;
	this.FocusInvalidElement = ew_FocusInvalidElement;
	this.TabView = null;
	this.SubmitButton = null;
	this.LastPageSubmit = false;
	this.HideDisabledButton = true;
}

// Multi page add element
function ew_MultiPageAddElement(elemid, pageIndex) {
	this.Elements.push([elemid, pageIndex]);
}

// Multi page init
function ew_InitMultiPage() {
	for (var i=0; i<this.Elements.length; i++) {
		if (this.Elements[i][1] > this.MaxPageIndex)
			this.MaxPageIndex = this.Elements[i][1]; 
	}	
	this.MinPageIndex = this.MaxPageIndex;
	for (var i=0; i<this.Elements.length; i++) {
		if (this.Elements[i][1] < this.MinPageIndex)
			this.MinPageIndex = this.Elements[i][1]; 
	}

	// if ASP.NET 
	if (typeof Page_ClientValidate == "function") {
			original_Page_ClientValidate = Page_ClientValidate; 
			Page_ClientValidate = function() { 
			var isValid;
			isValid = original_Page_ClientValidate();          
			if (!isValid) 
				this.FocusInvalidElement();
			return isValid; 
		} 
	}	
}

//// Multi page show this page
function ew_ShowPage() {
	for (var i=0; i<this.Elements.length; i++) {
		if (this.Elements[i][1] == this.PageIndex) {
			ew_CreateEditor(this.Elements[i][0]);
		}
	}
	this.EnableButtons();
}

// Multi page enable buttons
function ew_EnableButtons() {
	if (this.SubmitButton) {
		this.SubmitButton.disabled = (this.LastPageSubmit) ? (this.PageIndex != this.MaxPageIndex) : false;
		if (this.SubmitButton.disabled) {
			this.SubmitButton.style.display = (this.HideDisabledButton) ? "none" : "";
		} else {
			this.SubmitButton.style.display = "";	
		}
	}
}

// Get page index by element id
function ew_GetPageIndexByElementId(elemid) {
	var pageIndex = -1;
	for (var i=0; i<this.Elements.length; i++) {
		if (this.Elements[i][0] == elemid)
			return this.Elements[i][1];
	}
	return pageIndex;
}

// Goto page by index
function ew_GotoPageByIndex(pageIndex) {
	if (pageIndex < this.MinPageIndex || pageIndex > this.MaxPageIndex)
		return; 
	this.PageIndex = pageIndex;
	this.ShowPage();
}

// Goto page by element
function ew_GotoPageByElement(elem) {
	var pageIndex;
	if (!elem)
		return;
	var id = (!elem.type && elem[0]) ? elem[0].id : elem.id;
	if (id == "")
		return;
	pageIndex = this.GetPageIndexByElementId(id);
	if (pageIndex > -1) {
		this.GotoPageByIndex(pageIndex);
		if (this.TabView)
			this.TabView.set("activeIndex", pageIndex - 1);
	}
}

// for ASP.NET
// Focus invalid element
function ew_FocusInvalidElement() {	
 	for (var i=0; i<Page_Validators.length; i++) {
		if (!Page_Validators[i].isvalid) {
			var elem = document.getElementById(Page_Validators[i].controltovalidate);
			this.GotoPageByElement(elem);
			ew_SetFocus(elem);
			break;
		}
	}
}

// get selection list as element or radio/checkbox list as array
function ew_GetElements(name) {
	var ar = document.getElementsByName(name);
	if (ar.length == 1) {
		var el = ar[0];
		if (el.type && el.type != "checkbox" && el.type != "radio") 
			return ar[0];
	}	
	return ar;
}

// update multiple selection lists
function ew_UpdateOpts(ar) {
	if (ewLang.isArray(ar)) {
		var u;
		var cnt = ar.length;
		for (i = 0; i < cnt; i++) {
			u = ar[i];
			if (ewLang.isBoolean(u[2]) && !u[2]) { // Ajax and sync
				u[0] = {id: u[0], values: ew_GetOptValues(ew_GetElements(u[0]))};
				u[1] = {id: u[1], values: ew_GetOptValues(ew_GetElements(u[1]))};
			} else {
				ew_UpdateOpt(u[0], u[1], u[2], false);
			}
		}
		for (i = 0; i < cnt; i++) {
			u = ar[i];
			if (ewLang.isBoolean(u[2]) && !u[2]) {
				ew_UpdateOpt(u[0], u[1], true, false);
			}
		}
	}
}

// update child element options
function ew_UpdateOpt(id, parent_id, ds, updatechild) {
	var iff = -1;
	var oid, obj, ar, parentObj, arp;
	if (ewLang.isString(id)) {
		oid = id;
		obj = ew_GetElements(id);
		ar = ew_GetOptValues(obj);
	} else if (ewLang.isObject(id)) {
		oid = id.id;
		obj = ew_GetElements(id.id);
		ar = id.values;
	}
	if (ewLang.isString(parent_id)) {
		parentObj = ew_GetElements(parent_id);
		arp = ew_GetOptValues(parentObj);
	} else if (ewLang.isObject(parent_id)) {
		parentObj = ew_GetElements(parent_id.id);
		arp = parent_id.values;
		parent_id = parent_id.id;
	}
	var id = ew_GetId(obj);
	ew_ClearOpt(obj);
	var addOpt = function(aResults) {
		var cnt = aResults.length;
		for (var i=0; i<cnt; i++) {
			if (iff == 3) {
				for (var j=0; j<arp.length; j++) {
					if (aResults[i][3].toUpperCase() == arp[j].toUpperCase()) {
						ew_NewOpt(obj, aResults[i][0], aResults[i][1], aResults[i][2]);
						break;
					}
				}
			} else {
				ew_NewOpt(obj, aResults[i][0], aResults[i][1], aResults[i][2]);
			}
		}
		if (obj.length) // radio/checkbox list
			ew_RenderOpt(obj);
		ew_SelectOpt(obj, ar);
		if (updatechild != false) {
			if (obj.options) {
				if (typeof(obj.onchange) == "function") obj.onchange();
			} else if (obj.length) { // radio/checkbox list
				if (obj.length > 0) {
					var el = obj[0];
					if (typeof(el.onchange) == "function") el.onchange();
				}
			}
		}
	}
	if (ewLang.isArray(ds)) { // array => non-Ajax
		iff = 3;
		addOpt(ds);
	} else if (ewLang.isBoolean(ds)) { // async => Ajax
		var async = ds;
		var f = ewDom.getAncestorByTagName(ewDom.get(oid), "FORM");
		if (!f)
			return;
		var s = f.elements["s_" + id];
		var lft = f.elements["lft_" + id];
		if (!s || s.value == "")
			return;
		var cb = {
			success: function(oResponse) {
				var txt = oResponse.responseText;
				if (txt.length > 0) {
					var newLength = txt.length - EW_RECORD_DELIMITER.length;
					if (txt.substr(newLength) == EW_RECORD_DELIMITER)
						txt = txt.substr(0, newLength);
					var aResults = [];
					var aRecords = txt.split(EW_RECORD_DELIMITER);
					for (var n = aRecords.length-1; n >= 0; n--)
						aResults[n] = aRecords[n].split(EW_FIELD_DELIMITER);
					addOpt(aResults);
				}
			},
			failure: function(oResponse) {
			},
			scope: this,
			argument: null
		}
		var o = ewConnect.getConnectionObject(false);
		if (o) {
			var url = EW_LOOKUP_FILE_NAME + "?s=" + s.value + "&f=" + encodeURIComponent(arp.join(",")) +
				"&lft=" + encodeURIComponent(lft.value);
			o.conn.open("get", url, async);
			if (async)
				ewConnect.handleReadyState(o, cb);
			o.conn.send(null);
			if (!async)
				ewConnect.handleTransactionResponse(o, cb);
		}
	}
}

// Render repeat column table (rowcnt is zero based row count)
function ew_RepeatColumnTable(totcnt, rowcnt, repeatcnt, rendertype) {
	var sWrk = "";
	if (rendertype == 1) { // render start
		if (rowcnt == 0)
			sWrk += "<table class=\"" + EW_ITEM_TABLE_CLASSNAME + "\">";
		if (rowcnt % repeatcnt == 0)
			sWrk += "<tr>";
		sWrk += "<td>";
	} else if (rendertype == 2) { // render end
		sWrk += "</td>";
		if (rowcnt % repeatcnt == repeatcnt - 1) {
			sWrk += "</tr>";
		} else if (rowcnt == totcnt - 1) {
			for (i = (rowcnt % repeatcnt) + 1; i < repeatcnt; i++)
				sWrk += "<td>&nbsp;</td>";
			sWrk += "</tr>";
		}
		if (rowcnt == totcnt - 1) sWrk += "</table>";
	}
	return sWrk;
}

// Get existing selected values
function ew_GetOptValues(obj) {
	var ar = [];
	if (obj.options) { // selection list
		for (i=0; i<obj.options.length; i++) {
			if (obj.options[i].selected)
				ar.push(obj.options[i].value);
		}
	} else if (obj.length) { // radio/checkbox list
		var i, el;		
		var cnt = obj.length;		
		for (i=0; i<cnt; i++) {
			el = obj[i];
			if (el.checked)
				ar.push(el.value);
		}	
	} else if (obj) { // radio/checkbox/text/hidden
		ar.push(obj.value);
	}
	return ar;
}

// Clear existing options
function ew_ClearOpt(obj) {
	if (obj.options) { // selection list
		var lo = (obj.type == "select-multiple") ? 0 : 1;
		for (var i=obj.length-1; i>=lo; i--)
			obj.options[i] = null;
	} else if (obj.length) { // radio/checkbox list
		var id = ew_GetId(obj); 
		var p = document.getElementById("dsl_" + id); // parent element
		if (p) {
			var els = ewDom.getChildrenBy(p, function(node) {
				return (node.tagName == "TABLE" && node.className == EW_ITEM_TABLE_CLASSNAME);
			});			
			for (var i=0; i<els.length; i++)
				p.removeChild(els[i]);
			p._options = [];
		}
	}
}

// Get the id or name of an element
function ew_GetId(obj) {
	var id = "";
	if (!obj.options && obj.length)
		obj = obj[0];
	if (obj.id && obj.id != "") {
		id = obj.id;
	} else if (obj.name && obj.name != "") {
		id = obj.name;
	}
	if (id.substr(id.length-2, 2) == "[]")
		id = id.substr(0, id.length-2); 	
	return id;
}

// Create combobox option 
function ew_NewOpt(obj, value, text1, text2) {
	var text = text1;
	if (text2 && text2 != "")
		text += EW_FIELD_SEP + text2;	
	if (obj.options) { // selection list
		var optionName = new Option(text, value, false, false)
		obj.options[obj.length] = optionName;
	} else if (obj.length) { // radio/checkbox list
		var id = ew_GetId(obj); 
		var p = document.getElementById("dsl_" + id); // get parent element		
		if (p)
			p._options.push({val:value, lbl:text});
	}
}

// Render the options
function ew_RenderOpt(obj) {
	var id = ew_GetId(obj); 
	var p = document.getElementById("dsl_" + id); // parent element
	var t = document.getElementById("tp_" + id); // get the item template	
	if (!p || !p._options || !t)
		return;
	var cnt = p._options.length;
	var cols = p.getAttribute("repeatcolumn");
	if (!cols || cols == NaN || cols < 1)
		cols = 5;
	var tpl = t.innerHTML;		 
	var html = "";
	var ihtml;
	for (var i=0; i<cnt; i++) {
		html += ew_RepeatColumnTable(cnt, i, cols, 1);
		ihtml = tpl;		
		ihtml = ihtml.replace(/(\"){0,1}{value}(\"){0,1}/g, "\"" + ew_HtmlEncode(p._options[i].val) + "\""); // replace value		
		html += "<label>" + ihtml + p._options[i].lbl + "</label>";		
		html += ew_RepeatColumnTable(cnt, i, cols, 2);		
	} 
	p.innerHTML += html;
	p._options = [];		
}

// Select combobox option
function ew_SelectOpt(obj, value_array) {
	if (!obj || !value_array)
		return;
	var i, j, cnt2, el;
	var cnt = value_array.length; 
	for (i=0; i<cnt; i++) {		
		if (obj.options) { // listbox/combobox
			cnt2 = obj.length;
			for (j=0; j<cnt2; j++) {
				if (obj.options[j].value.toUpperCase() == value_array[i].toUpperCase()) {
					obj.options[j].selected = true;
					break;
				}
			}
		} else if (obj.length) { // radio/checkbox list
			cnt2 = obj.length;
			for (j=0; j<cnt2; j++) {
				if (obj[j].value.toUpperCase() == value_array[i].toUpperCase()) {
					obj[j].checked = true;
					break;
				}
			}		
		} else if (obj.type) {
			obj.value = value_array.join(",");
		}
	}
	if (obj.options && obj.getAttribute("autoselect") == "true") {
		if (obj.type == "select-one" && obj.options.length == 2 &&
			!obj.options[1].selected) {
			obj.options[1].selected = true;
		} else if (obj.type == "select-multiple" && obj.options.length == 1 &&
			!obj.options[0].selected) {
			obj.options[0].selected = true;
		}
	} else if (obj.length && obj.length == 2 && obj[0].getAttribute("autoselect") == "true") { // radio/checkbox list			
		obj[1].checked = true;
	}
}

// Auto-Suggest
function ew_AutoSuggest(elInput, elContainer, elSQL, elMessage, elValue, elParent, forceSelection) { 

	// Create DataSource
	this.ds = new ewUtil.XHRDataSource(EW_LOOKUP_FILE_NAME);
	this.ds.responseType = ewUtil.XHRDataSource.TYPE_TEXT;
	this.ds.responseSchema = {
		recordDelim: EW_RECORD_DELIMITER,
		fieldDelim: EW_FIELD_DELIMITER
	};
	this.ds.maxCacheEntries = 0; // DO NOT CHANGE!		
	this.ds.scriptQueryParam = "q";

	// create AutoComplete
	this.ac = new ewWidget.AutoComplete(elInput, elContainer, this.ds);
	this.ac._originalClearSelection = this.ac._clearSelection;
	this.ac._as = this;
	this.ac.useShadow = false;
	this.ac.animVert = false;
	this.ac.minQueryLength = 1;
	this.ac.typeAhead = true;
	this.ac.forceSelection = forceSelection;	
	this.ac.doBeforeExpandContainer = function(oTextbox, oContainer, sQuery, aResults) {
		var pos = ewDom.getXY(oTextbox);
		pos[1] += ewDom.get(oTextbox).offsetHeight + 1;
		ewDom.setXY(oContainer,pos);
		return true;
	};

	// if forceSelection
	this.ac._clearSelection = function() {
		if (this._elTextbox.value == "") {
			this._as.setValue("");
		} else {
			this._originalClearSelection();
		}
	}

	// format display value (Note: Override this function if link field <> display field)
	this.formatResult = function(ar) {
		return ar[0];
	};

	// set the key to the actual value field
	this.setValue = function(v) {
		if (elValue) {
			var el = ewDom.get(elValue);
			if (el) {
				el.value = v;		
				if (el.onchange)					
					el.onchange.call(el);
			}
		}	
	}

	// format result
	this.ac.formatResult = function(oResultItem, sQuery) {

		//var key = oResultItem[0];
		var lbl = this._as.formatResult(oResultItem);

		//oResultItem[0] = lbl;
		//oResultItem.push(key); // Save the key to last

		return lbl;
	};

	// generate request
	this.ac.generateRequest = function(sQuery) {
		this.dataSource.scriptQueryAppend = "s=" + ewDom.get(elSQL).value;
		if (elParent != "") {
			var arp = ew_GetOptValues(ew_GetElements(elParent));
			this.dataSource.scriptQueryAppend += "&f=" + encodeURIComponent(arp.join(","));
		}
		sQuery = (this.queryQuestionMark ? "?" : "") + (this.dataSource.scriptQueryParam || "query") + "=" + sQuery +
		(this.dataSource.scriptQueryAppend ? ("&" + this.dataSource.scriptQueryAppend) : "");
		return sQuery;
	};

	// update the key to the actual value field
	this.ac.itemSelectEvent.subscribe(function(type, e) {
		var ar = e[2];
		this._as.setValue(ar[0]);
		this._elTextbox.value = this._as.formatResult(ar);
	});

	// update the key to the actual value field
	this.ac.textboxFocusEvent.subscribe(function(type, e) {
		ewDom.removeClass(elInput, "ewUnmatched");
		ewDom.setStyle(elMessage, "display", "none");
	});

	// clear the actual value field
	if (forceSelection) {
		this.ac.selectionEnforceEvent.subscribe(function(type, e) {
			this._as.setValue("");
			ewDom.addClass(elInput, "ewUnmatched");
			ewDom.setStyle(elMessage, "display", "");
		});	
	} else {
		this.ac.unmatchedItemSelectEvent.subscribe(function(type, e) {
			this._as.setValue(this._elTextbox.value);	
		});
	}
}

// Init dialog
function ew_InitDialog() {
	ewDialog = new ewWidget.Dialog("ewAddOptDialog", { visible: false, constraintoviewport: true, hideaftersubmit: false, zIndex: 9000 }); 
	ewDialog.callback = { success: ew_AddOptHandleSuccess, failure: ew_DefaultHandleFailure };

	// Validate data
	ewDialog.validate = function() {
		var data = this.getData();
		var tablename = data.t;

		// Note: You can add your validation code here, return false if invalid, e.g.
// if (tablename == "xxx") {
// if (data.firstname == "" || data.lastname == "") {
// alert("Please enter your first and last names.");
// return false;
// }
// }

		return true;
	};

// ewDialog.beforeShowEvent.subscribe(function() {
// var w = this.header.offsetWidth;
// w = Math.max(w, this.body.offsetWidth);
// w = Math.max(w, this.footer.offsetWidth);
// this.header.style.width = w + "px";
// this.body.style.width = w + "px";
// this.footer.style.width = w + "px";
// });

    ewDialog.render();
}

function ew_DefaultHandleSubmit() {
	this.submit();
}

function ew_DefaultHandleCancel() {
	this.cancel();
	this.setBody("");	
}

function ew_DefaultHandleFailure(o) {
	ewDialog.hide();
	ewDialog.setBody("");
	alert("Server Error " + o.status + ": " + o.statusText);
}

function ew_AddOptHandleSuccess(o) {
	var results;
	if (o.responseXML)
		results = o.responseXML.getElementsByTagName("result");
	if (results && results.length > 0) {
		ewDialog.hide();
		ewDialog.setBody("");
		var xl;
		var result = results[0];
		var obj = ew_GetElements(o.argument.el);
		if (obj) {
			xl = result.getElementsByTagName(o.argument.lf);
			var lfv = (xl.length > 0 && xl[0].firstChild) ? xl[0].firstChild.nodeValue : "";
			xl = result.getElementsByTagName(o.argument.df);
			var dfv = (xl.length > 0 && xl[0].firstChild) ? xl[0].firstChild.nodeValue : "";
			var df2v = "";
			if (o.argument.df2 != "") {
				xl = result.getElementsByTagName(o.argument.df2);
				df2v = (xl.length > 0 && xl[0].firstChild) ? xl[0].firstChild.nodeValue : "";
			}
			var ffv = "";
			if (o.argument.ff != "") {
				xl = result.getElementsByTagName(o.argument.ff);
				ffv = (xl.length > 0 && xl[0].firstChild) ? xl[0].firstChild.nodeValue : "";
			}
			if (lfv != "" && dfv != "") {
				if (o.argument.pg) { // non-Ajax
					var elid = o.argument.el;
					if (elid.substr(elid.length - 2, 2) == "[]") // PHP
						elid = elid.substr(0, elid.length - 2);
					var ar = o.argument.pg["ar_" + elid];
					if (ar && ewLang.isArray(ar))
						ar[ar.length] = [lfv, dfv, df2v, ffv];
				}
				var add = true;

				// get the parent field value
				if (o.argument.pf != "") {
					var pobj = ew_GetElements(o.argument.pf);
					var par = ew_GetOptValues(pobj);
					var pcnt = par.length;
					add = false;
					for (var i=0; i<pcnt; i++) {
						if (par[i] == ffv) {
							add = true;
							break;
						}
					}
				}
				if (add) { // add the new option
					if (!obj.options && obj.length) { // radio/checkbox list
						var id = ew_GetId(obj);
						var p = document.getElementById("dsl_" + id); // parent element
						if (!p)
							return;
						var ar = [];
						var vals = [];
						var cnt = obj.length;
						for (var i=0; i<cnt; i++) {
							if (obj[i].type == "checkbox" && obj[i].checked)
								vals.push(obj[i].value);
							if (obj[i].nextSibling)
								ar.push({val: obj[i].value, lbl: obj[i].nextSibling.nodeValue});
						}
						ew_ClearOpt(obj);
						p._options = ar;
					}
					ew_NewOpt(obj, lfv, dfv, df2v);
					if (obj.options) {
						obj.options[obj.options.length-1].selected = true;
						if (obj.onchange)
							obj.onchange.call(obj);
						obj.focus();
					} else if (obj.length) { // radio/checkbox list
						ew_RenderOpt(obj);
						if (vals.length > 0)
							ew_SelectOpt(obj, vals);
						var obj = ew_GetElements(o.argument.el);
						if (obj.length > 0) {
							var el = obj[obj.length-1];
							el.checked = true;
							if (el.type == "radio")
								el.onclick.call(el);
							el.focus();
						}
					}
				}
			}
		}
	} else {
		ewDialog.setBody(o.responseText);
	}
}

// Show dialog
// argument object members:
// pg - page
// lnk - add option link id
// el - form element id
// url - URL of the Add form 
// hdr - dialog header
// lf - link field
// df - display field
// df2 - display field 2
// pf - parent field
// ff - filter field
function ew_AddOptDialogShow(oArg) {
	if (ewDialog && ewDialog.cfg.getProperty("visible")) ewDialog.hide();
	var f = {
		success: function(o) {
			if (ewDialog) {

				// get the parent field value
				var obj = ew_GetElements(o.argument.pf);
				var ar = ew_GetOptValues(obj);
				var cfg = { context: [o.argument.lnk, "tl", "bl"],
					buttons: [ { text:EW_BUTTON_SUBMIT_TEXT, handler:ew_DefaultHandleSubmit, isDefault:true },
						{ text:EW_BUTTON_CANCEL_TEXT, handler:ew_DefaultHandleCancel } ]
				};
				ewDialog.cfg.applyConfig(cfg);
				ewDialog.callback.argument = o.argument;
				if (ewDialog.header) ewDialog.header.style.width = "auto";
				if (ewDialog.body) ewDialog.body.style.width = "auto";
				if (ewDialog.footer) ewDialog.footer.style.width = "auto";
				ewDialog.setBody(o.responseText);
				ewDialog.setHeader(o.argument.hdr);
				ewDialog.render();
				ewDialog.registerForm(); // make sure the form is registered (otherwise, the form is not registered in the first time)

				// set the filter field value
				if (ar.length == 1 && o.argument.ff != "" && ewDialog.form && ewDialog.form.elements[o.argument.ff])
					ew_SelectOpt(ewDialog.form.elements[o.argument.ff], ar);
				ewDialog.show();
			}
		},
		failure: function(oResponse) {
		},
		scope: this,
		argument: oArg
	}
	ewConnect.asyncRequest("get", oArg.url, f, null);
}

// Auto fill text boxes by AJAX
function ew_AjaxAutoFill(obj) {
	if (ewLang.isString(obj))
		obj = ew_GetElements(obj);
	var ar = ew_GetOptValues(obj);
	var id = ew_GetId(obj);
	var sf = document.getElementById("sf_" + id);
	if (ar.length < 1 || ar[0] == "" || !sf || sf.value == "")
		return;
	var ds = new ewUtil.XHRDataSource(EW_LOOKUP_FILE_NAME);
	ds.responseSchema.recordDelim = EW_RECORD_DELIMITER;
	ds.responseSchema.fieldDelim = EW_FIELD_DELIMITER;
	ds.responseType = ewUtil.DataSourceBase.TYPE_TEXT;
	ds.maxCacheEntries = 0; // DO NOT CHANGE!
	var f = function(oRequest, oParsedResponse) {
		var aResults = oParsedResponse.results;
		var id = ew_GetId(this);
		var dn = document.getElementById("ln_" + id);
		var destNames = (dn) ? dn.value : "";
		var dest_array = destNames.split(",");
		var destEl, asEl, dfv;
		for (var j=0; j < dest_array.length; j++) {
			destEl = ew_GetElements(dest_array[j]);
			if (destEl && j < aResults[0].length) {
				dfv = aResults[0][j];
				if (destEl.options || destEl.length) {
					ew_SelectOpt(destEl, ar);
				} else if (destEl.type == "hidden") {
					asEl = ew_GetElements("sv_" + dest_array[j]);
					if (asEl) {
						destEl.value = ar[0];
						asEl.value = dfv;
					} else {
						destEl.value = dfv;
					}
				} else if (destEl.type == "textarea") {
					destEl.value = dfv;
					if (typeof ew_UpdateDHTMLEditor == "function")
						ew_UpdateDHTMLEditor(dest_array[j]);
				} else {
					destEl.value = dfv;
				}
			}
		}
	}
	var sQuery = "?q=" + encodeURIComponent(ar[0]) + "&s=" + sf.value;
	ds.sendRequest(sQuery, f, obj);
}

// Check US Date format (mm/dd/yyyy)
function ew_CheckUSDate(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	isplit = object_value.indexOf(EW_DATE_SEPARATOR); // Split by date separator
	if (isplit == -1 || isplit == object_value.length)
		return false;
	sMonth = object_value.substring(0, isplit);
	if (sMonth.length == 0)
		return false;
	isplit = object_value.indexOf(EW_DATE_SEPARATOR, isplit + 1); // Split by date separator
	if (isplit == -1 || (isplit + 1 ) == object_value.length)
		return false;
	sDay = object_value.substring((sMonth.length + 1), isplit);
	if (sDay.length == 0)
		return false;
	isep = object_value.indexOf(' ', isplit + 1); 
	if (isep == -1) {
		sYear = object_value.substring(isplit + 1);
	} else {
		sYear = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime(sTime)) // Check time
			return false; 
	}
	if (!ew_CheckInteger(sMonth))
		return false;
	else if (!ew_CheckRange(sMonth, 1, 12))
		return false;
	else if (!ew_CheckInteger(sYear))
		return false;
	else if (!ew_CheckRange(sYear, 0, 9999))
		return false;
	else if (!ew_CheckInteger(sDay))
		return false;
	else if (!ew_CheckDay(sYear, sMonth, sDay))
		return false;
	else
		return true;
}

// Check Date format (yyyy/mm/dd)
function ew_CheckDate(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	isplit = object_value.indexOf(EW_DATE_SEPARATOR); // Split by date separator
	if (isplit == -1 || isplit == object_value.length)
		return false;
	sYear = object_value.substring(0, isplit);
	isplit = object_value.indexOf(EW_DATE_SEPARATOR, isplit + 1); // Split by date separator
	if (isplit == -1 || (isplit + 1 ) == object_value.length)
		return false;
	sMonth = object_value.substring((sYear.length + 1), isplit);
	if (sMonth.length == 0)
		return false;
	isep = object_value.indexOf(' ', isplit + 1); 
	if (isep == -1) {
		sDay = object_value.substring(isplit + 1);
	} else {
		sDay = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime(sTime)) // Check time
			return false; 
	}
	if (sDay.length == 0)
		return false;
	if (!ew_CheckInteger(sMonth))
		return false;
	else if (!ew_CheckRange(sMonth, 1, 12))
		return false;
	else if (!ew_CheckInteger(sYear))
		return false;
	else if (!ew_CheckRange(sYear, 0, 9999))
		return false;
	else if (!ew_CheckInteger(sDay))
		return false;
	else if (!ew_CheckDay(sYear, sMonth, sDay))
		return false;
	else
		return true;
}

// Check Euro Date format (dd/mm/yyyy)
function ew_CheckEuroDate(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
	  return true;
	isplit = object_value.indexOf(EW_DATE_SEPARATOR); // Split by date separator
	if (isplit == -1 || isplit == object_value.length)
		return false;
	sDay = object_value.substring(0, isplit);
	monthSplit = isplit + 1;
	isplit = object_value.indexOf(EW_DATE_SEPARATOR, monthSplit); // Split by date separator
	if (isplit == -1 ||  (isplit + 1 )  == object_value.length)
		return false;
	sMonth = object_value.substring((sDay.length + 1), isplit);
	isep = object_value.indexOf(' ', isplit + 1); 
	if (isep == -1) {
		sYear = object_value.substring(isplit + 1);
	} else {
		sYear = object_value.substring(isplit + 1, isep);
		sTime = object_value.substring(isep + 1);
		if (!ew_CheckTime(sTime))
			return false; 
	}
	if (!ew_CheckInteger(sMonth))
		return false;
	else if (!ew_CheckRange(sMonth, 1, 12))
		return false;
	else if (!ew_CheckInteger(sYear))
		return false;
	else if (!ew_CheckRange(sYear, 0, null))
		return false;
	else if (!ew_CheckInteger(sDay))
		return false;
	else if (!ew_CheckDay(sYear, sMonth, sDay))
		return false;
	else
		return true;
}

// Check day
function ew_CheckDay(checkYear, checkMonth, checkDay) {
	maxDay = 31;
	if (checkMonth == 4 || checkMonth == 6 ||	checkMonth == 9 || checkMonth == 11) {
		maxDay = 30;
	} else if (checkMonth == 2)	{
		if (checkYear % 4 > 0)
			maxDay =28;
		else if (checkYear % 100 == 0 && checkYear % 400 > 0)
			maxDay = 28;
		else
			maxDay = 29;
	}
	return ew_CheckRange(checkDay, 1, maxDay);
}

// Check integer
function ew_CheckInteger(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	var decimal_format = ".";
	var check_char;
	check_char = object_value.indexOf(decimal_format);
	if (check_char < 1)
		return ew_CheckNumber(object_value);
	else
		return false;
}

// Check number range
function ew_NumberRange(object_value, min_value, max_value) {
	if (min_value != null) {
		if (object_value < min_value)
			return false;
	}
	if (max_value != null) {
		if (object_value > max_value)
			return false;
	}
	return true;
}

// Check number
function ew_CheckNumber(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	var start_format = " .+-0123456789";
	var number_format = " .0123456789";
	var check_char;
	var decimal = false;
	var trailing_blank = false;
	var digits = false;
	check_char = start_format.indexOf(object_value.charAt(0));
	if (check_char == 1)
		decimal = true;
	else if (check_char < 1)
		return false;
	for (var i = 1; i < object_value.length; i++)	{
		check_char = number_format.indexOf(object_value.charAt(i))
		if (check_char < 0) {
			return false;
		} else if (check_char == 1)	{
			if (decimal)
				return false;
			else
				decimal = true;
		} else if (check_char == 0) {
			if (decimal || digits)	
			trailing_blank = true;
		}	else if (trailing_blank) { 
			return false;
		} else {
			digits = true;
		}
	}	
	return true;
}

// Check range
function ew_CheckRange(object_value, min_value, max_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (!ew_CheckNumber(object_value))
		return false;
	else
		return (ew_NumberRange((eval(object_value)), min_value, max_value));
	return true;
}

// Check time
function ew_CheckTime(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	isplit = object_value.indexOf(':');
	if (isplit == -1 || isplit == object_value.length)
		return false;
	sHour = object_value.substring(0, isplit);
	iminute = object_value.indexOf(':', isplit + 1);
	if (iminute == -1 || iminute == object_value.length)
		sMin = object_value.substring((sHour.length + 1));
	else
		sMin = object_value.substring((sHour.length + 1), iminute);
	if (!ew_CheckInteger(sHour))
		return false;
	else if (!ew_CheckRange(sHour, 0, 23))
		return false;
	if (!ew_CheckInteger(sMin))
		return false;
	else if (!ew_CheckRange(sMin, 0, 59))
		return false;
	if (iminute != -1) {
		sSec = object_value.substring(iminute + 1);		
		if (!ew_CheckInteger(sSec))
			return false;
		else if (!ew_CheckRange(sSec, 0, 59))
			return false;	
	}
	return true;
}

// Check phone
function ew_CheckPhone(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (object_value.length != 12)
		return false;
	if (!ew_CheckNumber(object_value.substring(0,3)))
		return false;
	else if (!ew_NumberRange((eval(object_value.substring(0,3))), 100, 1000))
		return false;
	if (object_value.charAt(3) != "-" && object_value.charAt(3) != " ")
		return false
	if (!ew_CheckNumber(object_value.substring(4,7)))
		return false;
	else if (!ew_NumberRange((eval(object_value.substring(4,7))), 100, 1000))
		return false;
	if (object_value.charAt(7) != "-" && object_value.charAt(7) != " ")
		return false;
	if (object_value.charAt(8) == "-" || object_value.charAt(8) == "+")
		return false;
	else
		return (ew_CheckInteger(object_value.substring(8,12)));
}

// Check zip
function ew_CheckZip(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (object_value.length != 5 && object_value.length != 10)
		return false;
	if (object_value.charAt(0) == "-" || object_value.charAt(0) == "+")
		return false;
	if (!ew_CheckInteger(object_value.substring(0,5)))
		return false;
	if (object_value.length == 5)
		return true;
	if (object_value.charAt(5) != "-" && object_value.charAt(5) != " ")
		return false;
	if (object_value.charAt(6) == "-" || object_value.charAt(6) == "+")
		return false;
	return (ew_CheckInteger(object_value.substring(6,10)));
}

// Check credit card
function ew_CheckCreditCard(object_value) {
	var white_space = " -";
	var creditcard_string = "";
	var check_char;
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	for (var i = 0; i < object_value.length; i++) {
		check_char = white_space.indexOf(object_value.charAt(i));
		if (check_char < 0)
			creditcard_string += object_value.substring(i, (i + 1));
	}	
	if (creditcard_string.length == 0)
		return false;	 
	if (creditcard_string.charAt(0) == "+")
		return false;
	if (!ew_CheckInteger(creditcard_string))
		return false;
	var doubledigit = creditcard_string.length % 2 == 1 ? false : true;
	var checkdigit = 0;
	var tempdigit;
	for (var i = 0; i < creditcard_string.length; i++) {
		tempdigit = eval(creditcard_string.charAt(i));		
		if (doubledigit) {
			tempdigit *= 2;
			checkdigit += (tempdigit % 10);			
			if ((tempdigit / 10) >= 1.0)
				checkdigit++;			
			doubledigit = false;
		}	else {
			checkdigit += tempdigit;
			doubledigit = true;
		}
	}
	return (checkdigit % 10) == 0 ? true : false;
}

// Check social security number
function ew_CheckSSC(object_value) {
	var white_space = " -+.";
	var ssc_string="";
	var check_char;
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (object_value.length != 11)
		return false;
	if (object_value.charAt(3) != "-" && object_value.charAt(3) != " ")
		return false;
	if (object_value.charAt(6) != "-" && object_value.charAt(6) != " ")
		return false;
	for (var i = 0; i < object_value.length; i++) {
		check_char = white_space.indexOf(object_value.charAt(i));
		if (check_char < 0)
			ssc_string += object_value.substring(i, (i + 1));
	}	
	if (ssc_string.length != 9)
		return false;	 
	if (!ew_CheckInteger(ssc_string))
		return false;
	return true;
}

// Check email
function ew_CheckEmail(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (!(object_value.indexOf("@") > -1 && object_value.indexOf(".") > -1))
		return false;    
	return true;
}

// Check GUID {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}	
function ew_CheckGUID(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	object_value = object_value.replace(/^\s*|\s*$/g, "");
	var re1 = new RegExp("^(\{{1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{1})$");
	var re2 = new RegExp("^(([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12})$");
	return re1.test(object_value) || re2.test(object_value);
}

// Check file extension
function ew_CheckFileType(object_value) {
	if (object_value == null)
		return true;
	if (object_value.length == 0)
		return true;
	if (typeof EW_UPLOAD_ALLOWED_FILE_EXT == "undefined")
		return true;
	if (EW_UPLOAD_ALLOWED_FILE_EXT == "")
		return true;
	var fileTypes = EW_UPLOAD_ALLOWED_FILE_EXT.split(",");
	var ext = object_value.substring(object_value.lastIndexOf(".")+1, object_value.length).toLowerCase(); 
	for (var i=0; i < fileTypes.length; i++) { 
		if (fileTypes[i] == ext) 
			return true; 
	} 
	return false; 
}
