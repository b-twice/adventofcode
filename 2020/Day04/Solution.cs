using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D04
{

  public record Passport {
    public string Id;
    public int IssueYear;
    public int ExpirationYear;
    public string CountryId;
    public string EyeColor;
    public string HairColor;
    public int BirthYear;
    public int Height;
    public string HeightUnit;
    public Passport(string id, int issueYear, int expirationYear, string countryId, string eyeColor, string hairColor, int birthYear, int height, string heightUnit)
      => (Id, IssueYear, ExpirationYear, CountryId, EyeColor, HairColor, BirthYear, Height, HeightUnit) = (id, issueYear, expirationYear, countryId, eyeColor, hairColor, birthYear, height, heightUnit);

     public void Deconstruct(out string id, out int issueYear, out int expirationYear, out string countryId, out string eyeColor, out string hairColor, out int birthYear, out int height, out string heightUnit)
      => (id, issueYear, expirationYear, countryId, eyeColor, hairColor, birthYear, height, heightUnit) = (Id, IssueYear, ExpirationYear, CountryId, EyeColor, HairColor, BirthYear, Height, HeightUnit);

 };
  public class Solution: ISolution {

    public string Name {get;} = "Passport Processing";

    public long PartOne(string input) => SolvePartOne(input, ValidatePassportPartOne);
    public long PartTwo(string input) => SolvePartTwo(input, ValidatePassportPartTwo);


    long SolvePartOne(string input, Func<Passport, bool> validator) {
      return Passports(input).Where(validator).Count();
    }
    long SolvePartTwo(string input, Func<Passport, bool> validator) {
      return Passports(input).Where(validator).Count();
    }
    
    static bool ValidatePassportPartOne(Passport passport) {
      var (id, issueYear, expirationYear, countryId, eyeColor, hairColor, birthYear, height, heightUnit) = passport;
      return 
        !String.IsNullOrEmpty(id) 
        && issueYear != Int32.MinValue
        && expirationYear != Int32.MinValue
        && !String.IsNullOrEmpty(eyeColor)
        && !String.IsNullOrEmpty(hairColor)
        && birthYear != Int32.MinValue
        && height != Int32.MinValue;
    }

    static bool ValidatePassportPartTwo(Passport passport) {
      var validId = ValidateId(passport) ;
      var validIssueYear = ValidateIssueYear(passport);
      var validateExpirationYear = ValidateExpirationYear(passport);
      var validEyeColor = ValidateEyeColor(passport);
      var validHairColor = ValidateHairColor(passport);
      var validBirthYear = ValidateBirthYear(passport);
      var validHeight =  ValidateHeight(passport);
      return validId && validIssueYear && validEyeColor && validateExpirationYear && validHairColor && validBirthYear && validHeight;
    }

    static bool ValidateIssueYear(Passport passport) => passport.IssueYear switch
    {
      >= 2010 and <= 2020 => true,
      _ => false
    };
    static bool ValidateBirthYear(Passport passport) => passport.BirthYear switch
    {
      >= 1920 and <= 2002 => true,
      _ => false
    };
    static bool ValidateExpirationYear(Passport passport) => passport.ExpirationYear switch
    {
      >= 2020 and <= 2030 => true,
      _ => false
    };
    static bool ValidateEyeColor(Passport passport) => 
      passport.EyeColor is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth";

    static bool ValidateHairColor(Passport passport) => 
      Regex.Match(passport.HairColor, @"^#[0-9a-f]{6}$").Success;


    static bool ValidateHeight(Passport passport) => 
      (passport.HeightUnit is "in") && (passport.Height is >= 59 and <= 76)
      || (passport.HeightUnit is "cm") && (passport.Height is >= 150 and <= 193);
      
    static bool ValidateId(Passport passport) => 
      Regex.Match(passport.Id, @"^[0-9]{9}$").Success;

    IEnumerable<Passport> Passports(string input) {
      return  (
        from line in input.Split("\n\n") 
          let parts = line.Split('\n')
          let attributes  = parts.Select(s => s.Split(" ")).SelectMany(s => s.Select(x => x.Split(":"))).ToArray()
          let passport  = CreatePassport(attributes)
        select passport);
    }

    Passport CreatePassport(string[][] attributes) {
      int  issueYear = Int32.MinValue;
      int expirationYear = Int32.MinValue;
      int height = Int32.MinValue;
      string heightUnit = String.Empty;
      string hairColor = String.Empty;
      int birthYear  = Int32.MinValue;
      string eyeColor = String.Empty;
      string passportId = String.Empty;
      string countryId = String.Empty;

      foreach(var attribute in attributes) {
        switch(attribute[0]) {
          case "iyr":
            Int32.TryParse(attribute[1], out issueYear);
            break;
          case "eyr":
            Int32.TryParse(attribute[1], out expirationYear);
            break;
          case "hgt":
            Int32.TryParse(Regex.Replace(attribute[1], @"[^\d]", ""), out height);
            heightUnit = Regex.Replace(attribute[1], @"[\d]", "");
            break;
          case "hcl":
            hairColor = attribute[1];
            break;
          case "ecl":
            eyeColor = attribute[1];
            break;
          case "pid":
            passportId = attribute[1];
            break;
          case "cid":
            countryId = attribute[1];
            break;
          case "byr":
            Int32.TryParse(attribute[1], out birthYear);
            break;
          default:
            break;
        }
      }
      return new Passport(passportId, issueYear, expirationYear, countryId, eyeColor, hairColor, birthYear, height, heightUnit);
    }
  }


}