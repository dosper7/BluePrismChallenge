using System.Collections.Generic;

namespace BluePrismTechChallenge.WordPuzzle
{
    public interface ISearchStrategy
    {
        IEnumerable<string> SearchWords(IEnumerable<string> wordsList, string startWord, string endWord);
    }
}
