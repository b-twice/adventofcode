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
      throw new Exception("Number not found");
    }


    long SolvePartTwo(string input, long target) {
      var numbers = Numbers(input).TakeWhile(x => x != target).ToList();
      for (var i = numbers.Count - 2; i >= 0; i--) {
        var values = new List<long>();
        values.Add(numbers[i]);
        for (var j = i - 1; j >= 0; j--) {
          values.Add(numbers[j]);
          if (values.Sum() == target) {
            return values.Min() + values.Max();
          }
        }
      }
      return 1;
    }

    IEnumerable<long> Numbers(string input) {
      return (
        from line in input.Split("\n")
          let number = Int64.Parse(line)
        select number
      );
    }
    
  }


}