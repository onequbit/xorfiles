# xorfiles
XOR two or more files together (C# / Python)

usage:
```
xorfiles.py file1 file2 
```
output:
```
outfile-DATETIME-STAMP.txt
```

Notes:
- You must specify at least two files.
- The files can be any length - shorter files will be padded with zeros until the end of the longest file is reached.
- The output file is date-time stamped in the name.
