using System;
using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Opens a new Change Request (the write branch) on a space. GitBook's create endpoint only accepts
    // a Subject at creation time; if a Description is also supplied it is set with a follow-up
    // Update Change Request call. Response includes urls.app -- a direct link to view/edit the Change
    // Request in the GitBook web app.
    public class CreateChangeRequestComponent : ButtonComponent
    {
        public CreateChangeRequestComponent()
            : base("Create Change Request", "NewCR",
                "Opens a new Change Request (the write branch) on a space. A Description, if given, is set with a follow-up Update Change Request call since GitBook does not accept one at creation time.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Create";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Subject", "Su", "Change Request subject.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Description", "D", "Change Request description (set via a follow-up update call).", GH_ParamAccess.item, "");
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change Request ID", "CRID", "ID of the newly created Change Request.", GH_ParamAccess.item);
            pManager.AddTextParameter("URL", "URL", "Direct link to view/edit the Change Request in the GitBook web app.", GH_ParamAccess.item);
            pManager.AddTextParameter("Status", "S", "Request status.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw response JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string spaceId = "";
            string subject = "";
            string description = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref subject);
            DA.GetData(3, ref description);

            var client = new GitBookConnectClient(token);
            var result = client.CreateChangeRequestAsync(spaceId, subject).GetAwaiter().GetResult();

            if (!result.Item1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "");
                DA.SetData(1, "");
                DA.SetData(2, "Failed.");
                DA.SetData(3, result.Item3);
                return;
            }

            string crId = "";
            string url = "";
            try
            {
                var obj = JObject.Parse(result.Item2);
                crId = obj.Value<string>("id") ?? "";
                url = obj["urls"]?.Value<string>("app") ?? "";
            }
            catch { /* leave blank if response is not JSON */ }

            if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(crId))
            {
                var updateResult = client.UpdateChangeRequestAsync(spaceId, crId, null, description).GetAwaiter().GetResult();
                if (!updateResult.Item1)
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Change Request created, but setting the description failed: " + updateResult.Item3);
            }

            DA.SetData(0, crId);
            DA.SetData(1, url);
            DA.SetData(2, "Created.");
            DA.SetData(3, result.Item2);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_CreateChangeRequest;
        public override Guid ComponentGuid => new Guid("f0527569-3f94-41d3-9414-f0788a7bbda6");
    }
}
