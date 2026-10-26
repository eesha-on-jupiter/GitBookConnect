using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Lists every collection in an organization. GitBook's collections grouping feature is optional --
    // an organization that never used it will return an empty list.
    public class GetAllCollectionsComponent : ButtonComponent
    {
        public GetAllCollectionsComponent()
            : base("List Collections", "AllColls",
                "Lists every collection in an organization. Returns an empty list if the organization never used GitBook's collections feature.",
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
            var result = client.ListCollectionsAsync(organizationId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetAllCollections;
        public override Guid ComponentGuid => new Guid("3c2a8fc5-710d-4354-acbb-e9a0f057d141");
    }
}
