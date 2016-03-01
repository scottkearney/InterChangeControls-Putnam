using Akumina.WebParts.Documents.Properties;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.Documents.DocumentGrid
{
    public class DocumentGridConfigurableEditorPart : EditorPart
    {
        private readonly string[] _defaultMenuOptions = { Resources.btnCheckIn, Resources.btnOpen, Resources.btnCheckOutAll, Resources.btnDiscardChkOutAll, Resources.btnDownload, Resources.btnDelete, Resources.btnMore };
        private readonly string[] _defaultMoreOptions = { Resources.btnCheckIn, Resources.btnDiscardCheckOut, Resources.btnCheckOut, Resources.btnViewProperties, Resources.btnEditProperties, Resources.btnShare, Resources.btnShareWith, Resources.btnFollow, Resources.btnCompliance, Resources.btnWorkflow };
        private readonly string[] _defaultDocumentListOptionClassNames = { "Checkbox|ia-doclib-header-checkbox,ia-doclib-checkbox", "File Icon|ia-doclib-file-icon", "File Name|ia-doclib-header-name,ia-doclist-name-icon", "PreviewIcon|ia-document-preview", "Modified Date|ia-doc-list-modified,ia-doclib-header-modified", "Modified By|ia-doclib-header-modifiedBy,ia-doc-list-modifiedBy" };


        protected InputFormCheckBoxList MenuList;
        protected InputFormCheckBoxList MoreMenuList;
        protected InputFormCheckBoxList DocumentListOptions;

        public DocumentGridConfigurableEditorPart()
        {

            MenuList = new InputFormCheckBoxList
            {
                ID = "configurableMenu",
                DataSource = _defaultMenuOptions.ToList()
            };
            MenuList.DataBind();


            MoreMenuList = new InputFormCheckBoxList
            {
                ID = "configurableMoreMenu",
                DataSource = _defaultMoreOptions.ToList()
            };
            MoreMenuList.DataBind();

            DocumentListOptions = new InputFormCheckBoxList { ID = "configurableDocumentListOptions" };
            DocumentListOptions.DataBind();
            GetDocumentListOptions();

            foreach (ListItem item in MenuList.Items)
            {
                item.Selected = true;
            }
            foreach (ListItem item in MoreMenuList.Items)
            {
                item.Selected = true;
            }
        }

        private void GetDocumentListOptions()
        {
            DocumentListOptions.Items.Clear();
            foreach (var name in _defaultDocumentListOptionClassNames)
            {
                var listItem = new ListItem
                {
                    Text = name.Split('|')[0],
                    Value = name.Split('|')[1],
                    Selected = true
                };
                DocumentListOptions.Items.Add(listItem);
            }
        }

        public override bool ApplyChanges()
        {
            string selectedVaues = string.Empty;
            if (MenuList.Items.Count > 0)
            {
                foreach (ListItem item in MenuList.Items)
                {
                    if (item.Selected)
                        selectedVaues += item.Value + ";";

                }
                selectedVaues = selectedVaues.TrimEnd(';');
            }
            var grid = (DocumentGrid)WebPartToEdit;
            grid.MenuProperty = selectedVaues;

            selectedVaues = string.Empty;
            if (MoreMenuList.Items.Count > 0)
            {
                foreach (ListItem item in MoreMenuList.Items)
                {
                    if (item.Selected)
                        selectedVaues += item.Value + ";";

                }
                selectedVaues = selectedVaues.TrimEnd(';');
            }

            grid.MoreMenuProperty = selectedVaues;

            selectedVaues = string.Empty;
            if (DocumentListOptions.Items.Count > 0)
            {

                foreach (ListItem item in DocumentListOptions.Items)
                {
                    if (!item.Selected)
                        selectedVaues += item.Value + ";";
                }
                selectedVaues = selectedVaues.TrimEnd(';');
            }

            grid.DocumentListOptions = selectedVaues;

            return true;
        }

        public override void SyncChanges()
        {
            var grid = (DocumentGrid)WebPartToEdit;
            var selectedValues = new List<string>();
            if (!string.IsNullOrEmpty(grid.MenuProperty))
            {
                selectedValues = grid.MenuProperty.Split(';').ToList();
                if (MenuList.Items.Count > 0)
                {
                    foreach (ListItem item in MenuList.Items)
                    {
                        item.Selected = selectedValues.Contains(item.Value);
                    }
                }
            }

            if (!string.IsNullOrEmpty(grid.MoreMenuProperty))
            {
                selectedValues = grid.MoreMenuProperty.Split(';').ToList();
                if (MoreMenuList.Items.Count > 0)
                {
                    foreach (ListItem item in MoreMenuList.Items)
                    {
                        if (selectedValues.Contains(item.Value))
                            item.Selected = true;
                        else
                            item.Selected = false;
                    }
                }
            }

            if (!string.IsNullOrEmpty(grid.DocumentListOptions))
            {
                selectedValues = grid.DocumentListOptions.Split(';').ToList();
                if (MoreMenuList.Items.Count > 0)
                {
                    foreach (ListItem item in DocumentListOptions.Items)
                    {
                        if (!selectedValues.Contains(item.Value))
                            item.Selected = true;
                        else
                            item.Selected = false;
                    }
                }
            }
        }

        protected override void CreateChildControls()
        {
            var ltlMenu = new Literal { Text = @"<h4> Context Menu Options</h4><br/>" };
            Controls.Add(ltlMenu);
            Controls.Add(MenuList);

            var ltlMoreMenu = new Literal { Text = @"<hr/><h4> More Menu Options </h4> <br/>" };
            Controls.Add(ltlMoreMenu);
            Controls.Add(MoreMenuList);

            var ltlDocumentListOptions = new Literal { Text = @"<hr/><h4> Document List Options</h4><br/>" };
            Controls.Add(ltlDocumentListOptions);
            Controls.Add(DocumentListOptions);
        }
    }
}
