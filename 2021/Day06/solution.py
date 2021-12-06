from collections import Counter
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '6'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [int(f)
            for f in [line for line in f.read().splitlines()][0].split(',')]


def simulate(fishList, days):
    lives = [0, 1, 2, 3, 4, 5, 6, 7, 8]
    counter = Counter(fishList)
    s = 0
    size = len(lives)
    while s < days:
        current = [counter[life] for life in lives]
        six, eight = 0, 0
        if current[0] > 0:
            six, eight = current[0], current[0]
            counter[0] = 0
        for i in range(size-2, -1, -1):
            counter[i] = current[i+1]
        counter[6] += six
        counter[8] = eight
        s += 1
    return sum(counter.values())


# SOLUTION 1
solution_1 = simulate(data, 80)

# SOLUTION 2
solution_2 = simulate(data, 256)

# WRITE
write_output([str(solution_1), str(solution_2)])
