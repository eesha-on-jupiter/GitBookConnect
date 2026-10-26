using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a strikethrough mark to a Text JSON leaf. Closest available mark to "underline" --
    // GitBook's schema has no underline mark.
    public class StrikethroughMarkComponent : GH_Component
    {
        public StrikethroughMarkComponent()
            : base("Strikethrough Mark", "Strike",
                "Adds a strikethrough mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with strikethrough added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddStrikethrough(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_StrikethroughMark;
        public override Guid ComponentGuid => new Guid("bf98cafb-12a1-49ac-ae55-7e44ed5af301");
    }
}
