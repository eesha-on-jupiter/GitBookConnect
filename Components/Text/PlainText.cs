using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Wraps a raw string into a Text JSON leaf with no marks -- the starting point for a styled run.
    // Chain Mark components after it, or use it unstyled.
    public class PlainTextComponent : GH_Component
    {
        public PlainTextComponent()
            : base("Plain Text", "Text",
                "Wraps a raw string into a Text JSON leaf with no marks. Chain Mark components after it, or use it unstyled.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Tx", "Raw text.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, no marks.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            DA.GetData(0, ref text);

            DA.SetData(0, TextMarkBuilders.BuildPlainText(text));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_PlainText;
        public override Guid ComponentGuid => new Guid("81017435-1417-4720-a99a-6559156dd920");
    }
}
