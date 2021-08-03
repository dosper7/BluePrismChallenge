using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzle
{
    public class WordPuzzleEngine
    {
        public ISearchStrategy searchStrategy { get; private set; }
        private readonly IWordsDB wordsDB;

        public WordPuzzleEngine(ISearchStrategy searchStrategy, IWordsDB wordsDB)
        {
            this.searchStrategy = searchStrategy ?? throw new ArgumentNullException(nameof(searchStrategy));
            this.wordsDB = wordsDB ?? throw new ArgumentNullException(nameof(wordsDB));
        }

        public WordPuzzleEngine ChangeStrategy(ISearchStrategy search)
        {
            searchStrategy = search ?? throw new ArgumentNullException(nameof(search));
            return this;
        }

        public async Task<IEnumerable<string>> FindWordsAsync(string startWord, string endWord)
        {
            if (searchStrategy is null)
                throw new ArgumentNullException(nameof(searchStrategy));

            var listOfWords = await wordsDB.GetWords();
            var wordsResult = searchStrategy.SearchWords(listOfWords, startWord, endWord);
            await wordsDB.SaveWords(wordsResult);
            return wordsResult;
        }
    }
}
