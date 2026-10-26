using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Wraps one or more styled Text JSON runs (from the Text tab) into a paragraph block.
    public class ParagraphBlockComponent : GH_Component
    {
        public ParagraphBlockComponent()
            : base("Paragraph Block", "Para",
                "Wraps one or more styled Text JSON runs into a paragraph block.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Styled Text JSON runs.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Paragraph block JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var textJson = new List<string>();
            DA.GetDataList(0, textJson);

            DA.SetData(0, BlockBuilders.BuildParagraphBlock(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_ParagraphBlock;
        public override Guid ComponentGuid => new Guid("d802de3a-ab3e-4a72-a2a7-2d3dff8749b6");
    }
}
