import sys
import os
from time import sleep


def waitForFile(filename):
    while not os.path.exists(filename):
        sleep(1)

if __name__ == "__main__":
    waitForFile(sys.argv[1])
