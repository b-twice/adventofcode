using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D20
{
  
  public enum Side { Top, Right, Bottom, Left };

  public record Tile(int Id, char[][] Grid);
  public record Match(Tile candidate, Side tileSide, Side candidateSide, bool tileFlip, bool candidateFlip);

  public class Solution: ISolution {

  
    public string Name {get;} = "Jurassic Jigsaw";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    public Dictionary<int, Tile> Tiles  =new Dictionary<int, Tile>();
    public Dictionary<int, List<Match>> Matches = new Dictionary<int, List<Match>>();
    public Dictionary<(int x, int y), Tile> TileGrid = new Dictionary<(int x, int y), Tile>();

    long SolvePartOne(string input) {
      Tiles = MapTiles(input);
      Matches = MatchTiles(input);
      return Matches.Where(e => e.Value.Count == 2).Select(e => e.Key)
        .Aggregate(1L, (total, next) => total * next);
    }
    

    long SolvePartTwo(string input) {
      Tiles = MapTiles(input);
      Matches = MatchTiles(input);
      var tile = new Tile(0, MosaicTileGrid(BuildTileGrid()));
      var monster = new string[]{
          "                  # ",
          "#    ##    ##    ###",
          " #  #  #  #  #  #   "
      };

      var size = tile.Grid[0].Length;
      var tileString = string.Join("\n", Enumerable.Range(0, size).Select(i => string.Join("", tile.Grid[i])));
 
      foreach(var t in RotateTile(tile)) {
        tileString = string.Join("\n", Enumerable.Range(0, size).Select(i => string.Join("", t.Grid[i])));
        var monsterCount = MatchCount(t, monster);
        if (monsterCount > 0) {
          var hashCountInImage = tileString.Count(ch => ch == '#');
          var hashCountInMonster = string.Join("\n", monster).Count(ch => ch == '#');
          return hashCountInImage - monsterCount * hashCountInMonster;
        }
      }
      return 1;
    }

    Dictionary<int, Tile> MapTiles(string input) {
      var tiles = Parse(input);
      var map = new Dictionary<int, Tile>();
      foreach(var tile in tiles) {
        map[tile.Id] = tile;
      }
      return map;
    }


    private Dictionary<int, List<Match>> MatchTiles(string input) {
      var tiles = Parse(input);
      var candidates = Parse(input);
      var pairs = (from tile in tiles from candidate in candidates
        where candidate.Id != tile.Id
        select (tile:tile, candidate:candidate)
      ).ToArray();
      var arrangements = (
        from side in new [] {Side.Top, Side.Right, Side.Bottom, Side.Left}
        from otherSide in new [] {Side.Top, Side.Right, Side.Bottom, Side.Left}
        select (side, otherSide)
      ).ToArray();
      var matches = new Dictionary<int, List<Match>>();
      foreach(var pair in pairs) {
        if (!matches.ContainsKey(pair.tile.Id)) {
          matches[pair.tile.Id] = new List<Match>();
        }
        var match = MatchTiles(pair.tile, pair.candidate, arrangements);
        if (match != null) {
          matches[pair.tile.Id].Add(match);
        }
      }
      return matches;
    }
    private Tile[][] BuildTileGrid() {
      var tileSize = (int)Math.Sqrt(Tiles.Keys.Count());
      var startTile = Tiles[Matches.Where(t => t.Value.Count == 2).First().Key];
      startTile = new Tile(startTile.Id, FlipGrid(startTile.Grid));
      var result = new Char[startTile.Grid.Length * tileSize][];
      var process = new Queue<(int x, int y, Tile tile)>();
      process.Enqueue((0, 0, startTile));
      var seen = new HashSet<int>();

      while (process.Count > 0) {
        var (x, y, tile) = process.Dequeue();
        seen.Add(tile.Id);
        TileGrid[(x, y)] = tile;
        var neighbors = Matches[tile.Id].Select(m => Tiles[m.candidate.Id]).Where(t => !seen.Contains(t.Id));
        if (neighbors.Count() == 0) {
          continue;
        }
        var right = ExtractSide(tile, Side.Right);
        var bottom = ExtractSide(tile, Side.Bottom);
        (int x, int y, Tile tile)? matchesLeft = null;
        (int x, int y, Tile tile)? matchesTop = null;
        foreach (var n in neighbors) {
          foreach(var t in RotateTile(n)) {
            var left = ExtractSide(t, Side.Left);
            var top = ExtractSide(t, Side.Top);
            if (matchesLeft == null && SidesEqual(left, right)) {
              matchesLeft = (x + 1, y, t);
              break;
            }
            else if (matchesTop == null && SidesEqual(top, bottom)) {
              matchesTop = (x, y - 1, t);
              break;
            }
          }
        }
        if (matchesLeft == null && matchesTop == null) {
          throw new Exception("no match");
        }
        if (matchesLeft != null) {
          process.Enqueue(matchesLeft.Value);
          seen.Add(matchesLeft.Value.tile.Id);
          TileGrid[(matchesLeft.Value.x, matchesLeft.Value.y)] = matchesLeft.Value.tile;
        }
        if (matchesTop != null) {
          process.Enqueue(matchesTop.Value);
          seen.Add(matchesTop.Value.tile.Id);
          TileGrid[(matchesTop.Value.x, matchesTop.Value.y)] = matchesTop.Value.tile;
        }
      }
      var tileGrid = new Tile[tileSize][];
      for (var i = 0; i < tileSize; i++) {
        var row = new Tile[tileSize];
        tileGrid[i] = row;
      }
      foreach ( KeyValuePair<(int x, int y), Tile> entry in TileGrid) {
        tileGrid[entry.Key.y * -1][entry.Key.x] = entry.Value;
      }
      ValidateTileGrid();
      return tileGrid;
    }

    void ValidateTileGrid() {
      foreach ( KeyValuePair<(int x, int y), Tile> entry in TileGrid) {
        var (x, y) = entry.Key;
        var top = (x, y + 1);
        var left = (x - 1, y);
        var bottom = (x, y - 1);
        var right = (x + 1, y);
        if (TileGrid.ContainsKey(top) && !SidesEqual(ExtractSide(TileGrid[top], Side.Bottom), ExtractSide(entry.Value, Side.Top))) {
          throw new Exception("Does not Match");
        }
        if (TileGrid.ContainsKey(left) && !SidesEqual(ExtractSide(TileGrid[left], Side.Right), ExtractSide(entry.Value, Side.Left))) {
          throw new Exception("Does not Match");
        }
        if (TileGrid.ContainsKey(right) && !SidesEqual(ExtractSide(TileGrid[right], Side.Left), ExtractSide(entry.Value, Side.Right))) {
          throw new Exception("Does not Match");
        }
        if (TileGrid.ContainsKey(bottom) && !SidesEqual(ExtractSide(TileGrid[bottom], Side.Top), ExtractSide(entry.Value, Side.Bottom))) {
          throw new Exception("Does not Match");
        }
      }

    }

    Char[][] MosaicTileGrid(Tile[][] tileGrid) 
    {
      var rowSize = (int)Math.Sqrt(Tiles.Keys.Count());
      var tileSize = tileGrid[0][0].Grid.Length - 2;
      var result = new Char[rowSize * tileSize][];
      var writeIdx = 0;
      for (var i = 0; i < rowSize; i++) {
        for (var j = 1; j < tileSize + 1; j++) {
          result[writeIdx] = ReadTileLine(tileGrid[i], j);
          writeIdx++;
        }
      }
      return result;

    }

    int MatchCount(Tile tile, params string[] pattern) 
    {
      var size = tile.Grid[0].Length;
      var res = 0;
      var (ccolP, crowP) = (pattern[0].Length, pattern.Length);
      for (var irow = 0; irow < size - crowP; irow++) 
      for (var icol = 0; icol < size - ccolP ; icol++) {
          bool match() {
              for (var icolP = 0; icolP < ccolP; icolP++)
              for (var irowP = 0; irowP < crowP; irowP++) {
                  if (pattern[irowP][icolP] == '#' && tile.Grid[irow + irowP][icol + icolP] != '#') {
                      return false;
                  }
              }
              return true;
          }
          if(match()) {
              res++;
          }
      }
      return res;
    }

    IEnumerable<Tile> RotateTile(Tile tile)
    {
      var grid = tile.Grid;
      var grids = new List<char[][]>(){tile.Grid};
      var i = 0;
      while (i < 3) {
        grid = RotateGrid(grid);
        grids.Add(grid);
        i++;
      }
      grid = FlipGrid(grid);
      grids.Add(grid);
      i = 0;
      while (i < 3) {
        grid = RotateGrid(grid);
        grids.Add(grid);
        i++;
      }
      foreach (var g in grids) {
        yield return new Tile(tile.Id, g);
      }
    }
    

    char[][] FlipGrid(char[][] grid) 
    {
     var result = new char[grid.Length][];
     for (var i = 0; i < grid.Length; i++) {
       result[i] = grid[i].Reverse().ToArray();
     }
     return result;
    }

    public char[][] RotateGrid(char[][] input)
    {
        int length = input[0].Length;
        char[][] retVal = new char[length][];
        for(int x = 0; x < length; x++)
        {
            retVal[x] = input.Select(p => p[x]).Reverse().ToArray();
        }
        return retVal;
    }


    Char[] ReadTileLine(Tile[] tiles, int line) 
    {
      var readLine = new Char[tiles.Length * (tiles[0].Grid[0].Length -2)];
      var idx = 0;
      foreach(var tile in tiles) {
        var row = tile.Grid[line];
        for (var i = 1; i < row.Length - 1; i++)
        {
          readLine[idx] = row[i];
          idx++;
        }
      }
      return readLine;
    }

    Match MatchTiles(Tile tile, Tile candidate, (Side, Side)[] arrangements) {
      foreach (var (tileSide, candidateSide)  in arrangements) {
        var (match, tileFlip, candidateFlip) = AnySideEquals(ExtractSide(tile, tileSide), ExtractSide(candidate, candidateSide));
        if (match) {
          return new Match(candidate, tileSide, candidateSide, tileFlip, candidateFlip);
        }
      }
      return null;
    }

    (bool match, bool tileFlip, bool candidateFlip) AnySideEquals(char[] left, char[] right)
    {
      var leftReverse = left.Reverse().ToArray();
      var rightReverse = right.Reverse().ToArray();
      if (SidesEqual(left, right)) return (true, false, false);
      if (SidesEqual(leftReverse, right)) return (true, true, false); 
      if (SidesEqual(leftReverse, rightReverse)) return (true, true, true);
      if (SidesEqual(left, rightReverse)) return (true, false, true);
      return (false, false, false);
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