pub mod hunk_diff;

use hunk_diff::*;
use similar::{Algorithm, TextDiff};
use std::ffi::{c_char, CStr};
use std::ptr::null;

#[no_mangle]
pub unsafe extern "C" fn get_hunk_diffs(
    old_str: *const c_char,
    new_str: *const c_char,
) -> HunkDiffArray {
    let old_text = CStr::from_ptr(old_str)
        .to_str()
        .expect("failed to read old str");

    let new_text = CStr::from_ptr(new_str)
        .to_str()
        .expect("failed to read new str");

    let diff = TextDiff::configure()
        .algorithm(Algorithm::Myers)
        .diff_lines(old_text, new_text);

    let mut hunks = Vec::new();

    for op in diff.ops() {
        let hunk = match *op {
            similar::DiffOp::Equal {
                old_index,
                new_index,
                len,
            } => HunkDiff {
                diff_type: DiffType::Unmodified,
                modified: null(),
                deleted: null(),
                unmodified: Box::into_raw(Box::new(Unmodified {
                    old_range: old_index as i32..(old_index as i32 + len as i32),
                    new_range: new_index as i32..(new_index as i32 + len as i32),
                })),
                insert: null(),
            },
            similar::DiffOp::Delete {
                old_index,
                old_len,
                new_index,
            } => HunkDiff {
                diff_type: DiffType::Deleted,
                modified: null(),
                deleted: Box::into_raw(Box::new(Deleted {
                    old_range: old_index as i32..(old_index as i32 + old_len as i32),
                    new_start_line: new_index as i32,
                })),
                unmodified: null(),
                insert: null(),
            },
            similar::DiffOp::Insert {
                old_index,
                new_index,
                new_len,
            } => HunkDiff {
                diff_type: DiffType::Inserted,
                modified: null(),
                deleted: null(),
                unmodified: null(),
                insert: Box::into_raw(Box::new(Inserted {
                    old_start_line: old_index as i32,
                    new_range: new_index as i32..(new_index as i32 + new_len as i32),
                })),
            },
            similar::DiffOp::Replace {
                old_index,
                old_len,
                new_index,
                new_len,
            } => HunkDiff {
                diff_type: DiffType::Modified,
                modified: Box::into_raw(Box::new(Modified {
                    old_range: old_index as i32..(old_index as i32 + old_len as i32),
                    new_range: new_index as i32..(new_index as i32 + new_len as i32),
                })),
                deleted: null(),
                unmodified: null(),
                insert: null(),
            },
        };

        hunks.push(hunk);
    }

    let arr = HunkDiffArray {
        count: hunks.len() as i32,
        diffs: hunks.as_ptr(),
    };

    std::mem::forget(hunks);

    arr
}

#[no_mangle]
pub extern "C" fn free_hunk_diffs(hunk_diffs: HunkDiffArray) {
    for i in 0..hunk_diffs.count {
        let hunk_diff = unsafe { &*hunk_diffs.diffs.add(i as usize) };

        if !hunk_diff.unmodified.is_null() {
            unsafe {
                _ = Box::from_raw(hunk_diff.unmodified as *mut Unmodified);
            }
        }

        if !hunk_diff.deleted.is_null() {
            unsafe {
                _ = Box::from_raw(hunk_diff.deleted as *mut Deleted);
            }
        }

        if !hunk_diff.insert.is_null() {
            unsafe {
                _ = Box::from_raw(hunk_diff.insert as *mut Inserted);
            }
        }

        if !hunk_diff.modified.is_null() {
            unsafe {
                _ = Box::from_raw(hunk_diff.modified as *mut Modified);
            }
        }
    }
}
