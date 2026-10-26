using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace GitBookConnect
{
    // Secondary path for the tabular benchmark -- prefer Table Block (Rich Content) for JSON. Rows x
    // columns tree -> a Markdown table string, for Build Page (Markdown)'s Markdown Body.
    public class MarkdownTableFromGridComponent : GH_Component
    {
        public MarkdownTableFromGridComponent()
            : base("Markdown Table From Grid", "GridToMD",
                "Builds a Markdown table from a Header Row and a rows x columns Grid tree. Prefer Table Block for the JSON path.",
                PluginUtilities.TabName, PluginUtilities.CategoryAEPages)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Header Row", "H", "Column headers.", GH_ParamAccess.list);
            pManager.AddTextParameter("Grid", "G", "Cell data -- one branch per row, one item per column.", GH_ParamAccess.tree);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Markdown Body", "MB", "Markdown table.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var headerRow = new List<string>();
            GH_Structure<GH_String> grid;

            DA.GetDataList(0, headerRow);
            if (!DA.GetDataTree(1, out grid)) return;

            var rows = new List<IList<string>>();
            foreach (var branch in grid.Branches)
            {
                var row = new List<string>();
                foreach (var cell in branch) row.Add(cell?.Value ?? "");
                rows.Add(row);
            }

            DA.SetData(0, TableBuilders.BuildMarkdownTable(headerRow, rows));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_MarkdownTableFromGrid;
        public override Guid ComponentGuid => new Guid("116aa437-0ea3-45ce-944a-584e894a30bb");
    }
}
