using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a color mark to a Text JSON leaf. Text Color and Background Color both take a GitBook color
    // token (see the Text Color preset) -- Background Color is GitBook's equivalent of a "highlight".
    public class ColorMarkComponent : GH_Component
    {
        public ColorMarkComponent()
            : base("Color Mark", "Color",
                "Adds a color mark to a Text JSON leaf. Background Color is GitBook's equivalent of a highlight.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
            pManager.AddTextParameter("Text Color", "TC", "GitBook color token.", GH_ParamAccess.item, "default");
            pManager.AddTextParameter("Background Color", "BC", "GitBook color token (highlight).", GH_ParamAccess.item, "default");
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with color added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            string textColor = "default";
            string backgroundColor = "default";
            DA.GetData(0, ref textJson);
            DA.GetData(1, ref textColor);
            DA.GetData(2, ref backgroundColor);

            DA.SetData(0, TextMarkBuilders.AddColor(textJson, textColor, backgroundColor));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_ColorMark;
        public override Guid ComponentGuid => new Guid("c7045c73-61bc-459a-add0-f53a83686933");
    }
}
