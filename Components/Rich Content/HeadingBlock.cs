using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Wraps one or more styled Text JSON runs (from the Text tab) into a heading block.
    public class HeadingBlockComponent : GH_Component
    {
        public HeadingBlockComponent()
            : base("Heading Block", "Head",
                "Wraps one or more styled Text JSON runs into a heading block.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Styled Text JSON runs.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Level", "Lv", "Heading level (1-6).", GH_ParamAccess.item, 2);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Heading block JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var textJson = new List<string>();
            int level = 2;
            DA.GetDataList(0, textJson);
            DA.GetData(1, ref level);

            DA.SetData(0, BlockBuilders.BuildHeadingBlock(textJson, level));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_HeadingBlock;
        public override Guid ComponentGuid => new Guid("1b9782a9-8db5-478f-ace6-aeaa45702cb9");
    }
}
