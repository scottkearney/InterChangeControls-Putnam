using System;
using System.Collections.Generic;
using System.ComponentModel;
using Akumina.InterAction;
using Akumina.WebParts.Documents.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using System.Text;

namespace Akumina.WebParts.Documents.DocumentRefiner
{
    [ToolboxItem(false)]
    public partial class DocumentRefiner : DocumentRefinerBaseWebPart
    {

        private ICurrentPath _getCurrentPath;
        public ICurrentTab GetCurrentTab;
        private bool Enddateclear = false;
        private StringBuilder sbTaxonomy = new StringBuilder();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private void GetTermStoreTree()
        {
            if (!string.IsNullOrEmpty(CategoryName))
            {
                sbTaxonomy = new StringBuilder();
                SPSite thisSite = SPContext.Current.Site;
                TaxonomySession session = new TaxonomySession(thisSite);
                if (session.TermStores != null && session.TermStores.Count > 0)
                {
                    bool termsetavailable = false;
                    TermStore termStore = session.TermStores[0];
                    Group group = termStore.GetSiteCollectionGroup(thisSite);
                    foreach (TermSet termSet in group.TermSets)
                    {
                        if (termSet.Name == CategoryName)
                        {
                            ltlTaxonomyView.Text = string.Empty;
                            sbTaxonomy.Append("<ul>");
                            foreach (Term term in termSet.Terms)
                            {
                                StringAddTermSet(term, CategoryName);
                            }
                            sbTaxonomy.Append("</ul>");
                            ltlTaxonomyView.Text = sbTaxonomy.ToString();
                            termsetavailable = true;
                            break;
                        }
                    }
                    if (!termsetavailable)
                    { category.Visible = false; }
                }
            }
            else
            {

                category.Visible = false;
            }

        }

        private void StringAddTermSet(Term term, string termString)
        {

            string strTermString = termString + ">" + term.Name;
            sbTaxonomy.AppendFormat("<li><input type='checkbox' value='{1}' id='{0}' /><label>{0}</label>", term.Name, strTermString.ToLower());

            if (term.Terms.Count > 0)
            {
                sbTaxonomy.Append("<ul>");
                foreach (Term t in term.Terms)
                {
                    StringAddTermSet(t, strTermString);
                }
                sbTaxonomy.Append("</ul>");
            }
            sbTaxonomy.Append("</li>");

        }


        protected override void OnPreRender(EventArgs e)
        {

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string modifiedByClass = string.Empty, FileTypeClass = string.Empty;
                string hideFilters = FilterOptions != null ? FilterOptions.ToLower() : string.Empty;
                if (hideFilters.Contains("category"))
                    category.Attributes["class"] = category.Attributes["class"] + " hideimp";
                else
                    category.Attributes["class"] = category.Attributes["class"].Replace("hideimp", "");
                if (hideFilters.Contains("date"))
                    dateField.Attributes["class"] = dateField.Attributes["class"] + " hideimp";
                else
                    dateField.Attributes["class"] = dateField.Attributes["class"].Replace("hideimp", "");
                if (hideFilters.Contains("modifiedby"))
                    modifiedByClass = "hideimp";
                if (hideFilters.Contains("filetype"))
                    FileTypeClass = "hideimp";

                if (!Page.IsPostBack)
                {
                    GetTermStoreTree();
                    if (!string.IsNullOrEmpty(InstructionSet))
                    {
                        InstructionRepository clientServices = new InstructionRepository();
                        var response = clientServices.Execute(InstructionSet);
                        if (response != null && response.Dictionary != null)
                        {
                            ListName = clientServices.GetValue(response.Dictionary, Resources.colName_ListName, ListName);//response.Dynamic.ListName;

                        }
                    }
                    if (string.IsNullOrEmpty(RootResourcePath))
                        RootResourcePath = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/15/Akumina.WebParts.Documents";
                    today.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now);
                    lstweek.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-7));
                    month.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-30));
                    year.Attributes["date"] = String.Format("{0:MMM dd, yyyy}", DateTime.Now.AddDays(-365));
                    List<Filter> optionResults = new List<Filter>();
                    optionResults.Add(new Filter() { Key = "Modified By", Options = new List<string>(), ClassName = (" ia-filter-date" + modifiedByClass) });
                    optionResults.Add(new Filter() { Key = "File Type", Options = new List<string>(), ClassName = FileTypeClass });

                    rptCategory.DataSource = optionResults;
                    rptCategory.DataBind();
                }


            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

    }
}