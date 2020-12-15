using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D10
{
  public class Solution: ISolution {
  
    public string Name {get;} = "Adapter Array";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);


    public Dictionary<long, long> Cache = new Dictionary<long, long>(){{0L, 1L}}; 

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
      foreach (var jolt in jolts.Skip(1)) {
        Cache[jolt] = 
          (Cache.ContainsKey(jolt - 1) ? Cache[jolt - 1] : 0) + 
          (Cache.ContainsKey(jolt - 2) ? Cache[jolt - 2] : 0) + 
          (Cache.ContainsKey(jolt - 3) ? Cache[jolt - 3] : 0);
      }
      return Cache[jolts.Last()];
    }

    

    List<int> Parse(string input) {
      var numbers = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
      numbers.Insert(0, 0);
      numbers.Add(numbers.Last() + 3);
      return numbers;
    }

  }


}