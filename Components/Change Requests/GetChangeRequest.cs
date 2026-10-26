using System;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Retrieves one Change Request by ID. Response includes urls.app -- a direct link to view/edit
    // the Change Request in the GitBook web app.
    public class GetChangeRequestComponent : ButtonComponent
    {
        public GetChangeRequestComponent()
            : base("Get Change Request", "CR",
                "Retrieves one Change Request by ID.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Change Request ID", "CRID", "Change Request ID.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("URL", "URL", "Direct link to view/edit the Change Request in the GitBook web app.", GH_ParamAccess.item);
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
            var result = client.GetChangeRequestAsync(spaceId, changeRequestId).GetAwaiter().GetResult();

            if (result.Item1)
            {
                string url = "";
                try { url = JObject.Parse(result.Item2)["urls"]?.Value<string>("app") ?? ""; }
                catch { /* leave blank if response is not JSON */ }

                DA.SetData(0, url);
                DA.SetData(1, "OK");
                DA.SetData(2, result.Item2);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "");
                DA.SetData(1, "Failed.");
                DA.SetData(2, result.Item3);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetChangeRequest;
        public override Guid ComponentGuid => new Guid("5e136754-3b59-469b-9467-2815699cc8e7");
    }
}
