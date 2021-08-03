using BluePrismTechChallenge.WordPuzzle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class WordPuzzleEngineUnitTests
    {
        private readonly Mock<ISearchStrategy> _searchStrategy;
        private readonly Mock<IWordsDB> _wordsDB;
        private readonly WordPuzzleEngine _wordPuzzleEngine;

        public WordPuzzleEngineUnitTests()
        {
            _searchStrategy = new Mock<ISearchStrategy>();
            _wordsDB = new Mock<IWordsDB>();
            _wordPuzzleEngine = new WordPuzzleEngine(_searchStrategy.Object, _wordsDB.Object);
        }

        [TestMethod]

        public async Task FindWords_shouldReturn_listOfWords()
        {
            //arrange
            var expectedList = new[] { "a", "b", "c" };
            _wordsDB.Setup(m => m.GetWords()).ReturnsAsync(Enumerable.Empty<string>());
            _searchStrategy.Setup(m => m.SearchWords(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedList);

            //act
            var result = await _wordPuzzleEngine.FindWordsAsync(string.Empty, string.Empty);

            //assert
            CollectionAssert.AreEqual(result.ToList(), expectedList);

        }
    }
}
