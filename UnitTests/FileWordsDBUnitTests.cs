using BluePrismTechChallenge.WordPuzzle;
using BluePrismTechChallenge.WordPuzzleConcretes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{

    [TestClass]
    public class FileWordsDBUnitTests
    {
        private IWordsDB _wordsDB;
        private static readonly string sourceFile = $"{AppContext.BaseDirectory}words-english.txt";
        private static readonly string resultFile = $"{AppContext.BaseDirectory}result.txt";

        public FileWordsDBUnitTests()
        {
            _wordsDB = new FileWordsDB(sourceFile, resultFile);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetWords_whenSourceFileDoesntExist_shouldThrowException()
        {
            //arrange
            var wordsDB = new FileWordsDB("1", resultFile);
            //act
            wordsDB.GetWords();
        }

        [TestMethod]
        public void GetWords_should_read_all_file_entries()
        {
            //arrange
            int totalWords = 26880;
            //act
            var words =  _wordsDB.GetWords();
            //assert
            Assert.AreEqual(words.Count(), totalWords);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(6)]
        public void GetWords_ShouldOnlyReturnWords_WhenLenghtIsSet(int wordLength)
        {
            //act
            var words = _wordsDB.GetWords(wordLength);
            //assert
            Assert.IsTrue(words.All(x => x.Length == wordLength));

        }

        [TestMethod]
        [DataRow(new[] { "aa", "bb", "cc" })]
        public async Task SaveWords_ShouldSaveWordsInSpecifiedFileName(string[] words)
        {
            //arrange
            var testFile = $"{AppContext.BaseDirectory}testFile.txt";
            var wordsDB = new FileWordsDB(testFile, testFile);

            //act
            await wordsDB.SaveWordsAsync(words);
            var fileWords = wordsDB.GetWords();

            //assert
            CollectionAssert.AreEqual(words, fileWords.ToArray());

        }


    }
}
