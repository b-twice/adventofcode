import sys
import heapq
import functools
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '9'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [list(line) for line in f.read().splitlines()]


def iterate(rows):
    for i in range(len(rows)):
        for j in range(len(rows[i])):
            yield (i, j)


def isWithin(rows, pos):
    return pos[0] >= 0 and pos[1] >= 0 and pos[0] < len(rows) and pos[1] < len(rows[0])


def getNeighbors(rows, pos):
    return [n for n in [(pos[0] - 1, pos[1]), (pos[0] + 1, pos[1]),
                        (pos[0], pos[1] + 1), (pos[0], pos[1] - 1)] if isWithin(rows, n)]


def isLower(rows, pos):
    neighbors = getNeighbors(rows, pos)
    return all([int(rows[pos[0]][pos[1]]) < int(rows[n[0]][n[1]])
                for n in neighbors if rows[n[0]][n[1]] != '#'])


def isLower2(rows, pos):
    neighbors = getNeighbors(rows, pos)
    return any([rows[n[0]][n[1]] != '9' for n in neighbors])


# SOLUTION 1
solution_1 = sum([int(data[i][j]) + 1 for i,
                 j in iterate(data) if isLower(data, (i, j))])

# SOLUTION 2
solution_2 = 0
lowPoints = [(i, j) for i, j in iterate(data) if isLower(data, (i, j))]


def search(rows, pos):

    if not isWithin(rows, pos):
        return 0

    num = rows[pos[0]][pos[1]]
    if num == '9' or num == '#':
        return 0

    total = 0
    if isLower2(rows, pos):
        rows[pos[0]][pos[1]] = '#'
        total = 1

    for n in getNeighbors(rows, pos):
        total += search(rows, n)
    return total


basinSizes = []
for lowPoint in lowPoints:
    basinSizes.append(search(data, lowPoint))

solution_2 = functools.reduce(
    lambda a, b: a * b, heapq.nlargest(3, basinSizes), 1)

# WRITE
write_output([str(solution_1), str(solution_2)])
