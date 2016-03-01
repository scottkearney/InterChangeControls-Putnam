using Akumina.WebParts.Documents.Properties;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Akumina.WebParts.Documents.DocumentRefiner
{
    public class DocumentRefinerConfigurableEditorPart : EditorPart
    {
     
        string[] defaultFilter_ClassNames = { "Category", "Date", "ModifiedBy", "FileType"};

      
        protected InputFormCheckBoxList FilterList;


        public DocumentRefinerConfigurableEditorPart()
        {
                   
            FilterList = new InputFormCheckBoxList();
            FilterList.ID = "fitlerOptions";
            FilterList.DataSource = defaultFilter_ClassNames.ToList();
            FilterList.DataBind();
           
        }
        private void GetFilterOptions()
        { 
            FilterList.Items.Clear();
            foreach (var name in defaultFilter_ClassNames)
            { 
                ListItem listItem=new ListItem();
                listItem.Text=name.Split('|')[0];
                listItem.Value=name.Split('|')[1];
                listItem.Selected = true;
                FilterList.Items.Add(listItem);
            }
        }
        public override bool ApplyChanges()
        {
            try
            {
                string selectedVaues = string.Empty;
          
                DocumentRefiner refiner = (DocumentRefiner)WebPartToEdit;
               

                selectedVaues = string.Empty;
                if (FilterList.Items.Count > 0)
                {
                    
                    foreach (ListItem item in FilterList.Items)
                    {
                       
                       
                        if (!item.Selected)
                            selectedVaues += item.Value + ";";

                    }
                    selectedVaues = selectedVaues.TrimEnd(';');
                }

              refiner.FilterOptions = selectedVaues;




            }
            finally { }

            return true;
        }

        public override void SyncChanges()
        {
            try
            {
                   DocumentRefiner refiner = (DocumentRefiner)WebPartToEdit;
                List<string> selectedValues = new List<string>();


                if (refiner.FilterOptions!=null && !string.IsNullOrEmpty(refiner.FilterOptions))
                {
                    selectedValues = refiner.FilterOptions.Split(';').ToList();
                    if (FilterList.Items.Count > 0)
                    {
                        foreach (ListItem item in FilterList.Items)
                        {
                            if (!selectedValues.Contains(item.Value))
                                item.Selected = true;
                            else
                                item.Selected = false;

                        }
                    }
                }

            }
            finally
            {
            }
        }

        protected override void CreateChildControls()
        {
            try
            {
                Literal ltlMenu = new Literal();
                ltlMenu.Text = "<h4> Filter Options</h4><br/>";
                this.Controls.Add(ltlMenu);
                this.Controls.Add(FilterList);
              
            }
            finally
            {
            }
        }


    }
}
