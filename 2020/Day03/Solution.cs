using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D03
{
  public record SlopePosition (
    int column,
    int line
  );

  public class Solution: ISolution {


    public string Name {get;} = "Toboggan Trajector";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      return CountTreesOnSlope(input, 3, 1);
    }
    long SolvePartTwo(string input) {
      return CountTreesOnSlope(input, 1, 1)
      * CountTreesOnSlope(input, 3, 1)
      * CountTreesOnSlope(input, 5, 1)
      * CountTreesOnSlope(input, 7, 1)
      * CountTreesOnSlope(input, 1, 2);
    }

    long CountTreesOnSlope(string input, int right, int down) {
      var lines = Lines(input);
      var pos = new SlopePosition(right, down);
      var trees = 0;
      var lineIdx = down;
      foreach(var line in lines.Skip(down)) {
        if (lineIdx < pos.line) { 
          lineIdx++;
          continue;
        }
        trees = trees + (line[pos.column] == '#' ? 1 : 0);
        pos = new SlopePosition((pos.column + right) % line.Count(), pos.line + down);
        lineIdx++;
      }
      return trees;
    }

    IEnumerable<String> Lines(string input) {
      return  (from line in input.Split('\n') select line);
    }
  }


}