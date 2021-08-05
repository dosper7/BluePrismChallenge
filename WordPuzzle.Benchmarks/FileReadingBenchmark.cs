using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordPuzzle.Benchmarks
{
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class FileReadingBenchmark
    {
        private static readonly string wordsFile = $"{AppContext.BaseDirectory}words-english.txt";

        [Benchmark]
        public async Task<List<string>> ReadLinesBufferedAsync()
        {
            using var stream = File.Open(wordsFile, FileMode.Open, FileAccess.Read);
            using var bs = new BufferedStream(stream,int.MaxValue);
            using var reader = new StreamReader(stream);

            var words = new List<string>();
            string word = await reader.ReadLineAsync().ConfigureAwait(false);
            while (!string.IsNullOrWhiteSpace(word))
            {
                words.Add(word);
                word = await reader.ReadLineAsync().ConfigureAwait(false);
            }

            return words;
        }

        [Benchmark]
        public List<string> ReadLinesBufferedSync()
        {
            using var stream = File.Open(wordsFile, FileMode.Open, FileAccess.Read);
            using var bs = new BufferedStream(stream);
            using var reader = new StreamReader(stream);

            var words = new List<string>();
            string word = reader.ReadLine();
            while (!string.IsNullOrWhiteSpace(word))
            {
                words.Add(word);
                word = reader.ReadLine();
            }
            return words;
        }

        [Benchmark]
        public List<string> ReadLinesBufferedSyncBiggerBuffer()
        {
            using var stream = File.Open(wordsFile, FileMode.Open, FileAccess.Read);
            using var bs = new BufferedStream(stream, 65536);
            using var reader = new StreamReader(stream);

            var words = new List<string>();
            string word = reader.ReadLine();
            while (!string.IsNullOrWhiteSpace(word))
            {
                words.Add(word);
                word = reader.ReadLine();
            }
            return words;
        }

        [Benchmark]
        public List<string> ReadLines()
        {
            return File.ReadLines(wordsFile).ToList();
        }

        [Benchmark]
        public string ReadAllText()
        {
            return File.ReadAllText(wordsFile);
        }

        [Benchmark]
        public byte[] ReadAllBytes()
        {
            return File.ReadAllBytes(wordsFile);
        }

        [Benchmark]
        public string[] ReadAllLines()
        {
            return File.ReadAllLines(wordsFile);
        }
    }
}
