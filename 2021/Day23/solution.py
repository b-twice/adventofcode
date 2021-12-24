import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '23'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [line for line in f.read().splitlines()]

steps = [9, 500, 5000, 7, 300, 20, 500, 30, 40, 9000, 3, 3]
# SOLUTION 1
solution_1 = sum(steps)

# SOLUTION 2
steps2 = [5000, 50, 9, 9, 20, 800, 60, 900, 50, 60, 50, 60,
          2, 5, 700, 700, 2, 6000, 3, 5, 11000, 11000, 11000, 5, 5, 9, 9]
solution_2 = sum(steps2)

# WRITE
write_output([str(solution_1), str(solution_2)])
