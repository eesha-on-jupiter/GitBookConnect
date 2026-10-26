using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Lists every space in an organization.
    public class GetAllSpacesComponent : ButtonComponent
    {
        public GetAllSpacesComponent()
            : base("List Spaces", "AllSpaces",
                "Lists every space in an organization.",
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
            var result = client.ListSpacesAsync(organizationId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetAllSpaces;
        public override Guid ComponentGuid => new Guid("3e55dfac-3ef3-4a03-9cd6-a56eeb016b9a");
    }
}
