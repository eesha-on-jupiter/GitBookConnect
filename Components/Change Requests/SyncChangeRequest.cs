using System;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Rebases a Change Request on the space's latest live content before merging. An HTTP 409
    // "Branch is already up-to-date" is GitBook's normal response when there is nothing to rebase --
    // reported as an informational status here, not an error.
    public class SyncChangeRequestComponent : ButtonComponent
    {
        public SyncChangeRequestComponent()
            : base("Sync Change Request", "SyncCR",
                "Rebases a Change Request on the space's latest live content before merging.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Sync";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Change Request ID", "CRID", "Change Request ID.", GH_ParamAccess.item);
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
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref changeRequestId);

            var client = new GitBookConnectClient(token);
            var result = client.SyncChangeRequestAsync(spaceId, changeRequestId).GetAwaiter().GetResult();

            if (result.Item1)
            {
                bool alreadyUpToDate = false;
                try { alreadyUpToDate = JObject.Parse(result.Item2)["error"]?.Value<int?>("code") == 409; }
                catch { /* leave false if response is not JSON */ }

                DA.SetData(0, alreadyUpToDate ? "Already up-to-date." : "Synced.");
                DA.SetData(1, result.Item2);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "Failed.");
                DA.SetData(1, result.Item3);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_SyncChangeRequest;
        public override Guid ComponentGuid => new Guid("cc2cd68d-94e7-46f4-9161-771172994c76");
    }
}
