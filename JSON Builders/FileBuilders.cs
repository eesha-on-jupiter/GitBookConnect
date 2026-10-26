using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    public static class FileBuilders
    {
        #region Private helpers

        private static JObject BuildFileEntry(string name, string reference, string base64Content, string sourceUrl)
        {
            var entry = new JObject { ["name"] = name ?? "" };
            if (!string.IsNullOrWhiteSpace(reference)) entry["ref"] = reference;
            if (!string.IsNullOrWhiteSpace(sourceUrl)) entry["url"] = sourceUrl;
            else entry["content"] = base64Content ?? "";
            return entry;
        }

        #endregion

        #region Files

        // change
        public static string BuildInsertFilesOp(IList<string> names, IList<string> refs, IList<string> base64Contents, IList<string> sourceUrls)
        {
            var files = new JArray();
            int n = names?.Count ?? 0;

            for (int i = 0; i < n; i++)
            {
                string name = names[i] ?? "";
                string reference = refs != null && i < refs.Count ? refs[i] : "";
                string url = sourceUrls != null && i < sourceUrls.Count ? sourceUrls[i] : "";
                string content = base64Contents != null && i < base64Contents.Count ? base64Contents[i] : "";
                files.Add(BuildFileEntry(name, reference, content, url));
            }

            var op = new JObject { ["operation"] = "insert_files", ["files"] = files };
            return op.ToString(Formatting.None);
        }

        // block
        public static string BuildImageBlock(string reference)
        {
            var block = new JObject
            {
                ["object"] = "block",
                ["type"] = "images",
                ["nodes"] = new JArray(new JObject { ["object"] = "block", ["type"] = "image", ["data"] = new JObject { ["ref"] = reference ?? "" } })
            };
            return block.ToString(Formatting.None);
        }

        #endregion
    }
}
