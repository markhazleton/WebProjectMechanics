// YouTube Chromeless Player with Google Analytics Event Tracking
// Made by Eivind Savio 2008
// http://www.savio.no
// Explanation of this YouTube Chromeless Player can be found at
// http://www.savio.no/blogg/a/53/youtube-chromeless-player-with-google-analytics-event-tracking

// Event Tracking is based on a blog post by Justin Cutroni, Analytics Talk
// http://www.epikone.com/blog/2008/07/29/tracking-youttube-videos-with-google-analytics/

// For information about the YouTube Chromeless Player, see 
// http://code.google.com/apis/youtube/chromeless_player_reference.html
// and the API reference
// http://code.google.com/apis/youtube/js_api_reference.html

// For information about Google Analytics Event Tracking, see
// http://code.google.com/apis/analytics/docs/eventTrackerOverview.html

// Volume slider is based on a script from Arantius.com
// http://programming.arantius.com/lightweight+javascript+slider+control

// This code is provided As Is with no warranty expressed or implied. 
// I am not liable for anything that results from your use of this code.


var ytpEventTracker = pageTracker._createEventTracker("YouTube Video Player");
var eventLabel;
var nowPlaying;
var ytplayer;

function embedPlayer() {
// Hack for playing embedded YouTube videos in high quality. I do not guarantee that the use of this solution will work in the future.
var VideoQuality= document.getElementById('VideoQuality').value;
var ytQuality = '&amp;ap=%2526fmt%3D18';
	if (VideoQuality == 'high') {
		ytQuality = ytQuality;
	} else {
 		ytQuality = '';
}
//High Quality Video code end
    var params = { allowScriptAccess: 'always', bgcolor: '#D5DDB3' };
    var atts = { id: 'myytplayer' };
    swfobject.embedSWF('http://www.youtube.com/apiplayer?enablejsapi=1&amp;playerapiid=ytplayer' + ytQuality, 
            'ytapiplayer', '463', '365', '8', null, null, params, atts);
}

function updateHTML(elmId, value) {
	document.getElementById(elmId).innerHTML = value;
}

function setytplayerState(newState) {
		if (translateYTPState(newState) == 'ended' && parseInt(getCurrentTime()) > 0) { 
		recordEnd(eventLabel,parseInt(getCurrentTime()));
		}
}

function onYouTubePlayerReady(playerId) {
	ytplayer = document.getElementById('myytplayer');
	setInterval(updateytplayerInfo, 250);
	updateytplayerInfo();
	ytplayer.addEventListener('onStateChange', 'onytplayerStateChange');
		document.getElementById('controls').style.display = 'block';
	// Google Analytics Event Tracking
	ytpEventTracker._trackEvent("Video Player Loaded", eventLabel);
}

function onytplayerStateChange(newState) {
	setytplayerState(newState);
}

function translateYTPState(state) {
	switch (state) {
		case -1: return "unstarted";
		case 0 : return "ended";
		case 1 : return "playing";
		case 2 : return "paused";
		case 3 : return "buffering";
		case 5 : return "video cued";
	}
	return;
}

function onPlayerError(errorCode) {
	alert("An error occurred: "+ errorCode);
		//Google Analytics Event Tracking
		ytpEventTracker._trackEvent("Error: " + errorCode, eventLabel);
}
	
// functions for the YouTube API calls
function loadNewVideo(id, startSeconds) {
	if (ytplayer) {
		ytplayer.loadVideoById(id, startSeconds);
	}
}

function cueNewVideo(id, startSeconds) {
	if (ytplayer) {
		ytplayer.cueVideoById(id, startSeconds);
	}
}

function startNewVideo () {
// if there is a current video playing, record the end
	var oldTime = parseInt(getCurrentTime());
	var oldEventLabel = eventLabel;

	if (oldTime > 0) { 
		recordEnd(oldEventLabel, oldTime);
	}	
		eventLabel = (document.getElementById('loadvideoname').value);
		loadNewVideo(document.getElementById('loadvideoid').value, 0);
		ytpEventTracker._trackEvent("Video - Started", eventLabel);
	  		//Hiding/showing content on the video HTML page and unmuting the player when the video starts.
		HideContent('ytimage');
		HideContent('VideoPlay');
		ytplayer.unMute();
		HideContent('VideoUnMute');
}

function getBytesLoaded() {
	if (ytplayer) {
		return ytplayer.getVideoBytesLoaded();
	}
}

function getBytesTotal() {
	if (ytplayer) {
		return ytplayer.getVideoBytesTotal();
	}
}

function getCurrentTime() {
	if (ytplayer) {
		return ytplayer.getCurrentTime();
	}
}

function getDuration() {
	if (ytplayer) {
		return ytplayer.getDuration();
	}
}

function recordEnd(l,t) {
	//Reset Play/Pause button when video ends
	HideContent('VideoPause');
	ShowContent('VideoPlay');
	//Google Analytics Event Tracking
	ytpEventTracker._trackEvent("Video - Ended", l, parseInt(t));
}

function play() {
	if (ytplayer) {
		ytplayer.playVideo();
			//Google Analytics Event Tracking
			ytpEventTracker._trackEvent("Video - Play",eventLabel);
	}
		//Not part of the original YouTube or Event Tracking code. Hiding/showing content on the video HTML page.
		ShowContent('VideoPause');
		HideContent('VideoPlay');
}

function pause() {
	if (ytplayer) {
		ytplayer.pauseVideo();
		//Google Analytics Event Tracking		
		ytpEventTracker._trackEvent("Video - Pause",eventLabel);
	}
		//Hiding/showing content on the video HTML page.	
		ShowContent('VideoPlay');
		HideContent('VideoPause');
}
		
function stop() {
if (ytplayer) {
	//Google Analytics Event Tracking
	ytpEventTracker._trackEvent("Video - Stop",eventLabel,parseInt(getCurrentTime()));
	ytplayer.stopVideo();
	}
		//Hiding/showing content on the video HTML page.	
		ShowContent('ytimage');
		HideContent('ytapiplayer');
}	

function getPlayerState() {
	if (ytplayer) {
		return ytplayer.getPlayerState();
	}
}

// FORWARD
function fwd(fwdSeconds, allowSeekAhead) {
	if (ytplayer) {
	var fwdSeconds = getCurrentTime()+10;
	ytplayer.seekTo(fwdSeconds);
		//Google Analytics Event Tracking
		ytpEventTracker._trackEvent("Video - Forward",eventLabel);
	}
}

// REWIND
function rwd(rwdSeconds) {
	if (ytplayer) {
	var rwdSeconds = getCurrentTime()-10;
	ytplayer.seekTo(rwdSeconds);
		//Google Analytics Event Tracking
		ytpEventTracker._trackEvent("Video - Rewind",eventLabel);
	}
}

function getStartBytes() {
	if (ytplayer) {
		return ytplayer.getVideoStartBytes();
	}
}

// MUTE
function mute() {
	if (ytplayer) {
		ytplayer.mute();
		//Google Analytics Event Tracking
		//ytpEventTracker._trackEvent("Video - Mute",eventLabel);
	}
		//Not part of the original YouTube Event Tracking code. Hiding/showing content on the video HTML page.
		ShowContent('VideoUnMute');
		HideContent('VideoMute');
}

// UNMUTE
function unMute() {
	if (ytplayer) {
		ytplayer.unMute();
		//Google Analytics Event Tracking
		//ytpEventTracker._trackEvent("Video - Unmute",eventLabel);
	}
		//Hiding/showing content on the video HTML page.	
		ShowContent('VideoMute');
		HideContent('VideoUnMute');
}	
		
// Volume slider. //Not part of the original YouTube or Event Tracking code. The slider code is working together with slider.js
var slider=new Array();
	slider[1]=new Object();
	slider[1].min=0;
	slider[1].max=100;
	slider[1].slidervalue=70;
	slider[1].onchange=setSliderValue;
function setSliderValue(slidervalue) {
	var b=document.getElementById('output1');
	slidervalue=Math.round(slidervalue*1000)/1000;
	b.value=Math.round(slidervalue);

	if (ytplayer) {
		ytplayer.setVolume(slidervalue);
	}
}
		
function getVolume() {
	if (ytplayer) {
		return ytplayer.getVolume();
	}
}

//Calculating time so time less than 10 are shown with a 0 in front, ex. 09.		
function splitTime(a)
	{
		var tm=new Date(a*1000)
		var hours=Math.round(tm.getUTCHours());
		var minutes=Math.round(tm.getUTCMinutes());
		var seconds=Math.round(tm.getUTCSeconds()); 

	if (hours > 0){
		timeStr= "" + hours;
		timeStr+= ((minutes < 10) ? ":0" : ":") + minutes;
		timeStr+= ((seconds < 10) ? ":0" : ":") + seconds;
	}
	else{
		timeStr= "";
		timeStr= ((minutes < 10) ? "0" : ":") + minutes;
		timeStr+= ((seconds < 10) ? ":0" : ":") + seconds;
		} 
	
return timeStr
} 

function updateytplayerInfo() {
    if (ytplayer) {
        updateTimebar();
		updateBytesbar();
        updateHTML("videoduration", splitTime(getDuration()));
        updateHTML("videotime", splitTime(getCurrentTime()));
	}
}

function updateTimebar() {
    var all = ytplayer.getDuration();
    var part = ytplayer.getCurrentTime();
    var percent = getPercent1(all, part);
    var timebarWidth = 100;
    document.getElementById('timebarIndicator').style.left = percent * (timebarWidth / 100) + "%";
}

function updateBytesbar() {
    var all = ytplayer.getVideoBytesTotal();
    var partBytes = ytplayer.getVideoBytesLoaded();
    var percentBytes = getPercent2(all, partBytes);
    var bytesbarWidth = 100;
    document.getElementById('bytesbarIndicator').style.width = percentBytes * (bytesbarWidth / 100) + "%";
}

function getPercent1(all, part) {
	return (all > 0) ? (100 / all) * part : 0;
}

function getPercent2(all, partBytes) {
	return (all > 0) ? (100 / all) * partBytes : 0;
}

function clearVideo() {
	if (ytplayer) {
		ytplayer.clearVideo();
	}
}


// Show/hide content on the video HTML page. Not part of the YouTube or Google Analytics Javascript
function HideContent(d) {
	if(d.length < 1) { return; }
		document.getElementById(d).style.display = "none";
}
function ShowContent(d) {
	if(d.length < 1) { return; }
		document.getElementById(d).style.display = "block";
}
function ReverseContentDisplay(d) {
	if(d.length < 1) { return; }
	if(document.getElementById(d).style.display == "none") { document.getElementById(d).style.display = "block"; }
	else { document.getElementById(d).style.display = "none"; }
}