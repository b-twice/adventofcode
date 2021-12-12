from collections import Counter
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '12'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [line.split('-') for line in f.read().splitlines()]


class Cave:

    def __init__(self, name):
        self.name = name
        self.small = name.islower() and name != 'start' and name != 'end'
        self.neighbors = []
        self.visited = False
        self.visitCount = 0


def createCaveMap(caves):
    m = {}
    for d in caves:
        if d[0] not in m:
            m[d[0]] = Cave(d[0])
        if d[1] not in m:
            m[d[1]] = Cave(d[1])
        m[d[0]].neighbors.append(m[d[1]])
        m[d[1]].neighbors.append(m[d[0]])
    return m


def findPaths(node, prevNode=None):
    if node.name == 'end':
        return [[node.name]]

    node.visited = node.small
    paths = []
    for n in node.neighbors:
        if n.name == 'start' or (prevNode and not node.small and not prevNode.small and n == prevNode) or n.visited:
            continue
        path = findPaths(n, node)
        if path:
            for p in path:
                paths.append([node.name] + p)
    node.visited = False
    return paths


# SOLUTION 1
paths = findPaths(createCaveMap(data)['start'])
solution_1 = len(paths)

# SOLUTION 2


def validatePath(path):
    small = [p.name for p in path if p.small]
    if not len(small):
        return True
    c = Counter(small)
    mc = c.most_common(2)
    if mc[0][1] > 2 or (len(mc) > 1 and mc[0][1] == 2 and mc[1][1] == 2):
        return False
    return True


def findPaths2(node, currentPath=[], prevNode=None):
    if not validatePath(currentPath + [node]):
        return None
    currentPath = list(currentPath) + [node]
    if node.name == 'end':
        return [[node.name]]
    paths = []

    for n in node.neighbors:
        if n.name == 'start' or (prevNode and not node.small and not prevNode.small and n == prevNode):
            continue
        path = findPaths2(n, currentPath, node)
        if path:
            for p in path:
                paths.append([node.name] + p)
    return paths


paths2 = findPaths2(createCaveMap(data)['start'])
solution_2 = len(paths2)

# WRITE
write_output([str(solution_1), str(solution_2)])
