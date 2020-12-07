using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D07 
{
  public class Solution {

    public record Bag (
      string Name,
      IEnumerable<BagHolding> Holds 
    );

    public record BagHolding (
      string Name,
      int Count 
    );

    public string GetName() => "Handy Haversacks";
    private string Input = "2020/Day07/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long SolvePartOne(string input) {
      var bags = Bags(input);
      var currentBag = bags.FirstOrDefault(b => b.Name == "shiny gold");
      var bagSet = new HashSet<string>();
      Queue <Bag> luggage = new Queue<Bag>();
      luggage.Enqueue(currentBag);
      while (luggage.Count > 0) {
        bagSet.Add(currentBag.Name);
        foreach (var bag in bags.Where(b => b.Holds.Any(c => c.Name == currentBag.Name))) {
          luggage.Enqueue(bag);
        }
        currentBag = luggage.Dequeue();
      }
      return bagSet.Count - 1;
    }

    long CountBags(IEnumerable<Bag> bags, Bag currentBag, long multiplier) {
      var total = currentBag.Holds.Select(b => b.Count).Sum() * multiplier;
      foreach (var hold in currentBag.Holds) { 
        total = total + CountBags(bags, bags.First(b => b.Name == hold.Name), hold.Count * multiplier);
      }
      return total;
    }

    long SolvePartTwo(string input) {
      var bags = Bags(input);
      var currentBag = bags.FirstOrDefault(b => b.Name == "shiny gold");
      return CountBags(bags, currentBag, 1);
    }



    IEnumerable<Bag> Bags(string input) {
      return (
        from line in input.Split("\n") 
          let bagContainerSplit = line.Split(" contain ")
          let primaryBag = bagContainerSplit.FirstOrDefault()
          let bags = bagContainerSplit.LastOrDefault() == "no other bags." ? Enumerable.Empty<BagHolding>() : bagContainerSplit.Skip(1).LastOrDefault()
            .Trim('.').Split(", ")
            .Select(b => b.Split(" "))
            .Select(b =>  new BagHolding(String.Join(" ", b.Skip(1).Take(2)), Int32.Parse(b.FirstOrDefault())))
          let bag = new Bag(primaryBag.Replace(" bags", ""), bags)
        select bag 
      );
    }

  }


}