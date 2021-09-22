import sys
import os
from waitForFile import waitForFile

filename = sys.argv[1]
waitForFile(filename)
with open(filename) as file:
    exit(any(["One or more child tests had errors" in line for line in file.readlines()]))
