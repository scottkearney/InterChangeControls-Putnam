<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteSummaryList.ascx.cs" Inherits="Akumina.WebParts.SiteSummaryList.SiteSummaryList.SiteSummaryList" %>

<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<asp:Literal runat="server" ID="litTop" >
</asp:Literal>


<div id="targetSSL"></div>
<div id="targetNew"></div>

<script type="text/javascript">

    if (typeof jQuery == 'undefined') {
        document.write("<script type='text/javascript' src='http://code.jquery.com/jquery-2.1.3.min.js'" + "/>");
    }
</script>

<asp:TextBox runat="server" ID="controlHtml" ClientIDMode="Static" style="display:none"></asp:TextBox>
<asp:TextBox runat="server" ID="itemHtml" ClientIDMode="Static"  style="display:none"></asp:TextBox>
<script type="text/javascript">
    
    if (Akumina.InterAction.SSL === undefined) {
      
        Akumina.InterAction.SSL = function() {
            this.targetDiv = "#targetSSL";
            this.Url = "";
            this.Name = "";
            this.Date = "";
            
            this.NewTabDescription = "";
            this.NewTabCount= "";
            this.RecentTabDescription = "";
            this.RecentTabCount = "";
            this.PopularTabDescription= "";
            this.PopularTabCount = "";
            this.RecommendedTabDescription = "";
            this.RecommendedTabCount = "";
            this.searchResults = {};
            this.templateJson = {}; 
            this.resultNew = {}; 
            this.resultRecommended = {}; 
            this.tabNames = {};             
            
            
            this.AddItemsToTab = function(tabName,items,tabCount,tabIndex,liTemplate,me)
            {

                $.each(items, function(index, result) {
                    var resultJson = {};
                  
                    resultJson.Name = result.Site;
                    var myDate ="";
                    if(result.Date !=""){
                        myDate = new Date(result.Date);
                        myDate = myDate.format("MM/dd/yyyy");
                    }
                    resultJson.Date = myDate;
                    resultJson.Url = result.Url;

                    
                    switch(tabName){
                        case "recent":
                            var myDate ="";
                            if(result.latestAccess !=""){
                                myDate = new Date(result.latestAccess);
                                myDate = myDate.format("MM/dd/yyyy");
                            }
                            resultJson.Date = myDate;
                            if(myDate == '')
                            {
                                resultJson.Name ='';
                            }
                            break;
                        case "popular":
                            resultJson.Date = result.TotalAccessed;
                            break;
                        case "recommended":
                            resultJson.Date = '';
                            break;
                        default:
                            resultJson.ModifiedBy = result.ModifiedBy;
                            break;
                    }                    
                        
                    var li = liTemplate;
                    if(resultJson.Name !="")
                    {
                        li = Mustache.to_html(liTemplate, resultJson);
                        $(".tab" + tabIndex, me.targetDiv).append(li);
                    }
                    if(tabCount > 0 && tabCount == (index+1)){
                        return false;
                    }
                });
            }



            this.getSearchResultsUsingREST = function(me) {
                var me = this;
                
                var newTabIndex =-1;
                var recentTabIndex =-1;
                var popularTabIndex =-1;
                var recommendedTabIndex =-1;


                var results = this.resultNew;

                if (results.length == 0) {
                    $(me.targetDiv).html("");
                } else {
                    var searchResultsHtml = "";
                    var sslTemplate = $('#controlHtml').val(); //me.templateJson.ControlTemplate;
                    var liTemplate = $('#itemHtml').val();//me.templateJson.ItemTemplate;
                    //Template
                    var resultTemplateJson = {};
                    //Tabs Name
                    var newTabDescription = this.NewTabDescription;
                    var recentTabDescription = this.RecentTabDescription;
                    var popularTabDescription = this.PopularTabDescription;
                    var recommendedTabDescription = this.RecommendedTabDescription;
                    $.each(me.tabNames, function(index, tab) {
                        var field = 'tab' + (index + 1) + '_name';
                        var fieldDescription = 'tab' + (index + 1) + '_description';
                        var dateDescription = 'tab' + (index + 1) + '_DateDescription';
                        resultTemplateJson[field] = tab;
                        
                        switch(tab.toLowerCase().replace(' ',''))
                        {
                            case "newest":
                                newTabIndex= index +1;
                                resultTemplateJson[fieldDescription] = newTabDescription;
                                resultTemplateJson[dateDescription] = "Date Available";
                                break;
                            case "myrecent":
                                recentTabIndex= index +1;
                                resultTemplateJson[fieldDescription] =recentTabDescription; 
                                resultTemplateJson[dateDescription] = "Last Accessed";
                                break;
                            case "popular":
                                popularTabIndex= index +1;
                                resultTemplateJson[fieldDescription] = popularTabDescription; 
                                resultTemplateJson[dateDescription] = numberOfDaysPopular >0 ? "# Views Last " + numberOfDaysPopular + " days" : "# Views Last 30 days";
                                break;
                            case "recommended":
                                recommendedTabIndex= index +1;
                                resultTemplateJson[fieldDescription]= recommendedTabDescription;
                                resultTemplateJson[dateDescription] = "";
                                break;

                        }
                    });
                    
                   

                    sslTemplate = Mustache.to_html(sslTemplate, resultTemplateJson);
                    
                    $(me.targetDiv).append(sslTemplate);
                    //New
                    var newItems = results[0].Item.sort(function(obj1, obj2) {                       
                        return new Date(obj2.Date) - new Date(obj1.Date);
                    });

                    var newTabCount = this.NewTabCount;
                    this.AddItemsToTab('newest',newItems,newTabCount,newTabIndex,liTemplate,me);
                    //My Recent
                    var minDate = new Date(-8640000000000000);
                    
                    var recentItems = results[0].Item.sort(function(obj1, obj2) {                       
                        return (obj2.latestAccess != "" ? new Date(obj2.latestAccess) : minDate) -  (obj1.latestAccess != "" ? new Date(obj1.latestAccess) : minDate);
                    });
                    var recentTabCount = this.RecentTabCount;
                    this.AddItemsToTab('recent',recentItems,recentTabCount,recentTabIndex,liTemplate,me);                                                                        


                    //Popular
                    var popularItems = results[0].Item.sort(function(obj1, obj2) {                       
                        return new Date(obj2.TotalAccessed) -  new Date(obj1.TotalAccessed);
                    });
                    var popularTabCount = this.PopularTabCount;
                    this.AddItemsToTab('popular',popularItems,popularTabCount,popularTabIndex,liTemplate,me);
                   
                    //Recommended
                    var recommendedItems = this.resultRecommended[0].Item.sort(function(obj1, obj2) {                       
                        return new Date(obj2.Name) -  new Date(obj1.Name);
                    });
                    var recommendedTabCount = this.RecommendedTabCount;
                    this.AddItemsToTab('recommended',recommendedItems,recommendedTabCount,recommendedTabIndex,liTemplate,me);
                   
                }    
                //Tabs selection
                $.getScript(resourcePathValue + '/js/components/ia-transformer-tabs.js');
                //Sorter
                $('.ia-site-summary-list').addClass('tablesorter')
                $('.ia-site-summary-list').tablesorter();
                //hide tabs not informed
                $('.ia-transformer-tab-nav li a').each(function(){if($(this).html() ==''){$(this).parent().remove();}})
                
            };

          
            this.getCellValueByKey = function(results, key) {
                var result = "";
                for (i = 0; i < results.length; i++) {
                    if (results[i].Key == key) {
                        result = results[i].Value;
                        break;
                    }
                }
                return result;
            };
            
        };
    }
    
    
   


    $(document).ready(function() {
   
        $.ajaxSetup({
            cache: true
        });       
        
        var thisSSL = new Akumina.InterAction.SSL();        
        //thisSSL.templateJson = <asp:Literal ID="litTemplates" runat="server"></asp:Literal>;
        thisSSL.resultNew = <asp:Literal ID="litResultNew" runat="server"></asp:Literal>;        
        thisSSL.resultRecommended = <asp:Literal ID="litResultRecommended" runat="server"></asp:Literal>;        

        thisSSL.tabNames = <asp:Literal ID="litTabNames" runat="server"></asp:Literal>;        
        thisSSL.NewTabDescription = <asp:Literal ID="NewTabDescription" runat="server"></asp:Literal>;        
        thisSSL.NewTabCount = <asp:Literal ID="NewTabCount" runat="server"></asp:Literal>; 
        thisSSL.RecentTabDescription = <asp:Literal ID="RecentTabDescription" runat="server"></asp:Literal>;        
        thisSSL.RecentTabCount = <asp:Literal ID="RecentTabCount" runat="server"></asp:Literal>; 
        thisSSL.PopularTabDescription = <asp:Literal ID="PopularTabDescription" runat="server"></asp:Literal>;        
        thisSSL.PopularTabCount = <asp:Literal ID="PopularTabCount" runat="server"></asp:Literal>; 
        thisSSL.RecommendedTabDescription = <asp:Literal ID="RecommendedTabDescription" runat="server"></asp:Literal>;        
        thisSSL.RecommendedTabCount = <asp:Literal ID="RecommendedTabCount" runat="server"></asp:Literal>; 
        

        thisSSL.getSearchResultsUsingREST(thisSSL);        
        
        
    });
</script>



