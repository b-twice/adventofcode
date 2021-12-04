import os
import sys
import requests
sys.path.append('../.config')
from config import AOC_SESSION  # noqa

INPUT_URL = 'https://adventofcode.com/2021/day/{}/input'
INPUT_FILE = './input.in'
OUTPUT_FILE = './solution.out'


def write_input(day):
    if not os.path.exists(INPUT_FILE):
        with requests.Session() as s:
            r = s.get(INPUT_URL.format(day), cookies={"session": AOC_SESSION})
            with open(INPUT_FILE, 'w') as f:
                f.write(r.text)
    return INPUT_FILE


def write_output(solutions):
    with open(OUTPUT_FILE, 'w') as f:
        f.write("\n".join(solutions))
