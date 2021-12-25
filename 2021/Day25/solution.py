import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '25'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [line for line in f.read().splitlines()]

herds = {">": set(), "v": set()}

for x, l in enumerate(data):
    for y, c in enumerate(data[x]):
        if c in herds:
            herds[c].add((x, y))


def nextPosition(grid, pos, herd):
    offset = (0, 1) if herd == '>' else (1, 0)
    nextPos = (pos[0] + offset[0], pos[1] + offset[1])
    if nextPos[0] == len(grid):
        nextPos = (0, nextPos[1])
    if nextPos[1] == len(grid[0]):
        nextPos = (nextPos[0], 0)
    return nextPos


def move(grid, pos, herd, herds):
    nextPos = nextPosition(grid, pos, herd)
    if nextPos not in herds[">"] and nextPos not in herds["v"]:
        return nextPos
    return pos


def moveHerd(grid, herds, herd):
    moves = []
    for h in herds[herd]:
        nextPos = move(grid, h, herd, herds)
        if nextPos != h:
            moves.append([h, nextPos])
    for m in moves:
        herds[herd].remove(m[0])
        herds[herd].add(m[1])
    return len(moves)


def run(grid, herds):
    step = 0
    moving = True
    while moving:
        moved = 0
        moved += moveHerd(grid, herds, '>')
        moved += moveHerd(grid, herds, 'v')
        moving = moved > 0
        step += 1
    return step


# SOLUTION 1
solution_1 = run(data, herds)

# SOLUTION 2
solution_2 = 0

# WRITE
write_output([str(solution_1), str(solution_2)])
