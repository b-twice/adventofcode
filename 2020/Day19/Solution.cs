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
      Rules = ParseRules(input);
      var firstRule = BuildRule(Rules["42"]);
      var secondRule = BuildRule(Rules["31"]);
      // regex https://weblogs.asp.net/whaggard/377025
      var rule0 = Rules["0"]
          .Replace("8", $"(?:{firstRule})+")
          .Replace("11", $"(?<k>{firstRule})+(?<-k>{secondRule})+(?(k)(?!))");

      var rule0Regex = new Regex($"^{rule0}$", RegexOptions.IgnorePatternWhitespace);
      
      return ParseMessages(input).Count(rule0Regex.IsMatch);

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