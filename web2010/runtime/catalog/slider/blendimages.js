/*
 * Original from: http://brainerror.net/scripts/javascript/blendtrans/demo.html
 *
 * Edits by ASC:
 *   - Removed unnecessary OO code that caused MSIE to choke
 *   - Added pause between setting bg image and setting opacity to zero to
 *     prevent MSIE image flash
 *   - Increased opacity check argument to 103 for completely smooth fade
 *
 */

//find next image
function nextImage(o) {
    do o = o.nextSibling;
    while(o && o.tagName != 'IMG');
    return o;
}

//find first image inside an element
function firstChildImage(o) {
    o = o.firstChild;
    while(o && o.tagName != 'IMG') {
        o = o.nextSibling;
    }
    return o;
}

//set the opacity of an element to a specified value
function setOpacity(obj, o) {
    obj.style.opacity = (o / 100);
    obj.style.MozOpacity = (o / 100);
    obj.style.KhtmlOpacity = (o / 100);
    obj.style.filter = 'alpha(opacity=' + o + ')';
}

//make image invisible and set next one as current image
function getNextImage(image) {
    if (next = nextImage(image)) {
	image.style.display = 'none';
	image.style.zIndex = 0;
	next.style.display = 'block';
	next.style.zIndex = 100;
    } else {
	//if there is not a next image, get the first image again
	next = firstChildImage(image.parentNode);
    }
    return next;
}

//set default values for parameters and starting image
function blendImages(id, speed, pause, caption) {
    if(speed == null) {
        speed = 30;
    }
    
    if(pause == null) {
        pause = 1500;
    }

    var blend = document.getElementById(id);
    var image = firstChildImage(blend);
    startBlending(image, speed, pause, caption);
}

//make image a block-element and set the caption
function startBlending(image, speed, pause, caption) {

    image.style.display = 'block';
    if(caption != null) {
	document.getElementById(caption).innerHTML = image.alt;
    }
    continueFadeImage(image, 0, speed, pause, caption);
}

// ASC: copied from http://www.sean.co.uk/a/webdesign/javascriptdelay.shtm
function pausecomp(millis) {
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while(curDate-date < millis);
} 

//set an increased opacity and check if the image is done blending
function continueFadeImage(image, opacity, speed, pause, caption) {

    opacity = opacity + 3;
    if (opacity < 103) {
	setTimeout(function() {fadeImage(image, opacity, speed, pause, caption)}, speed);
    } else {
	//if the image is done, set it to the background and make it transparent
	image.parentNode.style.backgroundImage = "url("+image.src+")";
	// ASC: pause 1sec here to prevent MSIE image flash ...
	var paws=pause-1000;
	if (paws < 0 ) {
		paws = 0;
	}
	pausecomp(1000);
	setOpacity(image,0);
	//get the next image and start blending it again
	image = getNextImage(image);
	setTimeout(function() {startBlending(image, speed, pause, caption)}, paws);		
    }
}

//set the opacity to a new value and continue the fading
function fadeImage(image, opacity, speed, pause, caption) {
    setOpacity(image,opacity);
    continueFadeImage(image, opacity, speed, pause, caption);
}
