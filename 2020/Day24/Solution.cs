using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D24
{

  enum Direction {N, E, S, W, NE, SE, SW, NW };

  public class Solution: ISolution {
  
    public string Name {get;} = "Lobby Layout";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      return Parse(input).Select(FindTile)
        .GroupBy(
          (item => item),
          (key, elements) => new {
            key = key,
            count = elements.Where(p => p.dq == key.dq && p.dr == key.dr).Count()
          }).Where(x => x.count % 2 != 0).Count();
    }

    long SolvePartTwo(string input) {
      return 1;
    }

    (int dq, int dr) FindTile(string directions) {
      var pos = (dq:0, dr:0);
      foreach (var direction in Directions(directions)) {
         pos =  direction switch {
          Direction.NW => (pos.dq, pos.dr - 1),
          Direction.NE => (pos.dq + 1, pos.dr - 1),
          Direction.E => (pos.dq + 1, pos.dr),
          Direction.W => (pos.dq - 1, pos.dr),
          Direction.SW => (pos.dq - 1, pos.dr + 1),
          Direction.SE => (pos.dq, pos.dr + 1),
          Direction.N => (pos.dq + 1, pos.dr - 2),
          Direction.S => (pos.dq - 1, pos.dr + 2),
          _ => throw new Exception("Unexpected direction")
        };
      }
      return pos;
    }

    IEnumerable<Direction> Directions(string directions) {
      var idx = 0;
      while (idx < directions.Length) {
        var direction = directions[idx].ToString();
        if (direction == "s" || direction == "n") {
            direction = $"{direction}{directions[idx+1]}";
            idx++;
        }
        idx++;
        yield return direction switch {
          "nw" => Direction.NW,
          "ne" => Direction.NE,
          "e" => Direction.E,
          "w" => Direction.W,
          "sw" => Direction.SW,
          "se" => Direction.SE,
          "n" => Direction.N,
          "s" => Direction.S,
          _ => throw new Exception("Unexpected direction")
        };
      }
    }

    IEnumerable<string> Parse(string input) {
        return input.Split("\n").AsEnumerable();
    }
  }

}