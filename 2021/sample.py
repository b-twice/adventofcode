from collections import deque
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '1'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [int(line) for line in f.read().splitlines()]

# SOLUTION 1
solution_1 = 0

# SOLUTION 2
solution_2 = 0

# WRITE
write_output([str(solution_1), str(solution_2)])
