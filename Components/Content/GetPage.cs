using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves one published page by ID.
    public class GetPageComponent : ButtonComponent
    {
        public GetPageComponent()
            : base("Get Page", "Page",
                "Retrieves one published page by ID.",
                PluginUtilities.TabName, PluginUtilities.CategoryACContent)
        { }

        public override string ButtonLabel => "Get";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Page ID", "PID", "Page ID.", GH_ParamAccess.item);
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
            string pageId = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref pageId);

            var client = new GitBookConnectClient(token);
            var result = client.GetPageAsync(spaceId, pageId).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetPage;
        public override Guid ComponentGuid => new Guid("7f97dd2d-570b-4616-8d46-817c5a9f55b5");
    }
}
