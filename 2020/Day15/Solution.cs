using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D15
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Rambunctious Recitation";

    public long PartOne(string input) => PlayGame(input, 2020);
    public long PartTwo(string input) => PlayGame(input, 30000000);




    long PlayGame(string input, int numbersSeen)  {
      var startingNumbers = Parse(input);
      Dictionary<int, int> mem = startingNumbers
        .Select((num, index) => (num, index:index + 1))
        .ToDictionary(item => item.num, item => item.index);
      var lastNumberSpoken = startingNumbers.LastOrDefault();
      var seenLastNumber = startingNumbers.Where(s => s == lastNumberSpoken).Count() > 1;
      var currentNumberSpoken = 0;
      for (var i = startingNumbers.Count() + 1; i <= numbersSeen; i++) {
        if (!seenLastNumber) {
          currentNumberSpoken = 0;
          if (mem.ContainsKey(currentNumberSpoken)) {
            seenLastNumber = true;
          }
          else {
            mem[currentNumberSpoken] = i;
          }
        }
        else {
          currentNumberSpoken = i - 1 - mem[lastNumberSpoken];
          mem[lastNumberSpoken] = i - 1;
          if (!mem.ContainsKey(currentNumberSpoken)) {
            mem[currentNumberSpoken] = i;
            seenLastNumber = false;
          }
          else {
            seenLastNumber = true;
          }
        }
        lastNumberSpoken = currentNumberSpoken;
      }
      return lastNumberSpoken;
    }

    IEnumerable<int> Parse(string input) {
        return input.Split(",").Select(x => Int32.Parse(x));
    }
  }

}