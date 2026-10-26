using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // The single batch write call. Sends up to 50 Change JSON operations at once (API limit) -- wire
    // the Compose tab's Content Changes Aggregator output here.
    public class ApplyContentChangesComponent : ButtonComponent
    {
        public ApplyContentChangesComponent()
            : base("Apply Content Changes", "Apply",
                "Applies a batch of Change JSON operations to a Change Request. Wire the Content Changes Aggregator output here.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Apply";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Change Request ID", "CRID", "Change Request ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Change JSON", "CJ", "Change JSON operations to apply (up to 50).", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "S", "Request status.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw response JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string spaceId = "";
            string changeRequestId = "";
            var changeJson = new List<string>();
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref changeRequestId);
            DA.GetDataList(3, changeJson);

            if (changeJson.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No Change JSON supplied.");
                DA.SetData(0, "Nothing to apply.");
                DA.SetData(1, "");
                return;
            }

            var changesArray = new JArray();
            foreach (var c in changeJson) changesArray.Add(JObject.Parse(c));

            var client = new GitBookConnectClient(token);
            var result = client.ApplyContentChangesAsync(spaceId, changeRequestId, changesArray.ToString(Newtonsoft.Json.Formatting.None)).GetAwaiter().GetResult();

            if (result.Item1)
            {
                DA.SetData(0, "OK");
                DA.SetData(1, result.Item2);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "Failed.");
                DA.SetData(1, result.Item3);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_ApplyContentChanges;
        public override Guid ComponentGuid => new Guid("49d36883-02c8-44fc-ac0f-2da589740d2d");
    }
}
