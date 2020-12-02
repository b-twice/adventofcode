using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D01 
{
  public class Solution {

    public string GetName() => "Report Repair";
    private int Total = 2020;
    private string Input = "2020/Day01/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long SolvePartOne(string input) {
      List<int> numbers = Numbers(input).ToList();
      IEnumerable<int> values = numbers.Where(x => numbers.Contains(Total - x));
      return values.Aggregate(1, (acc, t) => acc * t);
    }
    long SolvePartTwo(string input) {
      List<int> numbers = Numbers(input).ToList();
      var values = new HashSet<int>();
      foreach(var x in numbers) {
        foreach(var y in numbers) {
          if (numbers.Contains(Total - (x+y))) {
             values.Add(y);
          }
        }
      }
      return values.Aggregate(1, (acc, t) => acc * t);
    }



    IEnumerable<int> Numbers(string input) {
      return input.Split('\n').Select(int.Parse);
    }

  }


}