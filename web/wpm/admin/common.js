var value = null;

var oPopup = window.createPopup();
var UniqueID;

function gotoURL(sURL) {
	top.location.href=sURL + UniqueID;
}

function createPopUp(iWidth,iHeight,sUniqueID,sType,oObject)
{
  //== Define Box Content
  switch(sType) {
    case 'Page':{
      oPopup.document.body.innerHTML = page.innerHTML;
      UniqueID = sUniqueID;break}
    case 'Company':{
      oPopup.document.body.innerHTML = Company.innerHTML;
      UniqueID = sUniqueID;break}
    case 'Contact':{
      oPopup.document.body.innerHTML = contact.innerHTML;
      UniqueID = sUniqueID;break}
    case 'Article':{
      oPopup.document.body.innerHTML = Article.innerHTML;
      UniqueID = sUniqueID;break}
    default:{
      oPopup.document.body.innerHTML = page.innerHTML;
      UniqueID = sUniqueID;break}
  }
  //== Display Box
  var lefter2 = event.offsetY+0;
  var topper2 = event.offsetX+0;
oPopup.show(topper2 , lefter2, iWidth, iHeight,oObject);
}

function openWin(value,wval,hval)
{
  window.open(value,'popup','resizable=no,width=' + wval + ',height=' + hval + ',status=no,location=no,toolbar=no,menubar=no,scrollbars=yes');
}
function CloseWin()
{
  window.close();
}

    function inset(elmnt)
        {
        elmnt.style.border="inset 2";
        elmnt.style.background="<%=Session("nav_hover")%>";
        }
    function outset(elmnt)
        {
        elmnt.style.border="outset 2";
        elmnt.style.background="<%=Session("nav_off")%>";
        }
    function MM_swapImgRestore() 
        { //v3.0
        var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
        }

    function MM_preloadImages() 
        { //v3.0
        var d=document; 
        if(d.images)
          { 
            if(!d.MM_p) d.MM_p=new Array();
            var i,j=d.MM_p.length,a=MM_preloadImages.arguments; 
            for(i=0; i<a.length; i++)
            if (a[i].indexOf("#")!=0)
            { 
              d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];
            }
          }
        }

    function MM_findObj(n, d) 
        { //v4.01
        var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) 
        {d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
        if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
        for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
        if(!x && d.getElementById) x=d.getElementById(n); return x;
        }

function MM_swapImage() 
{ //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
  if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
function SwitchImg()
{
	var rem, keep=0, store, obj, switcher=new Array, history=document.Data;
	for (rem=0; rem < (SwitchImg.arguments.length-2); rem+=3)
	{
		store = SwitchImg.arguments[(navigator.appName == 'Netscape')?rem:rem+1];
		if ((store.indexOf('document.layers[')==0 && document.layers==null) || (store.indexOf('document.all[')==0 && document.all==null))
			store = 'document'+store.substring(store.lastIndexOf('.'),store.length);
		obj = eval(store);
		if (obj != null)
		{
			switcher[keep++] = obj;
			switcher[keep++] = (history==null || history[keep-1]!=obj)?obj.src:history[keep];
			obj.src = SwitchImg.arguments[rem+2];
		}
	}
	document.Data = switcher;
}
function RestoreImg()
{
	if (document.Data != null)
	for (var rem=0; rem<(document.Data.length-1); rem+=2)
		document.Data[rem].src=document.Data[rem+1];
}
