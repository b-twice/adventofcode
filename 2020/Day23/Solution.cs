using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D23
{

  internal class Cup
  {
      public int Value { get; set; }
      public Cup Next { get; set; }
      
      public Cup(int value)
      {
          Value = value;
      }
  }

  public class Solution: ISolution {
  
    public string Name {get;} = "Crab Cups";

    public long PartOne(string input) => ReadCupsAfterOne(PlayCrabCups(Parse(input), 100).ToArray());
    public long PartTwo(string input) => ReadNextTwoCups(PlayCrabCups(MillionCups(Parse(input)), 10_000_000));

    int[] MillionCups(int[] cups) {
      var newCups = new int[1_000_000];
      for (var i = 0; i < 1_000_000; i++) {
        newCups[i] = i < cups.Length ? cups[i] : i + 1;
      }
      return newCups;
    }

    Cup[] MakeIndex(int[] cups, int max) {
      var idx = new Cup[max + 1];
      var cup = new Cup(cups.First());
      idx[cup.Value] = cup;
      foreach(var num in cups.Skip(1)) {
        idx[num] = new Cup(num);
        cup.Next = idx[num];
        cup = idx[num];
      }
      cup.Next = idx[cups.First()];
      return idx;
    }



    Cup[] PlayCrabCups(int[] cups, int rounds) {
      var max = cups.Max();
      var cupIdx = MakeIndex(cups, max);
      Func<int, int, int> subtractCup = (int value, int max) => (value - 1) <= 0 ? max : value - 1;

      var cup = cupIdx[cups.First()];
      for (var i = 1; i <= rounds;i++) {
        var c1 = cup.Next;
        var c2 = c1.Next;
        var c3 = c2.Next;
        var nextCup = c3.Next;
        var targetValue = subtractCup(cup.Value, max);
        while (targetValue == c1.Value || targetValue == c2.Value || targetValue == c3.Value) {
          targetValue = subtractCup(targetValue, max);
        }
        var targetCup = cupIdx[targetValue];
        c3.Next = targetCup.Next;
        targetCup.Next = c1;
        cup.Next = nextCup;
        cup = nextCup;
      }
      return cupIdx;
    }

    long ReadCupsAfterOne(Cup[] cups) {
      var cupsAfterOne = new int[cups.Length -2];
      var cup = cups[1].Next;
      var writeIdx = 0;
      while (cup.Value != 1) {
        cupsAfterOne[writeIdx] = cup.Value;
        cup = cup.Next;
        writeIdx++;
      }
      return Int64.Parse(string.Join("", cupsAfterOne));
    }

    long ReadNextTwoCups(Cup[] cups) {
      return (long)cups[1].Next.Value * (long)cups[1].Next.Next.Value;
    }

    int[] Parse(string input) {
      return (from c in input.ToCharArray()
          let i = Int32.Parse(c.ToString())
        select i
      ).ToArray();
    }
  }

}