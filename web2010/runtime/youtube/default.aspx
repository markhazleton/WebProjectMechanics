<%@LANGUAGE="VBSCRIPT" CODEPAGE="65001"%>
<%
If Len(Request.QueryString("VideoID")) = 0 Then
Response.Write("No video found")
Else
%>
<!DOCTYPE html
PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
<title><%=Server.HTMLEncode(Request.QueryString("VideoTitle"))%> - YouTube Player</title>
<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />
<meta http-equiv="imagetoolbar" content="no" />
<meta name="robots" content="noindex, nofollow" />
<link href="player.css" rel="stylesheet" type="text/css" media="screen, tv, projection" />

<script type="text/javascript" src="slider.js"></script>
<script type="text/javascript" src="swfobject.js"></script>
<script type="text/javascript" src="youtube.js"></script>
</head>
<body onLoad="embedPlayer();">
<div id="ytWrapper">
<div id="ytimage"><a href="javascript:startNewVideo('<%=Server.HTMLEncode(Request.QueryString("VideoID"))%>', 0);"><span></span>
  <img src="http://img.youtube.com/vi/<%=Server.HTMLEncode(Request.QueryString("VideoID"))%>/0.jpg" alt ="<%=Server.HTMLEncode(Request.QueryString("VideoTitle"))%>" width="463" height="347" /></a>
</div>
<div id="ytapiplayer">You need Flash player 8+ and JavaScript enabled to view this video.</div>
<div id="controls">
    <ul id="buttons">
    	<li id="VideoRewind"><a href="javascript:rwd();" title="Rewind">Rewind</a></li>
    	<li id="VideoPlay"><a href="javascript:play();" title="Play">Play</a></li>
    	<li id="VideoPause"><a href="javascript:pause();" title="Pause">Pause</a></li>
    	<li id="VideoForward"><a href="javascript:fwd();" title="Forward">Forward</a></li>
    	<li id="VideoStop"><a href="javascript:stop();" title="Stop">Stop</a></li>
    	<li id="VideoMute" ><a href="javascript:mute();" title="Mute">Mute</a></li>
    	<li id="VideoUnMute"><a href="javascript:unMute();" title="Unmute">Unmute</a></li>
    	<li><div id="slider01"><span title="Volume"></span></div></li>
    </ul>
  <div id="timebar">
    <div id="bytesbarIndicator"></div>
    <div id="timebarIndicator"></div>
    <div id="timespan"><span id="videotime">--:--</span>/<span id="videoduration">--:--</span></div>
  </div>
</div>
</div>
<input id="VideoQuality" type="hidden" value="<%=Server.HTMLEncode(Lcase(Request.QueryString("Quality")))%>" />
<input id="loadvideoname" type="hidden" value="<%=Server.HTMLEncode(Request.QueryString("VideoTitle"))%>" />
<input id="loadvideoid" type="hidden" value="<%=Server.HTMLEncode(Request.QueryString("VideoID"))%>" />
<input id="output1" type="hidden" />
</div>
</body>
</html>
<%End If%>