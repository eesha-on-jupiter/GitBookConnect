using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitBookConnect
{
    public static class TableBuilders
    {
        #region Private helpers

        private static string EscapeMarkdownCell(string cell) => (cell ?? "").Replace("|", "\\|").Replace("\n", " ");

        private static IEnumerable<string> ToCells(IList<string> row) => (row ?? new List<string>()).Select(EscapeMarkdownCell);

        // Wraps plain cell text as a GitBook text node holding one unstyled leaf
        private static JObject TextNode(string text)
        {
            return new JObject
            {
                ["object"] = "text",
                ["leaves"] = new JArray(new JObject { ["object"] = "leaf", ["text"] = text ?? "", ["marks"] = new JArray() })
            };
        }

        #endregion

        #region Markdown table

        // table
        public static string BuildMarkdownTable(IList<string> headerRow, IList<IList<string>> rows)
        {
            var header = headerRow ?? new List<string>();
            int columnCount = System.Math.Max(header.Count, 1);

            var sb = new StringBuilder();
            sb.Append("| ").Append(string.Join(" | ", ToCells(header))).Append(" |\n");
            sb.Append("|").Append(string.Concat(Enumerable.Repeat(" --- |", columnCount))).Append("\n");
            foreach (var row in rows ?? new List<IList<string>>())
                sb.Append("| ").Append(string.Join(" | ", ToCells(row))).Append(" |\n");

            return sb.ToString();
        }

        #endregion

        #region JSON table block

        // table
        public static string BuildTableBlock(IList<string> headerRow, IList<IList<string>> rows)
        {
            var rowNodes = new JArray();

            var header = headerRow ?? new List<string>();
            if (header.Count > 0)
            {
                var headerCells = new JArray(header.Select(c => new JObject { ["object"] = "block", ["type"] = "table-cell", ["nodes"] = new JArray(TextNode(c)) }));
                rowNodes.Add(new JObject { ["object"] = "block", ["type"] = "table-row", ["data"] = new JObject { ["header"] = true }, ["nodes"] = headerCells });
            }

            foreach (var row in rows ?? new List<IList<string>>())
            {
                var cells = new JArray((row ?? new List<string>()).Select(c => new JObject { ["object"] = "block", ["type"] = "table-cell", ["nodes"] = new JArray(TextNode(c)) }));
                rowNodes.Add(new JObject { ["object"] = "block", ["type"] = "table-row", ["nodes"] = cells });
            }

            var block = new JObject
            {
                ["object"] = "block",
                ["type"] = "table",
                ["data"] = new JObject { ["view"] = "grid" },
                ["nodes"] = rowNodes
            };

            return block.ToString(Formatting.None);
        }

        #endregion
    }
}
