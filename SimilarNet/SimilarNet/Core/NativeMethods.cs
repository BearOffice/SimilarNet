using System.Runtime.InteropServices;

namespace SimilarNet.Core;

internal static partial class NativeMethods
{
    [LibraryImport("similar_rs.dll", EntryPoint = "get_hunk_diffs", StringMarshalling = StringMarshalling.Utf8)]
    unsafe internal static partial HunkDiffArray GetHunkDiffs(string oldText, string newText);

    [LibraryImport("similar_rs.dll", EntryPoint = "free_hunk_diffs")]
    internal static partial void FreeHunkDiffs(HunkDiffArray hunkDiffArray);
}
