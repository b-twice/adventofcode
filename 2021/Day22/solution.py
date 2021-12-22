import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '22'

input_file = write_input(DAY)

# READ
data = []
with open(input_file, 'r') as f:
    lines = [line.split(" ") for line in f.read().splitlines()]
    for line in lines:
        coords = line[1].split(',')
        coords = [[int(c) for c in coord.split('=')[1].split('..')]
                  for coord in coords]
        data.append([line[0], coords])


# SOLUTION 1
def createCubes(coords):
    result = [()]
    for i, coords in enumerate(coords):
        result = [
            (*r, coord) for r in result for coord in range(coords[0], coords[1] + 1)]
    return set(result)


def reboot(steps):
    cubes = set()
    for step in steps:
        newCubes = createCubes(step[1])
        cubes = cubes | newCubes if step[0] == 'on' else cubes - newCubes
    return cubes


sol1Data = [step for step in data if all(
    [c >= -50 and c <= 50 for group in step[1] for c in group])]

solution_1 = len(reboot(sol1Data))

# SOLUTION 2
solution_2 = len(reboot(data))

# WRITE
write_output([str(solution_1), str(solution_2)])
