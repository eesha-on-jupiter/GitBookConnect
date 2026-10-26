using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds an inline code mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class CodeMarkComponent : GH_Component
    {
        public CodeMarkComponent()
            : base("Code Mark", "Code",
                "Adds an inline code mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with code added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddCode(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_CodeMark;
        public override Guid ComponentGuid => new Guid("7ab8a063-5f58-44be-a908-eb869d2da8fb");
    }
}
