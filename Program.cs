using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = new AdventOfCode.Y2020.D01.Solution();
            Console.WriteLine($"{d1.GetName()} P1: {d1.PartOne()}");
            Console.WriteLine($"{d1.GetName()} P2: {d1.PartTwo()}");

            var d2 = new AdventOfCode.Y2020.D02.Solution();
            Console.WriteLine($"{d2.GetName()} P1: {d2.PartOne()}");
            Console.WriteLine($"{d2.GetName()} P2: {d2.PartTwo()}");

            var d3 = new AdventOfCode.Y2020.D03.Solution();
            Console.WriteLine($"{d3.GetName()} P1: {d3.PartOne()}");
            Console.WriteLine($"{d3.GetName()} P2: {d3.PartTwo()}");

            var d4 = new AdventOfCode.Y2020.D04.Solution();
            Console.WriteLine($"{d4.GetName()} P1: {d4.PartOne()}");
            Console.WriteLine($"{d4.GetName()} P2: {d4.PartTwo()}");

            var d5 = new AdventOfCode.Y2020.D05.Solution();
            Console.WriteLine($"{d5.GetName()} P1: {d5.PartOne()}");
            Console.WriteLine($"{d5.GetName()} P2: {d5.PartTwo()}");

            var d6 = new AdventOfCode.Y2020.D06.Solution();
            Console.WriteLine($"{d6.GetName()} P1: {d6.PartOne()}");
            Console.WriteLine($"{d6.GetName()} P2: {d6.PartTwo()}");

            var d7 = new AdventOfCode.Y2020.D07.Solution();
            Console.WriteLine($"{d7.GetName()} P1: {d7.PartOne()}");
            Console.WriteLine($"{d7.GetName()} P2: {d7.PartTwo()}");
        }
    }
}
