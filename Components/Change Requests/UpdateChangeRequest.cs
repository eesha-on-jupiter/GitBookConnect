using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Updates an existing Change Request's subject and/or description. Both inputs are optional --
    // only the fields supplied are sent.
    public class UpdateChangeRequestComponent : ButtonComponent
    {
        public UpdateChangeRequestComponent()
            : base("Update Change Request", "UpdCR",
                "Updates an existing Change Request's subject and/or description.",
                PluginUtilities.TabName, PluginUtilities.CategoryADChangeRequests)
        { }

        public override string ButtonLabel => "Update";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Change Request ID", "CRID", "Change Request ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Subject", "Su", "New subject.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Description", "D", "New description.", GH_ParamAccess.item, "");
            pManager[3].Optional = true;
            pManager[4].Optional = true;
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
            string subject = "";
            string description = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref changeRequestId);
            DA.GetData(3, ref subject);
            DA.GetData(4, ref description);

            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(description))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Provide a Subject and/or a Description to update.");
                DA.SetData(0, "Nothing to update.");
                DA.SetData(1, "");
                return;
            }

            var client = new GitBookConnectClient(token);
            var result = client.UpdateChangeRequestAsync(spaceId, changeRequestId, subject, description).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_UpdateChangeRequest;
        public override Guid ComponentGuid => new Guid("ba26f2a7-1e72-42ee-a5f5-82f8789b05b0");
    }
}
