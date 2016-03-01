using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI;
using Akumina.InterAction;

namespace Akumina.WebParts.Miscellaneous.PlaceHolder
{
    [ToolboxItem(false)]
    public class PlaceHolder : AkuminaBaseWebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath =
            @"~/_CONTROLTEMPLATES/15/Akumina.WebParts.Miscellaneous/PlaceHolder/PlaceHolderUserControl.ascx";

        protected override void CreateChildControls()
        {
            try
            {
                var response = GetInstructionSet(InstructionSet);
                var controlInstruction = response.GetValue("WebPartId", "");
                var items = controlInstruction.Replace("#", "").Split(';');
                foreach (var item in items)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }
                    Control control;
                    switch (item.ToLower())
                    {
                        case "image":
                            control = Page.LoadControl(typeof(Image.Image), null);
                            Controls.Add(control);
                            break;
                        case "traffic":
                            control = Page.LoadControl(typeof(Map.Map), null);
                            Controls.Add(control);
                            break;
                        case "weather":
                            control = Page.LoadControl(typeof(Weather.Weather), null);
                            Controls.Add(control);
                            break;
                        case "video":
                            control = Page.LoadControl(typeof(Video.Video), null);
                            Controls.Add(control);
                            break;
                        case "banner":
                            var webPartAssembly = Assembly.Load("Akumina.WebParts.Banner.dll");
                            var webPartType = webPartAssembly.GetType("Banner.Banner");
                            control = Page.LoadControl(webPartType, new object[] { "AkuminaBannerIDS.1" });
                            Controls.Add(control);
                            break;
                        default:
                            control = Page.LoadControl(_ascxPath);
                            Controls.Add(control);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Page.Response.Write(ex.ToString());
            }
        }
    }
}