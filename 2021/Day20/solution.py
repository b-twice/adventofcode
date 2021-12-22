from collections import defaultdict
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '20'

input_file = write_input(DAY)

# READ
enhancementAlgorithm = None
inputImage = []
with open(input_file, 'r') as f:
    for i, line in enumerate(f.read().splitlines()):
        if i == 0:
            enhancementAlgorithm = line
        elif line and len(line):
            inputImage.append(line)

matrix = [(x, y) for x in [-1, 0, 1] for y in [-1, 0, 1]]

algo = [
    {i for i, p in enumerate(enhancementAlgorithm) if p == "."},
    {i ^ 511 for i, p in enumerate(enhancementAlgorithm) if p == "#"},
]


def enhance(lights, minPoint, maxPoint, a):
    newLights = set()
    minx, miny = minPoint
    maxx, maxy = maxPoint
    for i in range(minx-1, maxx+2):
        for j in range(miny-1, maxy+2):
            positions = [(i + m[0], j + m[1]) for m in matrix]
            k = ''.join(
                [('1' if pos in lights else '0') for pos in positions])
            if int(k, 2) in a:
                newLights.add((i, j))
    return newLights


def run(img, times):
    lights = set()
    for x, line in enumerate(img):
        for y, c in enumerate(line):
            if c == '#':
                lights.add((x, y))
    minPoint = (0, 0)
    maxPoint = (len(img)-1, len(img[0])-1)

    for i in range(times):
        lights = enhance(lights, minPoint, maxPoint, algo[i % len(algo)])
        minPoint = (minPoint[0] - 1, minPoint[1] - 1)
        maxPoint = (maxPoint[0] + 1, maxPoint[1] + 1)

    return lights


# SOLUTION 1
def printLights(lights):
    minx = min([p[0] for p in lights])
    miny = min([p[1] for p in lights])
    maxx = max([p[0] for p in lights])
    maxy = max([p[1] for p in lights])
    for i in range(minx-1, maxx+2):
        line = []
        for j in range(miny-1, maxy+2):
            if (i, j) in lights:
                line.append('#')
            else:
                line.append('.')
        print(''.join(line))
    print('')


solution_1 = len(run(inputImage, 2))

# SOLUTION 2
solution_2 = len(run(inputImage, 50))

# WRITE
write_output([str(solution_1), str(solution_2)])
