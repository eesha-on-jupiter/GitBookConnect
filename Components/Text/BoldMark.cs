using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a bold mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class BoldMarkComponent : GH_Component
    {
        public BoldMarkComponent()
            : base("Bold Mark", "Bold",
                "Adds a bold mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with bold added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddBold(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BoldMark;
        public override Guid ComponentGuid => new Guid("02699ba1-c415-4b01-a0e8-8027692ddc38");
    }
}
