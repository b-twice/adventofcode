import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '13'

input_file = write_input(DAY)

# READ
data = []

with open(input_file, 'r') as f:
    data = f.read().splitlines()


def createInstructions(rows):
    points = []
    foldInstructions = []
    startFoldInstructions = False
    for line in rows:
        if line == '':
            startFoldInstructions = True
            continue
        if not startFoldInstructions:
            x, y = line.split(',')
            points.append((int(x), int(y)))
        else:
            axis, loc = line.split(' ')[-1].split('=')
            foldInstructions.append((axis, int(loc)))
    return points, foldInstructions


def runInstructions(points, foldInstructions, print=True):
    for axis, loc in foldInstructions:
        for i in range(len(points)):
            x, y = points[i]
            if axis == 'x' and x > loc:
                points[i] = (x - ((x - loc) * 2), y)
            elif axis == 'y' and y > loc:
                points[i] = (x, y - ((y - loc) * 2))
    if print:
        printInstructions(points)
    return list(set(points))


def printInstructions(points):
    mx = max([p[0] for p in points])
    my = max([p[1] for p in points])
    grid = [[' ' for _ in range(my+1)] for _ in range(mx+1)]
    for p in points:
        grid[p[0]][p[1]] = '█'
    for line in reversed(grid):
        print(''.join(line))


# SOLUTION 1
points, folds = createInstructions(data)
solution_1 = len(runInstructions(points, [folds[0]], False))

# SOLUTION 2
solution_2 = len(runInstructions(*createInstructions(data)))

# WRITE
write_output([str(solution_1), str(solution_2)])
