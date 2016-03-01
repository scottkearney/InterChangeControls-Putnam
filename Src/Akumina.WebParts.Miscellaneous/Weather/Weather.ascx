<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Weather.ascx.cs" Inherits="Akumina.WebParts.Miscellaneous.Weather.Weather" %>

<div class="interAction">
    <div class="ia-control-header">
        <span class="ia-control-header-icon fa fa-sun-o"></span>
        <h3 class="ia-control-header-heading">Weather</h3>
    </div>
    <div class="weatherWidget">
        <a href="http://www.accuweather.com/en/us/new-york-ny/10007/weather-forecast/349727" class="aw-widget-legal">
            <!--
        By accessing and/or using this code snippet, you agree to AccuWeather’s terms and conditions (in English) which can be found at http://www.accuweather.com/en/free-weather-widgets/terms and AccuWeather’s Privacy Statement (in English) which can be found at http://www.accuweather.com/en/privacy.
        -->
        </a>
        <div id="awcc1422235549845" class="aw-widget-current" data-locationkey="" data-unit="f" data-language="en-us" data-useip="true" data-uid="awcc1422235549845"></div>
        <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
    </div>
</div>
