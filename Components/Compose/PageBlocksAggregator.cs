using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Gathers Block JSON from any Rich Content block or Build Insert File into the Blocks JSON list
    // that Build Page (JSON) consumes. Validates each item is parseable JSON; invalid items are
    // dropped with a warning rather than failing the whole page.
    public class PageBlocksAggregatorComponent : GH_Component
    {
        public PageBlocksAggregatorComponent()
            : base("Blocks Aggregator", "Blocks",
                "Gathers Block JSON from any Rich Content block or Build Insert File into the Blocks JSON list that Build Page (JSON) consumes.",
                PluginUtilities.TabName, PluginUtilities.CategoryAHCompose)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Block JSON items, in the order they should appear on the page.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Blocks JSON", "BLJ", "Validated Block JSON items, in order.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var blockJson = new List<string>();
            DA.GetDataList(0, blockJson);

            var valid = new List<string>();
            for (int i = 0; i < blockJson.Count; i++)
            {
                var item = blockJson[i];
                try { JObject.Parse(item); valid.Add(item); }
                catch { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Item " + i + " is not valid JSON -- skipped."); }
            }

            DA.SetDataList(0, valid);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_PageBlocksAggregator;
        public override Guid ComponentGuid => new Guid("f69836cc-6b31-4813-bc92-7854051b39fd");
    }
}
