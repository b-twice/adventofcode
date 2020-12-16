using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Example
{

  public class Solution: ISolution {
  
    public string Name {get;} = "";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      return 1;
    }

    long SolvePartTwo(string input) {
      return 1;
    }

    IEnumerable<string> Parse(string input) {
        return input.Split(",").AsEnumerable();
    }
  }

}