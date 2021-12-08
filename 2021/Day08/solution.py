import functools
from collections import defaultdict
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '8'

input_file = write_input(DAY)

# READ
data = []
with open(input_file, 'r') as f:
    for line in f.read().splitlines():
        parts = line.split(' | ')
        data.append([parts[0].split(), parts[1].split()])

# SOLUTION 1
segmentMap = {
    0: 'abcefg',
    1: 'cf',
    2: 'acdeg',
    3: 'acdfg',
    4: 'bcdf',
    5: 'abdfg',
    6: 'abdefg',
    7: 'acf',
    8: 'abcdefg',
    9: 'abcdfg'
}
solution_1 = 0
searchValues = [len(segmentMap[1]), len(segmentMap[4]),
                len(segmentMap[7]), len(segmentMap[8])]
solution_1 = functools.reduce(
    lambda a, b: a + (1 if len(b) in searchValues else 0), [output for outputs in data for output in outputs[1]], 0)

# SOLUTION 2
segmentMap = {
    'abcefg': 0,
    'cf': 1,
    'acdeg': 2,
    'acdfg': 3,
    'bcdf': 4,
    'abdfg': 5,
    'abdefg': 6,
    'acf': 7,
    'abcdefg': 8,
    'abcdfg': 9
}

solution_2 = 0
for signals, outputs in data:
    sm = {}
    m = defaultdict(list)
    for signal in signals:
        m[len(signal)].append(set(signal))
    # 7 - 1 = 'a'
    one = m[2][0]
    sm[(m[3][0] - one).pop()] = 'a'
    # 1 & 6 = 'f'
    f = [one & v for v in m[6] if len(one & v) == 1][0]
    sm[f.pop()] = 'f'
    # 1 - 6 = 'c'
    c = [m[2][0] - v for v in m[6] if len(one - v) == 1][0]
    sm[c.pop()] = 'c'

    three = [v for v in m[5] if len(one & v) == 2][0]
    d = [three - v for v in m[6]
         if len(three - v) == 1 and len(one & v) == 2][0]
    sm[d.pop()] = 'd'

    found = set(sm.keys())
    sm[(m[4][0] - found).pop()] = 'b'
    found = set(sm.keys())
    sm[(three - found).pop()] = 'g'
    found = set(sm.keys())
    sm[(m[7][0] - found).pop()] = 'e'

    total = 0
    for output in outputs:
        total = total * 10 + \
            segmentMap[''.join(sorted([sm[c] for c in output]))]

    solution_2 += total

# WRITE
write_output([str(solution_1), str(solution_2)])
