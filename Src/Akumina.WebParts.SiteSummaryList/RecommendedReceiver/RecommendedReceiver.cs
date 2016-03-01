using System;
using Microsoft.SharePoint;

namespace Akumina.WebParts.SiteSummaryList.RecommendedReceiver
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class RecommendedReceiver : SPWebEventReceiver
    {
        private const string listName = "AkuminaSiteRecommendation";

        /// <summary>
        /// A site collection was deleted.
        /// </summary>
        public override void SiteDeleted(SPWebEventProperties properties)
        {
            base.SiteDeleted(properties);
        }

        /// <summary>
        /// A site was deleted.
        /// </summary>
        public override void WebDeleted(SPWebEventProperties properties)
        {
            base.WebDeleted(properties);
            try
            {
                string url = properties.FullUrl.Substring(0, properties.FullUrl.LastIndexOf("/"));
                using (SPSite siteCollection = new SPSite(url))
                {
                    SPList recommendedList = siteCollection.RootWeb.Lists[listName];
                    SPQuery query = new SPQuery();
                    query.Query = "<Where><Eq><FieldRef Name=\"SiteId\" /><Value Type=\"Text\">" + properties.WebId.ToString() + "</Value></Eq></Where>";
                    var listItems = recommendedList.GetItems(query);
                    if (listItems.Count > 0)
                    {
                        listItems[0].Delete();
                        recommendedList.Update();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }


        /// <summary>
        /// A site was moved.
        /// </summary>
        public override void WebMoved(SPWebEventProperties properties)
        {
            base.WebMoved(properties);
            {
                string url = properties.FullUrl.Substring(0, properties.FullUrl.LastIndexOf("/"));
                using (SPSite siteCollection = new SPSite(url))
                {
                    SPList recommendedList = siteCollection.RootWeb.Lists[listName];
                    SPQuery query = new SPQuery();
                    query.Query = "<Where><Eq><FieldRef Name=\"SiteId\" /><Value Type=\"Text\">" + properties.WebId.ToString() + "</Value></Eq></Where>";
                    var listItems = recommendedList.GetItems(query);
                    if (listItems.Count > 0)
                    {
                        listItems[0]["Title"] = properties.Web.Name;
                        listItems[0]["SiteName"] = properties.Web.Name;
                        SPFieldUrlValue urlValue = new SPFieldUrlValue();
                        urlValue.Description = properties.Web.Name;
                        urlValue.Url = properties.Web.Url;
                        listItems[0]["Url"] = urlValue;
                        listItems[0]["SiteId"] = properties.WebId.ToString();
                        recommendedList.Update();
                    }
                }
            }

        }



        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            try
            {
                using (SPSite siteCollection = new SPSite(properties.Web.Url))
                {
                    SPList recommendedList = siteCollection.RootWeb.Lists[listName];
                    SPQuery query = new SPQuery();
                    query.Query = "<Where><Eq><FieldRef Name=\"SiteId\" /><Value Type=\"Text\">" + properties.WebId.ToString() + "</Value></Eq></Where>";
                    var listItems = recommendedList.GetItems(query);
                    if (listItems.Count < 1)
                    {
                        if (recommendedList != null)
                        {
                            SPListItem oListItem = recommendedList.AddItem();
                            oListItem["Title"] = properties.Web.Name;
                            oListItem["SiteName"] = properties.Web.Name;
                            SPFieldUrlValue urlValue = new SPFieldUrlValue();
                            urlValue.Description = properties.Web.Name;
                            urlValue.Url = properties.Web.Url;
                            oListItem["Url"] = urlValue;
                            oListItem["SiteId"] = properties.WebId.ToString();
                            oListItem.Update();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}