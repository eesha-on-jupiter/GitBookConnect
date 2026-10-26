using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a subscript mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class SubscriptMarkComponent : GH_Component
    {
        public SubscriptMarkComponent()
            : base("Subscript Mark", "Sub",
                "Adds a subscript mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with subscript added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddSubscript(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_SubscriptMark;
        public override Guid ComponentGuid => new Guid("43c70096-99f6-43b3-8ce8-f8592dca8a8d");
    }
}
