$(function(){
	var width = $(document).width();
	var height = $(document).height();
	
	//alert(width);
	try{
	parent.setIframe(width,height);
	}catch(e){}
});