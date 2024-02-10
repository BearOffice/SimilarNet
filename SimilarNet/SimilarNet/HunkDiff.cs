namespace SimilarNet;

public enum DiffType
{
    Unmodified = 0,
    Deleted = 1,
    Inserted = 2,
    Modified = 3,
}

public class HunkDiff
{
    public DiffType DiffType;
    public Unmodified? Unmodified;
    public Deleted? Deleted;
    public Inserted? Insert;
    public Modified? Modified;
}

public struct Unmodified
{
    public Range OldRange;
    public Range NewRange;
}

public struct Deleted
{
    public Range OldRange;
    public int NewStartLine;
}

public struct Inserted
{
    public int OldStartLine;
    public Range NewRange;
}

public struct Modified
{
    public Range OldRange;
    public Range NewRange;
}
