using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Secondary/lightweight page path -- prefer Build Page (JSON) in Rich Content for styled text,
    // blocks, and tables. Produces the same Document JSON contract as Build Page (JSON), so both are
    // interchangeable into Insert Page / Update Page.
    public class BuildPageMarkdownComponent : GH_Component
    {
        public BuildPageMarkdownComponent()
            : base("Build Page (Markdown)", "MDPage",
                "Builds a page's Document JSON from a plain Markdown body. Prefer Build Page (JSON) for styled text, blocks, and tables.",
                PluginUtilities.TabName, PluginUtilities.CategoryAEPages)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Title", "Ti", "Page title.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Markdown Body", "MB", "Page content as Markdown.", GH_ParamAccess.item);
            pManager.AddTextParameter("Icon", "I", "Page icon (emoji or GitBook icon token).", GH_ParamAccess.item, "");
            pManager[0].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Document JSON", "DJ", "Page Document JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string title = "";
            string markdownBody = "";
            string icon = "";
            DA.GetData(0, ref title);
            DA.GetData(1, ref markdownBody);
            DA.GetData(2, ref icon);

            DA.SetData(0, PageBuilders.BuildDocumentFromMarkdown(title, markdownBody, icon));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildPageMarkdown;
        public override Guid ComponentGuid => new Guid("0b4d39a7-c731-41e8-9e79-269cb19d1ce7");
    }
}
