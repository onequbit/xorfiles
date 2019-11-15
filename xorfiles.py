#!/usr/bin/env python3

__author__ = "Alfred Aquino"
__license__ = "MIT"
__version__ = "1.0.0"
__maintainer__ = "Alfred Aquino"
__email__ = "onequbit@yahoo.com"

import argparse, hashlib, io, os, sys

def timestamp():
    from datetime import datetime
    t = datetime.now().isoformat().replace('-','').replace('T','_').replace(':','')
    return t[:15]

def parsed_arguments():
    parser = argparse.ArgumentParser(description='xor two or more files together')    
    parser.add_argument(dest='files', nargs=argparse.REMAINDER)
    return parser.parse_args()

def get_parameters(args):
    assert (len(args.files) >= 2), 'two or more files required for xor'
    sizes = [0] * len(args.files)
    for i in range(0,len(args.files)):
        file = args.files[i]
        assert (os.path.isfile(file) is True), f'unable to open "{file}"'
        sizes[i] = os.path.getsize(file)
    return args.files, sizes

def file_byte_iterator(path):
    """given a path, return an iterator over the file
    that lazily loads the file
    """
    from pathlib import Path
    from functools import partial
    from io import DEFAULT_BUFFER_SIZE

    path = Path(path)
    with path.open('rb') as file:
        reader = partial(file.read1, DEFAULT_BUFFER_SIZE)
        file_iterator = iter(reader, bytes())
        for chunk in file_iterator:
            for byte in chunk:
                yield byte
    while True:
        yield 0

def do_xors(files, sizes):
    import tempfile
    outfile = open('outfile' + timestamp() + '.txt', 'wb')
    filecount = len(files)
    bytecount = 0
    lastbyte = max(sizes)
    streams = [file_byte_iterator(f) for f in files]
    while bytecount < lastbyte:
        accumulator = 0
        for i in range(0,len(files)):
            b = next(streams[i])            
            accumulator = accumulator ^ int(b)        
        outfile.write(accumulator.to_bytes(1,sys.byteorder))
        bytecount = bytecount + 1
    outfile.close()    

try:
    files, sizes = get_parameters(parsed_arguments())
    do_xors(files, sizes)

except Exception as caught:
    print(type(caught).__name__, caught.args)

