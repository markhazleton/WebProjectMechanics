/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

Originally from:
http://programming.arantius.com/lightweight+javascript+slider+control

Copyright (c) 2006 Anthony Lieuallen, http://www.arantius.com/

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
//add event function from http://www.dynarch.com/projects/calendar/
function addAnEvent(el, evname, func) {
    if (el.attachEvent) { // IE
        el.attachEvent("on" + evname, func);
    } else if (el.addEventListener) { // Gecko / W3C
        el.addEventListener(evname, func, true);
    } else {
        el["on" + evname] = func;
    }
}

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function drawSliderByVal(slider) {
	var knob=slider.getElementsByTagName('span')[0];
	var pSlider=(slider.slidervalue-slider.min)/(slider.max-slider.min);
	var xSlider=(slider.scrollWidth-6)*pSlider;
	knob.style.width=xSlider+"px";
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function setSliderByClientX(slider, clientX) {
	var pSlider=(clientX-slider.offsetLeft-0)/(slider.scrollWidth-6);
	slider.slidervalue=(slider.max-slider.min)*pSlider + slider.min;
	if (slider.slidervalue>slider.max) slider.slidervalue=slider.max;
	if (slider.slidervalue<slider.min) slider.slidervalue=slider.min;

	drawSliderByVal(slider);
	slider.onchange(slider.slidervalue, slider.num);
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function sliderClick(e) {
	var el=sliderFromEvent(e);
	if (!el) return;

	setSliderByClientX(el, e.clientX);
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function sliderMouseMove(e) {
	var el=sliderFromEvent(e);
	if (!el) return;
	if (activeSlider<0) return;

	setSliderByClientX(el, e.clientX);
	stopEvent(e);
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function sliderFromEvent(e) {
	if (!e && window.event) e=window.event;
	if (!e) return false;

	var el;
	if (e.target) el=e.target;
	if (e.srcElement) el=e.srcElement;

	if (!el.id || !el.id.match(/slider\d+/)) el=el.parentNode;
	if (!el) return false;
	if (!el.id || !el.id.match(/slider\d+/)) return false;

	return el;
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
function attachSliderEvents() {
	var divs=document.getElementsByTagName('div');
	var divNum;
	for(var i=0; i<divs.length; i++) {
		if (divNum=divs[i].id.match(/\bslider(\d+)\b/)) {
			// set initial properties
			divNum=parseInt(divNum[1]);
			divs[i].min=slider[divNum].min;
			divs[i].max=slider[divNum].max;
			divs[i].slidervalue=slider[divNum].slidervalue;
			divs[i].onchange=slider[divNum].onchange;
			divs[i].num=divNum;
			// and make sure the display matches
			drawSliderByVal(divs[i]);
			divs[i].onchange(divs[i].slidervalue, divNum);

			addAnEvent(divs[i], 'mousedown', function(e){
				sliderClick(e);
				var el=sliderFromEvent(e);
				if (!el) return;
				activeSlider=el.num;
				stopEvent(e);
			});
			addAnEvent(divs[i], 'mouseup', function(e){
				activeSlider=-1;
				stopEvent(e);
			});
		}
	}
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
//borrowed from prototype: http://prototype.conio.net/
function stopEvent(event) {
	if (event.preventDefault) {
		event.preventDefault();
		event.stopPropagation();
	} else {
		event.returnValue=false;
		event.cancelBubble=true;

	}
}
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
addAnEvent(window, 'load', attachSliderEvents);
addAnEvent(document, 'mousemove', sliderMouseMove);
var activeSlider=-1;
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
