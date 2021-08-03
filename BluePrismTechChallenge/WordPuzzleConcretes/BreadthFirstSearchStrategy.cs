using BluePrismTechChallenge.WordPuzzle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzleConcretes
{
    /// <summary>
    /// Breadth First Search
    /// </summary>
    public class BreadthFirstSearchStrategy : ISearchStrategy
    {
        public IEnumerable<string> SearchWords(IEnumerable<string> wordsList, string startWord, string stopWord)
        {
            if (startWord.Length != stopWord.Length)
                throw new ArgumentException("Start Word and Stop word dont have the same lenght.");

            var graph = Graph.Build(wordsList, startWord.Length);
            var visited = new List<string>();
            var queue = new Queue<string>();
            var wordParents = new Dictionary<string, string>();

            queue.Enqueue(startWord);
            visited.Add(startWord);

            while (queue.Count > 0)
            {
                string wordToCheck = queue.Dequeue();
                IEnumerable<string> neighbours = graph.GetNeighbours(wordToCheck);

                foreach (var neighbour in neighbours)
                {
                    if (!visited.Any(v => string.Compare(v, neighbour, StringComparison.InvariantCultureIgnoreCase) == 0))
                    {
                        queue.Enqueue(neighbour);
                        visited.Add(neighbour);
                        wordParents.Add(neighbour, wordToCheck);
                    }
                }
            }

            var shortestPath = new List<string>();
            string pathItem = stopWord;
            while (pathItem != null)
            {
                shortestPath.Add(pathItem);
                pathItem = wordParents.ContainsKey(pathItem) ? wordParents[pathItem] : null;
            }

            shortestPath.Reverse();

            return shortestPath[0] == startWord ?
                shortestPath :
                Enumerable.Empty<string>();
        }

        private class Graph
        {
            private ConcurrentDictionary<string, IEnumerable<string>> _nodes = new ConcurrentDictionary<string, IEnumerable<string>>();

            public IEnumerable<string> GetNeighbours(string word) => _nodes[word];

            public static Graph Build(IEnumerable<string> wordsList, int wordLength)
            {
                if (wordsList is null)
                    throw new ArgumentNullException(nameof(wordsList));

                var graph = new Graph();
                var sameLengthWords = wordsList.Where(x => x.Length == wordLength);
                Parallel.ForEach(sameLengthWords, word =>
                {
                    var edges = wordsList.Where(w => DifferInOneLetter(word, w));
                    graph._nodes.TryAdd(word, edges);
                });

                return graph;
            }

            private static bool DifferInOneLetter(ReadOnlySpan<char> firstWord, ReadOnlySpan<char> secondWord)
            {
                int count = 0;
                if (firstWord.Length == secondWord.Length)
                {
                    for (int i = 0; i < firstWord.Length; i++)
                    {
                        if (firstWord[i] != secondWord[i] && ++count > 1)
                            return false;
                    }
                }
                return count == 1;

            }
        }

    }



}

