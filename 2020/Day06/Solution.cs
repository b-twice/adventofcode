using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D06 
{
  public class Solution: ISolution {

    public string Name {get;} = "Custom Customs";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

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