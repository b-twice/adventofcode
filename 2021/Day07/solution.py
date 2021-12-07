from collections import Counter
import functools
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '7'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [int(v)
            for v in [line for line in f.read().splitlines()][0].split(',')]


@functools.lru_cache(None)
def steps(step):
    if step == 0:
        return 0
    if step == 1:
        return 1
    return steps(step - 1) + step


# SOLUTION 1
def move_crabs(positions):
    solution = float('inf')
    c = Counter(positions)
    mc = c.most_common()
    for num, count in mc:
        running_count = 0
        for num2, count2 in mc:
            running_count += abs((num2 * count2) - (num * count2))
        solution = min(solution, running_count)
    return solution


solution_1 = move_crabs(data)

# SOLUTION 2


def move_crabs2(positions):
    solution = float('inf')
    c = Counter(positions)
    mc = c.most_common()
    start = min(positions)
    end = max(positions)
    for i in range(start, end+1):
        running_count = 0
        for num, count2 in mc:
            movements = steps(abs(num - i))
            running_count += movements * count2
        solution = min(solution, running_count)
    return solution


solution_2 = move_crabs2(data)

# WRITE
write_output([str(solution_1), str(solution_2)])
