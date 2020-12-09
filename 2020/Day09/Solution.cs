using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D09 
{
  public class Solution {

    public string GetName() => "Encoding Error";
    private string Input = "2020/Day09/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input), 1639024365);

    long SolvePartOne(string input) {
      var numbers = Numbers(input).ToList();
      var slice = new Queue<long>(numbers.Take(25));
      var size = numbers.Count;
      foreach(var target in numbers.Skip(25)) {
        var found = false;
        foreach (var current in slice) {
          if (current + current == target || slice.Contains(target - current)) {
            found = true;
            break;
          }
        }
        if (!found) {
          return target;
        }
        slice.Dequeue();
        slice.Enqueue(target);
      }
      throw new Exception("Not found");
    }


    long SolvePartTwo(string input, long target) {
      var numbers = Numbers(input).TakeWhile(x => x != target).SkipLast(1).ToList();
      var sums = CumulativeSum(numbers).ToList();
      for (var i = sums.Count() - 1; i >= 0; i--) {
        var sum = sums[i];
        for (var j = i - 1; j >= 0; j--) {
          if (sum - sums[j] == target) {
            var range = numbers.GetRange(j + 1, i - j + 1);
            return range.Min() + range.Max();
          }
        }
      }
      throw new Exception("Not found");
    }

    IEnumerable<long> Numbers(string input) {
      return (
        from line in input.Split("\n")
          let number = Int64.Parse(line)
        select number
      );
    }

    IEnumerable<long> CumulativeSum(IEnumerable<long> sequence)
    {
      long sum = 0;
      foreach(var item in sequence)
      {
          sum += item;
          yield return sum;
      }        
    }
    
  }


}