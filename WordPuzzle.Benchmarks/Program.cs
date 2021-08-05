using BenchmarkDotNet.Running;
using System;

namespace WordPuzzle.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FileReadingBenchmark>();
            Console.WriteLine(summary.ToString());
            Console.ReadLine();
        }
    }
}
