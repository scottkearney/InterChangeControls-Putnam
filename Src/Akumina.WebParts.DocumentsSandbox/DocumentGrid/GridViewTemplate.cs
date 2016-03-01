using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Akumina.InterAction;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DocumentsSandbox.DocumentGrid
{
    class GridViewTemplate : DocumentGrid, ITemplate
    {
        //A variable to hold the type of ListItemType.

        private string[] imageTypes = { "gif", "tiff", "jpg", "jpeg", "bmp", "png", "rif" };
        public ListItemType _templateType { get; set; }

        //A variable to hold the column name.
        public string _columnName { get; set; }
        string strColumnName;

        private string pathString { get; set; }
        private string fileType { get; set; }
        private string typeUrl { get; set; }
        private string linkFileStr { get; set; }
        private string webUrl = SPContext.Current.Web.ServerRelativeUrl.Equals("/") ? "" : SPContext.Current.Web.ServerRelativeUrl;
        //Constructor where we define the template type and column name.
        public GridViewTemplate(ListItemType type, string colname)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            try
            {
                Label lbl = new Label();
                CheckBox chkBox = new CheckBox();
                switch (_templateType)
                {
                    case ListItemType.Header:
                        //Creates a new label control and add it to the container.
                        //Allocates the new label object.
                        if (this._columnName == "Column1")
                        {
                            HtmlInputCheckBox hm = new HtmlInputCheckBox();
                            hm.Attributes["class"] = "ia-doclist-selectAll";
                            hm.Attributes["onchange"] = "selectall(this.checked);";
                            container.Controls.Add(hm);
                            break;
                        }

                        else if (this._columnName == "File_x0020_Type")
                        {
                            strColumnName = "File Type";
                        }
                        else if (this._columnName == "LinkFilename")
                        {
                            strColumnName = "Name";

                            lbl.Text = strColumnName;
                            lbl.ID = "headerlbl1";
                            lbl.ClientIDMode = ClientIDMode.Static;
                            container.Controls.Add(lbl);

                            break;
                        }
                        else if (this._columnName == "Modified")
                        {
                            strColumnName = "Modified";

                            lbl.Text = strColumnName;
                            lbl.ID = "headerlbl2";
                            container.Controls.Add(lbl);

                            break;

                        }
                        else if (this._columnName == "Editor")
                        {
                            strColumnName = "Modified By";

                            lbl.Text = strColumnName;
                            lbl.ID = "headerlbl3";
                            container.Controls.Add(lbl);

                            break;
                        }
                        else
                        {
                            strColumnName = this._columnName;
                        }

                        lbl.Text = strColumnName;             //Assigns the name of the column in the lable.
                        container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                        break;

                    case ListItemType.Item:
                        if (this._columnName == "Column1")
                        {
                            HtmlInputCheckBox hm = new HtmlInputCheckBox();
                            hm.Attributes["class"] = "selection";
                            hm.Attributes["onchange"] = "onselection();";
                            container.Controls.Add(hm);
                            break;
                        }
                        else if (this._columnName == "LinkFilename")
                        {
                            HtmlGenericControl filename = new HtmlGenericControl("span");
                            filename.Attributes["class"] = "ia-doclist-name";
                            HtmlGenericControl linkFileName = new HtmlGenericControl("a");
                            linkFileName.DataBinding += new EventHandler(this.linkFileName_DataBinding);


                            HtmlGenericControl spDocIcon = new HtmlGenericControl();
                            spDocIcon.DataBinding += new EventHandler(this.spDocIcon_DataBinding);
                            container.Controls.Add(spDocIcon);

                            HyperLink hrefFileName = new HyperLink();

                            hrefFileName.DataBinding += new EventHandler(this.hrefFileName_DataBinding);


                            HyperLink hrefPreview = new HyperLink();
                            hrefPreview.DataBinding += new EventHandler(this.hrefPreview_DataBinding);

                            HtmlGenericControl hiddDiv = new HtmlGenericControl("div");
                            hiddDiv.Attributes["class"] = "hdnbutton";
                            hiddDiv.DataBinding += hiddDiv_DataBinding;


                            filename.Controls.Add(linkFileName);
                            filename.Controls.Add(hrefFileName);

                            filename.Controls.Add(hrefPreview);
                            filename.Controls.Add(hiddDiv);
                            container.Controls.Add(filename);

                            break;
                        }
                        else
                        {
                            //Creates a new text box control and add it to the container.
                            Literal tb1 = new Literal();                           //Allocates the new text box object.
                            tb1.DataBinding += new EventHandler(tb1_DataBinding);
                            container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        void hiddDiv_DataBinding(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl hiddDiv = (HtmlGenericControl)sender;
                GridViewRow container = (GridViewRow)hiddDiv.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, "LinkFilename");
                linkFileStr = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "Path");
                pathString = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "File_x0020_Type");
                fileType = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "Editor");
                string editor = dataValue.ToString();

                dataValue = DataBinder.Eval(container.DataItem, "ModifiedTime");
                string modifiedTime = dataValue.ToString();
                hiddDiv.InnerHtml = String.Format("{0}.{1},{2}/{3},{4},{5},{6}", linkFileStr, fileType, webUrl, pathString, editor, modifiedTime, fileType);

            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }



        void hrefPreview_DataBinding(object sender, EventArgs e)
        {

            try
            {
                HyperLink hrefPreview = (HyperLink)sender;
                GridViewRow container = (GridViewRow)hrefPreview.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, "File_x0020_Type");
                fileType = dataValue.ToString();
                HtmlGenericControl spanPreview = new HtmlGenericControl();
                spanPreview.Attributes["class"] = "fa fa-search-plus";
                hrefPreview.Attributes["href"] = "#document-preview-1";

                hrefPreview.Visible = fileType != "" ? true : false;
                hrefPreview.Attributes["onclick"] = fileType != "" ? "OpenItemFilePreviewCallOut(this);" : "";
                hrefPreview.CssClass = "ia-document-preview ia-modal-inline-trigger";
                hrefPreview.Controls.Add(spanPreview);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        void hrefFileName_DataBinding(object sender, EventArgs e)
        {
            try
            {
                var currentWeb = SPContext.Current.Web;

                HyperLink hrefFileName = (HyperLink)sender;
                GridViewRow container = (GridViewRow)hrefFileName.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, "Path");
                pathString = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, _columnName);
                linkFileStr = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "File_x0020_Type");
                fileType = dataValue.ToString();
                string webExtension = (!imageTypes.Contains(fileType) ? "?web=1" : string.Empty);
                if (fileType != "one")
                    hrefFileName.Attributes["class"] = "docOpen disp-File-Name";
                else
                    hrefFileName.Attributes["class"] = "filename docOpen disp-File-Name";
                hrefFileName.Attributes["onclick"] = "setCookie();";
                hrefFileName.Attributes["onmousedown"] = "return VerifyHref(this,event,'0','SharePoint.OpenDocuments','')";
                hrefFileName.NavigateUrl = currentWeb.ServerRelativeUrl.Equals("/") ? ("/" + pathString + webExtension) : (currentWeb.ServerRelativeUrl) + "/" + pathString + webExtension;
                hrefFileName.Text = linkFileStr;
                //hrefFileName.Attributes.Add("onclick", "DispEx(this,event,'TRUE','FALSE','FALSE','SharePoint.OpenDocuments.3','0','SharePoint.OpenDocuments','','','','1073741823','1','0','0x7fffffffffffffff')");

                hrefFileName.Visible = fileType != "" ? true : false;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        void spDocIcon_DataBinding(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl spDocIcon = (HtmlGenericControl)sender;
                GridViewRow container = (GridViewRow)spDocIcon.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, "TypeUrl");
                typeUrl = dataValue.ToString();
                spDocIcon.Attributes["class"] = String.Format("ia-doclib-file-icon {0}", typeUrl == "folder" ? "fa fa-folder" : string.Empty);
                Image imgdata = new Image();
                imgdata.ImageUrl = typeUrl;
                imgdata.Visible = typeUrl == "folder" ? false : true;
                object chckOutImg = DataBinder.Eval(container.DataItem, "Status");
                Literal chckoutImg = new Literal();
                chckoutImg.Text = chckOutImg.ToString();
                spDocIcon.Controls.Add(imgdata);
                spDocIcon.Controls.Add(chckoutImg);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        void linkFileName_DataBinding(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl lbldata = (HtmlGenericControl)sender;
                GridViewRow container = (GridViewRow)lbldata.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, _columnName);
                linkFileStr = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "Path");
                pathString = dataValue.ToString();
                dataValue = DataBinder.Eval(container.DataItem, "File_x0020_Type");
                fileType = dataValue.ToString();
                lbldata.InnerText = linkFileStr;
                lbldata.Attributes["class"] = "filename";
                lbldata.Attributes["url-data"] = pathString;
                lbldata.Attributes["href"] = "#";
                lbldata.Attributes["onclick"] = "getcurrentfolder(this);";
                lbldata.Visible = (fileType == "") ? true : false;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
        void tb1_DataBinding(object sender, EventArgs e)
        {
            try
            {
                Literal lbldata = (Literal)sender;
                GridViewRow container = (GridViewRow)lbldata.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, _columnName);
                if (dataValue != DBNull.Value)
                    lbldata.Text = dataValue.ToString();

                else

                    lbldata.Text = "&nbsp;";

            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /*---------------------------------------------------------------------------------------*/
    }

}
