using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Builds an expandable/collapsible block from styled Text JSON runs (from the Text tab).
    public class BuildExpandableBlockComponent : GH_Component
    {
        public BuildExpandableBlockComponent()
            : base("Expandable Block", "Expand",
                "Builds an expandable/collapsible block from styled Text JSON runs.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "Ti", "Expandable title.", GH_ParamAccess.item);
            pManager.AddTextParameter("Text JSON", "TJ", "Styled Text JSON runs.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Default Expanded", "DE", "Whether the block starts expanded.", GH_ParamAccess.item, false);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Expandable block JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string title = "";
            var textJson = new List<string>();
            bool defaultExpanded = false;
            DA.GetData(0, ref title);
            DA.GetDataList(1, textJson);
            DA.GetData(2, ref defaultExpanded);

            DA.SetData(0, BlockBuilders.BuildExpandableBlock(title, textJson, defaultExpanded));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildExpandableBlock;
        public override Guid ComponentGuid => new Guid("f815800a-43ef-47ec-af31-1cbf943930c1");
    }
}
