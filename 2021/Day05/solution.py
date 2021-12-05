from collections import defaultdict
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '5'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [[tuple([int(p) for p in pos.split(',')])
             for pos in line.split(' -> ')] for line in f.read().splitlines()]


def mapLines(lines, part2=False):
    m = defaultdict(int)
    for line in lines:
        start, end = line
        xStart, xEnd = min([start[0], end[0]]), max([start[0], end[0]])
        yStart, yEnd = min([start[1], end[1]]), max([start[1], end[1]])
        if start[0] != end[0] and start[1] != end[1]:
            if part2:
                xStep = 1 if start[0] <= end[0] else -1
                yStep = 1 if start[1] <= end[1] else -1
                coords = zip([i for i in range(start[0], end[0] + xStep, xStep)],
                             [i for i in range(start[1], end[1] + yStep, yStep)])
                for coord in coords:
                    m[(coord[0], coord[1])] += 1
                continue
            else:
                continue

        if xStart != xEnd:
            for i in range(abs(xEnd-xStart+1)):
                m[(xStart+i, yStart)] += 1
        if yStart != yEnd:
            for i in range(abs(yEnd-yStart+1)):
                m[(xStart, yStart+i)] += 1
    return m


# SOLUTION 1
solution_1 = len([v for v in mapLines(data, False).values() if v > 1])


# SOLUTION 2
solution_2 = len([v for v in mapLines(data, True).values() if v > 1])

# WRITE
write_output([str(solution_1), str(solution_2)])
