using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D22
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Crab Combat";

    public long PartOne(string input) => PlayCrabCombat(Parse(input));
    public long PartTwo(string input) => PlayRecursiveCombat(Parse(input)).Score;

    long PlayCrabCombat((int[] p1, int[] p2) players) {
      var p1Deck = new Queue<int>(players.p1); 
      var p2Deck = new Queue<int>(players.p2); 
      while (p1Deck.Count() > 0 && p2Deck.Count() > 0) {
        var firstCard = p1Deck.Dequeue();
        var secondCard = p2Deck.Dequeue();
        if (firstCard > secondCard) {
          p1Deck.Enqueue(firstCard);
          p1Deck.Enqueue(secondCard);
        }
        else {
          p2Deck.Enqueue(secondCard);
          p2Deck.Enqueue(firstCard);
        }
      }
      return p1Deck.Count() > 0 ? SumDeck(p1Deck) : SumDeck(p2Deck);
    }

    (string Winner, long Score) PlayRecursiveCombat((int[] p1, int[] p2) players) {
      var p1Deck = new Queue<int>(players.p1); 
      var p2Deck = new Queue<int>(players.p2); 
      var previousDecks = new HashSet<string>();
      while (p1Deck.Any() && p2Deck.Any()) {
        // instant winner 
        var deckHash = $"{string.Join(",", p1Deck)};{String.Join(",", p2Deck)}";
        if (previousDecks.Contains(deckHash)) {
          return ("p1", SumDeck(p1Deck));
        }
        previousDecks.Add(deckHash);

        var firstCard = p1Deck.Dequeue();
        var secondCard = p2Deck.Dequeue();
        if (p1Deck.Count >= firstCard && p2Deck.Count >= secondCard) {
          var (winner, score) = PlayRecursiveCombat((p1:new Queue<int>(p1Deck.Take(firstCard)).ToArray(), p2:new Queue<int>(p2Deck.Take(secondCard)).ToArray()));
          if (winner == "p1") {
            p1Deck.Enqueue(firstCard);
            p1Deck.Enqueue(secondCard);
          }
          else {
            p2Deck.Enqueue(secondCard);
            p2Deck.Enqueue(firstCard);
          }
        }
        else if (firstCard > secondCard) {
          p1Deck.Enqueue(firstCard);
          p1Deck.Enqueue(secondCard);
        }
        else {
          p2Deck.Enqueue(secondCard);
          p2Deck.Enqueue(firstCard);
        }
      }
      return p1Deck.Count() > 0 ? ("p1", SumDeck(p1Deck)) : ("p2", SumDeck(p2Deck));
    }

    long SumDeck(Queue<int> deck) => 
      deck.Select((card, index) => (card, index: deck.Count - index))
          .Aggregate(0, (total, item) => total + (item.card * item.index));

    (int[] p1, int[] p2) Parse(string input) {
        var parts = from line in input.Split("\n\n") select line;
        var p1 = parts.First().Split('\n').Skip(1).Select(Int32.Parse).ToArray();
        var p2 = parts.Last().Split('\n').Skip(1).Select(Int32.Parse).ToArray();
        return (p1, p2);
    }
  }

}