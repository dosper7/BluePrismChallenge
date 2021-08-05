using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzle
{
    public interface IWordsDB
    {
        IEnumerable<string> GetWords(int? wordLength = null);
        Task SaveWordsAsync(IEnumerable<string> words);
    }
}
