// -------------------------------------------------------------------
// Photo Album Script v2.0- By Dynamic Drive, available at: http://www.dynamicdrive.com
// Mar 11th, 07': Script updated to v2.0
// -------------------------------------------------------------------

function photogallery(garray, cols, rows, twidth, theight, paginatetext){
	gcount=(typeof gcount=="undefined")? 1 : gcount+1 //global var to keep count of current instance of photo gallery
	this.gcount=gcount
	this.galleryarray=garray
	this.cols=cols
	this.rows=rows
	var twidth=twidth || "700x"    //default table width is 700px
	var theight=theight || "500px"
	var ptext=(typeof paginatetext=="object")? paginatetext : ["Browse Gallery:", ""] //Store 2 compontents of paginate DIV text inside array
	this.pagecount=Math.ceil(this.galleryarray.length/(cols*rows)) //calculate number of "pages" needed to show the images
	document.write('<div class="photonavlinks" id="photogallerypaginate-'+gcount+'"></div><br/>') //Generate Paginate Div
	document.write('<table class="photogallery" id="photogallery-'+gcount+'" style="width:'+twidth+'; height:'+theight+';">') //Generate table for Photo Gallery
	for (var r=0; r<rows; r++){
		document.write('<tr>')
		for (var c=0; c<cols; c++)
			document.write('<td valign="top"></td>')
		document.write('</tr>')
	}
	document.write('</table>')

	var gdiv=document.getElementById("photogallery-"+this.gcount)
	var pdiv=document.getElementById("photogallerypaginate-"+this.gcount)
	gdiv.onselectphoto=function(imgobj, linkobj){return true} //custom event handler "onselectphoto", invoked when user clicks on an image within gallery
	this.showpage(gdiv, 0)
	this.createNav(gdiv, pdiv, ptext)
	gdiv.onclick=function(e){return photogallery.defaultselectaction(e, this)} //attach default custom event handler action to "onclick" event
	return gdiv
}


photogallery.prototype.createImage=function(imgparts){
	var imageHTML='<img src="'+imgparts[0]+'" title="'+imgparts[1]+'"/>'
	if (typeof imgparts[2]!="undefined" && imgparts[2]!=""){ //Create URL?
		var linktarget=imgparts[3] || ""
		imageHTML='<a href="'+imgparts[2]+'" target="'+linktarget+'">'+imageHTML+'</a>'
	}
	if (typeof imgparts[1]!="undefined" && imgparts[1]!="") //Display description?
		imageHTML+='<br />'+imgparts[1]
	return imageHTML
}


photogallery.prototype.showpage=function(gdiv, pagenumber){
	var totalitems=this.galleryarray.length //total number of images
	var showstartindex=pagenumber*(this.rows*this.cols) //array index of div to start showing per pagenumber setting
	var showendindex=showstartindex+(this.rows*this.cols) //array index of div to stop showing after per pagenumber setting
	var tablecells=gdiv.getElementsByTagName("td")
	for (var i=showstartindex, currentcell=0; i<showendindex && i<totalitems; i++, currentcell++) //Loop thru this page's images and populate cells with them
		tablecells[currentcell].innerHTML=this.createImage(this.galleryarray[i])
	while (currentcell<tablecells.length){ //For unused cells, if any, clear out its contents
		tablecells[currentcell].innerHTML=""
		currentcell++
	}
}

photogallery.prototype.createNav=function(gdiv, pdiv , ptext){
	var instanceOfGallery=this
	var navHTML=""
	for (var i=0; i<this.pagecount; i++)
		navHTML+='<a href="#navigate" rel="'+i+'">'+ptext[1]+(i+1)+'</a> ' //build sequential nav links
	pdiv.innerHTML=ptext[0]+' '+navHTML
	var navlinks=pdiv.getElementsByTagName("a")
	navlinks[0].className="current" //Select first link by default
	this.previouspage=navlinks[0] //Set previous clicked on link to current link for future ref
	for (var i=0; i<navlinks.length; i++){
		navlinks[i].onclick=function(){
			instanceOfGallery.previouspage.className="" //"Unhighlight" last link clicked on...
			this.className="current" //while "highlighting" currently clicked on flatview link (setting its class name to "selected"
			instanceOfGallery.showpage(gdiv, this.getAttribute("rel"))
			instanceOfGallery.previouspage=this //Set previous clicked on link to current link for future ref
			return false
		}
	}
}

photogallery.defaultselectaction=function(e, gdiv){ //function that runs user defined "onselectphoto()" event handler
	var evtobj=e || window.event
	var clickedobj=evtobj.target || evtobj.srcElement
	if (clickedobj.tagName=="IMG"){
		var linkobj=(clickedobj.parentNode.tagName=="A")? clickedobj.parentNode : null
		return gdiv.onselectphoto(clickedobj, linkobj)
	}
}