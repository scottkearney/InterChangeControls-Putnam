<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DSLUserControl.ascx.cs" Inherits="Akumina.WebParts.DocumentSummaryList.DocumentSummaryList.DocumentSummaryList" %>
<script type="text/javascript">
    if (Akumina === undefined) {
        var Akumina = {};
    }
    if (Akumina.InterAction === undefined) {
        Akumina.InterAction = {};
    }
</script>

<asp:Literal runat="server" ID="litTop">
</asp:Literal>

<asp:TextBox runat="server" ID="controlDslHtml" ClientIDMode="Static" style="display:none"></asp:TextBox>
<asp:TextBox runat="server" ID="itemDslHtml" ClientIDMode="Static"  style="display:none"></asp:TextBox>


<div id="targetDSL"></div>
<div id="targetNew"></div>

<script type="text/javascript">

    if (typeof jQuery == 'undefined') {
        document.write("<script type='text/javascript' src='http://code.jquery.com/jquery-2.1.3.min.js'" + "/>");
    }
</script>


<script type="text/javascript">
    
    if (Akumina.InterAction.DSL === undefined) {
      
        Akumina.InterAction.DSL = function() {
            this.targetDiv = "#targetDSL";
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
            this.resultRecent = {}; 
            this.resultPopular = {}; 
            this.resultRecommended = {}; 
            this.tabNames = {};             
            
            
            this.AddItemsToTab = function(tabName,items,tabCount,tabIndex,liTemplate,me)
            {

                $.each(items, function(index, result) {
                    var resultJson = {};
                    resultJson.IconSrc = result.IconSrc;
                    resultJson.FileUrl = result.FileUrl;
                    resultJson.FileName = result.FileName;
                    resultJson.SiteName = result.SiteName;
                    resultJson.Url = result.Url;

                    var myDate ="";
                    if(result.DateModified !=""){
                        myDate = new Date(result.DateModified);
                        myDate = myDate.format("MM/dd/yyyy");
                    }
                    resultJson.DateModified = myDate;
                    switch(tabName){
                        case "recent":
                            resultJson.ModifiedBy = "";
                            myDate = new Date(result.LogDateOcurred);
                            myDate = myDate.format("MM/dd/yyyy");
                            resultJson.DateModified = myDate;
                            break;
                        case "popular":
                            resultJson.ModifiedBy = "";
                            resultJson.DateModified = result.TotalLogs;
                            break;
                        default:
                            resultJson.ModifiedBy = result.ModifiedBy;
                            break;
                    }                    
                    if(tabName == 'popular' && result.TotalLogs ==0){}
                    else{
                        var li = liTemplate;
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


               
                var searchResultsHtml = "";
                var DSLTemplate = $("#controlDslHtml").val();//me.templateJson.ControlTemplate;
                var liTemplate =$("#itemDslHtml").val(); //me.templateJson.ItemTemplate;
                //Template
                var resultTemplateJson = {};
                //Tabs Name
                var newTabDescription = this.NewTabDescription;
                var recentTabDescription = this.RecentTabDescription;
                var popularTabDescription = this.PopularTabDescription;
                var recommendedTabDescription = this.RecommendedTabDescription;

                resultTemplateJson["Title"]  = '<asp:Literal ID="litDSL_Title" runat="server"></asp:Literal>';
                resultTemplateJson["WebPartIcon"]  ='<asp:Literal ID="litDSL_Icon" runat="server"></asp:Literal>';
                resultTemplateJson["ShowHeader"]  = <asp:Literal ID="litDSL_ShowHeader" runat="server"></asp:Literal>;
                
                $.each(me.tabNames, function(index, tab) {
                    var field = 'tab' + (index + 1) + '_name';
                    var fieldDescription = 'tab' + (index + 1) + '_description';
                    var Column2Desc = 'tab' + (index + 1) + '_Column2Desc';
                    var Column3Desc = 'tab' + (index + 1) + '_Column3Desc';
                    var Column4Desc = 'tab' + (index + 1) + '_Column4Desc';
                    resultTemplateJson[field] = tab;
                        
                    switch(tab.toLowerCase().replace(' ',''))
                    {
                        case "newest":
                            newTabIndex= index +1;
                            resultTemplateJson[fieldDescription] = newTabDescription;
                            resultTemplateJson[Column2Desc] = "Site";
                            resultTemplateJson[Column3Desc] = "Modified Date";
                            resultTemplateJson[Column4Desc] = "Modified By";
                            break;
                        case "myrecent":
                            recentTabIndex= index +1;
                            resultTemplateJson[fieldDescription] =recentTabDescription; 
                            resultTemplateJson[Column2Desc] = "Site";
                            resultTemplateJson[Column3Desc] = "Access Date";
                            resultTemplateJson[Column4Desc] = "";
                            break;
                        case "popular":
                            popularTabIndex= index +1;
                            resultTemplateJson[fieldDescription] = popularTabDescription; 
                            resultTemplateJson[Column2Desc] = "Site";
                            resultTemplateJson[Column3Desc] = numberOfDaysPopular >0 ? "# Views Last " + numberOfDaysPopular + " days" : "# Views Last 30 days";
                            resultTemplateJson[Column4Desc] = "";
                            break;
                        case "recommended":
                            recommendedTabIndex= index +1;
                            resultTemplateJson[fieldDescription]= recommendedTabDescription;
                            resultTemplateJson[Column2Desc] = "Site";
                            resultTemplateJson[Column3Desc] = "Modified Date";
                            resultTemplateJson[Column4Desc] = "Modified By";
                            break;

                    }
                });
                   
                   
                DSLTemplate = Mustache.to_html(DSLTemplate, resultTemplateJson);
                    
                $(me.targetDiv).append(DSLTemplate);                   
                var newTabCount = this.NewTabCount;
                newestItems = this.resultNew.sort(function(obj1, obj2) {                       
                    return new Date(obj2.DateModified) - new Date(obj1.DateModified);
                });
                this.AddItemsToTab('newest',newestItems,newTabCount,newTabIndex,liTemplate,me);                    

                var recentTabCount = this.RecentTabCount;
                recentItems = this.resultRecent.sort(function(obj1, obj2) {                       
                    return new Date(obj2.DateModified) - new Date(obj1.DateModified);
                });
                this.AddItemsToTab('recent',recentItems,recentTabCount,recentTabIndex,liTemplate,me);               

                popularItems = this.resultPopular.sort(function(obj1, obj2) {                       
                    return new Date(obj2.TotalLogs) - new Date(obj1.TotalLogs);
                });

                var popularTabCount = this.PopularTabCount;
                this.AddItemsToTab('popular',popularItems,popularTabCount,popularTabIndex,liTemplate,me);
                    
                var recommendedTabCount = this.RecommendedTabCount;
                this.AddItemsToTab('recommended',this.resultRecommended,recommendedTabCount,recommendedTabIndex,liTemplate,me);                                                                                 
                                       
                this.ApplyFinalJS();                   
                    
                if (me.CurrentSite)
                {
                    $(me.targetDiv).find('table.ia-site-summary-list td:nth-child(2),table.ia-site-summary-list th:nth-child(2)').hide();
                }
            };

            this.ApplyFinalJS = function(){
                //Tabs selection
                $.getScript(resourcePathValue + '/js/components/ia-transformer-tabs.js');
                //Sorter
                $('.ia-site-summary-list').addClass('tablesorter')
                $('.ia-site-summary-list').tablesorter();
                //hide tabs not informed
                $('.ia-transformer-tab-nav li a').each(function(){if($(this).html() ==''){$(this).hide();}})
            }
          
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
       
        
        var thisDSL = new Akumina.InterAction.DSL();        
        //thisDSL.templateJson = <asp:Literal ID="litTemplates" runat="server"></asp:Literal>;
        thisDSL.resultNew = <asp:Literal ID="litResultNew" runat="server"></asp:Literal>;        
        thisDSL.resultRecent = <asp:Literal ID="litResultRecent" runat="server"></asp:Literal>;        
        thisDSL.resultPopular = <asp:Literal ID="litResultPopular" runat="server"></asp:Literal>;        
        thisDSL.resultRecommended = <asp:Literal ID="litResultRecommended" runat="server"></asp:Literal>;        
        thisDSL.tabNames = <asp:Literal ID="litTabNames" runat="server"></asp:Literal>;        
        thisDSL.NewTabDescription = <asp:Literal ID="NewTabDescription" runat="server"></asp:Literal>;        
        thisDSL.NewTabCount = <asp:Literal ID="NewTabCount" runat="server"></asp:Literal>; 
        thisDSL.RecentTabDescription = <asp:Literal ID="RecentTabDescription" runat="server"></asp:Literal>;        
        thisDSL.RecentTabCount = <asp:Literal ID="RecentTabCount" runat="server"></asp:Literal>; 
        thisDSL.PopularTabDescription = <asp:Literal ID="PopularTabDescription" runat="server"></asp:Literal>;        
        thisDSL.PopularTabCount = <asp:Literal ID="PopularTabCount" runat="server"></asp:Literal>; 
        thisDSL.RecommendedTabDescription = <asp:Literal ID="RecommendedTabDescription" runat="server"></asp:Literal>;        
        thisDSL.RecommendedTabCount = <asp:Literal ID="RecommendedTabCount" runat="server"></asp:Literal>; 
        thisDSL.CurrentSite = <asp:Literal ID="litCurrentSite" runat="server"></asp:Literal>; 
        
        thisDSL.getSearchResultsUsingREST(thisDSL);

        
        
    });
</script>



