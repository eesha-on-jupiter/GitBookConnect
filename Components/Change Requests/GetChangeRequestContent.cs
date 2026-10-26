using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves the draft content tree of a Change Request.
    public class GetChangeRequestContentComponent : ButtonComponent
    {
        public GetChangeRequestContentComponent()
            : base("Get Change Request Content", "CRContent",
                "Retrieves the draft content tree of a Change Request.",
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
            var result = client.GetChangeRequestContentAsync(spaceId, changeRequestId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetChangeRequestContent;
        public override Guid ComponentGuid => new Guid("29526793-0054-4e4a-b507-3c2644f2cf11");
    }
}
