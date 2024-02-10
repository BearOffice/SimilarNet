using System.Runtime.InteropServices;

namespace SimilarNet.Core;

internal enum DiffType
{
    Unmodified = 0,
    Deleted = 1,
    Inserted = 2,
    Modified = 3,
}

[StructLayout(LayoutKind.Sequential)]
internal struct HunkDiffArray
{
    internal int count;
    internal IntPtr diffs;
}

[StructLayout(LayoutKind.Sequential)]
internal struct HunkDiff
{
    internal DiffType diff_type;
    internal IntPtr unmodified;
    internal IntPtr deleted;
    internal IntPtr insert;
    internal IntPtr modified;
}

[StructLayout(LayoutKind.Sequential)]
internal struct Unmodified
{
    internal Range old_range;
    internal Range new_range;
}

[StructLayout(LayoutKind.Sequential)]
internal struct Deleted
{
    internal Range old_range;
    internal int new_start_line;
}

[StructLayout(LayoutKind.Sequential)]
internal struct Inserted
{
    internal int old_start_line;
    internal Range new_range;
}

[StructLayout(LayoutKind.Sequential)]
internal struct Modified
{
    internal Range old_range;
    internal Range new_range;
}
