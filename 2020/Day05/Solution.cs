using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D05
{

  public class Solution: ISolution {

    public string Name {get;} = "Binary Boarding";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);


    long SolvePartOne(string input) {
      return BoardingPasses(input).Max();
    }
    long SolvePartTwo(string input) {
      var passes = BoardingPasses(input).OrderBy(x => x);
      var ll = new LinkedList<int>(passes);
      var node = ll.First;
      while (node != null) {
        var previous = node.Previous;
        if (previous != null && node.Value - previous.Value == 2) {
          return node.Value - 1;
        }
        node = node.Next;
      }
      return -1;
    }

    (byte, byte) FindHalf(byte first, byte second) {
      if (first > second) {
          return (second, Convert.ToByte(first - ((first + 1 - second) >> 1)));
      }
      else {
          return (Convert.ToByte(second + 1 - ((second + 1 - first) >> 1)), second);
      }
    }

    IEnumerable<int> BoardingPasses(string input) {
      var lines = input.Split('\n');
      for (var i = 0; i < lines.Length; i++) {
        var boardingPass = lines[i];
        (byte, byte) rowRange = (0, 127);
        (byte, byte) columnRange = (0, 7);
        int rowSeat = -1;
        int columnSeat = -1;
        for (var j = 0; j < boardingPass.Length; j++) {
          char seatPosition = boardingPass[j];
          var (lowerRow, upperRow) = rowRange;
          var (lowerColumn, upperColumn) = columnRange;
          switch (seatPosition) {
            case 'F':
              rowRange = FindHalf(upperRow, lowerRow);
              (lowerRow, upperRow) = rowRange;
              if (lowerRow == upperRow) {
                rowSeat = lowerRow;
              }
              break;
            case 'B':
              rowRange = FindHalf(lowerRow, upperRow);
              (lowerRow, upperRow) = rowRange;
              if (lowerRow == upperRow) {
                rowSeat = upperRow;
              }
              break;
            case 'L':
              columnRange = FindHalf(upperColumn, lowerColumn);
              (lowerColumn, upperColumn) = columnRange;
              if (lowerColumn == upperColumn) {
                columnSeat = lowerColumn;
              }
              break;
            case 'R':
              columnRange = FindHalf(lowerColumn, upperColumn);
              (lowerColumn, upperColumn) = columnRange;
              if (lowerColumn == upperColumn) {
                columnSeat = upperColumn;
              }
              break;
            default:
              break;
          }
        }
        if (rowSeat == -1 || columnSeat == -1) {
          throw new Exception("Unexpected error finding seat position");
        }
        yield return rowSeat * 8 + columnSeat;
      }
    }
  }


}