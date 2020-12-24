using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D24
{

  enum Direction {E, W, NE, SE, SW, NW };

  public class Solution: ISolution {
  
    public string Name {get;} = "Lobby Layout";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      return Parse(input).Select(FindTile)
        .GroupBy(
          (item => item),
          (tile, tiles) => new {
            Tile = tile,
            Count = tiles.Where(t => t.dq == tile.dq && t.dr == tile.dr).Count()
          }).Where(x => x.Count % 2 != 0).Count();
    }

    long SolvePartTwo(string input) {
      var tileMap = Parse(input).Select(FindTile)
        .GroupBy(
          (item => item),
          (tile, tiles) => new {
            Tile = tile,
            Count = tiles.Where(t => t.dq == tile.dq && t.dr == tile.dr).Count()})
        .Where(t => t.Count % 2 != 0)
        .Select(t => t.Tile)
        .ToDictionary<(int dq, int dr), (int dq, int dr), (bool active, int count)>(tile => tile, tile => (active: true, count:0));
      return RunExhibit(tileMap, 100).Keys.Count();
    }

    Dictionary<(int dq, int dr), (bool active, int count)> RunExhibit(Dictionary<(int dq, int dr), (bool active, int count)> tileMap, int days) {
      while (days > 0) {
        foreach(var tile in tileMap.Keys.ToList()) {
          foreach(var n in Neighbors(tile)) {
            if (!tileMap.ContainsKey(n)) {
              tileMap[n] = (false, 1);
              continue;
            }
            if (!tileMap[n].active) {
              tileMap[n] = (tileMap[n].active, tileMap[n].count + 1);
            }
            else {
              tileMap[tile] = (tileMap[tile].active, tileMap[tile].count + 1);
            }
          }
        }
        foreach(KeyValuePair<(int dq, int dr), (bool active, int count)> entry in tileMap) {
          if (entry.Value.active is true) {
            if (entry.Value.count == 0 || entry.Value.count > 2) tileMap.Remove(entry.Key);
            else tileMap[entry.Key] = (true, 0);
          }
          else {
            if (entry.Value.count == 2) tileMap[entry.Key] = (true, 0);
            else tileMap.Remove(entry.Key);
          }
        }
        days--;
      }
      return tileMap;
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
          _ => throw new Exception("Unexpected direction")
        };
      }
      return pos;
    }
    IEnumerable<(int dq, int dr)> Neighbors((int dq, int dr) tile) {
      foreach (var direction in Enum.GetValues(typeof(Direction))) {
         yield return direction switch {
          Direction.NW => (tile.dq, tile.dr - 1),
          Direction.NE => (tile.dq + 1, tile.dr - 1),
          Direction.E => (tile.dq + 1, tile.dr),
          Direction.W => (tile.dq - 1, tile.dr),
          Direction.SW => (tile.dq - 1, tile.dr + 1),
          Direction.SE => (tile.dq, tile.dr + 1),
          _ => throw new Exception("Unexpected direction")
        };
      }
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
          _ => throw new Exception("Unexpected direction")
        };
      }
    }

    // Dictionary<(int dq, int dr), (bool active, int neighbors)> InitMap(char[][] grid) {
    //   var map = new Dictionary<(int x, int y, int z, int w), (bool active, int count)>();
    //   for (var x = 0; x < grid.Length; x++) {
    //     for (var y = 0; y < grid[x].Length; y++) {
    //       if (grid[x][y] == '#') {
    //         map.Add((x, y, 0, 0), (true, 0));
    //       }
    //     }
    //   }
    //   return map;
    // }

    IEnumerable<string> Parse(string input) {
        return input.Split("\n").AsEnumerable();
    }
  }

}