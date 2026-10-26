using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves a space's currently published (merged) content — read-only, never the write path.
    public class GetPublishedContentComponent : ButtonComponent
    {
        public GetPublishedContentComponent()
            : base("Get Published Content", "Content",
                "Retrieves a space's currently published (merged) content.",
                PluginUtilities.TabName, PluginUtilities.CategoryACContent)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
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
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);

            var client = new GitBookConnectClient(token);
            var result = client.GetPublishedContentAsync(spaceId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetPublishedContent;
        public override Guid ComponentGuid => new Guid("62e648bd-e057-48d7-8044-42f70a7e2f91");
    }
}
