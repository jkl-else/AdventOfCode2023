using System.Diagnostics;
using ConsoleApp.Callendar.D09; //<-- Change this to your namespace by date

namespace ConsoleApp;

internal static class Program
{
    static async Task Main()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"Running day: {Path.GetExtension(typeof(Part1).Namespace)![2..]}{Environment.NewLine}");
        Console.ForegroundColor = ConsoleColor.Gray;
        foreach (var type in new[] { typeof(Part1), typeof(Part2) })
        {
            try
            {
                // Create instance
                var instance = (Part)Activator.CreateInstance(type)!;
                // start measure time after instance is created
                var w = Stopwatch.StartNew();
                var result = await instance.GetResultAsync();
                w.Stop(); // stop since WriteLine takes time
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{type.Name}: Result = {result}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{type.Name}: Result in: {w.Elapsed}");
            }
            catch (NotImplementedException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{type.Name} is not Implemented!");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}