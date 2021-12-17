import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '17'

input_file = write_input(DAY)

# READ
gridRanges = []
with open(input_file, 'r') as f:
    for p in f.read().splitlines()[0].split(' ')[2:]:
        ranges = [int(p.replace('x=', '').replace('y=', '').replace(',', ''))
                  for p in p.split('..')]
        gridRanges.append(ranges)


def fireProbe(x, y):
    mx = x
    my = y
    pos = (x, y)

    xrange, yrange = gridRanges
    while True:
        mx = max(mx, pos[0])
        my = max(my, pos[1])

        if xrange[0] <= pos[0] <= xrange[1] and yrange[0] <= pos[1] <= yrange[1]:
            return {"success": True, 'x': mx, 'y': my}
        if pos[0] > xrange[1] or pos[1] < yrange[0]:
            return {"success": False, 'x': float('-inf'), 'y': float('-inf')}
        xadjust = 0
        if x > 0:
            xadjust = -1
        if x < 0:
            xadjust = 1
        x = x + xadjust
        y -= 1
        pos = ((pos[0] + x), (pos[1] + y))


maxy = float('-inf')

total = 0
for x in range(0, 500):
    for y in range(-200, 500):
        result = fireProbe(x, y)
        if result['success']:
            maxy = max(maxy, result['y'])
            total += 1


# SOLUTION 1
solution_1 = maxy

# SOLUTION 2
solution_2 = total

# WRITE
write_output([str(solution_1), str(solution_2)])
