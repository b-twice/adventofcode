using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D16
{
  public record Range(int min, int max);

  public record TicketRule(string name, IList<Range> ranges);

  public class Solution: ISolution {
  
    public string Name {get;} = "Ticket Translation";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      var ticketRules = TicketRules(input);
      var otherTickets = NearbyTickets(input);
      var invalidTicketFields = new List<int>();
      foreach (var ticket in otherTickets) {
        var (valid, num) = IsTicketValid(ticket, ticketRules);
        if (!valid) {
          invalidTicketFields.Add(num);
        }
      }
      return invalidTicketFields.Sum();
    }

    long SolvePartTwo(string input) {
      var ticketRules = new HashSet<TicketRule>(TicketRules(input));
      var nearbyTickets = NearbyTickets(input).Where(t => IsTicketValid(t, ticketRules).valid).ToList();
      var myTicket = MyTicket(input);
      nearbyTickets.Add(myTicket);
      long total = 1L;
      var columns = Enumerable.Range(0, nearbyTickets.First().Count()).ToHashSet();
      while (ticketRules.Any()) {
        foreach (var column in columns) {
          var fields = nearbyTickets.Select(s => s[column]);
          var validTicketRules = ticketRules.Where(t => AreFieldsValid(fields, t));
          if (validTicketRules.Count() == 1) {
            var ticketRule = validTicketRules.First();
            ticketRules.Remove(ticketRule);
            columns.Remove(column);
            if (ticketRule.name.StartsWith("departure")) {
              total = total * myTicket[column];
            }
            break;
          }
        }
      }
      return total;
    }

    (bool valid, int num) IsTicketValid(int[] ticket, IEnumerable<TicketRule> ticketRules) {
      foreach (var field in ticket) {
        var valid = false;
        foreach(var ticketRule in ticketRules) {
          valid = IsFieldValid(field, ticketRule);
          if (valid) {
            break;
          }
        }
        if (!valid) {
          return (false, field);
        }
      }
      return (true, 0);
    }

    bool AreFieldsValid(IEnumerable<int> nums, TicketRule rule) => nums.All(num => IsFieldValid(num, rule));
    bool IsFieldValid(int num, TicketRule rule) => rule.ranges.Any(r => InTicketRange(num, r));
    bool InTicketRange(int num, Range range) => num >= range.min && num <= range.max;


    IEnumerable<int[]> NearbyTickets(string input) {
        return (
          from line in input.Split("\n\n").Last().Split("\n").Skip(1).Select(x => x.Split(","))
          let nums = line.Select(x => Int32.Parse(x)).ToArray()
          select nums
        );
    }
    int[] MyTicket(string input) {
        return (
          from line in input.Split("\n\n").Skip(1).First().Split("\n").Skip(1).Select(x => x.Split(","))
          let nums = line.Select(x => Int32.Parse(x)).ToArray()
          select nums
        ).First();
    }

    IEnumerable<TicketRule> TicketRules(string input) {
        return (
          from line in input.Split("\n\n").First().Split("\n")
            let parts = line.Split(": ")
            let name = parts.First()
            let ranges = parts.Last().Split(" or ")
            let firstRange = ranges.First().Split("-").Select(x => Int32.Parse(x))
            let secondRange = ranges.Last().Split("-").Select(x => Int32.Parse(x))
            let ticketRule = new TicketRule(name, new List<Range>(){new Range(firstRange.First(), firstRange.Last()), new Range(secondRange.First(), secondRange.Last())})
          select ticketRule
        );
    }


  }

}