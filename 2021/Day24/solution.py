import functools
import sys
import itertools
sys.path.append('..')
from data_handler import write_input, write_output  # noqa


DAY = '24'

input_file = write_input(DAY)

# READ
instructions = []
with open(input_file, 'r') as f:
    curr = None
    for line in f.read().splitlines():
        parts = line.split(' ')
        if parts[0] == 'inp':
            if curr:
                instructions.append(curr)
            curr = []
        curr.append(parts)
    instructions.append(curr)


def execInstruction(inst, a, b):
    if inst == 'add':
        return a + b
    elif inst == 'mod':
        return a % b
    elif inst == 'mul':
        return a * b
    elif inst == 'div':
        return a // b
    elif inst == 'eql':
        return int(a == b)


statePositions = {'w': 0, 'x': 1, 'y': 2, 'z': 3}


def runInstruction(state, inst, var1, var2=0):
    result = execInstruction(
        inst, state[statePositions[var1]], state[statePositions[var2]] if var2 in statePositions else int(var2))
    return setState(state, var1, result)


def setState(state, var1, result):
    if var1 == 'w':
        return (result, state[1], state[2], state[3])
    elif var1 == 'x':
        return (state[0], result, state[2], state[3])
    elif var1 == 'y':
        return (state[0], state[1], result, state[3])
    elif var1 == 'z':
        return (state[0], state[1],  state[2], result)


def countDigits(num):
    d = 0
    while num > 0:
        num //= 10
        d += 1
    return d or 1


def runAlu(modelNumber):
    base = 10 ** (countDigits(modelNumber)-1)
    state = (0, 0, 0, 0)
    pos = 0
    while modelNumber > 0:
        num = modelNumber // base
        # print(num, state)
        for i, instruction in enumerate(instructions[pos]):
            if i == 0:
                state = setState(
                    state, instruction[1], num)
            else:
                state = runInstruction(state, *instruction)
        modelNumber -= base * num
        base //= 10
        pos += 1
    return state


@functools.lru_cache(None)
def calcAlu(modelNumber, state, pos):
    for i, instruction in enumerate(instructions[pos]):
        if i == 0:
            state = setState(
                state, instruction[1], modelNumber[pos])
        else:
            state = runInstruction(state, *instruction)
    return state


def run(modelNumber):

    state = (0, 0, 0, 0)
    for i in range(len(modelNumber)):
        state = calcAlu(modelNumber, state, i)
    return state


result = 0
for modelNumber in itertools.product(range(9, 0, -1), repeat=14):
    state = run(modelNumber)
    if state[3] == 0:
        result = max(modelNumber, result)


# SOLUTION 1
solution_1 = result

# SOLUTION 2
solution_2 = 0

# WRITE
write_output([str(solution_1), str(solution_2)])


'''
inp w
mul x 0
add x z
mod x 26
div z {a}
add x {b}
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y {c}
mul y x
add z y

w = int(input())
x = int((z % 26) + b != w)
z //= a
z *= 25*x+1
z += (w+c)*x

'''
