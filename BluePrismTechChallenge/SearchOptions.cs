using CommandLine;

namespace BluePrismTechChallenge
{
    public record SearchOptions()
    {
        [Option('s', "startWord", Required = true, HelpText = "Start Word.")]
        public string StartWord { get; init; }
        [Option('e', "endWod", Required = true, HelpText = "End Word.")]
        public string EndWord { get; init; }
        [Option('d', "dictionary", Required = true, HelpText = "Name of the file with the list of words.")]
        public string WordsDictionaryFilePath { get; init; }
        [Option('r', "result", Required = true, HelpText = "Name of file where the results will be writen.")]
        public string ResultsFilePath { get; init; }
    }


}
