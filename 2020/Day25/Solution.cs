using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D25
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Combo Breaker";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      var publicKeys = Parse(input);
      var k1LoopSize = FindLoopSize(publicKeys.k1, 7);
      var k2LoopSize = FindLoopSize(publicKeys.k2, 7);
      var k2Key = FindKey(k1LoopSize,publicKeys.k2);
      var k1Key = FindKey(k2LoopSize,publicKeys.k1);
      return k2Key;
    }

    long SolvePartTwo(string input) {
      return 0;
    }

    int FindLoopSize(long key, long subjectNumber) {
      var value = 1L;
      var loopSize = 0;
      while (value != key) {
        value = (value * subjectNumber) % 20201227;
        loopSize++;
      }
      return loopSize;
    }

    long FindKey(int loopSize, long subjectNumber) {
      var value = 1L;
      while (loopSize > 0) {
        value = (value * subjectNumber) % 20201227;
        loopSize--;
      }
      return value;
    }

    (long k1, long k2) Parse(string input) {
        var keys = input.Split("\n").Select(Int64.Parse);
        return (keys.First(), keys.Last());
    }
  }

}