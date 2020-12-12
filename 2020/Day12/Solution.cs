using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D12
{
  public class Solution {

    public record Vessel(int x, int y, char dir);

    public string GetName() =>  "Rain Risk";
    private string Input = "2020/Day12/input.in";

    public long PartOne() => DistanceOfShipNavigation(System.IO.File.ReadAllText(Input), false);
    public long PartTwo() => DistanceOfShipNavigation(System.IO.File.ReadAllText(Input), true);

    long DistanceOfShipNavigation(string input, bool hasWaypoint) {
      var instructions = Parse(input);
      var vessel = Navigate(instructions, hasWaypoint);
      return Math.Abs(vessel.x) + Math.Abs(vessel.y);
    }

    Vessel Navigate(IEnumerable<(char action, int value)> instructions, bool hasWaypoint)  {
      var vessel = new Vessel(0, 0, 'E');
      var waypoint = new Vessel(10, 1, 'N');
      foreach((char action, int value) in instructions) {
        if (hasWaypoint) {
          switch (action) {
            case 'L' or 'R':
              waypoint = TurnWaypoint(waypoint, (value, action));
              break;
            case 'F':
              vessel = new Vessel(vessel.x + value * waypoint.x, vessel.y + value * waypoint.y, vessel.dir);
              break;
            default:
              waypoint = Move(waypoint, action, value);
              break;
          }
        }
        else {
          vessel = action switch {
            'L' or 'R' => Turn(vessel, value, action),
            'F' => Move(vessel, vessel.dir, value),
            _ => Move(vessel, action, value)
          };
        }
      }
      return vessel;
    }

    Vessel Move(Vessel v, char direction, int value) => direction switch 
    {
      'N' => new Vessel(v.x, v.y + value, v.dir),
      'E' => new Vessel(v.x + value, v.y, v.dir),
      'S' => new Vessel(v.x, v.y - value, v.dir),
      'W' => new Vessel(v.x - value, v.y, v.dir),
      _ => new Vessel(v.x, v.y, v.dir)
    };

    Vessel Turn(Vessel v, int degree, char action) 
    {
      char dir = Rotate(new char[]{'N', 'E', 'S', 'W'}, v.dir, degree, action == 'L');
      return new Vessel(v.x, v.y, dir);
    }

    char Rotate(char[] directions, char dir, int degree, bool left=false) 
    {
      Func<int, int, int> mod = (int x, int m) => (x%m + m)%m;
      var idx = Array.IndexOf(directions, dir);
      var i = left ? -1 : 1;
      var turn = mod((idx + (i * degree / 90)), 4);
      return directions[turn];
    }

    Vessel TurnWaypoint(Vessel v, (int, char) turn) 
    {
      (int x, int y) = turn switch {
        (90, 'L') or (270, 'R') => (-v.y, v.x),
        (270, 'L') or (90, 'R') => (v.y, -v.x),
        (180, 'L') or (180, 'R') => (-v.x, -v.y),
        _ => (v.x, v.y)
      };
      return new Vessel(x, y, v.dir);
    }

    IEnumerable<(char action, int value)> Parse(string input) {
      return (
        from line in input.Split("\n")
          let data = (action: line[0], value: Int32.Parse(Regex.Replace(line, @"[^\d]", "")))
        select data
      );
    }
  }

}