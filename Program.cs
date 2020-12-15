using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t?.Namespace != null ? t.Namespace.StartsWith("AdventOfCode.Y2020") && t.Name == "Solution" : false)
                .OrderBy(x => x.Namespace);

            foreach (var t in types)
            {
                if (t.Namespace.StartsWith("AdventOfCode.Y2020.D07")) continue;

                var pathParts = t.Namespace.Split(".");
                var inputPath = pathParts.Skip(1).Take(2).Select(s => s.Replace("Y", "").Replace("D", "Day"));
                string input = Path.Combine(inputPath.First(), inputPath.Last(), "input.in");
                ISolution instance = (ISolution)Activator.CreateInstance(t);
                Run($"{pathParts.Skip(1).FirstOrDefault()} {pathParts.Last()} {instance.Name}", "1", instance.PartOne, input);
                Run($"{pathParts.Skip(1).FirstOrDefault()} {pathParts.Last()} {instance.Name}", "2", instance.PartTwo, input);
            }
        }
        static void Run(string name, string part, Func<string, long> method, string input) {
            Stopwatch sw = Stopwatch.StartNew();
            var result = method(System.IO.File.ReadAllText(input));
            sw.Stop();
            Console.WriteLine($"{name} P{part} in {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalSeconds}s) with result {result} ");
        }
    }

}
