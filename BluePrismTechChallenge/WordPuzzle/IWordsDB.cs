using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzle
{
    public interface IWordsDB
    {
        Task<IEnumerable<string>> GetWordsAsync(int? wordLength = null);
        Task SaveWordsAsync(IEnumerable<string> words);
    }
}
