from collections import Counter
from collections import defaultdict
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '14'

input_file = write_input(DAY)

# READ
rules = {}
polymerTemplate = None
with open(input_file, 'r') as f:
    for i, line in enumerate(f.read().splitlines()):
        if i == 0:
            polymerTemplate = line
        elif line != '':
            parts = line.split(' -> ')
            rules[parts[0]] = parts[1]


class PolymerNode:

    def __init__(self, val):
        self.val = val
        self.next = None


def createPolymer(template):
    root = PolymerNode(None)
    head = root
    for c in template:
        head.next = PolymerNode(c)
        head = head.next
    return root.next


def growPolymer(polymer, steps):
    i = 0
    head = polymer
    while i < steps:
        while head and head.next:
            newPolymer = PolymerNode(rules[head.val + head.next.val])
            head.next, head, newPolymer.next, = newPolymer, head.next, head.next
        head = polymer
        i += 1

    return polymer


def polymerValues(polymer):
    vals = []
    while polymer:
        vals.append(polymer.val)
        polymer = polymer.next
    return vals


polymer = createPolymer(polymerTemplate)
growPolymer(polymer, 10)
vals = polymerValues(polymer)
c = Counter(vals).most_common()
solution_1 = c[0][1] - c[-1][1]

# SOLUTION 2


def initPolymerCount(template):
    m = defaultdict(int)
    total = defaultdict(int)
    total[template[0]] += 1
    for i in range(1, len(template)):
        polymer = template[i-1] + template[i]
        m[polymer] += 1
        total[template[i]] += 1
    return m, total


def growPolymerCount(counter, total):
    m = defaultdict(int)
    for polymer, c in counter.items():
        newNode = rules[polymer]
        m[polymer[0] + newNode] += c
        m[newNode + polymer[1]] += c
        total[newNode] += c
    return m


def growPolymer2(counter, total, steps):
    i = 0
    while i < steps:
        counter = growPolymerCount(counter, total)
        i += 1
    return total


counter, total = initPolymerCount(polymerTemplate)
total = growPolymer2(counter, total, 40)
values = [v for v in total.values()]
solution_2 = max(values) - min(values)


# WRITE
write_output([str(solution_1), str(solution_2)])
