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

    long SolvePartOne(string input) {
      Tiles = MapTiles(input);
      Matches = MatchTiles(input);
      return Matches.Where(e => e.Value.Count == 2).Select(e => e.Key)
        .Aggregate(1L, (total, next) => total * next);
    }
    

    long SolvePartTwo(string input) {
      // Tiles = MapTiles(input);
      // Matches = MatchTiles(input);
      // var tile = MosaicTileGrid(BuildTileGrid());
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
      // var result = new Char[startTile.Grid.Length * tileSize][];
      var tileGrid = new Tile[tileSize][];
      var rowSide = FindNextRowSide(startTile.Id);
      var columnSide = FindNextColumnSide(startTile.Id);
      // var writeIdx = 0;
      // go through each row
      for (var i = 0; i < tileSize; i++) {
        // pull out the tiles
        var tiles = OrderTileRow(startTile, rowSide);
        // for (var j = 0; j < startTile.Grid.Length; j ++) {
        //   result[writeIdx] = ReadTileLine(tiles, j);
        //   writeIdx++;
        // }
        tileGrid[i] = tiles;
        // start at next row
        if (i < tileSize - 1) {
          (startTile, columnSide) = FindNextMatch(startTile.Id, columnSide);
          rowSide = FindNextAdjacentSide(startTile.Id, columnSide);
        }
      }
      // foreach (var row in result) {
      //   Console.WriteLine(row);
      // }
      return tileGrid;
    }

    Tile[][] MosaicTileGrid(Tile[][] tileGrid) 
    {
      (int xx, int yy)[] neighbors = new []{(1, 0), (-1, 0), (0, 1), (0, -1)};
      // var map = new Dictionary<int, (int x, int y)>();
      for (var x = 0; x < tileGrid.Length; x++)
      {
        for (var y=0; y < tileGrid.Length; y++) {
          var tile = tileGrid[x][y];
          foreach (var (xx, yy) in neighbors)
          {
            if (x + xx > 0 && x + xx < tileGrid.Length && y + yy > 0 && y + yy < tileGrid.Length) {
              var other = tileGrid[x+xx][y+yy];
            }
          }
        }
      }
      return tileGrid;
    }

    Side NeighborToSide((int x, int y) point)  {
      if (point is {x:0} and {y:1}) {
        return Side.Top;
      }
      else if (point is {x:1} and {y:0}) {
        return Side.Right;
      }
      else if (point is {x:0} and {y:-1}) {
        return Side.Bottom;
      }
      else if (point is {x:-1} and {y:0}) {
        return Side.Left;
      }
      throw new Exception("Bad point");
    }

    Tile MatchTile(Tile tile, Side targetSide, char[] matchValues) {
      var i = 0;
      while (i < 4) {
        if (SidesEqual(ExtractSide(tile, targetSide), matchValues)) {
          return tile;
        }
        tile = new Tile(tile.Id,  RotateGrid(tile.Grid));
        i++;
      }
      tile = new Tile(tile.Id,  FlipGrid(tile.Grid));
      i = 0;
      while (i < 4) {
        if (SidesEqual(ExtractSide(tile, targetSide), matchValues)) {
          return tile;
        }
        tile = new Tile(tile.Id,  RotateGrid(tile.Grid));
        i++;
      }
      throw new Exception("no match");
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
            retVal[x] = input.Select(p => p[x]).ToArray();
        }
        return retVal;
    }

    Tile[] OrderTileRow(Tile tile, Side side) 
    {
      var tileSize = (int)Math.Sqrt(Tiles.Keys.Count());
      var tiles = new Tile[tileSize];
      tiles[0] = tile;
      var startingSide = side;
      var matchSide = OppositeSide(startingSide);
      for (var i = 1; i < tileSize; i++) {
        var matchValues = ExtractSide(tile, startingSide);
        (tile, side)  = FindNextMatch(tile.Id, side);
        tiles[i] = MatchTile(tile, matchSide, matchValues);
      }
      return tiles;
    }

    Side FindNextAdjacentSide(int id, Side side) => Matches[id].Find(t => t.tileSide != side && t.tileSide != OppositeSide(side)).tileSide;
    Side FindNextRowSide(int id) => Matches[id].Find(t => t is {tileSide: Side.Left} or {tileSide: Side.Right}).tileSide;
    Side FindNextColumnSide(int id) => Matches[id].Find(t => t is {tileSide: Side.Top} or {tileSide: Side.Bottom}).tileSide;

    (Tile tile, Side tileSide) FindNextMatch(int id, Side side) {
      var nextMatch = Matches[id].Find(t => t.tileSide == side);
      return (nextMatch.candidate, OppositeSide(nextMatch.candidateSide));
    }

    Side OppositeSide(Side side) => side switch {
      Side.Top => Side.Bottom,
      Side.Bottom => Side.Top,
      Side.Left => Side.Right,
      Side.Right => Side.Left,
      _ => throw new Exception("Unknown side")
    };

    Char[] ReadTileLine(Tile[] tiles, int line) 
    {
      var readLine = new Char[tiles.Length * tiles[0].Grid[0].Length];
      var idx = 0;
      foreach(var tile in tiles) {
        var row = tile.Grid[line];
        for (var i = 0; i < row.Length; i++)
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