using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D10
{
  public class Solution {

    public string GetName() =>  "Adapter Array";
    private string Input = "2020/Day10/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long SolvePartOne(string input)  {
      var jolts = Parse(input);
      for (var i = 0; i < jolts.Count; i ++) {
        if (i == jolts.Count - 1) {
          jolts[i] = 0;
        }
        else {
          jolts[i] = jolts[i + 1] - jolts[i];
        }
      }
      var ones = jolts.Where(i => i == 1).Count();
      var threes = jolts.Where(i => i == 3).Count();
      return ones * threes;
    }


    long SolvePartTwo(string input)  {
      var jolts = Parse(input);
      var (a, b, c) = (1L, 0L, 0L); 
      for (var i = jolts.Count - 2; i >= 0; i--) {
          var s =  
              (i + 1 < jolts.Count && jolts[i + 1] - jolts[i] <= 3 ? a : 0) +
              (i + 2 < jolts.Count && jolts[i + 2] - jolts[i] <= 3 ? b : 0) +
              (i + 3 < jolts.Count && jolts[i + 3] - jolts[i] <= 3 ? c : 0);
          (a, b, c) = (s, a, b);
      }
      return a;
    }

    

    List<int> Parse(string input) {
      var numbers = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
      numbers.Insert(0, 0);
      numbers.Add(numbers.Last() + 3);
      return numbers;
    }

  }


}