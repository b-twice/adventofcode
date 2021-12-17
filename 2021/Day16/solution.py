from math import prod
from collections import deque
from functools import reduce
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '16'

input_file = write_input(DAY)

# READ
packet = None
with open(input_file, 'r') as f:
    packet = f.read().splitlines()[0]


class Packet:

    def __init__(self, version, typeId, value, packets):
        self.version = version
        # 4 = literal, !4 = operator
        self.typeId = typeId
        self.value = value
        self.packets = packets or []


bitMap = {
    '0': '0000',
    '1': '0001',
    '2': '0010',
    '3': '0011',
    '4': '0100',
    '5': '0101',
    '6': '0110',
    '7': '0111',
    '8': '1000',
    '9': '1001',
    'A': '1010',
    'B': '1011',
    'C': '1100',
    'D': '1101',
    'E': '1110',
    'F': '1111',
}


def toBits(h):
    p = []
    for c in h:
        p.append(bitMap[c])
    return ''.join(p)


def parseValue(bits, i):
    parts = []
    while i < len(bits):
        lead = int(bits[i], 2)
        parts.append(bits[i+1:i+5])
        i += 5
        if lead == 0:
            break
    return i, int(''.join(parts), 2)


def parseOperator(bits, i):
    lengthTypeId = int(bits[i], 2)
    offset = 16 if lengthTypeId == 0 else 12
    numberOfSubpackets = int(bits[i+1:i+offset], 2)
    i += offset
    packets = []
    if lengthTypeId == 0:
        numberOfSubpackets += i
        while i < numberOfSubpackets:
            i, packet = extract(bits, i)
            if packet:
                packets.append(packet)
    else:
        for _ in range(numberOfSubpackets):
            i, packet = extract(bits, i)
            if packet:
                packets.append(packet)
    return i, packets


def extract(bits, i):
    try:
        header = int(bits[i:i+3], 2)
        typeId = int(bits[i+3:i+6], 2)

        i += 6
        value = None
        packets = []
        if typeId == 4:
            i, value = parseValue(bits, i)
        else:
            i, packets = parseOperator(bits, i)
        return i, Packet(header, typeId, value, packets)
    except:
        return i, None


i, packetNode = extract(toBits(packet), 0)


# SOLUTION 1
def dfs(packet):
    total = packet.version
    for packet in packet.packets:
        total += dfs(packet)
    return total


solution_1 = dfs(packetNode)

# SOLUTION 2


def applyValue(packet, values):
    applied = 0
    if packet.typeId == 0:
        applied = sum(values)
    if packet.typeId == 1:
        applied = prod(values)
    if packet.typeId == 2:
        applied = min(values)
    if packet.typeId == 3:
        applied = max(values)
    if packet.typeId == 5:
        applied = int(values[0] > values[1])
    if packet.typeId == 6:
        applied = int(values[1] > values[0])
    if packet.typeId == 7:
        applied = int(values[0] == values[1])
    return applied


def dfs2(node):
    if node.typeId == 4:
        return node.value
    result = []
    for p in node.packets:
        result.append(dfs2(p))

    return applyValue(node, result)


solution_2 = dfs2(packetNode)

# WRITE
write_output([str(solution_1), str(solution_2)])
