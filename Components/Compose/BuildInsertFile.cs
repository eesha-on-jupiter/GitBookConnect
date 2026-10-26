using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace GitBookConnect
{
    // Builds an image/file from whichever source is provided per list item: File Path (reads local
    // disk), Bitmap (a GH Bitmap object, PNG-encoded), Source URL (GitBook fetches it), Base64
    // (inline, capped at 1MB by the API), or Capture Rhino Viewport (captures the active Rhino 3D
    // view when true -- View Name selects a named view, empty = active view). Exactly one source
    // should be set per item; Capture Rhino Viewport takes priority when true. Bundles all items into
    // one insert_files Change JSON and emits one Image Block per item.
    public class BuildInsertFileComponent : ButtonComponent
    {
        public BuildInsertFileComponent()
            : base("Build Insert File", "InsFile",
                "Builds an image/file insert from whichever source is provided per item (File Path, Bitmap, Source URL, Base64, or a captured Rhino viewport). Bundles all items into one insert_files Change JSON and emits one Image Block per item.",
                PluginUtilities.TabName, PluginUtilities.CategoryAHCompose)
        { }

        public override string ButtonLabel => "Build";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Name", "FN", "File name(s), e.g. \"diagram.png\".", GH_ParamAccess.list);
            pManager.AddTextParameter("File Path", "FP", "Local file path(s) to read from disk.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Bitmap", "BMP", "GH Bitmap object(s) (PNG-encoded).", GH_ParamAccess.list);
            pManager.AddTextParameter("Source URL", "URL", "Externally hosted URL(s) for GitBook to fetch.", GH_ParamAccess.list);
            pManager.AddTextParameter("Base64", "B64", "Inline base64-encoded file content (capped at 1MB by the API).", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Capture Rhino Viewport", "CRV", "Capture the active Rhino 3D view for this item (takes priority when true).", GH_ParamAccess.list);
            pManager.AddTextParameter("View Name", "V", "Named Rhino viewport to capture (blank = the currently active viewport).", GH_ParamAccess.list);
            pManager.AddTextParameter("Ref", "Ref", "Reference key linking each file to its Image Block (blank = auto-generated).", GH_ParamAccess.list);
            for (int i = 1; i <= 7; i++) pManager[i].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Change JSON", "CJ", "One insert_files Change JSON op covering every item.", GH_ParamAccess.item);
            pManager.AddTextParameter("Block JSON", "BJ", "One Image Block per item.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            var fileNames = new List<string>();
            var filePaths = new List<string>();
            var bitmaps = new List<IGH_Goo>();
            var sourceUrls = new List<string>();
            var base64s = new List<string>();
            var captureFlags = new List<bool>();
            var viewNames = new List<string>();
            var refs = new List<string>();

            DA.GetDataList(0, fileNames);
            DA.GetDataList(1, filePaths);
            DA.GetDataList(2, bitmaps);
            DA.GetDataList(3, sourceUrls);
            DA.GetDataList(4, base64s);
            DA.GetDataList(5, captureFlags);
            DA.GetDataList(6, viewNames);
            DA.GetDataList(7, refs);

            int n = fileNames.Count;
            if (n == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Provide at least one File Name.");
                return;
            }

            var resolvedRefs = new List<string>();
            var base64Contents = new List<string>();
            var urls = new List<string>();
            var blockJson = new List<string>();

            for (int i = 0; i < n; i++)
            {
                string reference = i < refs.Count && !string.IsNullOrWhiteSpace(refs[i]) ? refs[i] : Guid.NewGuid().ToString("N");
                bool capture = i < captureFlags.Count && captureFlags[i];
                string filePath = i < filePaths.Count ? filePaths[i] : "";
                var bitmapGoo = i < bitmaps.Count ? bitmaps[i] : null;
                string sourceUrl = i < sourceUrls.Count ? sourceUrls[i] : "";
                string base64 = i < base64s.Count ? base64s[i] : "";
                string viewName = i < viewNames.Count ? viewNames[i] : "";

                string resolvedBase64 = "";
                string resolvedUrl = "";

                if (capture)
                {
                    string captured = CaptureViewportToBase64(viewName);
                    if (captured == null)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Item " + i + ": viewport capture failed.");
                        continue;
                    }
                    resolvedBase64 = captured;
                }
                else if (!string.IsNullOrWhiteSpace(filePath))
                {
                    try { resolvedBase64 = Convert.ToBase64String(File.ReadAllBytes(filePath)); }
                    catch (Exception ex)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Item " + i + ": could not read \"" + filePath + "\" (" + ex.Message + ").");
                        continue;
                    }
                }
                else if (bitmapGoo != null)
                {
                    string encoded = EncodeGooBitmap(bitmapGoo);
                    if (encoded == null)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Item " + i + ": Bitmap input is not a recognized image type.");
                        continue;
                    }
                    resolvedBase64 = encoded;
                }
                else if (!string.IsNullOrWhiteSpace(sourceUrl))
                {
                    resolvedUrl = sourceUrl;
                }
                else if (!string.IsNullOrWhiteSpace(base64))
                {
                    resolvedBase64 = base64;
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Item " + i + ": no source provided (File Path / Bitmap / Source URL / Base64 / Capture Rhino Viewport).");
                    continue;
                }

                resolvedRefs.Add(reference);
                base64Contents.Add(resolvedBase64);
                urls.Add(resolvedUrl);
                blockJson.Add(FileBuilders.BuildImageBlock(reference));
            }

            var namesUsed = new List<string>();
            for (int i = 0; i < resolvedRefs.Count; i++)
                namesUsed.Add(i < fileNames.Count ? fileNames[i] : "");

            DA.SetData(0, FileBuilders.BuildInsertFilesOp(namesUsed, resolvedRefs, base64Contents, urls));
            DA.SetDataList(1, blockJson);
        }

        // Encodes a GH-wrapped Bitmap goo (typically a GH_ObjectWrapper around a System.Drawing.Bitmap) to base64 PNG
        private static string EncodeGooBitmap(IGH_Goo goo)
        {
            object value = null;
            if (goo is GH_ObjectWrapper wrapper) value = wrapper.Value;
            else value = goo;

            if (!(value is Bitmap bitmap)) return null;

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        // Captures the active (or named) Rhino viewport in-memory and returns it as base64 PNG
        private static string CaptureViewportToBase64(string viewName)
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            if (doc == null) return null;

            var view = string.IsNullOrWhiteSpace(viewName) ? doc.Views.ActiveView : doc.Views.Find(viewName, false);
            if (view == null) return null;

            var bitmap = view.CaptureToBitmap();
            if (bitmap == null || bitmap.Width <= 0 || bitmap.Height <= 0) { bitmap?.Dispose(); return null; }

            try
            {
                using (var clean = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(clean)) g.DrawImageUnscaled(bitmap, 0, 0);
                    using (var ms = new MemoryStream())
                    {
                        clean.Save(ms, ImageFormat.Png);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            finally { bitmap.Dispose(); }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GC_BuildInsertFile;
        public override Guid ComponentGuid => new Guid("0d9274ec-2fcc-474e-83da-ad2ab7dd1284");
    }
}
