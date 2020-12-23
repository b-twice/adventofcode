using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D23
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Crab Cups";

    public long PartOne(string input) => ReadCupsAfterOne(PlayCrabCups(Parse(input), 100));
    public long PartTwo(string input) => PlayCrabCups(MillionCups(Parse(input)), 10000000).Count();

    int[] MillionCups(int[] cups) {
      var newCups = new int[1000000];
      for (var i = 0; i < 1000000; i++) {
        newCups[i] = i < cups.Length ? cups[i] : i + 1;
      }
      return newCups;
    }

    int[] PlayCrabCups(int[] cups, int rounds) {
      Func<int,int> mod = (int x) => (x%cups.Length + cups.Length)%cups.Length;
      Func<int, int> nextCup = (int cup) => cup - 1 >= cups.Min() ? cup - 1 : cups.Max();
      Func<int, int> getCup = (int idx) => cups[mod(idx)];
      var currentCupIdx = 0;
      for (var r = 1; r <= rounds; r++) {
        var destinationCup = nextCup(getCup(currentCupIdx));
        var pickedUpCups = new int[3]{getCup(currentCupIdx+1), getCup(currentCupIdx+2), getCup(currentCupIdx+3)};
        // find destination cup
        while (pickedUpCups.Contains(destinationCup)) {
          destinationCup = nextCup(destinationCup);
        }
        // get all cups between pickedUp cups and destination
        var nextCups = new Queue<int>();
        var readIdx = currentCupIdx+4;
        while (!nextCups.Contains(destinationCup)) {
          nextCups.Enqueue(getCup(readIdx));
          readIdx++;
        }
        foreach(var cup in pickedUpCups) {
          nextCups.Enqueue(cup);
        }
        // move cups around
        var writeIdx = currentCupIdx+1; 
        while (nextCups.Any()) {
          var cup = nextCups.Dequeue();
          cups[mod(writeIdx)] = cup;
          writeIdx++;
        }
        currentCupIdx++;
      }
      return cups;
    }

    long ReadCupsAfterOne(int[] cups) {
      Func<int,int> mod = (int x) => (x%cups.Length + cups.Length)%cups.Length;
      var cupsAfterOne = new int[cups.Length -1];
      var readIdx = Array.IndexOf(cups, 1) + 1;
      var writeIdx = 0;
      while(readIdx < (Array.IndexOf(cups, 1) + cups.Length)) {
        cupsAfterOne[writeIdx] = cups[mod(readIdx)];
        readIdx++;
        writeIdx++;
      }
      return Int64.Parse(string.Join("", cupsAfterOne));
    }

    int[] Parse(string input) {
      return (from c in input.ToCharArray()
          let i = Int32.Parse(c.ToString())
        select i
      ).ToArray();
    }
  }

}