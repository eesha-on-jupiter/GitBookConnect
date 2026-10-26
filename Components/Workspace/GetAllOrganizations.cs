using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Lists every organization the token's account belongs to.
    public class GetAllOrganizationsComponent : ButtonComponent
    {
        public GetAllOrganizationsComponent()
            : base("List Organizations", "AllOrgs",
                "Lists every organization the token's account belongs to.",
                PluginUtilities.TabName, PluginUtilities.CategoryABWorkspace)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
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
            DA.GetData(0, ref token);

            var client = new GitBookConnectClient(token);
            var result = client.ListOrganizationsAsync().GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetAllOrganizations;
        public override Guid ComponentGuid => new Guid("aaf50f83-e1d9-49e0-9b54-f9ff91296308");
    }
}
