# Dupe Report App


## What

Simple fast program to find, report and optionally delete duplicate files.

Default mode does not delete, just writes a report in a text file.

_Warning_ This program reads enitre files into memory before calculating hashes, not ideal if files are too big for your RAM.
Also may use up ram if you have a very fast disk and slow processor (the hash calculation is threadpooled).


## Usage

Simply put the exe in a folder and run it, it'll scan the current folder and all subdirs recursively and output `dupes.txt` with the results.

Run from the CLI in order to observe progress and use options.


### Options

Options must be added separately, with spaces.

`-s`
: Silent mode, no console spam.

`-d`
: Delete mode, duplicates are deleted and logged as deleted, duplicate with the shortest name is kept. If there are multiple duplicates with the shortest name the choice between them is random.

`-x`
: Simulate delete mode, files will not be deleted, only logged as deleted.


## How It Works

It works by first comparing file sizes and then comparing the SHA512 hash if the size matches.
Does not do full bit for bit comparisons, trusts the hash.


## Output

The outputs the names of the duplicate files on one line separated with `|` and then the full paths on the following lines indented. 