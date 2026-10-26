using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace GitBookConnect
{
    // Dual Style Pattern parametric variant. One shared parent target (item) plus every page property
    // as an independent, optional tree-access input, joined positionally per branch. Emits insert_page
    // when no Page ID is given for a branch/item, update_page when one is.
    public class BuildPagesFromTreeComponent : GH_Component
    {
        public BuildPagesFromTreeComponent()
            : base("Build Pages From Tree", "TreePages",
                "Builds one Change JSON op per branch item from parallel trees of page properties -- insert_page when Page ID is blank, update_page when it is set.",
                PluginUtilities.TabName, PluginUtilities.CategoryAEPages)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Into", "Into", "Shared parent page ID for every new page (blank = space root).", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Page ID", "PID", "Existing page ID per branch item (blank = insert a new page instead of updating).", GH_ParamAccess.tree);
            pManager.AddTextParameter("Title", "Ti", "Page title per branch item.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Body Text", "BT", "Page body (Markdown) per branch item.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Icon", "I", "Page icon per branch item.", GH_ParamAccess.tree);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "One insert_page/update_page Change JSON op per branch item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string into = "";
            GH_Structure<GH_String> pageIdTree, titleTree, bodyTree, iconTree;

            DA.GetData(0, ref into);
            DA.GetDataTree(1, out pageIdTree);
            DA.GetDataTree(2, out titleTree);
            DA.GetDataTree(3, out bodyTree);
            DA.GetDataTree(4, out iconTree);

            // Adapt the GH trees (GH_Structure/GH_Path) to plain per-branch lists, aligned by the union
            // of branch paths across all four trees -- the actual bulk assembly is a pure builder
            // method (PageBuilders.BuildChangeOpsFromBranches), so it stays unit-testable headless.
            var paths = new HashSet<GH_Path>();
            foreach (var t in new[] { pageIdTree, titleTree, bodyTree, iconTree })
                foreach (var p in t.Paths) paths.Add(p);

            var orderedPaths = paths.OrderBy(p => p.ToString()).ToList();
            var pageIdBranches = orderedPaths.Select(p => (IList<string>)BranchOrEmpty(pageIdTree, p)).ToList();
            var titleBranches = orderedPaths.Select(p => (IList<string>)BranchOrEmpty(titleTree, p)).ToList();
            var bodyBranches = orderedPaths.Select(p => (IList<string>)BranchOrEmpty(bodyTree, p)).ToList();
            var iconBranches = orderedPaths.Select(p => (IList<string>)BranchOrEmpty(iconTree, p)).ToList();

            var changeJson = PageBuilders.BuildChangeOpsFromBranches(into, pageIdBranches, titleBranches, bodyBranches, iconBranches, orderedPaths.Count);

            DA.SetDataList(0, changeJson);
        }

        private static List<string> BranchOrEmpty(GH_Structure<GH_String> tree, GH_Path path)
        {
            if (!tree.PathExists(path)) return new List<string>();
            return tree.get_Branch(path).Cast<GH_String>().Select(s => s?.Value ?? "").ToList();
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildPagesFromTree;
        public override Guid ComponentGuid => new Guid("1e6560f7-0bee-4a75-a26a-c4e941b87319");
    }
}
