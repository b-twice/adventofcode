using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D17
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Conway Cubes";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);


    // 26 positions for neighbors
    (int x, int y, int z)[] Neighbors = new []{
      (1, 0,  0), (-1, 0,  0), (0, 1,  0), (0, -1,  0),(1, 1,  0), (-1, 1,  0), (-1, -1,  0), (1, -1,  0),
      (1, 0, -1), (-1, 0, -1), (0, 1, -1), (0, -1, -1),(1, 1, -1), (-1, 1, -1), (-1, -1, -1), (1, -1, -1),
      (1, 0,  1), (-1, 0,  1), (0, 1,  1), (0, -1,  1),(1, 1,  1), (-1, 1,  1), (-1, -1,  1), (1, -1,  1),
      (0, 0, 1), (0, 0, -1)
    };

    long SolvePartOne(string input) {
      return Run(new []{Parse(input)}, 6);
    }

    long SolvePartTwo(string input) {
      return Run(new []{Parse(input)}, 6);
    }

    Char[][][] CreateState(int x, int y, int z) {
      var states = new List<char[][]>();
      for (var zz = 0; zz < z; zz++) {
        var state = new List<char[]>();
        for (var xx = 0; xx < x; xx++) {
          var row = new Char[y];
          for (var yy = 0; yy < y; yy++) {
            row.Append('.');
          }
          state.Add(row);
        }
        states.Add(state.ToArray());
      }
      return states.ToArray();
    }

    Char[][][] GrowState(int x, int y, int z, Char[][][] currentState) {
      var states = new Char[z][][];
      for (var zz = 0; zz < z; zz++) {
        states[zz] = new Char[x][];
        for (var xx = 0; xx < x; xx++) {
          states[zz][xx] = new Char[y];
          for (var yy = 0; yy < y; yy++) {
            if (zz > 0 && yy > 0 && xx > 0 && zz < z - 1 && xx < x - 1 && yy < y - 1) {
              states[zz][xx][yy] = GetCube(xx - 1, yy - 1, zz - 1, currentState);
            }
            else {
              states[zz][xx][yy] = '.';
            }
          }
        }
      }
      return states.ToArray();
    }

    long Run(char[][][] currentState, int cycles)
    {
      var z = currentState.Length;
      var x = currentState[0].Length; 
      var y = currentState[0][0].Length;
      var activeCount = 0L;
      for (var cycle = 1; cycle <= cycles; cycle++) {
        activeCount = 0;
        z = z + 2;
        x = x + 2;
        y = y + 2;
        currentState = GrowState(x, y, z, currentState);
        var nextState = CreateState(x, y, z);
        // e.g. if 0, -1 to 1 or -2 to 2
        for (var zz = 0; zz < z; zz++) {
          for (var xx = 0; xx < x; xx++) {
            for (var yy = 0; yy < y; yy++) {
              var cube = ProcessCube(GetCube(xx, yy, zz, currentState), xx, yy, zz, currentState);
              nextState[zz][xx][yy] = cube;
              if (cube == '#') {
                activeCount += 1;
              }
            }
          }
        }
        currentState = nextState;
      }
      return activeCount;
    }

    char GetCube(int x, int y, int z, char[][][] state) {
      if (z >= 0 && x >= 0 && y >= 0 && z < state.Length && x < state[0].Length && y < state[0][0].Length) {
        return state[z][x][y];
      }
      return '.';
    }

    char ProcessCube(char cube, int x, int y, int z, char[][][] states) 
    {
      var activeCount = 0;
      foreach (var (xOff, yOff, zOff) in Neighbors) {
        if (GetCube(x + xOff, y + yOff, z + zOff, states) == '#') {
          activeCount++;
        } 
        if (activeCount > 3) {
          break;
        }
      }
      if (cube == '#' && (activeCount == 2 || activeCount == 3)) {
        return '#';
      }
      else if(cube == '.' && activeCount == 3) {
        return '#';
      }
      return '.';
    }

    char[][] Parse(string input) {
        return input.Split("\n").Select(s => s.ToCharArray()).ToArray();
    }
  }

}