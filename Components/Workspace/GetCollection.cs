using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves one collection by ID.
    public class GetCollectionComponent : ButtonComponent
    {
        public GetCollectionComponent()
            : base("Get Collection", "Coll",
                "Retrieves one collection by ID.",
                PluginUtilities.TabName, PluginUtilities.CategoryABWorkspace)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Collection ID", "CID", "Collection ID.", GH_ParamAccess.item);
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
            string collectionId = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref collectionId);

            var client = new GitBookConnectClient(token);
            var result = client.GetCollectionAsync(collectionId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetCollection;
        public override Guid ComponentGuid => new Guid("fb5783d9-8a6d-4f0b-9ec5-2149fe2207b9");
    }
}
