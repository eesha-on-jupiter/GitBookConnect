using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Primary page-authoring path. Takes the assembled Blocks JSON from Compose's Blocks Aggregator.
    // Same Document JSON contract as Build Page (Markdown), so both are interchangeable into
    // Insert Page / Update Page.
    public class BuildRichPageComponent : GH_Component
    {
        public BuildRichPageComponent()
            : base("Build Page (JSON)", "JSONPage",
                "Builds a page's Document JSON from assembled Blocks JSON. Primary page-authoring path.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "Ti", "Page title.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Blocks JSON", "BLJ", "Assembled Blocks JSON (from the Blocks Aggregator).", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Document JSON", "DJ", "Page Document JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string title = "";
            var blocksJson = new List<string>();
            DA.GetData(0, ref title);
            DA.GetDataList(1, blocksJson);

            DA.SetData(0, PageBuilders.BuildDocumentFromBlocks(title, blocksJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildRichPage;
        public override Guid ComponentGuid => new Guid("e56efe46-e65f-4c3c-b34f-948bd34bdf21");
    }
}
