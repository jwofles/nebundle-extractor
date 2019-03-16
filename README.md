# nebundle-extractor
Extract .PNGs (textures) from [Alice 3.0](http://www.alice.org/get-alice/alice-3/)'s .Nebundle files.

## what is this?
This is a little tool I made to scan through the .nebundle files included in Alice 3.0 and export any .PNGs found and save them to their own files.
Theoretically it can extract PNGs from any binary file that contains .PNGs, however it must be uncompressed.

It works by finding certain byte sequences that are part of the .PNG byte chunks. It finds the start of the the PNG chunk (IHDR) and then reads until it finds the end of the chunk (IEND). Then it saves that to a new file with the PNG header.

## how do i use this?
Download the .exe from the [releases](https://github.com/jwofles/nebundle-extractor/releases/tag/v1.0) and open it. Then drag any .nebundle file onto it, and it will go through the whole file to export any .PNGs in there onto a new folder on your desktop.

