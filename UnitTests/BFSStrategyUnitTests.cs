using BluePrismTechChallenge.WordPuzzle;
using BluePrismTechChallenge.WordPuzzleConcretes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{

    [TestClass]
    public class BFSStrategyUnitTests
    {
        private ISearchStrategy searchStrategy;
        private Lazy<IEnumerable<string>> wordsList;

        public BFSStrategyUnitTests()
        {
            searchStrategy = new BreadthFirstSearchStrategy();
            wordsList = new Lazy<IEnumerable<string>>(GetWords);
        }

        [TestMethod]
        [DataRow("same", "cost", new[] { "same", "came", "case", "cast", "cost" })]
        public void GetShortestPath(string startWord, string endWord, string[] expected)
        {
            //arrange
            var words = wordsList.Value;

            //act
            var result = searchStrategy.SearchWords(words, startWord, endWord);

            //assert
            CollectionAssert.AreEqual(result.ToList(), expected);

        }


        private IEnumerable<string> GetWords()
        {
            var words = new List<string>();
            using (var fs = File.Open($"{AppContext.BaseDirectory}words-english.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (var reader = new StreamReader(bs))
            {
                string word = reader.ReadLine();
                while (word != null)
                {
                    words.Add(word);
                    word = reader.ReadLine();
                }
            }
            return words;
        }
    }
}
