using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace GitBookConnect
{
    // Cross-platform tabular benchmark, JSON path. Rows x columns tree -> a GitBook grid-view table
    // block with plain text cells. Simplified v1 scope -- does not cover typed columns or card-view tables.
    public class TableBlockComponent : GH_Component
    {
        public TableBlockComponent()
            : base("Table Block", "Table",
                "Builds a GitBook grid-view table block from a Header Row and a rows x columns Grid tree.",
                PluginUtilities.TabName, PluginUtilities.CategoryAGRichContent)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Header Row", "H", "Column headers.", GH_ParamAccess.list);
            pManager.AddTextParameter("Grid", "G", "Cell data -- one branch per row, one item per column.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Block JSON", "BJ", "Table block JSON.", GH_ParamAccess.item);
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

            DA.SetData(0, TableBuilders.BuildTableBlock(headerRow, rows));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_TableBlock;
        public override Guid ComponentGuid => new Guid("4b90f4b0-e48c-4354-9418-b464920f9145");
    }
}
