import sys
from collections import defaultdict
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '4'

input_file = write_input(DAY)

# READ
nums = []
boards = []
with open(input_file, 'r') as f:
    board = []

    for i, line in enumerate(f.read().splitlines()):
        if i == 0:
            nums = [n for n in line.split(',')]
            continue
        if not line:
            if len(board):
                boards.append(board)
                board = []
        else:
            board.append([n for n in line.split(' ') if n])


def validateBoard(board):
    for i, row in enumerate(board):
        rowValid = all([True if v == 'x' else False for v in row])
        columnValid = all(
            [True if board[j][i] == 'x' else False for j in range(len(row))])
        if rowValid or columnValid:
            return True
    return False


def boardPositions(board):
    for i in range(len(board)):
        for j in range(len(board[0])):
            yield (i, j)


def scoreBoard(board):
    s = sum([int(board[pos[0]][pos[1]])
            for pos in boardPositions(board) if board[pos[0]][pos[1]] != 'x'])
    return s


def playBingo(nums, boards):
    for num in nums:
        for board in boards:
            for i, j in boardPositions(board):
                if board[i][j] == num:
                    board[i][j] = 'x'
                    if validateBoard(board):
                        return scoreBoard(board) * int(num)
                    break


# SOLUTION 1
solution_1 = playBingo(nums, boards)


def playBingo2(nums, boards):
    winners = {}
    winOrder = []
    for num in nums:
        for idx, board in enumerate(boards):
            if idx in winners:
                continue
            for i, j in boardPositions(board):
                if board[i][j] == num:
                    board[i][j] = 'x'
                    if validateBoard(board):
                        winners[idx] = scoreBoard(board) * int(num)
                        winOrder.append(idx)
                        break
    return winners[winOrder[-1]]


# SOLUTION 2
solution_2 = playBingo2(nums, boards)

# WRITE
write_output([str(solution_1), str(solution_2)])
