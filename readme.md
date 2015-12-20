# Dupe Report App

## What
App to find duplicate files and output result to text file.

## Usage

Simply put the exe in a folder and run it, it'll scan the current folder and all subdirs recursively and output `dupes.txt` with the results.

Console app with no output (might add some progress text in the future).

_Warning_ It reads enitre files into memory before calculating hashes, not ideal if possible duplicates are too big for your RAM (rather unlikely).

## How It Works

It works by first comparing file sizes and then comparing the SHA512 hash if the size matches.
Does not do full bit for bit comparisons, trusts the hash.

## Output

The outputs the names of the duplicate files on one line separated with `|` and then the full paths on the following lines indented. 