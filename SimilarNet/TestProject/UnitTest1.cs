using SimilarNet;

namespace TestProject;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        var hunks= Similar.DiffTexts("b\nc\nd\nd", "a\nb\nC\nd\nd\nx");

        foreach (var hunk in hunks)
        {
            switch (hunk.DiffType)
            {
                case DiffType.Modified:
                    Console.WriteLine($"Modified: {hunk.Modified!.Value.OldRange} - {hunk.Modified!.Value.NewRange}");
                    break;
                case DiffType.Deleted:
                    Console.WriteLine($"Deleted: {hunk.Deleted!.Value.OldRange} - {hunk.Deleted!.Value.NewStartLine}");
                    break;
                case DiffType.Inserted:
                    Console.WriteLine($"Inserted: {hunk.Insert!.Value.OldStartLine} - {hunk.Insert!.Value.NewRange}");
                    break;
                case DiffType.Unmodified:
                    Console.WriteLine($"Unmodified: {hunk.Unmodified!.Value.OldRange} - {hunk.Unmodified!.Value.NewRange}");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}