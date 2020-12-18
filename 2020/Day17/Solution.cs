using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D17
{

  public class Solution: ISolution {

    public string Name {get;} = "Conway Cubes";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      var neighbors = (from x in new[] { -1, 0, 1 }
          from y in new[] { -1, 0, 1 }
          from z in new[] { -1, 0, 1 }
          where x != 0 || y != 0 || z != 0
          select (x, y, z, w:0)).ToArray();
      var map = InitMap(Parse(input));
      return Run(map, 6, neighbors);
    }

    long SolvePartTwo(string input) {
      var neighbors = (from x in new[] { -1, 0, 1 }
          from y in new[] { -1, 0, 1 }
          from z in new[] { -1, 0, 1 }
          from w in new[] { -1, 0, 1 }
          where x != 0 || y != 0 || z != 0 || w != 0
          select (x, y, z, w:w)).ToArray();
      var map = InitMap(Parse(input));
      return Run(map, 6, neighbors);
    }

    Dictionary<(int x, int y, int z, int w), (bool active, int neighbors)> InitMap(char[][] grid) {
      var map = new Dictionary<(int x, int y, int z, int w), (bool active, int count)>();
      for (var x = 0; x < grid.Length; x++) {
        for (var y = 0; y < grid[x].Length; y++) {
          if (grid[x][y] == '#') {
            map.Add((x, y, 0, 0), (true, 0));
          }
        }
      }
      return map;
    }

    long Run(Dictionary<(int x, int y, int z, int w), (bool active, int count)> map, int cycles, (int x, int y, int z, int w)[] neighbors)
    {
      while (cycles > 0) {
        foreach(var p in map.Keys.ToList()) {
          foreach (var n in neighbors)  {
            var neighbor = (p.x + n.x, p.y + n.y, p.z + n.z, p.w + n.w);
            if (!map.ContainsKey(neighbor)) {
              map[neighbor] = (false, 1);
              continue;
            }
            if (!map[neighbor].active) {
              map[neighbor] = IncrementPoint(map[neighbor]);
            }
            else {
              map[p] = IncrementPoint(map[p]);
            }
          }
        }
        foreach(var p in map.Keys) {
          var (active, count) = map[p];
          if (active == true && (count == 2 || count == 3)) {
            map[p] = (true, 0);
          }
          else if(active == false && count == 3) {
            map[p] = (true, 0);
          }
          else {
            map.Remove(p);
          }
        }
        cycles--;
      }
      return map.Keys.Count(); 
    }

    (bool active, int count) IncrementPoint((bool active, int count) point) {
      return (point.active, point.count + 1);
    }

    char[][] Parse(string input) {
        return input.Split("\n").Select(s => s.ToCharArray()).ToArray();
    }
  }

}