<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Map.ascx.cs" Inherits="Akumina.WebParts.Miscellaneous.Map.Map" %>
<script src="https://maps.googleapis.com/maps/api/js"></script>
<%--<script src="http://code.jquery.com/jquery-1.11.3.min.js"></script>--%>
<div class="interAction">
<div class="ia-control-header">
<span class="ia-control-header-icon fa fa-car"></span>
<h3 class="ia-control-header-heading">Traffic</h3>
    
</div>
 <style>
      #map {
        width: 100%;
        height: 270px;
      }
    </style>
    
<div class="ia-traffic">
   <div id="map"></div>
	<div>
	<input type="text" id="locationTrafficSearch" PlaceHolder="Enter City, State or Zip" />
	<input type="button" id="trafficGoSearch" value="Go" />
	</div>

 </div>
<script>
    
    var browser = {
        isIe: function () {
            return navigator.appVersion.indexOf("MSIE") != -1;
        },
        navigator: navigator.appVersion,
        getVersion: function () {
            var version = 999; // we assume a sane browser
            if (navigator.appVersion.indexOf("MSIE") != -1)
                // bah, IE again, lets downgrade version number
                version = parseFloat(navigator.appVersion.split("MSIE")[1]);
            return version;
        }
    };

    if (browser.isIe() && browser.getVersion() <= 9) {

        function CheckIsloadGoogleMap() {
            if($('#map').html() ==""){
                CallInitializeGoogleMaps();
            }
            else{ 
                clearInterval(intervalTraffic);
            }
        }

        var intervalTraffic = setInterval(CheckIsloadGoogleMap, 1000);
    }

    function initializeGoogleMapControl(location, newCookie) {

        var googleMapUrlLocation = "http://maps.googleapis.com/maps/api/geocode/json?address=" + location;
        if (browser.isIe() && browser.getVersion() <= 9) {
            // Use Microsoft XDR
            var xdr = new XDomainRequest();
            xdr.open("get", googleMapUrlLocation);
            xdr.onload = function () {
                var JSON = $.parseJSON(xdr.responseText);
                if (JSON == null || typeof (JSON) == 'undefined') {
                    JSON = $.parseJSON(data.firstChild.textContent);
                }
                processDataTraffic(JSON, newCookie);
            };
            xdr.send();
        } else {
            $.getJSON(googleMapUrlLocation).done(function (data) {
                processDataTraffic(data, newCookie);
            });
        }
    }
    function processDataTraffic(data, newCookie) {
        try {

            var obj = data.results[0]["geometry"]["location"];
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 13,
                center: obj
            });
            var trafficLayer = new google.maps.TrafficLayer();
            trafficLayer.setMap(map);
            if (newCookie) {
                createCookie("TrafficLocation_InterActionGoogle", $('#locationTrafficSearch').val(), "");
            }
        }
        catch (exception) {

        }
    }
    
    function CallInitializeGoogleMaps() {
        var cookieLocation = readCookie("TrafficLocation_InterActionGoogle") == null ? "Boston" : readCookie("TrafficLocation_InterActionGoogle");
        initializeGoogleMapControl(cookieLocation);
    }

    $(document).ready(function () {
        CallInitializeGoogleMaps();
        $('#trafficGoSearch').bind('click', function () {
            initializeGoogleMapControl($('#locationTrafficSearch').val(), true);
            
        });
    });


    function createCookie(name, value, days) {
        var expires;

        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = "";
        }
        document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
    }

    function readCookie(name) {
        var nameEQ = encodeURIComponent(name) + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
        }
        return null;
    }

</script>