using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace Akumina.WebParts.QuickLinks.QuickLinks
{
    [ToolboxItemAttribute(false)]
    public partial class QuickLinks : QuickLinksBaseWebPart
    {
        string _listIdentifier = "";
        string _itemIdentifier = "";
        bool _isRootSite = false;
        bool _listIdentifierIsGuid = false;
        bool _itemIdentifierIsTitle = false;
        string _itemIcon = "link";
        Guid _uniqueId = Guid.NewGuid();
        List<int> _recursionAvoidList = new List<int>();

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //public QuickLinks()
        //{
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            quickLinksTarget.Attributes.Add("class", _uniqueId.ToString());
            UniqueIdItem.Text = _uniqueId.ToString();

            if (!Page.IsPostBack)
            {
                if (!SPContext.Current.IsDesignTime && !string.IsNullOrWhiteSpace(InstructionSet))
                {
                    try
                    {
                        MapInstructionSetToProperties(GetInstructionSet(InstructionSet), this);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                DisplayList();
            }
        }

        #region private


        private void DisplayList()
        {
            if (string.IsNullOrEmpty(RootResourcePath))
                RootResourcePath = SPContext.Current.Web.Site.RootWeb.Url + "/Akumina.WebParts.QuickLinks";

            _itemIcon = GetIcon(Icon);

            var quickLinks = GetList();

            QuickLinksItem.Text = Serialize(quickLinks);

            var styles = Resources.CssLibrary.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var style in styles)
            {
                sb.AppendLine("<link rel=\"stylesheet\" href=\"" + RootResourcePath + "/" + style + "\" />");
            }

            litTop.Text = sb.ToString();

            if (DisplayAsTopMenu)
            {
                ItemListTemplate.Text = Serialize(new List<string> { Resources.ItemDetailTemplate });
            }
            else
            {
                Directions direction = Directions;
                switch (direction)
                {
                    case Directions.LeftRight:
                        ItemListTemplate.Text = Serialize(new List<string> { Resources.ItemTemplate });
                        break;
                    case Directions.TopBottom:
                        ItemListTemplate.Text = Serialize(new List<string> { Resources.ItemTemplateSubSite });
                        break;
                }
            }

            if (IsSearchCrawler())
            {
                var html = new StringBuilder();
                html.AppendFormat(@"{0} {1}", quickLinks.Title, quickLinks.Link);
                foreach (var item in quickLinks.Items)
                {
                    html.AppendFormat(@"{0} {1}", item.Title, item.Link);
                }
                __searchCrawlFeed.Text = html.ToString();
            }
        }

        private QuickLinksModel GetList()
        {
            var model = new QuickLinksModel();

            try
            {
                GetItemValues();

                SPList spList;
                if (_listIdentifierIsGuid)
                {
                    Guid listGuid = Guid.Parse(_listIdentifier);
                    spList = _isRootSite ? SPContext.Current.Site.RootWeb.Lists.GetList(listGuid, false) : SPContext.Current.Web.Lists.GetList(listGuid, false);
                }
                else
                {
                    spList = _isRootSite ? SPContext.Current.Site.RootWeb.Lists[_listIdentifier] : SPContext.Current.Web.Lists[_listIdentifier];
                }

                if (spList != null)
                {
                    var list = new List<QuickLinksModel>();
                    SPQuery query = new SPQuery();
                    SPListItemCollection items = spList.GetItems(query);
                    foreach (SPListItem item in items)
                    {
                        if (items.List.EnableVersioning)
                        {
                            var listItem = GetPreview(IsPreview, item);
                            if (listItem.Count > 0)
                            {
                                list.Add(GetItems(listItem));
                            }
                        }
                        else
                        {
                            list.Add(GetItems(item));
                        }
                    }
                    model = GetModel(list);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return model;
        }

        private QuickLinksModel GetItems(Dictionary<string, object> quicklinkItem)
        {
            var list = new QuickLinksModel();

            if (quicklinkItem == null) return list;
            var nodeType = QuickLinksNodeType.Item;
            if (quicklinkItem["NodeType"] != null)
            {
                Enum.TryParse(quicklinkItem["NodeType"].ToString(), out nodeType);
            }

            int parentId = 0;
            if (quicklinkItem["ParentItem_x003a_ID"] != null)
            {
                parentId = GetIntValue(quicklinkItem["ParentItem_x003a_ID"].ToString().Split(';')[0]);
            }

            var link = GetLinkValue(quicklinkItem["Link"]);

            var openInNewWindow = GetStringValue(quicklinkItem["Open_x0020_With"]).ToLower().Contains("new");

            var isActive = GetStringValue(quicklinkItem["Active"]).ToLower().Contains("true");

            var target = openInNewWindow ? "_blank" : "_self";

            var item = new QuickLinksModel()
            {
                Id = GetIntValue(quicklinkItem["ID"]),
                ParentId = parentId,
                Link = link,
                Target = target,
                NodeType = nodeType,
                OpenInNewWindow = openInNewWindow,
                Title = nodeType == QuickLinksNodeType.Root ? Title : GetStringValue(quicklinkItem["Title"]),
                DisplayOrder = GetIntValue(quicklinkItem["DisplayOrder"], -1),
                WebPartIcon = _itemIcon,
                ShowHeader = _itemIcon != "none" || Title.Trim() != ""
            };

            if (isActive)
                list = item;

            return list;
        }

        private QuickLinksModel GetItems(SPListItem quicklinkItem)
        {
            var list = new QuickLinksModel();

            if (quicklinkItem == null) return list;
            var nodeType = QuickLinksNodeType.Item;
            if (quicklinkItem["NodeType"] != null)
            {
                Enum.TryParse(quicklinkItem["NodeType"].ToString(), out nodeType);
            }

            int parentId = 0;
            if (quicklinkItem["ParentItem_x003a_ID"] != null)
            {
                parentId = GetIntValue(quicklinkItem["ParentItem_x003a_ID"].ToString().Split(';')[0]);
            }

            var link = GetLinkValue(quicklinkItem["Link"]);

            var openInNewWindow = GetStringValue(quicklinkItem["Open_x0020_With"]).ToLower().Contains("new");
            var isActive = GetStringValue(quicklinkItem["Active"]).ToLower().Contains("true");
            var target = openInNewWindow ? "_blank" : "_self";
            var item = new QuickLinksModel()
            {
                Id = GetIntValue(quicklinkItem["ID"]),
                ParentId = parentId,
                Link = link,
                NodeType = nodeType,
                Target = target,
                OpenInNewWindow = openInNewWindow,
                Title = nodeType == QuickLinksNodeType.Root ? Title : GetStringValue(quicklinkItem["Title"]),
                DisplayOrder = GetIntValue(quicklinkItem["DisplayOrder"], -1),
                WebPartIcon = _itemIcon,
                ShowHeader = _itemIcon != "none" || Title.Trim() != ""
            };

            if (isActive)
                list = item;
            

            return list;
        }

        private QuickLinksModel GetModel(List<QuickLinksModel> list)
        {
            var model = new QuickLinksModel();
            var rootItem = list.Find(x => x.NodeType.Equals(QuickLinksNodeType.Root));

            if (rootItem == null) return model;

            model = rootItem;
            _recursionAvoidList.Add(model.Id);// get sub items
            model.Items = GetModelItems(model.Id, list);

            return model;
        }

        private List<QuickLinksModel> GetModelItems(int parentId, List<QuickLinksModel> list)
        {
            var items = list.FindAll(x => x.ParentId == parentId && !_recursionAvoidList.Contains(x.Id));

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    _recursionAvoidList.Add(item.Id);
                    item.Items = GetModelItems(item.Id, list);
                    if (item.NodeType == QuickLinksNodeType.Category)
                    {
                        item.IsFolder = true;
                    }
                    if (item.NodeType == QuickLinksNodeType.Item)
                    {
                        item.IsItem = true;
                    }
                    item.HasCategories = item.Items.FindAll(x => x.NodeType == QuickLinksNodeType.Category).Count > 0;
                    if (!item.HasCategories)
                    {
                        item.HasLinks = item.Items.FindAll(x => x.NodeType == QuickLinksNodeType.Item).Count > 0;
                    }
                }
            }

            items.Sort(delegate(QuickLinksModel x, QuickLinksModel y)
            {
                if (x.DisplayOrder == -1 && y.DisplayOrder == -1) return 0;
                if (x.DisplayOrder == -1) return -1;
                return y.DisplayOrder == -1 ? 1 : x.DisplayOrder.CompareTo(y.DisplayOrder);
            });

            return items;
        }

        private void GetItemValues()
        {
            if (string.IsNullOrWhiteSpace(QueryPart)) return;
            var parts = QueryPart.Split(new[] { '.' }, 3);
            _listIdentifier = parts[0];
            if (parts.Length > 1)
            {
                _itemIdentifier = parts[1];
            }

            if (parts.Length == 2 && parts[0].Equals("~"))
            {
                _listIdentifier = parts[1];
                _isRootSite = true;
            }

            Guid listGuid;
            if (Guid.TryParse(_listIdentifier, out listGuid))
            {
                _listIdentifierIsGuid = true;
            }
            if (parts.Length > 1)
            {
                int itemId;
                if (int.TryParse(_itemIdentifier, out itemId))
                {
                    _itemIdentifierIsTitle = false;
                }
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            var scripts = Resources.JsLibrary.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var script in scripts)
            {
                if (script.ToLower().Contains("jquery"))
                {
                    writer.WriteLine("<script type=\"text/javascript\">");
                    writer.WriteLine("if (typeof jQuery == 'undefined') { document.write(unescape(\"%3Cscript src='" + RootResourcePath + script + "' type='text/javascript'%3E%3C/script%3E\")); }");
                    writer.WriteLine("</script>");
                }
                else
                {
                    WriteJs(writer, script);
                }
            }
            if (DisplayAsTopMenu)
            {
                WriteJs(writer, "/js/controls/ia-top-nav.js");
            }

            base.RenderContents(writer);
        }

        private string GetLinkValue(object val)
        {
            string link = GetStringValue(val);
            var linkParts = link.Split(new[] { "," }, StringSplitOptions.None);
            return linkParts[0];
        }

        private static string GetStringValue(object val)
        {
            return val != null ? val.ToString() : "";
        }

        private static int GetIntValue(object val)
        {
            return GetIntValue(val, 0);
        }

        private static int GetIntValue(object val, int defaultValue)
        {
            int returnVal = defaultValue;
            if (val != null)
            {
                int.TryParse(val.ToString(), out returnVal);
            }
            return returnVal;
        }

        #endregion
    }
}