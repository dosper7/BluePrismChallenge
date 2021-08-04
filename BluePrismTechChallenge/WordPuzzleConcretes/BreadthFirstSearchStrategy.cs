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
        private ConcurrentDictionary<string, IEnumerable<string>> _graphNodes = null;

        public BreadthFirstSearchStrategy()
        {
            _graphNodes = new ConcurrentDictionary<string, IEnumerable<string>>();
        }

        public IEnumerable<string> SearchWords(IEnumerable<string> wordsList, string startWord, string stopWord)
        {
            if (wordsList is null)
                throw new ArgumentNullException(nameof(wordsList));

            if (string.IsNullOrEmpty(startWord))
                throw new ArgumentNullException($"'{nameof(startWord)}' cannot be null or empty.", nameof(startWord));

            if (string.IsNullOrEmpty(stopWord))
                throw new ArgumentNullException($"'{nameof(stopWord)}' cannot be null or empty.", nameof(stopWord));

            if (startWord.Length != stopWord.Length)
                throw new ArgumentException("Start Word and Stop word dont have the same lenght.");

            startWord = startWord.ToLower();
            stopWord = stopWord.ToLower();
            var visitedNodes = new List<string>();
            var nodesToCheck = new Queue<string>();
            var nodeParents = new Dictionary<string, string>();

            BuildGraphNodes(wordsList, startWord.Length);

            nodesToCheck.Enqueue(startWord);
            visitedNodes.Add(startWord);

            while (nodesToCheck.Any())
            {
                string wordToCheck = nodesToCheck.Dequeue();
                IEnumerable<string> neighbours = GetNodeNeighbours(wordToCheck);

                foreach (var neighbour in neighbours)
                {
                    bool nodeNotVisited = !visitedNodes.Any(v => string.Compare(v, neighbour, StringComparison.InvariantCultureIgnoreCase) == 0);
                    if (nodeNotVisited)
                    {
                        nodesToCheck.Enqueue(neighbour);
                        visitedNodes.Add(neighbour);
                        nodeParents.Add(neighbour, wordToCheck);
                    }
                }
            }

            var shortestList = new List<string>();
            string nodeWord = stopWord;
            while (nodeWord != null)
            {
                shortestList.Add(nodeWord);
                nodeWord = nodeParents.ContainsKey(nodeWord) ? nodeParents[nodeWord] : null;
            }

            shortestList.Reverse();

            return 
                shortestList[0] == startWord ? 
                shortestList : 
                Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetNodeNeighbours(string word) => _graphNodes[word];

        private void BuildGraphNodes(IEnumerable<string> wordsList, int wordLength)
        {
            if (wordsList is null)
                throw new ArgumentNullException(nameof(wordsList));

            var sameLengthWords = wordsList.Where(x => x.Length == wordLength);
            Parallel.ForEach(sameLengthWords, word =>
            {
                var edges = wordsList.Where(w => DifferInOneLetter(word, w));
                _graphNodes.TryAdd(word, edges);
            });
        }

        private static bool DifferInOneLetter(string firstWord, string secondWord)
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

