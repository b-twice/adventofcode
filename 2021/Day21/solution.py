from functools import lru_cache
from itertools import product
import sys
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '21'

input_file = write_input(DAY)

# READ
data = None
with open(input_file, 'r') as f:
    data = [int(line.split(': ')[1]) for line in f.read().splitlines()]


def countDiceScore(dice, inc, goal):
    score = 0

    while inc > 0:
        dice += 1
        score += dice
        if dice == goal:
            dice = 0
        inc -= 1
    return score, dice


def turn(player, die, goal):
    diceIdx, diceRolls = die
    pos, score = player
    result, diceIdx = countDiceScore(diceIdx, 3, goal)
    diceRolls += 3
    pos = ((pos + (result % 10)) % 10) or 10
    score += pos
    return (pos, score), (diceIdx, diceRolls)


def play(player1, player2, die, goal):

    stop = False

    while not stop:
        player1, die = turn(player1, die, goal)
        if player1[1] >= goal:
            stop = True
            break
        player2, die = turn(player2, die, goal)
        if player2[1] >= goal:
            stop = True
            break
    return [player1, player2], die


# SOLUTION 1
sol1State, sol1Die = play((data[0], 0), (data[1], 0), (100, 0), 1000)
solution_1 = sol1Die[1] * min([s[1] for s in sol1State])

# SOLUTION 2


diracDie = [1, 2, 3]


@lru_cache(None)
def play2(player1, player2):
    if player1[1] >= 21:
        return (1, 0)
    if player2[1] >= 21:
        return (0, 1)

    score = (0, 0)
    for d1, d2, d3 in product(diracDie, diracDie, diracDie):
        pPos, pScore = player1
        pPos = ((pPos + ((d1 + d2 + d3) % 10)) % 10) or 10
        pScore += pPos
        p2Win, p1Win = play2(player2, (pPos, pScore))
        score = (score[0] + p1Win, score[1] + p2Win)
    return score


ans2 = play2((data[0], 0), (data[1], 0))
solution_2 = max(ans2)

# WRITE
write_output([str(solution_1), str(solution_2)])
