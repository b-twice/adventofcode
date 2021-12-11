import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '11'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [[int(c) for c in list(line)] for line in f.read().splitlines()]


def isWithin(rows, pos):
    return pos[0] >= 0 and pos[1] >= 0 and pos[0] < len(rows) and pos[1] < len(rows[0])


def getNeighbors(rows, pos):
    return [n for n in [(pos[0] - 1, pos[1]), (pos[0] + 1, pos[1]),
                        (pos[0], pos[1] + 1), (pos[0], pos[1] - 1),
                        (pos[0] - 1, pos[1] - 1), (pos[0] + 1, pos[1] + 1),
                        (pos[0] + 1, pos[1] - 1), (pos[0] - 1, pos[1] + 1),
                        ] if isWithin(rows, n)]


def iterate(rows):
    for i in range(len(rows)):
        for j in range(len(rows[i])):
            yield (i, j)


def flash(rows, pos):
    if rows[pos[0]][pos[1]] <= 9:
        return 0
    rows[pos[0]][pos[1]] = 0

    total = 1
    for n in getNeighbors(rows, pos):
        if rows[n[0]][n[1]] == 0:
            continue
        if rows[n[0]][n[1]] <= 9:
            rows[n[0]][n[1]] += 1
        total += flash(rows, n)

    return total


def run(rows):
    flashes = 0
    i = 0
    while i < 100:
        flashers = []
        for pos in iterate(rows):
            rows[pos[0]][pos[1]] += 1
            if rows[pos[0]][pos[1]] > 9:
                flashers.append(pos)

        for pos in flashers:
            flashes += flash(rows, pos)
        i += 1
    return flashes


# SOLUTION 1
solution_1 = run([list(d) for d in data])


# SOLUTION 2
def run2(rows):
    flashed = False
    i = 0
    while not flashed:
        i += 1
        flashers = []
        for pos in iterate(rows):
            rows[pos[0]][pos[1]] += 1
            if rows[pos[0]][pos[1]] > 9:
                flashers.append(pos)

        for pos in flashers:
            flash(rows, pos)
        if all([rows[pos[0]][pos[1]] == 0 for pos in iterate(rows)]):
            flashed = True
            return i


solution_2 = run2([list(d) for d in data])

# WRITE
write_output([str(solution_1), str(solution_2)])
