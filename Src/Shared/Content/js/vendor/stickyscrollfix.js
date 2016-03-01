var windowElem = "#s4-workspace";
var elem = ".ia-action-menu-content";
var ribbonElem = "#s4-ribbonrow";

$(document).ready(function () {
    ribbonPosition = $(ribbonElem).position().top;
    if ($(elem).length > 0) {
        var eTop = $(elem).offset().top;


        var initialRibbonHeight = $(ribbonElem).height();
        var topScrollPos = eTop - $(windowElem).scrollTop() - initialRibbonHeight;

        $(windowElem).scroll(function () {

            var ribbonHeight = $(ribbonElem).height();
            var pxFromTop = eTop - $(windowElem).scrollTop() - ribbonHeight;
            if (pxFromTop < 0) {
                //$(elem).css("position", "absolute");
                var right = $(elem).offset().right;
                $(elem).offset({ top: ribbonHeight + 30, right: right });
            }
            else if (pxFromTop >= topScrollPos) {
                //Hits Top Position - Return back to normal
                //$(elem).css("position", "static");
                var right = $(elem).offset().right;
                $(elem).offset({ top: eTop, right: right });
            }
        });
    }
    
});
