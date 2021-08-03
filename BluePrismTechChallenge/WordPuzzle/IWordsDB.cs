using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzle
{
    public interface IWordsDB
    {
        Task<IEnumerable<string>> GetWords();
        Task SaveWords(IEnumerable<string> words);
    }
}
