// JavaScript for Multiple Page Update
// (C) 2006 e.World Technology Ltd.
// v2.0 - 2006/8/5

var ew_MultiPageElements = new Array();

function ew_MultiPageAddElement(elemid, pageIndex) {
	var item = new Array(2);
	item[0] = elemid;
	item[1] = pageIndex;
	ew_MultiPageElements.push(item);
}

function ew_InitMultiPage() {
	if (!(document.getElementById || document.all))
		return;
	ew_MaxPageIndex = 0;
	if (ew_MultiPageElements) {
		for (var i=0; i<ew_MultiPageElements.length; i++) {
			if (ew_MultiPageElements[i][1] > ew_MaxPageIndex)
				ew_MaxPageIndex = ew_MultiPageElements[i][1]; 
		}	
		ew_MinPageIndex = ew_MaxPageIndex;
		for (var i=0; i<ew_MultiPageElements.length; i++) {
			if (ew_MultiPageElements[i][1] < ew_MinPageIndex)
				ew_MinPageIndex = ew_MultiPageElements[i][1]; 
		}
		ew_NextPage();
		
		// if ASP.NET 
		if (typeof Page_ClientValidate == "function") {
		original_Page_ClientValidate = Page_ClientValidate; 
			Page_ClientValidate = function() { 
				var isValid;
				isValid = original_Page_ClientValidate();          
				if (!isValid) 
					ew_FocusInvalidElement();
				return isValid; 
			} 
		}
	}
}

function ew_PageHasElements(pageIndex) {
	for (var i=0; i<ew_MultiPageElements.length; i++) {
		if (ew_MultiPageElements[i][1] == pageIndex)
			return true;
	}
	return false;
}

function ew_NextPage() {
	if (!(document.getElementById || document.all))
		return;
	ew_EnableButtons(false);
	var hasElements = false;
	while (!hasElements && ew_PageIndex < ew_MaxPageIndex) {
		hasElements = ew_PageHasElements(++ew_PageIndex);
		if (hasElements)
			ew_ShowPage();
	}
	ew_UpdateButtons();
	ew_EnableButtons(true);
}

function ew_PrevPage() {
	if (!(document.getElementById || document.all))
		return;
	ew_EnableButtons(false);
	var hasElements = false;
	while (!hasElements && ew_PageIndex > ew_MinPageIndex) {
		hasElements = ew_PageHasElements(--ew_PageIndex);
		if (hasElements)
			ew_ShowPage();
	}
	ew_UpdateButtons();
	ew_EnableButtons(true);
}

function ew_ShowPage() {
	var fn;
	if (!fn && typeof ew_CreateEditor == 'function')
		fn = ew_CreateEditor;
	if (!fn && typeof EW_createEditor == 'function')
		fn = EW_createEditor; // for backward compatibility
	for (var i=0; i<ew_MultiPageElements.length; i++) {
		var row = ew_GetRowByElementId(ew_MultiPageElements[i][0]);		
		if (row) {
			row.style.display = (ew_MultiPageElements[i][1] == ew_PageIndex) ? '' : 'none';
			if (row.style.display == '' && fn)
				fn(ew_MultiPageElements[i][0]);
		}	
	}
}

function ew_UpdateButtons() {	
	if (ew_MaxPageIndex == ew_MinPageIndex)
		return;
	var elem = ew_GetElement('ewMultiPagePager');
	if (!elem)
		return;		
	var pager = "<table class='ewMultiPagePager'><tr>";
	if (ew_PageIndex <= ew_MinPageIndex) {
		pager = pager + "<td>" + ew_MultiPagePrev + "</td>";
	} else {
		pager = pager + "<td><a href='javascript:ew_PrevPage();'>" + ew_MultiPagePrev + "</a></td>";
	}
	for (var i=ew_MinPageIndex; i<=ew_MaxPageIndex; i++) {
		if (i == ew_PageIndex) {
			pager = pager + "<td>" + i + "</td>";
		} else {
			pager = pager + "<td><a href='javascript:ew_GotoPageByIndex(" + i + ");'>" + i + "</a></td>";
		}
	}  
	if (ew_PageIndex >= ew_MaxPageIndex) {
		pager = pager + "<td>" + ew_MultiPageNext + "</td>";
	} else {
		pager = pager + "<td><a href='javascript:ew_NextPage();'>" + ew_MultiPageNext + "</a></td>";
	}	
	pager = pager + "</tr><tr><td colspan=" + (ew_MaxPageIndex - ew_MinPageIndex + 3) +">";
	pager = pager + ew_MultiPagePage + " " + (ew_PageIndex) + " " + ew_MultiPageOf + " " + (ew_MaxPageIndex);
	pager = pager + "</td></tr></table>";
	elem.innerHTML = pager;
}

function ew_EnableButtons(bool) {
	var btn = ew_GetElement('btnAction'); 
	if (btn)
		btn.disabled = !bool;   
}

function ew_GetElement(elemid) {
	return (document.getElementById) ? document.getElementById(elemid) : (document.all) ? document.all(elemid) : null;
}

function ew_GetPageIndexByElementId(elemid) {
	var pageIndex = -1;
	for (var i=0; i<ew_MultiPageElements.length; i++) {
		if (ew_MultiPageElements[i][0] == elemid)
			return ew_MultiPageElements[i][1];
	}
	return pageIndex;
}

function ew_GotoPageByIndex(pageIndex) {
	if (pageIndex < ew_MinPageIndex || pageIndex > ew_MaxPageIndex)
		return; 
	ew_PageIndex = pageIndex - 1;
	ew_NextPage();
}

function ew_GotoPageByElement(elem) {
	var pageIndex;
	if (!elem || elem.id == "")
		return;	
	pageIndex = ew_GetPageIndexByElementId(elem.id);
	ew_GotoPageByIndex(pageIndex);
}

function ew_GetRowByElementId(elemid) {
	var elem;
	elem = ew_GetElement(elemid);
	if (!elem)
		return null;
	if (document.all) {		
		while (elem.tagName != "TR")
			elem = elem.parentElement;
	}	else if (document.getElementById) {		
		while (elem.tagName != "TR")
			elem = elem.parentNode;
	}
	return elem;
}

function ew_IsElementVisible(elemid) {
	if (!(document.getElementById || document.all))
		return true;
	var elem = ew_GetElement(elemid);
	return (elem && elem.style.display == '');
}

// for ASP.NET
function ew_FocusInvalidElement() {	
 	for (var i=0; i<Page_Validators.length; i++) {
		if (!Page_Validators[i].isvalid) {
			var elem = ew_GetElement(Page_Validators[i].controltovalidate);
			ew_GotoPageByElement(elem);
			ew_SetFocus(elem);
			break;
		}
	}
}
