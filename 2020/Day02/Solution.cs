using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode.Y2020.D02
{
  public record PasswordRecord (
    char Character,
    int Minimum,
    int Maximum,
    string Password
  );

  public class Solution {


    public string GetName() => "Password Philosophy";
    private string Input = "2020/Day02/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));

    long SolvePartOne(string input) {
      var records = Records(input).Where(r => ValidatePasswordRecordV1(r));
      return records.Count();
    }
    long SolvePartTwo(string input) {
      var records = Records(input).Where(r => ValidatePasswordRecordV2(r));
      return records.Count();
    }

    bool ValidatePasswordRecordV1(PasswordRecord record) {
      var characterCount = record.Password.Count(c => c == record.Character);
      return characterCount > 0 && record.Minimum <= characterCount && characterCount <= record.Maximum;
    }

    bool ValidatePasswordRecordV2(PasswordRecord record) {
      // xor - char can only be in 1 position
      return (record.Password[record.Minimum - 1] == record.Character) ^ (record.Password[record.Maximum - 1] == record.Character);
    }

    IEnumerable<PasswordRecord> Records(string input) {
      return 
      (from line in input.Split('\n')
          let parts = line.Split(' ')
          let range = parts[0].Split('-').Select(int.Parse).ToArray()
          let ch = parts[1][0]
          let pr = new PasswordRecord(ch, range[0], range[1], parts[2])
        select pr
      );
    }
  }


}