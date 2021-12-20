from collections import defaultdict
import heapq
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '15'

input_file = write_input(DAY)


def dijkstra(G, src, dst, get_neighbors=None):
    if get_neighbors is None:
        get_neighbors = G.get

    distance = defaultdict(lambda: float('inf'), {src: 0})
    queue = [(0, src)]
    visited = set()

    while queue:
        dist, node = heapq.heappop(queue)

        if node == dst:
            return dist

        if node not in visited:
            visited.add(node)
            neighbors = get_neighbors(node)

            if not neighbors:
                continue

            for neighbor, weight in filter(lambda n: n[0] not in visited, neighbors):
                new_dist = dist + weight

                if new_dist < distance[neighbor]:
                    distance[neighbor] = new_dist
                    heapq.heappush(queue, (new_dist, neighbor))

    return float('inf')


def isWithin(rows, pos):
    return pos[0] >= 0 and pos[1] >= 0 and pos[0] < len(rows) and pos[1] < len(rows[0])


def getNeighbors(rows, pos):
    return [n for n in [(pos[0] - 1, pos[1]), (pos[0] + 1, pos[1]),
                        (pos[0], pos[1] + 1), (pos[0], pos[1] - 1)] if isWithin(rows, n)]


# READ
data = None
with open(input_file, 'r') as f:
    data = [[int(v) for v in line] for line in f.read().splitlines()]

m = len(data)
n = len(data[0])

start = (0, 0)
end = (m-1, n-1)


def makeG(g):
    G = defaultdict(list)

    for r, row in enumerate(g):
        for c, column in enumerate(row):
            for nr, nc in getNeighbors(g, (r, c)):
                G[r, c].append(((nr, nc), g[nr][nc]))
                G[nr, nc].append(((r, c), g[r][c]))

    return G


def copyGrid(grid):
    return [[grid[r][c] for c, col in enumerate(row)] for r, row in enumerate(grid)]


def incrementGrid(grid):
    return [[grid[r][c] + 1 if grid[r][c] < 9 else 1 for c, col in enumerate(row)] for r, row in enumerate(grid)]


# SOLUTION 1
solution_1 = dijkstra(makeG(data), start, end)

new = copyGrid(data)
grid = copyGrid(data)
R, C = len(new), len(new[0])

for _ in range(4):
    tmp = [[(x + 1 if x < 9 else 1) for x in row] for row in grid]

    for r in range(R):
        new[r].extend(tmp[r])

    grid = copyGrid(tmp)

old = copyGrid(new)

for _ in range(4):
    tmp = [[(x + 1 if x < 9 else 1) for x in row] for row in old]
    new.extend(tmp)
    old = copyGrid(tmp)

# SOLUTION 2
start, end = (0, 0), (len(new)-1, len(new[0])-1)
solution_2 = dijkstra(makeG(new), start, end)

# WRITE
write_output([str(solution_1), str(solution_2)])
