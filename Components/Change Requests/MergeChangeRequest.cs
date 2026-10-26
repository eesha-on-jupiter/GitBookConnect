using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Publishes the Change Request into the space's live content. This is the only way content
    // actually goes live -- irreversible via the API.
    public class MergeChangeRequestComponent : ButtonComponent
    {
        public MergeChangeRequestComponent()
            : base("Merge Change Request", "Merge",
                "Publishes a Change Request's draft content into the space's live content. Irreversible.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Merge";

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
            var result = client.MergeChangeRequestAsync(spaceId, changeRequestId).GetAwaiter().GetResult();

            if (result.Item1)
            {
                DA.SetData(0, "Merged.");
                DA.SetData(1, result.Item2);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "Failed.");
                DA.SetData(1, result.Item3);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_MergeChangeRequest;
        public override Guid ComponentGuid => new Guid("a45202ab-c3d2-4ab8-aada-6dda2e09e509");
    }
}
