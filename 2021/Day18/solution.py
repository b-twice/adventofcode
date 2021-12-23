import json
import sys
import functools
sys.path.append('..')
from data_handler import write_input, write_output  # noqa

DAY = '18'

input_file = write_input(DAY)


class ExplodeException(Exception):
    pass


class SplitException(Exception):
    pass


def magnitude(a):
    if isinstance(a, Value):
        return a.val
    return (magnitude(a[0]) * 3) + (magnitude(a[1]) * 2)


def inorder(a, values=None):
    values = [] if values is None else values
    if isinstance(a, Value):
        values.append(a)
    else:
        inorder(a[0], values)
        inorder(a[1], values)
    return values


def depth(a, i=1):
    if isinstance(a, Value):
        return i - 1
    return max(depth(a[0], i + 1), depth(a[1], i+1))


def hasSplit(a):
    if isinstance(a, Value):
        return a.val >= 10
    return hasSplit(a[0]) or hasSplit(a[1])


class Value:
    def __init__(self, val):
        self.val = val

    def __str__(self):
        return str(self.val)


def test():
    assert magnitude(arrSnailNumbers('[[1,2],[[3,4],5]]')) == 143
    assert magnitude(arrSnailNumbers(
        '[[[[0,7],4],[[7,8],[6,0]]],[8,1]]')) == 1384
    assert magnitude(arrSnailNumbers('[[[[1,1],[2,2]],[3,3]],[4,4]]')) == 445
    assert magnitude(arrSnailNumbers('[[[[3,0],[5,3]],[4,4]],[5,5]]')) == 791
    assert magnitude(arrSnailNumbers('[[[[5,0],[7,4]],[5,5]],[6,6]]')) == 1137
    assert magnitude(arrSnailNumbers(
        '[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]')) == 3488

    e1 = arrSnailNumbers('[[[[[9,8],1],2],3],4]')
    try:
        explodeNumber(e1, inorder(e1))
    except ExplodeException:
        pass
    assert strSnailNumbers(e1) == '[[[[0,9],2],3],4]'

    e1 = arrSnailNumbers('[7,[6,[5,[4,[3,2]]]]]')
    try:
        explodeNumber(e1, inorder(e1))
    except ExplodeException:
        pass
    assert strSnailNumbers(e1) == '[7,[6,[5,[7,0]]]]'

    e1 = arrSnailNumbers('[[6,[5,[4,[3,2]]]],1]')
    try:
        explodeNumber(e1, inorder(e1))
    except ExplodeException:
        pass
    assert strSnailNumbers(e1) == '[[6,[5,[7,0]]],3]'

    e1 = arrSnailNumbers('[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]')
    try:
        explodeNumber(e1, inorder(e1))
    except ExplodeException:
        pass
    assert strSnailNumbers(e1) == '[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]'

    e1 = arrSnailNumbers('[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]')
    try:
        explodeNumber(e1, inorder(e1))
    except ExplodeException:
        pass
    assert strSnailNumbers(e1) == '[[3,[2,[8,0]]],[9,[5,[7,0]]]]'

    e1 = arrSnailNumbers('[[[[0,7],4],[15,[0,13]]],[1,1]]')
    try:
        splitNumber(e1)
    except SplitException:
        pass
    assert strSnailNumbers(e1) == '[[[[0,7],4],[[7,8],[0,13]]],[1,1]]'

    e1 = arrSnailNumbers('[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]')
    try:
        splitNumber(e1)
    except SplitException:
        pass
    assert strSnailNumbers(e1) == '[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]'

    e1 = '[[[[4,3],4],4],[7,[[8,4],9]]]'
    e2 = '[1,1]'
    added = addNumbers(e1, e2)
    assert added == '[[[[0,7],4],[[7,8],[6,0]]],[8,1]]'


def arrSnailNumbers(str):
    return json.loads(str, parse_int=lambda x: Value(int(x)))


def strSnailNumbers(a, result=''):
    if isinstance(a, Value):
        result += str(a.val)
        return result

    result += '['
    result += strSnailNumbers(a[0])
    result += ','
    result += strSnailNumbers(a[1])
    result += ']'
    return result


def isPair(a):
    return isinstance(a[0], Value) and isinstance(a[1], Value)


def explodePair(a, orderedValues):
    lValue = a[0].val
    lIdx = orderedValues.index(a[0])
    if lIdx > 0:
        orderedValues[lIdx-1].val += lValue
    rValue = a[1].val
    rIdx = orderedValues.index(a[1])
    if rIdx < len(orderedValues) - 1:
        orderedValues[rIdx+1].val += rValue


def explodeNumber(a, orderedValues, i=1):
    if isinstance(a, Value):
        return

    if isPair(a) and i > 4:
        return a

    left = explodeNumber(a[0], orderedValues, i+1)

    if left:
        explodePair(a[0], orderedValues)
        a[0] = Value(0)
        raise ExplodeException

    right = explodeNumber(a[1], orderedValues, i+1)
    if right:
        explodePair(a[1], orderedValues)
        a[1] = Value(0)
        raise ExplodeException


def splitNumber(a):
    if isinstance(a, Value):
        return a.val >= 10

    left = splitNumber(a[0])

    if left:
        a[0] = [Value(a[0].val//2), Value(a[0].val-(a[0].val//2))]
        raise SplitException

    right = splitNumber(a[1])
    if right:
        a[1] = [Value(a[1].val//2), Value(a[1].val-(a[1].val//2))]
        raise SplitException


def addNumbers(a, b):
    c = [arrSnailNumbers(a), arrSnailNumbers(b)]
    canReduce = True
    while canReduce:
        explode = depth(c) > 4
        split = hasSplit(c)
        canReduce = explode or split
        orderedValues = inorder(c)
        if explode:
            # handle explode
            try:
                explodeNumber(c, orderedValues)
            except ExplodeException:
                pass
            continue
        elif split:
            try:
                splitNumber(c)
            except SplitException:
                pass
            continue
    return strSnailNumbers(c)


test()
# SOLUTION_1
data = None
with open(input_file, 'r') as f:
    data = [line for line in f.read().splitlines()]

result = functools.reduce(lambda a, b: addNumbers(a, b), data)
solution_1 = magnitude(arrSnailNumbers(result))

# SOLUTION_2
max_magnitude = 0
for i in range(len(data)):
    for j in range(i+1, len(data)):
        m = magnitude(arrSnailNumbers(addNumbers(data[i], data[j])))
        m2 = magnitude(arrSnailNumbers(addNumbers(data[j], data[i])))
        max_magnitude = max(max_magnitude, m, m2)
solution_2 = max_magnitude


write_output([str(solution_1), str(solution_2)])
