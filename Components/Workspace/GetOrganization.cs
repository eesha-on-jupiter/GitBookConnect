using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves one organization by ID.
    public class GetOrganizationComponent : ButtonComponent
    {
        public GetOrganizationComponent()
            : base("Get Organization", "Org",
                "Retrieves one organization by ID.",
                PluginUtilities.TabName, PluginUtilities.CategoryABWorkspace)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Organization ID", "OID", "Organization ID.", GH_ParamAccess.item);
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
            string organizationId = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref organizationId);

            var client = new GitBookConnectClient(token);
            var result = client.GetOrganizationAsync(organizationId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetOrganization;
        public override Guid ComponentGuid => new Guid("a5262f53-74b4-46ce-b34d-ea404cea26b5");
    }
}
