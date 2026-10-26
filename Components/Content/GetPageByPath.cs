using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Retrieves one published page by its path (slug), e.g. "getting-started/quickstart".
    public class GetPageByPathComponent : ButtonComponent
    {
        public GetPageByPathComponent()
            : base("Find Page By Path", "FindPage",
                "Retrieves one published page by its path (slug) rather than its ID.",
                PluginUtilities.TabName, PluginUtilities.CategoryACContent)
        { }

        public override string ButtonLabel => "Find";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Space ID", "SID", "Space ID.", GH_ParamAccess.item);
            pManager.AddTextParameter("Page Path", "PP", "Page path (slug), e.g. \"getting-started/quickstart\".", GH_ParamAccess.item);
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
            string pagePath = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref spaceId);
            DA.GetData(2, ref pagePath);

            var client = new GitBookConnectClient(token);
            var result = client.GetPageByPathAsync(spaceId, pagePath).GetAwaiter().GetResult();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GetPageByPath;
        public override Guid ComponentGuid => new Guid("e8ed0b18-0feb-4514-8672-e7c5b415b77b");
    }
}
