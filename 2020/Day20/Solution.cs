using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D20
{
  
  public enum Side { Top, Right, Bottom, Left };

  public record Tile(int Id, char[][] Grid);

  public class Solution: ISolution {

  
    public string Name {get;} = "";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);




    long SolvePartOne(string input) {
      var tiles = Parse(input);
      var candidates = Parse(input);
      var pairs = (
        from tile in tiles 
        from candidate in candidates
        where candidate.Id != tile.Id
        select (tile:tile, candidate:candidate)
      ).ToArray();
      var arrangements = (
        from side in new [] {Side.Top, Side.Right, Side.Bottom, Side.Left}
        from otherSide in new [] {Side.Top, Side.Right, Side.Bottom, Side.Left}
        select (side, otherSide)
      ).ToArray();
      var matches = new Dictionary<int, int>();
      foreach(var pair in pairs) {
        if (!matches.ContainsKey(pair.tile.Id)) {
          matches[pair.tile.Id] = 0;
        }
        if (MatchTiles(pair.tile, pair.candidate, arrangements)) {
          matches[pair.tile.Id] = matches[pair.tile.Id] + 1;
        }
      }
      return matches.Where(e => e.Value == 2).Select(e => e.Key)
        .Aggregate(1L, (total, next) => total * next);
    }
    

    long SolvePartTwo(string input) {
      return 1;
    }

    bool MatchTiles(Tile tile, Tile candidate, (Side, Side)[] arrangements) {
      foreach (var (tileSide, candidateSide)  in arrangements) {
        if (AnySideEquals(ExtractSide(tile, tileSide), ExtractSide(candidate, candidateSide))) {
          return true;
        }
      }
      return false;
    }

    bool AnySideEquals(char[] left, char[] right)
    {
      var leftReverse = left.Reverse().ToArray();
      var rightReverse = right.Reverse().ToArray();
      if(SidesEqual(left, right) || SidesEqual(leftReverse, right) || SidesEqual(leftReverse, rightReverse) || SidesEqual(left, rightReverse)) {
        return true;
      }
      return false;
    }

    bool SidesEqual(char[] left, char[] right)
    {
      if(left.Length != right.Length) return false;
      for (var i = 0; i < left.Length; i++) {
        if (left[i] != right[i]) return false;
      }
      return true;
    }

    char[] ExtractSide(Tile tile, Side side) => side switch
    {
      Side.Top => tile.Grid[0],
      Side.Bottom => tile.Grid[tile.Grid.Length -1],
      Side.Left => tile.Grid.Select(t => t.First()).ToArray(),
      Side.Right => tile.Grid.Select(t => t.Last()).ToArray(),
    };

    IEnumerable<Tile> Parse(string input) {
        return (from lines in input.Split("\n\n").Select(g => g.Split('\n'))
            let id = Int32.Parse(Regex.Replace(lines.First(), @"[^\d]", ""))
            let grid = lines.Skip(1).Select(s => s.ToCharArray()).ToArray()
          select new Tile(id, grid)
        );
    }
  }

}