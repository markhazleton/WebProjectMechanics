/*******************
Author: Patrick Ryan
URL: http://www.agavegroup.com
Feel free to use this however you like.  Credit is always appreciated.
*******************/

	//need to set a couple of images here:
imageCheckboxChecked = "/runtime/form/checkboxChecked.gif";
imageCheckboxUnchecked = "/runtime/form/checkboxUnchecked.gif";
imageRadioChecked = "/runtime/form/radiobuttonChecked.gif";
imageRadioUnchecked = "/runtime/form/radiobuttonUnchecked.gif";
imageSelectDropDownArrow = "/runtime/form/selectDrop.gif";
	
	//the rest of the images are in the CSS


	function prettyForms(){
		fixTextBoxes();
		fixTextareas();
		fixSelects();
		fixChecks();
		fixRadios();
		fixSubmits();
	}
	
	
//****
//**** functions that apply the look to the form elements
//****	

	//this function is run for all form elements (except radio buttons)
	//this function accepts one element, and wraps it in four divs that are styled with shadows
	function appendParentsTo(currItem){
		//create the divs
		tl = document.createElement("div");
		br = document.createElement("div");
		bl = document.createElement("div");
		tr = document.createElement("div");

		if(document.all){							//IE
			//give them the proper class
			tl.className="frmShdwTopLt";
			br.className="frmShdwBottomRt";
			bl.className="frmShdwBottomLt";
			tr.className="frmShdwTopRt";
			//insert the top level div
			t1=currItem.insertAdjacentElement("BeforeBegin",tl);
		}else{										//FFX
			//give them the proper class
			tl.setAttribute("class", "frmShdwTopLt");
			br.setAttribute("class", "frmShdwBottomRt");
			bl.setAttribute("class", "frmShdwBottomLt");
			tr.setAttribute("class", "frmShdwTopRt");
			inputParent = currItem.parentNode;
			//insert the top level div
			tl = inputParent.insertBefore(tl, currItem);
		}
		
		//append children
		br = tl.appendChild(br);
		bl = br.appendChild(bl);
		tr = bl.appendChild(tr);
		//move input to child of divs
		tr.appendChild(currItem);
	}


	//apply look to text boxes
	function fixTextBoxes(){
		inputs = document.getElementsByTagName("input");
		for(i=0;i<inputs.length;i++){
			if(inputs[i].type=="text"){
				appendParentsTo(inputs[i]);
			}
		}
	}
	
	//apply look to textareas
	function fixTextareas(){
		textareas = document.getElementsByTagName("textarea")
		for(i=0;i<textareas.length;i++){
			appendParentsTo(textareas[i]);
		}
	}
	
	
	//apply look to submit buttons
	function fixSubmits(){
		inputs = document.getElementsByTagName("input");
		for(i=0;i<inputs.length;i++){
			if(inputs[i].type=="submit"){
				appendParentsTo(inputs[i]);
				inputs[i].className="frmShdwSubmit";
			}
		}
	}
	
	
	//apply look to radio buttons
	function fixRadios(){
		inputs = document.getElementsByTagName("input");
		for(i=0;i<inputs.length;i++){
			if(inputs[i].type=="radio"){
				lnk = document.createElement("a");
				lnk.style.margin="4px";
				lnk.className="frmShdwRadio";
				img = document.createElement("img");
				if(inputs[i].checked==true){
					img.src = imageRadioChecked;
				}else{
					img.src = imageRadioUnchecked;
				}
				
				//elements created, now pass functionality
				//give the checkbox an id if it doesn't have one
				if(inputs[i].id){
					realId = inputs[i].id;
				}else{
					realId = "radio"+i;
					inputs[i].id = realId;
				}
				
				//give the fake check an id
				fakeId = "fake"+realId;
				img.id=fakeId
				
				lnk.href="javascript:toggleRadio('"+realId+"','"+fakeId+"')";
			
				//insert the new image into the document
				if(document.all){				//IE
					lnk = inputs[i].insertAdjacentElement("BeforeBegin",lnk)
				}else{
					inputParent = inputs[i].parentNode;
					lnk = inputParent.insertBefore(lnk,inputs[i]);
				}
				lnk.appendChild(img);
				
				//remove the actual checkbox
				inputs[i].style.display="none";
			}
		}
	}
	
	
	
	//apply look to check boxes
	function fixChecks(){
		inputs = document.getElementsByTagName("input");
		for(i=0;i<inputs.length;i++){
			if(inputs[i].type=="checkbox"){
				appendParentsTo(inputs[i]);
				//have shadow box, now replace checkbox with image of check, or no image.
				//need to create an <a> element AND an <img> element because IE won't happily put an onclick on the img alone
				lnk = document.createElement("a");
				lnk.style.margin="4px";
				lnk.className="frmShdwCheck";
				img = document.createElement("img");
				
				
				if(inputs[i].checked==true){
					img.src = imageCheckboxChecked;
				}else{
					img.src = imageCheckboxUnchecked;
				}
				
				
				//elements created, now pass functionality
				//give the checkbox an id if it doesn't have one
				if(inputs[i].id){
					realId = inputs[i].id;
				}else{
					realId = "check"+i;
					inputs[i].id = realId;
				}
				
				//give the fake check an id
				fakeId = "fake"+realId;
				img.id=fakeId
				
				lnk.href="javascript:toggleCheck('"+realId+"','"+fakeId+"')";
				
				
				//insert the new image into the document
				if(document.all){				//IE
					lnk = inputs[i].insertAdjacentElement("BeforeBegin",lnk)
				}else{
					inputParent = inputs[i].parentNode;
					lnk = inputParent.insertBefore(lnk,inputs[i]);
				}
				lnk.appendChild(img);
				
				//remove the actual checkbox
				inputs[i].style.display="none";
			}
		}
	}
	
	//apply look to select boxes
	function fixSelects(){
		selects = document.getElementsByTagName("select")
		for(i=0;i<selects.length;i++){
			//create the standard shadows
			appendParentsTo(selects[i]);
			
			//give this thing an id if it doesn't have one
			if(selects[i].id==""){
				selects[i].id="dynId"+i;
			}
			
			//create new div to hold list
			//this is a wrapper div to hold everything together
			fakeSelectWrapper = document.createElement("div");
			
			//this is the link that holds the select's drop down arrow
			fakeSelectIcon = document.createElement("a")
			
			if(document.all){				//IE
				fakeSelectIcon.href="javascript:dropDownMenu(\"frmShdwMenu"+i+"\", \"frmShdwMenuChosen"+i+"\",\""+selects[i].id+"\")";
				fakeSelectIcon.innerHTML = "<img class=\"fakeSelectImg\" src=\""+imageSelectDropDownArrow+"\" />";
				//this is the div that actually contains the list of options
				fakeSelect = document.createElement("div");
				fakeSelect.id="frmShdwMenu"+i;
				fakeSelect.className="frmShdwSelectDrop";
				options = selects[i].getElementsByTagName("option");
				//this div is displayed when the box is NOT dropped down.  Shows currently displayed item
				fakeSelectedHolder = document.createElement("a");

				fakeSelectedHolder.className="frmShdwSelectDropChosen";
				fakeSelectedHolder.id="frmShdwMenuChosen"+i;
				fakeSelectedHolder.style.width=selects[i].clientWidth-15+"px";
				fakeSelectedHolder.href="javascript:dropDownMenu(\"frmShdwMenu"+i+"\", \"frmShdwMenuChosen"+i+"\",\""+selects[i].id+"\")";

				
				for(j=0;j<options.length;j++){
					//create a p tag for each element, and append it to the parent div
					fakeOption = document.createElement("a")
					fakeOption.innerHTML = options[j].innerHTML;
					fakeOption.style.width=selects[i].clientWidth-16+"px";
					//here's some crazy IE stuff.
					fakeOption.href='javascript:chooseSelect("'+selects[i].id+'",'+j+',"frmShdwMenu'+i+'", "frmShdwMenuChosen'+i+'")'
					fakeSelect.appendChild(fakeOption);
					//set the default text to show
					if(options[j].selected==true){
						fakeSelectedHolder.innerHTML=options[j].innerHTML;
						fakeOption.className="selected";
					}
				}
				
				//construct the menu parts Wrapper around list of options and image
				fakeSelectWrapper.appendChild(fakeSelectedHolder);
				fakeSelectWrapper.appendChild(fakeSelect);
				fakeSelectWrapper.appendChild(fakeSelectIcon);
				
				//now put the new div inside the shadows, above the select box
				selectParent = selects[i].parentNode;
				fakeSelect.style.width=selects[i].clientWidth-15+"px";
				
				// more crazy IE stuff : push the dropped down menu to the left where it belongs
				fakeSelect.style.margin="3px 3px 3px -"+(selects[i].clientWidth-5)+"px";
				
				
				fakeSelectWrapper = selects[i].insertAdjacentElement("BeforeBegin",fakeSelectWrapper)
				//hide the real select box
				selects[i].style.display="none"; 
				
				
			}else{
				fakeSelectIcon.setAttribute("href","javascript:dropDownMenu(\"frmShdwMenu"+i+"\", \"frmShdwMenuChosen"+i+"\",\""+selects[i].id+"\")");
				fakeSelectIcon.innerHTML = "<img class=\"fakeSelectImg\" src=\""+imageSelectDropDownArrow+"\" />";
				//this is the div that actually contains the list of options
				fakeSelect = document.createElement("div");
				fakeSelect.setAttribute("id","frmShdwMenu"+i);
				fakeSelect.setAttribute("class","frmShdwSelectDrop");
				options = selects[i].getElementsByTagName("option");
				//this div is displayed when the box is NOT dropped down.  Shows currently displayed item
				fakeSelectedHolder = document.createElement("div");
				fakeSelectedHolder.setAttribute("class","frmShdwSelectDropChosen");
				fakeSelectedHolder.setAttribute("id","frmShdwMenuChosen"+i);
				fakeSelectedHolder.style.width=selects[i].clientWidth-15+"px";
				fakeSelectedHolder.setAttribute("onclick","javascript:dropDownMenu(\"frmShdwMenu"+i+"\", \"frmShdwMenuChosen"+i+"\",\""+selects[i].id+"\")");
				
				
				for(j=0;j<options.length;j++){
					//create a p tag for each element, and append it to the parent div
					fakeOption = document.createElement("a")
					fakeOption.innerHTML = options[j].innerHTML;
					fakeOption.setAttribute("href","javascript:chooseSelect(\""+selects[i].id+"\","+j+",\"frmShdwMenu"+i+"\", \"frmShdwMenuChosen"+i+"\")");	//clicking calls the function chooseSelect passing the select object, and the chosen index
					fakeSelect.appendChild(fakeOption);
					
					//set the default text to show
					if(options[j].selected==true){
						fakeSelectedHolder.innerHTML=options[j].innerHTML;
						fakeOption.setAttribute("class","selected");
					}				
				}
				
				//construct the menu parts Wrapper around list of options and image
				fakeSelectWrapper.appendChild(fakeSelectedHolder);
				fakeSelectWrapper.appendChild(fakeSelect);
				fakeSelectWrapper.appendChild(fakeSelectIcon);
				
				//now put the new div inside the shadows, above the select box
				selectParent = selects[i].parentNode;
				fakeSelect.style.width=selects[i].clientWidth-15+"px";
				fakeSelectWrapper = selectParent.insertBefore(fakeSelectWrapper,selects[i]);
				//hide the real select box
				selects[i].style.display="none"; 
			}
		}
	}
	
	
	
	
	
	
//****
//**** functions that apply the functionality to the form elements
//****		
	
	//function runs when a radio button is clicked
	function toggleRadio(realRadioId, fakeRadioId){
		realRadio = document.getElementById(realRadioId);
		fakeRadio = document.getElementById(fakeRadioId);
		//want to ONLY look in the correct form, so get this radio button's parent form (supports multiple forms)
		radioForm = realRadio.parentNode;
		tmpCnt=1;
		while(radioForm.tagName!="FORM"){
			radioForm = radioForm.parentNode;
			tmpCnt++;
			if(tmpCnt>50){
				window.alert("encountered javascript error\n[parentNode]")
				break;
			}
		}	
		inputs=radioForm.getElementsByTagName("input");
		for(i=0;i<inputs.length;i++){
			if(inputs[i].type=="radio"){		
				//IDs look like this:  realId: blah    fakeId: fakeblah
				if(inputs[i].name==realRadio.name){	//is part of the same radio group, uncheck it.
					inputs[i].checked=false;	//uncheck the actual button
					document.getElementById("fake"+inputs[i].id).src=imageRadioUnchecked;
					if(inputs[i].id==realRadioId){
						inputs[i].checked=true;	//check the actual button
						document.getElementById("fake"+inputs[i].id).src=imageRadioChecked;
					}
				}
			}
		}
		
		//**** EVENT HANDLING
		// Clicking the radiobutton equivalent to the button's onClick event and onChange event . fire it.
		triggerEvent(realRadio,"change");
		triggerEvent(realRadio,"click");
	}
	

	//this function handles the actual check box handling
	function toggleCheck(realCheckId, fakeCheckId){
		fakeCheck = document.getElementById(fakeCheckId);
		realCheck = document.getElementById(realCheckId);
		if(fakeCheck.src.indexOf("checkboxChecked.gif") != -1){
			fakeCheck.src = imageCheckboxUnchecked;
		}else{
			fakeCheck.src = imageCheckboxChecked;
		}
		
		if(realCheck.checked==true){
			realCheck.checked=false;
		}else{
			realCheck.checked=true;
		}
		
		//**** EVENT HANDLING
		// Clicking the box equivalent to the box's onClick event and onChange event . fire it.
		triggerEvent(realCheck,"change");
		//NOTE cannot use click event on checkbox - it causes bubbling (that cannot be prevented:mozilla bug?) and the change event gets fired multiple times
		
	}
	
	
	//function runs when drop down arrow next to select box is clicked
	function dropDownMenu(menuId, chosenMenuId, realMenuId){
		//hide the default Text item
		//document.getElementById(chosenMenuId).style.display="none";
		//show the full list
		document.getElementById(menuId).className="frmShdwSelectDropShown";
		
		//**** EVENT HANDLING
		// Clicking the list is equivalent to the selects onClick event. fire it.
		realMenu = document.getElementById(realMenuId);
		if(document.all){
			res = realMenu.fireEvent("onclick");
		}else{
			mouseEvent = document.createEvent('MouseEvents');
			mouseEvent.initMouseEvent('click',true,true,window,1,0,0,0,0,false,false,false,false,0,null);
			realMenu.dispatchEvent(mouseEvent); 
		}
		
	}
	
	//function runs when a drop down item is clicked
	function chooseSelect(chosenSelect,chosenIndex,menuId, chosenMenuId){

		realDropdown = document.getElementById(chosenSelect);
		fakeDropDown = document.getElementById(menuId);
		fakeChosenItem = document.getElementById(chosenMenuId)
		//set the chosen item to be selected in the REAL drop down		
		currSelect = realDropdown.selectedIndex=chosenIndex;
				
		//put the chosen text into the display div
		//for some reason setting innerHTML breaks in IE
		fakeChosenItem.childNodes[0].nodeValue=realDropdown[chosenIndex].innerHTML;
		
		//deselect all the items under the dropdown
		fakeOptions=fakeDropDown.getElementsByTagName("a");
		
		for(i=0;i<fakeOptions.length;i++){
			fakeOptions[i].className="";
			//if this is the selected item, set to selected
			if(fakeOptions[i].innerHTML ==  realDropdown[chosenIndex].innerHTML){
				fakeOptions[i].className="selected";
			}
		}
		
		//hide the rest of the dropdown
		fakeDropDown.className="frmShdwSelectDrop";
		
		//show the display div
		fakeChosenItem.style.display="block";
		
		//**** EVENT HANDLING
		// Choosing an item is equivalent to the selects onChange event. fire it.
		triggerEvent(realDropdown,"change");
		
	}
	
	
	
	
	//function to trigger events that are built into the form elements that have been hidden
	function triggerEvent(obj, evt){
		if(document.all){
			if(evt=="click"){
				res = obj.fireEvent("onclick");
			}else if(evt=="change"){
				res = obj.fireEvent("onchange");
			}
		}else{
			//NOTE - in the mozilla event model, I am cancelling the bubbleUp!  (1st false)
			// this is needed to prevent odd interaction, but could cause other issues!
			if(evt=="click"){
				mouseEvent = document.createEvent('MouseEvents');
				mouseEvent.initMouseEvent('click',true,true,window,1,0,0,0,0,false,false,false,false,0,null);
				obj.dispatchEvent(mouseEvent); 
			}else if(evt=="change"){
				mouseEvent = document.createEvent('HTMLEvents');
				mouseEvent.initEvent('change',true,true,window,1,0,0,0,0,false,false,false,false,0,null);
				obj.dispatchEvent(mouseEvent);
			}
		}
	}
	
