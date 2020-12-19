using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D19
{

  public class Solution: ISolution {
  
    public string Name {get;} = "Monster Messages";

    public long PartOne(string input) => SolvePartOne(input);
    public long PartTwo(string input) => SolvePartTwo(input);

    public Dictionary<string, string> Rules {get; set;}

    string BuildRule(string rule) {
      var pattern = new Regex(@"\b(\d+)\b");
      var matches = pattern.Matches(rule);
      while (matches.Count > 0) {
        rule = pattern.Replace(rule, new MatchEvaluator(ReplaceRule));
        matches = pattern.Matches(rule);
      }
      return rule.Replace(" ", "");
    }

    public string ReplaceRule(Match m) {
      return Rules[m.Value];
    }


    long SolvePartOne(string input) {
      var messages = ParseMessages(input);
      Rules = ParseRules(input);
      var rule = BuildRule(Rules["0"]);
      return messages.Where(s => Regex.Match(s, $"^{rule}$").Success).Count();
    }

    long SolvePartTwo(string input) {
      var messages = ParseMessages(input);
      Rules = ParseRules(input);
      Rules["8"] = "42";
      Rules["11"] = "31";
      var firstRule = BuildRule(Rules["42"]);
      var secondRule = BuildRule(Rules["31"]);
      var startsPattern = new Regex($"^({firstRule})");
      var endsPattern = new Regex($"({secondRule})$");
      var firstPattern = new Regex($"({firstRule})");
      var secondPattern = new Regex($"({secondRule})");
      var count = 0;

      foreach (var message in messages) {
        var start = startsPattern.Match(message);
        var end = endsPattern.Match(message);
        if (start.Success && end.Success) {
          var sub = message.Substring(start.Length, message.Length - end.Length - end.Length);
          var (first, firstLength) = CountConsecutiveMatches(firstPattern.Matches(sub));
          var (second, secondLength) = CountConsecutiveMatches(secondPattern.Matches(ReverseEx(sub)));
          if (firstLength + secondLength != sub.Length) continue;
          if (first > 0 && first >= second) count++;
        };
      }
      return count;
    }

    (int, int) CountConsecutiveMatches(MatchCollection matches) {
      var count = 0;
      var idx = 0;
      var length = 0;
      foreach(Match match in matches) 
      {
        if (match.Index != idx) {
          break;
        }
        idx = idx + match.Length;
        count++;
        length = length + match.Length;
      }
      return (count, length);

    }

    private string ReverseEx(string str)
    {
        char[] chrArray = str.ToCharArray();
        int len = chrArray.Length - 1;
        char rev = 'n';
        for (int i = 0; i <= len/2; i++)
        {
            rev = chrArray[i];
            chrArray[i] = chrArray[len - i];
            chrArray[len - i] = rev;
        }
        return new string(chrArray);
    }

    IEnumerable<string> ParseMessages(string input) {
      return input.Split("\n\n").Last().Split("\n");
    }

    Dictionary<string, string> ParseRules(string input) {
      var map = new Dictionary<string,string>();
      var rules = (
        from rule in input.Split("\n\n").First().Split("\n")
          let parts = rule.Split(": ")
          let key = parts.First()
          let pattern = parts.Last()
        select (key, pattern)
      );
      foreach (var rule in rules) {
        map.Add(rule.key, 
          rule.pattern.Contains("|") ? $"({rule.pattern})" : 
            (rule.pattern == "\"a\"" || rule.pattern == "\"b\"" ? rule.pattern.Replace("\"", "") : rule.pattern));
      }
      return map;
    }
  }

}