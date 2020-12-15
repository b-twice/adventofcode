using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D11
{
  public class Solution: ISolution {
  
    public string Name {get;} = "Seating System";

    public long PartOne(string input) => ArrangeSeats(input, 4);
    public long PartTwo(string input) => ArrangeSeats(input, 5, true);


    (int, int)[] dirs = new []{(1, 0), (-1, 0), (0, 1), (0, -1),(1, 1), (-1, 1), (-1, -1), (1, -1)};

    int ArrangeSeats(string input, int seatLimit, bool lineOfSight=false) {
      var seatingArrangement = Parse(input);
      var occupiedSeats = 0;
      var runSeatingArrangement = true;
      while (runSeatingArrangement) {
        seatingArrangement = AssignSeats(seatingArrangement, Parse(input), seatLimit, lineOfSight);
        var seatCount = seatingArrangement.SelectMany(s => s.Where(c => c == '#')).Count();
        if (seatCount == occupiedSeats) {
          break;
        }
        else {
          occupiedSeats = seatCount;
        }
      }
      return occupiedSeats;
    }

    char[][] AssignSeats(char[][] currentGrid, char[][] newGrid, int seatLimit, bool lineOfSight) {
      for (int i = 0; i < currentGrid.Length; i++) {
        for (int j = 0; j < currentGrid[0].Length; j++) {
          if (currentGrid[i][j] == '.') continue;
            
          var occupiedSeats = 0;
          foreach (var dir in dirs) {
            var (drow, dcol) = dir;
            int ii = i + drow;
            int jj = j + dcol;
            while (ii >= 0 && jj >= 0 && ii < currentGrid.Length && jj < currentGrid[0].Length) {
              if (currentGrid[ii][jj] == '#') {
                occupiedSeats++;
                break;
              }
              else if (currentGrid[ii][jj] == 'L') {
                break;
              }
              else if (!lineOfSight) {
                break;
              }
              ii = ii + drow;
              jj = jj + dcol;
            }
          }
          if (occupiedSeats == 0) {
            newGrid[i][j] = '#';
          }
          else if (occupiedSeats >= seatLimit) {
            newGrid[i][j] = 'L';
          }
          else {
            newGrid[i][j] = currentGrid[i][j];
          }
        }
      }
      return newGrid;
    }


    char[][] Parse(string input) {
      return input.Split("\n").Select(s => s.ToCharArray()).ToArray();
    }
  }

}