using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    // Document JSON = { title?, icon?, document: {...} } -- a page's content plus its own metadata,
    // so a single "Document JSON" item carries everything Insert/Update Page need, with no separate
    // Title input on those components. document.markdown (Markdown path) and document.object/nodes
    // (JSON block path, from Build Page (JSON)) are interchangeable at this level.
    public static class PageBuilders
    {
        #region Private helpers

        private static JObject WrapDocument(string title, string icon, JObject document)
        {
            var wrapper = new JObject { ["document"] = document ?? new JObject() };
            if (!string.IsNullOrWhiteSpace(title)) wrapper["title"] = title;
            if (!string.IsNullOrWhiteSpace(icon)) wrapper["icon"] = icon;
            return wrapper;
        }

        #endregion

        #region Document JSON

        // document
        public static string BuildDocumentFromMarkdown(string title, string markdownBody, string icon)
        {
            var document = new JObject { ["markdown"] = markdownBody ?? "" };
            return WrapDocument(title, icon, document).ToString(Formatting.None);
        }

        // document
        public static string BuildDocumentFromBlocks(string title, IEnumerable<string> blocksJson)
        {
            var nodes = new JArray();
            foreach (var b in blocksJson ?? Array.Empty<string>())
                if (!string.IsNullOrWhiteSpace(b)) nodes.Add(JObject.Parse(b));

            var document = new JObject { ["object"] = "document", ["nodes"] = nodes };
            return WrapDocument(title, null, document).ToString(Formatting.None);
        }

        #endregion

        #region Change JSON (insert_page / update_page ops)

        // change
        public static string BuildInsertPageOp(string documentJson, string into, int? at, string reference)
        {
            JObject doc = string.IsNullOrWhiteSpace(documentJson) ? new JObject() : JObject.Parse(documentJson);

            var op = new JObject { ["operation"] = "insert_page" };
            if (doc["title"] != null) op["title"] = doc["title"];
            if (doc["icon"] != null) op["icon"] = doc["icon"];
            op["document"] = doc["document"] ?? new JObject();
            if (!string.IsNullOrWhiteSpace(into)) op["parent"] = into;
            if (at.HasValue) op["position"] = at.Value;
            if (!string.IsNullOrWhiteSpace(reference)) op["ref"] = reference;

            return op.ToString(Formatting.None);
        }

        // change
        public static string BuildUpdatePageOp(string pageId, string documentJson, string into, int? at)
        {
            var op = new JObject { ["operation"] = "update_page", ["page"] = pageId ?? "" };

            if (!string.IsNullOrWhiteSpace(documentJson))
            {
                JObject doc = JObject.Parse(documentJson);
                if (doc["title"] != null) op["title"] = doc["title"];
                if (doc["icon"] != null) op["icon"] = doc["icon"];
                if (doc["document"] != null) op["document"] = doc["document"];
            }
            if (!string.IsNullOrWhiteSpace(into)) op["parent"] = into;
            if (at.HasValue) op["position"] = at.Value;

            return op.ToString(Formatting.None);
        }

        #endregion

        #region Bulk assembly (tree -> list)

        // change
        // Pure tree->list assembly for Build Pages From Tree -- the component only adapts the GH tree
        // (GH_Structure/GH_Path) to/from these plain per-branch lists; this method does the actual
        // per-branch, per-item join and insert/update decision, so it is unit-testable headless.
        public static List<string> BuildChangeOpsFromBranches(string into, IList<IList<string>> pageIdBranches, IList<IList<string>> titleBranches, IList<IList<string>> bodyBranches, IList<IList<string>> iconBranches, int branchCount)
        {
            var changeJson = new List<string>();

            for (int b = 0; b < branchCount; b++)
            {
                var pageIds = b < (pageIdBranches?.Count ?? 0) ? pageIdBranches[b] : new List<string>();
                var titles = b < (titleBranches?.Count ?? 0) ? titleBranches[b] : new List<string>();
                var bodies = b < (bodyBranches?.Count ?? 0) ? bodyBranches[b] : new List<string>();
                var icons = b < (iconBranches?.Count ?? 0) ? iconBranches[b] : new List<string>();

                int n = new[] { pageIds.Count, titles.Count, bodies.Count, icons.Count }.Max();
                for (int i = 0; i < n; i++)
                {
                    string pageId = i < pageIds.Count ? pageIds[i] : "";
                    string title = i < titles.Count ? titles[i] : "";
                    string body = i < bodies.Count ? bodies[i] : "";
                    string icon = i < icons.Count ? icons[i] : "";

                    string documentJson = BuildDocumentFromMarkdown(title, body, icon);
                    changeJson.Add(string.IsNullOrWhiteSpace(pageId)
                        ? BuildInsertPageOp(documentJson, into, null, null)
                        : BuildUpdatePageOp(pageId, documentJson, into, null));
                }
            }

            return changeJson;
        }

        #endregion
    }
}
