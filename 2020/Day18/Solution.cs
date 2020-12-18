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
      var expressions = Parse(input);
      return expressions.Select(CalculateExpression).Sum();

    }

    long SolvePartTwo(string input) {
      var expressions = Parse(input);
      return expressions.Select(CalculateExpression).Sum();
    }


    long CalculateExpression(string expression) {
      var tokens = ScanExpression(expression);
      var (total, idx) = Evaluate(tokens.ToList());
      return total;
    }

    (long total, int idx) Evaluate(IList<Token> tokens) {
      var total = 0L;
      var op = TokenType.PLUS;
      var idx = 0;
      while (idx < tokens.Count()) {
        var token = tokens[idx];
        var value = 0L;
        if (token.type == TokenType.LEFT_PAREN) {
          var (result, shift) = Evaluate(tokens.Skip(idx + 1).Take(FindTerminatingScope(tokens, idx) - idx - 1).ToList());
          idx = idx + shift + 2;
          value = result;
        }
        else if (token.type == TokenType.NUMBER) {
          value = token.number.Value;
          idx++;
        }
        else {
          throw new Exception("Invalid");
        }
        total = op switch {
          TokenType.PLUS => total + value,
          TokenType.STAR => total * value
        };
        if (idx < tokens.Count) {
          op = tokens[idx].type;
          idx++;
        }
      }
      return (total, idx);
    }

    int FindTerminatingScope(IList<Token> tokens, int idx) {
      var scopes = 1;
      while (scopes > 0) {
        idx++;
        scopes = tokens[idx].type switch {
          TokenType.RIGHT_PAREN => scopes - 1,
          TokenType.LEFT_PAREN => scopes + 1,
          _ => scopes
        };
      }
      return idx;
    }



    IEnumerable<Token> ScanExpression(string expression) {
      IList<Token> tokens = new List<Token>();
      var idx = 0;
      while (idx < expression.Length) {
        var c = expression.ElementAt(idx);
        switch(c) {
          case '(': tokens.Add(new Token(TokenType.LEFT_PAREN, c, null)); break;
          case ')': tokens.Add(new Token(TokenType.RIGHT_PAREN, c, null)); break;
          case '+': tokens.Add(new Token(TokenType.PLUS, c, null)); break;
          case '*': tokens.Add(new Token(TokenType.STAR, c, null)); break;
          case ' ': break;
          default: 
            if (isDigit(c)) {
              var start = idx;
              while (true) {
                if (idx + 1 < expression.Length && isDigit(expression.ElementAt(idx +1))) {
                  idx++;
                  continue;
                }
                break;
              }
              var number = Int32.Parse(expression.Substring(start, start + 1 - idx));
              tokens.Add(new Token(TokenType.NUMBER, null, number));
            }
            break;
        }
        idx++;
      }
      return tokens.AsEnumerable();
    }

    private bool isDigit(char c) => c >= '0' && c <= '9';   

    IEnumerable<string> Parse(string input) {
        return input.Split("\n").AsEnumerable();
    }


  }

}