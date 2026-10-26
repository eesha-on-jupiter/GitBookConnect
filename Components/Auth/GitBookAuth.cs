using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Validates a GitBook Personal Access Token against GET /user and reports the connected account.
    // Use a Personal Access Token (Settings -> Developer -> Personal Access Tokens), not an integration
    // token -- Change Request writes explicitly reject integration tokens. A personal token also inherits
    // the user's actual space role (Admin/Editor/Reviewer/Viewer); Reviewer/Viewer roles are still
    // blocked from creating Change Requests even with a valid token.
    public class GitBookAuthComponent : ButtonComponent
    {
        public GitBookAuthComponent()
            : base("Connect to GitBook", "Connect",
                "Validates a GitBook Personal Access Token via GET /user and reports the connected account.",
                PluginUtilities.TabName, PluginUtilities.CategoryAAAuth)
        { }

        public override string ButtonLabel => "Connect";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "GitBook Personal Access Token (Bearer).", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "S", "Connection status.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw GET /user response.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            DA.GetData(0, ref token);

            if (string.IsNullOrWhiteSpace(token))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No token supplied.");
                DA.SetData(0, "No token supplied.");
                DA.SetData(1, "");
                return;
            }

            var client = new GitBookConnectClient(token);
            var result = client.GetCurrentUserAsync().GetAwaiter().GetResult();

            if (result.Item1)
            {
                DA.SetData(0, "Connected.");
                DA.SetData(1, result.Item2);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, result.Item3);
                DA.SetData(0, "Failed to authenticate.");
                DA.SetData(1, result.Item3);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_GitBookAuth;
        public override Guid ComponentGuid => new Guid("52bebc6e-6c4e-459c-895f-0aa03878c71a");
    }
}
