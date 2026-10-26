using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds a superscript mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class SuperscriptMarkComponent : GH_Component
    {
        public SuperscriptMarkComponent()
            : base("Superscript Mark", "Sup",
                "Adds a superscript mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with superscript added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddSuperscript(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_SuperscriptMark;
        public override Guid ComponentGuid => new Guid("459efda1-0c51-431f-a4f4-968a73e77d72");
    }
}
