use std::ops::Range;

#[repr(C)]
#[derive(Debug, Clone, Copy)]
pub enum DiffType {
    Unmodified = 0,
    Deleted = 1,
    Inserted = 2,
    Modified = 3,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct HunkDiff {
    pub diff_type: DiffType,
    pub unmodified: *const Unmodified,
    pub deleted: *const Deleted,
    pub insert: *const Inserted,
    pub modified: *const Modified,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct HunkDiffArray {
    pub count: i32,
    pub diffs: *const HunkDiff,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct Unmodified {
    pub old_range: Range<i32>,
    pub new_range: Range<i32>,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct Deleted {
    pub old_range: Range<i32>,
    pub new_start_line: i32,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct Inserted {
    pub old_start_line: i32,
    pub new_range: Range<i32>,
}

#[repr(C)]
#[derive(Debug, Clone)]
pub struct Modified {
    pub old_range: Range<i32>,
    pub new_range: Range<i32>,
}
