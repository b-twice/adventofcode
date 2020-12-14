using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Numerics;

namespace AdventOfCode.Y2020.D13
{
  public class Solution {

    public string GetName() =>  "Shuttle Search";
    private string Input = "2020/Day13/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long CalculateEarliestDeparture(long departureTime, long bus) {
      return departureTime + (bus - (departureTime % bus));
    }

    long SolvePartOne(string input)  {
      var time = EarliestTime(input);
      var (earliestTime, busId) = DepartureTimes(input)
        .Aggregate(
          (Time: Int64.MaxValue, Id: 0),
          (result, next) => CalculateEarliestDeparture(time, next.BusId) < result.Time ? (CalculateEarliestDeparture(time, next.BusId), next.BusId) : result
        );
      return (earliestTime - time) * busId;
    }

    long SolvePartTwo(string input)  {
      return ChineseRemainderTheorem(
        DepartureTimes(input).Select(bus => (mod: (long)bus.BusId, a: (long)bus.BusId - bus.Index)).ToArray()
      );
    }

    int EarliestTime(string input) {
      return Int32.Parse(input.Split("\n").First());
    }
    IEnumerable<(int Index, int BusId)> DepartureTimes(string input) {
      return input.Split("\n")
        .Last().Split(",")
        .Select((c, index) => (Index:index, BusId:c == "x" ? 0 : Int32.Parse(c)))
        .Where(c => c.BusId != 0);
    }

    // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
    long ChineseRemainderTheorem((long mod, long a)[] items) {
        var prod = items.Aggregate(1L, (acc, item) => acc * item.mod);
        var sum = items.Select((item, i) => {
            var p = prod / item.mod;
            return item.a * ModInv(p, item.mod) * p;
        }).Sum();

        return sum % prod;
    }

    long ModInv(long a, long m) => (long)BigInteger.ModPow(a, m - 2, m);

  }
}