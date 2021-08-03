using BluePrismTechChallenge.WordPuzzle;
using BluePrismTechChallenge.WordPuzzleConcretes;
using CommandLine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrismTechChallenge
{
    class Program
    {
        static async Task Main(string[] args)
        {

#if DEBUG
            do
            {
#endif
                await Parser.Default.ParseArguments<SearchOptions>(args?.Length > 0 ? args : new[] { "--help" })
                    .WithParsedAsync(async opts =>
                    {
                        ISearchStrategy searchStrategy = new BreadthFirstSearchStrategy();
                        IWordsDB wordsDB = new FileWordsDB(opts.WordsDictionaryFilePath, opts.ResultsFilePath);

                        var wp = new WordPuzzleEngine(searchStrategy, wordsDB);
                        var result = await wp.FindWordsAsync(opts.StartWord, opts.EndWord);

                        if (result.Any())
                        {
                            foreach (var item in result)
                            {
                                Console.WriteLine($"Words: {result}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No words were found.");
                        }
                       

                    }).ConfigureAwait(false);

                Console.WriteLine("Please enter the required arguments:");
                string options = Console.ReadLine();
                args = options.Split(' ', StringSplitOptions.RemoveEmptyEntries);
#if DEBUG

            } while (args?.Length != 0);
#endif
        }



    }
}
