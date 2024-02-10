# SimilarNet
A text lines diff library.  
This library is a simple wrapper for the rust crate [similar](https://github.com/mitsuhiko/similar).

## How to use
```cs
var hunks= Similar.DiffTexts("b\nc\nd\nd", "a\nb\nC\nd\nd\nx");
```
## How to build
1. Run the following command in the rust project (`./similar-rs`)
```cmd
cargo build --release
```
2. Place the built `similar-rs.dll` to `./SimilarNet/SimilarNet`.
3. Build the nupkg in c# project.
