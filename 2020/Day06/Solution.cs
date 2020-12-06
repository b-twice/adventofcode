using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D06 
{
  public class Solution {

    public string GetName() => "Custom Customs";
    private string Input = "2020/Day06/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long SolvePartOne(string input) {
      return GroupAnswers(input).Sum(group => group.SelectMany(c => c).Distinct().Count());
    }

    long SolvePartTwo(string input) {
      return GroupAnswers(input).Sum(group => 
        group.Aggregate((answers, next) => answers.Intersect(next).ToArray())
          .Count()
      );
    }



    IEnumerable<char[][]> GroupAnswers(string input) {
      return  (
        from line in input.Split("\n\n") 
          let answers = line.Split('\n')
          let attributes  = answers.Select(s => s.ToCharArray()).ToArray()
        select attributes
      );
    }

  }


}