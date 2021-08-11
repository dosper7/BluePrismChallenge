using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;

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
