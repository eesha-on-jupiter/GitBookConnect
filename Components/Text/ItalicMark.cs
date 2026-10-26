using System;
using Grasshopper.Kernel;

namespace GitBookConnect
{
    // Adds an italic mark to a Text JSON leaf. Chain onto another mark's output to combine styles.
    public class ItalicMarkComponent : GH_Component
    {
        public ItalicMarkComponent()
            : base("Italic Mark", "Italic",
                "Adds an italic mark to a Text JSON leaf.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFText)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text JSON", "TJ", "Text JSON leaf, with italic added.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string textJson = "";
            DA.GetData(0, ref textJson);

            DA.SetData(0, TextMarkBuilders.AddItalic(textJson));
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_ItalicMark;
        public override Guid ComponentGuid => new Guid("69b22698-9749-4e99-ab8b-4b001eb6abad");
    }
}
