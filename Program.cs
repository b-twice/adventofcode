using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = new AdventOfCode.Y2020.D01.Solution();
            Console.WriteLine($"D01 P1: {d1.PartOne()}");
            Console.WriteLine($"D01 P2: {d1.PartTwo()}");

            var d2 = new AdventOfCode.Y2020.D02.Solution();
            Console.WriteLine($"D02 P1: {d2.PartOne()}");
            Console.WriteLine($"D02 P2: {d2.PartTwo()}");
        }
    }
}
