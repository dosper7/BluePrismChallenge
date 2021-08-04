using BluePrismTechChallenge.WordPuzzle;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrismTechChallenge.WordPuzzleConcretes
{
    public class FileWordsDB : IWordsDB
    {
        public string SourceFilePath { get; }
        public string ResultFilePath { get; }

        public FileWordsDB(string sourceFilePath, string resultFilePath)
        {
            SourceFilePath = sourceFilePath ?? throw new System.ArgumentNullException(nameof(sourceFilePath));
            ResultFilePath = resultFilePath ?? throw new System.ArgumentNullException(nameof(resultFilePath));
        }

        public async Task<IEnumerable<string>> GetWordsAsync(int? wordLength = null)
        {
            var words = new List<string>();
            using (var fs = File.Open(SourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (var reader = new StreamReader(bs))
            {
                string word = await reader.ReadLineAsync().ConfigureAwait(false);
                while (!string.IsNullOrWhiteSpace(word))
                {
                    if (wordLength is null || word.Length == wordLength)
                        words.Add(word);

                    word = await reader.ReadLineAsync().ConfigureAwait(false);
                }
            }

            return words;
        }

        public async Task SaveWordsAsync(IEnumerable<string> words)
        {
            var last = words.Last();
            using (var streamwriter = new StreamWriter(ResultFilePath, false))
            {
                foreach (string word in words)
                {
                    await streamwriter.WriteAsync(word);
                    if (word != last)
                        await streamwriter.WriteLineAsync();
                }
            }
        }
    }
}
