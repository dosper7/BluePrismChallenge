using BluePrismTechChallenge.WordPuzzle;
using BluePrismTechChallenge.WordPuzzleConcretes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        [DataRow("Same", "Cost", new[] { "same", "came", "case", "cast", "cost" })]
        [DataRow("SAME", "Cost", new[] { "same", "came", "case", "cast", "cost" })]
        [DataRow("same", "cost", new[] { "same", "came", "case", "cast", "cost" })]
        public void SearchWords_should_return_valid_shortestPath(string startWord, string endWord, string[] expected)
        {
            //arrange
            var words = wordsList.Value;
            //act
            var result = searchStrategy.SearchWords(words, startWord, endWord);
            //assert
            CollectionAssert.AreEqual(result.ToList(), expected);

        }

        [TestMethod]
        [DataRow("", "1", new[] { "1" })]
        [DataRow(null, "1", new[] { "1" })]
        [DataRow("1", "", new[] { "1" })]
        [DataRow("", null, new string[] { })]
        [DataRow("1", "1", null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchWords_when_AnyParameterIsNull_should_throwExcepton(string startWord, string endWord, string[] wordsList)
        {
            //act
            searchStrategy.SearchWords(wordsList, startWord, endWord);

        }

        [TestMethod]
        [DataRow("1", "11", new[] { "1" })]
        [DataRow("11", "1", new[] { "1" })]
        [ExpectedException(typeof(ArgumentException))]
        public void SearchWords_when_startWord_And_StopWord_NotTheSameLenght_shouldThrowExcepton(string startWord, string endWord, string[] wordsList)
        {
            //act
            searchStrategy.SearchWords(wordsList, startWord, endWord);
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
