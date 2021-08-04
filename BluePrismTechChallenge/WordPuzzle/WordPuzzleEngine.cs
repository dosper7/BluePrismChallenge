using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzle
{
    public class WordPuzzleEngine
    {
        public ISearchStrategy SearchStrategy { get; private set; }
        private readonly IWordsDB wordsDB;

        public WordPuzzleEngine(ISearchStrategy searchStrategy, IWordsDB wordsDB)
        {
            this.SearchStrategy = searchStrategy ?? throw new ArgumentNullException(nameof(searchStrategy));
            this.wordsDB = wordsDB ?? throw new ArgumentNullException(nameof(wordsDB));
        }

        public WordPuzzleEngine ChangeStrategy(ISearchStrategy search)
        {
            SearchStrategy = search ?? throw new ArgumentNullException(nameof(search));
            return this;
        }

        public async Task<IEnumerable<string>> FindWordsAsync(string startWord, string endWord, int? wordLength = null)
        {
            var listOfWords = await wordsDB.GetWordsAsync(wordLength);
            var wordsResult = SearchStrategy.SearchWords(listOfWords, startWord, endWord);
            await wordsDB.SaveWordsAsync(wordsResult);
            return wordsResult;
        }
    }
}
