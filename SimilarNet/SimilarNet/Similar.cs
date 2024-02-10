using SimilarNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimilarNet;

public static class Similar
{
    private static readonly int _hunkSize = Marshal.SizeOf<Core.HunkDiff>();

    public static IEnumerable<HunkDiff> DiffTexts(string oldText, string newText)
    {
        var rawHunks = NativeMethods.GetHunkDiffs(oldText, newText);

        var hunks = new List<HunkDiff>();

        for (int i = 0; i < rawHunks.count; i++)
        {
            var hunkPtr = IntPtr.Add(rawHunks.diffs, i * _hunkSize);
            var rawHunk = Marshal.PtrToStructure<Core.HunkDiff>(hunkPtr);

            switch (rawHunk.diff_type)
            {
                case Core.DiffType.Modified:
                    var modified = Marshal.PtrToStructure<Core.Modified>(rawHunk.modified);
                    hunks.Add(new HunkDiff
                    {
                        DiffType = DiffType.Modified,
                        Modified = new Modified { OldRange = modified.old_range, NewRange = modified.new_range },
                    });

                    break;
                case Core.DiffType.Deleted:
                    var deleted = Marshal.PtrToStructure<Core.Deleted>(rawHunk.deleted);
                    hunks.Add(new HunkDiff
                    {
                        DiffType = DiffType.Deleted,
                        Deleted = new Deleted { OldRange = deleted.old_range, NewStartLine = deleted.new_start_line },
                    });

                    break;
                case Core.DiffType.Inserted:
                    var inserted = Marshal.PtrToStructure<Core.Inserted>(rawHunk.insert);
                    hunks.Add(new HunkDiff
                    {
                        DiffType = DiffType.Inserted,
                        Insert = new Inserted { OldStartLine = inserted.old_start_line, NewRange = inserted.new_range },
                    });

                    break;
                case Core.DiffType.Unmodified:
                    var unmodified = Marshal.PtrToStructure<Core.Unmodified>(rawHunk.unmodified);
                    hunks.Add(new HunkDiff
                    {
                        DiffType = DiffType.Unmodified,
                        Unmodified = new Unmodified { OldRange = unmodified.old_range, NewRange = unmodified.new_range },
                    });

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        NativeMethods.FreeHunkDiffs(rawHunks);

        return hunks;
    }
}
