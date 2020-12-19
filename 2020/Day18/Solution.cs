using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.D18
{

  public enum TokenType {
    LEFT_PAREN, RIGHT_PAREN, STAR, PLUS, NUMBER
  }

  public record Token(TokenType type, char? lexeme, long? number);

  public class Solution: ISolution {
  
    public string Name {get;} = "";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    long SolvePartOne(string input) {
      var statements = Parse(input);
      return statements.Select(s => Solve(s, true)).Sum();

    }

    long SolvePartTwo(string input) {
      var statements = Parse(input);
      return statements.Select(s => Solve(s, false)).Sum();
    }

    long Solve(string stmt, bool part1) {
      // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
      var opStack = new Stack<char>();
      var valStack = new Stack<long>();
      void evalUntil(string ops) {
        while (!ops.Contains(opStack.Peek())) {
          if (opStack.Pop() == '+') {
            valStack.Push(valStack.Pop() + valStack.Pop());
          } else {
            valStack.Push(valStack.Pop() * valStack.Pop());
          }
        }
      }

      opStack.Push('(');

      foreach (var ch in stmt) {
        switch (ch) {
          case ' ': break;
          case '*': evalUntil("("); opStack.Push('*'); break;
          case '+': evalUntil(part1 ? "(" : "(*"); opStack.Push('+'); break;
          case '(': opStack.Push('('); break;
          case ')': evalUntil("("); opStack.Pop(); break;
          default:
            valStack.Push(long.Parse(ch.ToString()));
            break;
        }
      }
      evalUntil("(");
      return valStack.Single();

    }

    IEnumerable<string> Parse(string input) {
        return input.Split("\n").AsEnumerable();
    }
  }

}