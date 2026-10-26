using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Batch-updates pages by wiring lists -- one Change JSON update_page op per Page ID, index-matched
    // against Document JSON/Into/At.
    public class UpdatePageComponent : GH_Component
    {
        public UpdatePageComponent()
            : base("Update Page", "UpdPage",
                "Builds one update_page Change JSON op per Page ID, index-matched against Document JSON/Into/At.",
                PluginUtilities.TabName, PluginUtilities.CategoryAEPages)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Page ID", "PID", "Page ID(s) to update.", GH_ParamAccess.list);
            pManager.AddTextParameter("Document JSON", "DJ", "New Document JSON, index-matched to Page ID.", GH_ParamAccess.list);
            pManager.AddTextParameter("Into", "Into", "New parent page ID, index-matched to Page ID.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("At", "At", "New position among siblings, index-matched to Page ID.", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "One update_page Change JSON op per item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var pageIds = new List<string>();
            var documentJson = new List<string>();
            var into = new List<string>();
            var at = new List<int>();
            DA.GetDataList(0, pageIds);
            DA.GetDataList(1, documentJson);
            DA.GetDataList(2, into);
            DA.GetDataList(3, at);

            var changeJson = new List<string>();
            for (int i = 0; i < pageIds.Count; i++)
            {
                string doc = i < documentJson.Count ? documentJson[i] : "";
                string parent = i < into.Count ? into[i] : "";
                int? position = i < at.Count ? at[i] : (int?)null;
                changeJson.Add(PageBuilders.BuildUpdatePageOp(pageIds[i], doc, parent, position));
            }

            DA.SetDataList(0, changeJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_UpdatePage;
        public override Guid ComponentGuid => new Guid("a022359f-2f88-42f8-816e-a1a004523f4e");
    }
}
