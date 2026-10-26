using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Builds a hint/callout block from styled Text JSON runs (from the Text tab).
    public class BuildHintBlockComponent : GH_Component
    {
        public BuildHintBlockComponent()
            : base("Hint Block", "Hint",
                "Builds a hint/callout block from styled Text JSON runs.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Style", "St", "Hint style (info / warning / danger / success).", GH_ParamAccess.item, "info");
            pManager.AddTextParameter("Text JSON", "TJ", "Styled Text JSON runs.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Hint block JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string style = "info";
            var textJson = new List<string>();
            DA.GetData(0, ref style);
            DA.GetDataList(1, textJson);

            DA.SetData(0, BlockBuilders.BuildHintBlock(style, textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildHintBlock;
        public override Guid ComponentGuid => new Guid("8e5e63da-421a-4403-886c-87300dee6385");
    }
}
