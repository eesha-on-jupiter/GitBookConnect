using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Batch-creates pages by wiring lists -- one Change JSON insert_page op per Document JSON item,
    // index-matched against Into/At/Ref.
    public class InsertPageComponent : GH_Component
    {
        public InsertPageComponent()
            : base("Insert Page", "InsPage",
                "Builds one insert_page Change JSON op per Document JSON item, index-matched against Into/At/Ref.",
                PluginUtilities.TabName, PluginUtilities.CategoryAEPages)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Document JSON", "DJ", "Page Document JSON item(s).", GH_ParamAccess.list);
            pManager.AddTextParameter("Into", "Into", "Parent page ID, index-matched to Document JSON (blank = space root).", GH_ParamAccess.list);
            pManager.AddIntegerParameter("At", "At", "Position among siblings, index-matched to Document JSON.", GH_ParamAccess.list);
            pManager.AddTextParameter("Ref", "Ref", "Reference key, index-matched to Document JSON.", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "One insert_page Change JSON op per item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var documentJson = new List<string>();
            var into = new List<string>();
            var at = new List<int>();
            var refs = new List<string>();
            DA.GetDataList(0, documentJson);
            DA.GetDataList(1, into);
            DA.GetDataList(2, at);
            DA.GetDataList(3, refs);

            var changeJson = new List<string>();
            for (int i = 0; i < documentJson.Count; i++)
            {
                string parent = i < into.Count ? into[i] : "";
                int? position = i < at.Count ? at[i] : (int?)null;
                string reference = i < refs.Count ? refs[i] : "";
                changeJson.Add(PageBuilders.BuildInsertPageOp(documentJson[i], parent, position, reference));
            }

            DA.SetDataList(0, changeJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_InsertPage;
        public override Guid ComponentGuid => new Guid("e1e0e7e2-97b1-4c63-bfa8-42da33da9f73");
    }
}
