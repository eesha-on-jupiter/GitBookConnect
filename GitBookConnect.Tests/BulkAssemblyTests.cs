using System.Collections.Generic;
using System.Linq;
using GitBookConnect;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GitBookConnect.Tests
{
    // Build Pages From Tree's tree -> list assembly (PageBuilders.BuildChangeOpsFromBranches),
    // tested headless with plain per-branch lists in place of a GH_Structure.
    public class BulkAssemblyTests
    {
        private static IList<IList<string>> Tree(params string[][] branches) =>
            branches.Select(b => (IList<string>)b.ToList()).ToList();

        [Fact]
        public void OneOpPerItem_AcrossBranches()
        {
            var ops = PageBuilders.BuildChangeOpsFromBranches("",
                null,
                Tree(new[] { "L1", "L2" }, new[] { "Roof" }),
                Tree(new[] { "a", "b" }, new[] { "c" }),
                null, 2);
            Assert.Equal(3, ops.Count);
            Assert.All(ops, o => Assert.Equal("insert_page", (string)JObject.Parse(o)["operation"]));
            Assert.Equal(new[] { "L1", "L2", "Roof" }, ops.Select(o => (string)JObject.Parse(o)["title"]));
        }

        [Fact]
        public void PageId_SwitchesItemToUpdate()
        {
            var ops = PageBuilders.BuildChangeOpsFromBranches("",
                Tree(new[] { "", "pg2" }),
                Tree(new[] { "New", "Existing" }),
                Tree(new[] { "n", "e" }),
                null, 1);
            var first = JObject.Parse(ops[0]);
            var second = JObject.Parse(ops[1]);
            Assert.Equal("insert_page", (string)first["operation"]);
            Assert.Equal("update_page", (string)second["operation"]);
            Assert.Equal("pg2", (string)second["page"]);
            Assert.Equal("e", (string)second["document"]["markdown"]);
        }

        [Fact]
        public void LongestListWins_ShorterListsPadBlank()
        {
            var ops = PageBuilders.BuildChangeOpsFromBranches("",
                null,
                Tree(new[] { "A", "B", "C" }),
                Tree(new[] { "only first" }),
                Tree(new[] { "📐" }),
                1);
            Assert.Equal(3, ops.Count);
            var third = JObject.Parse(ops[2]);
            Assert.Equal("C", (string)third["title"]);
            Assert.Equal("", (string)third["document"]["markdown"]);
            Assert.Null(third["icon"]);
            Assert.Equal("📐", (string)JObject.Parse(ops[0])["icon"]);
        }

        [Fact]
        public void Into_AppliedToEveryOp()
        {
            var ops = PageBuilders.BuildChangeOpsFromBranches("parentX",
                Tree(new[] { "", "pg" }),
                Tree(new[] { "a", "b" }),
                null, null, 1);
            Assert.All(ops, o => Assert.Equal("parentX", (string)JObject.Parse(o)["parent"]));
        }

        [Fact]
        public void MissingBranches_AreTreatedAsEmpty()
        {
            var ops = PageBuilders.BuildChangeOpsFromBranches("",
                null,
                Tree(new[] { "only branch 0" }),
                null, null, 3);
            Assert.Single(ops);
        }

        [Fact]
        public void ZeroBranches_NoOps()
        {
            Assert.Empty(PageBuilders.BuildChangeOpsFromBranches("", null, null, null, null, 0));
        }
    }
}
