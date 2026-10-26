using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Flattens every wired Change JSON (from Insert/Update Page, Build Pages From Tree, Build Insert
    // File) into one ordered batch, capped at the API's 50-per-call limit. Extra items beyond 50 are
    // dropped with a warning rather than silently truncating the request GitBook would reject.
    public class ContentChangesAggregatorComponent : GH_Component
    {
        private const int MaxChangesPerCall = 50;

        public ContentChangesAggregatorComponent()
            : base("Content Changes Aggregator", "Changes",
                "Flattens every wired Change JSON into one ordered batch, capped at the API's 50-per-call limit.",
                PluginUtilities.TabName, PluginUtilities.CategoryAHCompose)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "Change JSON items, in the order they should be applied.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "Validated batch, capped at 50 items.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var changeJson = new List<string>();
            DA.GetDataList(0, changeJson);

            var valid = new List<string>();
            for (int i = 0; i < changeJson.Count; i++)
            {
                try { JObject.Parse(changeJson[i]); valid.Add(changeJson[i]); }
                catch { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Item " + i + " is not valid JSON -- skipped."); }
            }

            if (valid.Count > MaxChangesPerCall)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, valid.Count + " changes supplied; only the first " + MaxChangesPerCall + " are sent (API limit). Split into multiple Apply Content Changes calls for the rest.");
                valid = valid.GetRange(0, MaxChangesPerCall);
            }

            DA.SetDataList(0, valid);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_ContentChangesAggregator;
        public override Guid ComponentGuid => new Guid("180af87f-09a1-428b-b64a-0e9e9b45ad69");
    }
}
