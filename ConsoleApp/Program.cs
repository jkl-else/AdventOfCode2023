using System.Diagnostics;

namespace ConsoleApp
{
    internal static class Program
    {
        static async Task Main()
        {
            Part part = new Callendar.D01.Part1();

            var w = Stopwatch.StartNew();
            var result = await part.GetResultAsync();
            w.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Result in: {w.Elapsed}");
        }
    }
}