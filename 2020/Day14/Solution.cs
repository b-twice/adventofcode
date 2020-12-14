using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.D14
{
  public class Solution {

    public string GetName() =>  "Docking Data";
    private string Input = "2020/Day14/input.in";

    public long PartOne() => SolvePartOne(System.IO.File.ReadAllText(Input));
    public long PartTwo() => SolvePartTwo(System.IO.File.ReadAllText(Input));


    long SolvePartOne(string input)  {
      Dictionary<long, long> mem = new Dictionary<long, long>(); 
      var programOps = Parse(input).ToList();
      foreach (var (mask, ops) in programOps) {
        var andMask = Convert.ToInt64(mask.Replace('X', '1'), 2);
        var orMask = Convert.ToInt64(mask.Replace('X', '0'), 2);
        foreach(var (address, value) in ops) {
          mem[address] = value & andMask | orMask;
        }
      }
      return mem.Sum(x => x.Value);
    }

    long SolvePartTwo(string input)  {
      Dictionary<long, long> mem = new Dictionary<long, long>(); 
      var programOps = Parse(input).ToList();
      foreach (var (mask, ops) in programOps) {
        var andMask = Convert.ToInt64(mask.Replace('0', '1').Replace('X', '0'), 2);
        foreach(var (address, value) in ops) {
          var floats = mask.Select((bit, idx) => (bit, idx)).Where(item => item.bit == 'X');
          var size = Math.Pow(2, floats.Count());
          long addressValue = address & andMask; // keep only 1s not masked by x
          for (var i = 0L; i < size; i++) {
            // set all Xs in address to shifted mask
            mem[addressValue | ProcessAddressMask(mask, i)] = value;
          }
        }
      }
      return mem.Values.Sum();
    }


    long ProcessAddressMask(string mask, long shift) {
      var binary = new System.Text.StringBuilder(mask.PadLeft(36, '0'));
      var floats = new Stack<(char bit, int idx)>(mask.Select((bit, idx) => (bit, idx)).Where(item => item.bit == 'X'));
      while(floats.Count > 0) {
        var f = floats.Pop();
        binary[f.idx] = (shift & 1).ToString().ToCharArray()[0];
        shift = shift >> 1;

      }
      return Convert.ToInt64(binary.ToString(), 2);
    }
 
    IEnumerable<(string mask, (long address, long value)[] ops)> Parse(string input) {
        var lines = input.Split("mask = ").Skip(1).Select(l => l.Split("\n")).ToArray();
        return (from line in lines
            let mask = line.FirstOrDefault()
            let opLines = line.Skip(1).Where(x => x != String.Empty).Select(l => l.Split(" = ")).ToArray()
            let ops = opLines.Select(o => 
              (Int64.Parse(Regex.Replace(o[0], @"[^\d]", "")), Int64.Parse(o[1]))
            ).ToArray()
          select (mask, ops)
        );
    }
  }

}