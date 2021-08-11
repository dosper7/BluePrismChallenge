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

        public IEnumerable<string> GetWords(int? wordLength = null)
        {
            var words = new List<string>();
            using (var fs = File.Open(SourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs, 65536))
            using (var reader = new StreamReader(bs))
            {
                string word = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(word))
                {
                    if (wordLength is null || word.Length == wordLength)
                        words.Add(word);

                    word = reader.ReadLine();
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
