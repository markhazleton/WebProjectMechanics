  <style type="text/css">
	.spanSlide {
		position: absolute;
		background: #FFF;
		overflow: hidden;
		border: 2px solid #000;
	}
	.imgSlide {
		position: absolute;
		left: 0%;
		top: 0%;
		width: 100%;
		height: 100%;
		overflow: hidden;
	}
	.txtSlide {
		position: absolute;
		top: 5%;
		left: 50px;
		width:100%;
		color:#FFF;
		font-family: arial, helvetica, verdana, sans-serif;
		font-weight: bold;
		font-size:36px;
		letter-spacing:12px;
		filter: alpha(opacity=70);
		-moz-opacity:0.7;
		opacity:0.7;
		white-space: nowrap;
	}
</style>
<script type="text/javascript">
// =======================================================================
// script by Gerard Ferrandez - Ge-1-doot - Mars 2006
// original version: http://perso.wanadoo.fr/serge.knopf/anim/Venise.htm
// http://www.dhteumeuleu.com
// =======================================================================

var ym=10;
var ny=0;
var leftoffset=1.60;
var topoffset=1.25;
var speed=20;
var imagecount=<ImageCount>;
var zoomfactor=5;

createElement = function(container, type, param){
	o=document.createElement(type);
	for(var i in param)o[i]=param[i];
	container.appendChild(o);
	return o;
}

mooz = {
	O:[],
	/////////
	mult:zoomfactor,
	nbI:imagecount,
	/////////
	rwh:0,
	imgsrc:0,
	W:0,
	H:0,

	Xoom:function(N){
		this.o = createElement(document.getElementById("screen"), "span", {
			'className':'spanSlide'
		});
		img = createElement(this.o, "img", {
			'className':"imgSlide",
			'src':mooz.imgsrc[N%mooz.imgsrc.length].src
		});
		spa = createElement(this.o, "span", {
			'className':"imgSlide"
		});
		txt = createElement(spa, "span", {
			'className':"txtSlide",
			'innerHTML':mooz.imgsrc[N%mooz.imgsrc.length].title
		});
		this.N = 10000+N;
	},

	mainloop:function(){
		with(this){
			for(i=0; i<imagecount; i++) {
				O[i].N += (ym-ny)/8000;
				N = O[i].N%nbI;
				ti = Math.pow(mult,N);
				with(O[i].o.style){
					left   = Math.round((W-(ti*rwh))/(W+ti)*(W*leftoffset))+"px";
					top    = Math.round((H-ti)/(H+ti)*(H*topoffset))+"px";
					zIndex = Math.round(10000-ti*.1);
					width  = Math.round(ti*rwh)+"px";
					height = Math.round(ti)+"px";
				}
			}
		}
		setTimeout("mooz.mainloop();", 16);
	},

	oigres:function(){
		with(this){
			W = parseInt(document.getElementById("screen").style.width);
			H = parseInt(document.getElementById("screen").style.height);
			imgsrc = document.getElementById("images").getElementsByTagName("img");
			rwh = imgsrc[0].width/imgsrc[0].height;
			for(var i=0;i<imagecount;i++) O[i] = new Xoom(i);
			mainloop();
		}
	}
}


window.onload = function(){
	ym = ny+speed;
	mooz.oigres();
}

</script>
<div id="screen" style="position:relative;width:100%;height:400px;overflow:hidden"></div>
<div id="images" style="visibility:hidden;height:1px;width:1px;overvlow:hidden">
   <PageImageArray>
</div>
