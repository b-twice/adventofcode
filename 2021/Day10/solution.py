import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '10'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [line for line in f.read().splitlines()]

points = {')': 3, ']': 57, '}': 1197, '>': 25137}

closed = ['()', '[]', '{}', '<>']
open = ['(', '[', '{', '<']

# SOLUTION 1


def countFirstError(line):
    s = []
    for c in line:

        if c in open:
            s.append(c)
        else:
            last = s.pop()
            if not last+c in closed:
                return points[c]
    return 0


solution_1 = sum([countFirstError(line) for line in data])


# SOLUTION 2
points2 = {'(': 1, '[': 2, '{': 3, '<': 4}


def countIncompletes(line):
    s = []
    for c in line:
        if c in open:
            s.append(c)
        else:
            s.pop()
    points = 0
    for c in reversed(s):
        points = points * 5 + points2[c]

    return points


scores = [countIncompletes(line)
          for line in data if countFirstError(line) == 0]

solution_2 = sorted(scores)[len(scores) // 2]
# WRITE
write_output([str(solution_1), str(solution_2)])
