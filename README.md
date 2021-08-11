WordPuzzle
===
For this challenge I created 3 main files
- WordPuzzleEngine.cs - class that works with two interfaces (ISearchStrategy and IWordsDB).
- IWordsDB.cs - Interface that defines the type of database the WordPuzzleEngine will use to get and save the words.
- ISearchStrategy.cs - Interface that defines the type of search to be done by the WordPuzzleEngine.

FileWordsDB (IWordsDB implementation)
---
The method GetWords a BufferedStream that helps with performance and memory allocations in case of big files.

At first i've set the method as async but after running some benchmarks (project inside the solution) I noticed that the sync method (with a buffer of size 64k) was faster and allocates less memory.

The 64k magic buffer sized is based on a report called "Sequential File Programming Patterns and Performance with .NET" done by microsoft reasearch (last link in the references sections)

```
|                            Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------------------------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
|            ReadLinesBufferedAsync | 6.571 ms | 0.0955 ms | 0.0846 ms | 562.5000 | 242.1875 | 117.1875 |      3 MB |
|             ReadLinesBufferedSync | 1.954 ms | 0.0176 ms | 0.0156 ms | 246.0938 | 121.0938 | 121.0938 |      2 MB |
| ReadLinesBufferedSyncBiggerBuffer | 1.868 ms | 0.0120 ms | 0.0094 ms | 248.0469 | 123.0469 | 123.0469 |      2 MB |
|                         ReadLines | 2.064 ms | 0.0334 ms | 0.0312 ms | 214.8438 | 109.3750 | 101.5625 |      2 MB |
|               ReadAllTextWithSpan | 2.921 ms | 0.0492 ms | 0.0585 ms | 503.9063 | 449.2188 | 250.0000 |      2 MB |
|                      ReadAllLines | 2.042 ms | 0.0343 ms | 0.0534 ms | 277.3438 | 183.5938 | 183.5938 |      2 MB |
```

Also it has the added benefit that in the rere case that the amount of words to read are bigger than Int.MaxValue, if I used File.ReadAllLines that returns an array it avoids the memory overflow exception.

The method SaveWordsAsync uses a StreamWriter to write to file, I could instead use File.WriteAllText because the amount of data to write is small (at least during my tests) but with the StreamWrite gives more control on how to write to disk (by setting up the buffer size, for instance, can increase performance for big files).


BreadthFirstSearchStrategy (ISearchStrategy implementation)
---
In this file I added the Graph implementation for the Breadth First Search, I use the Parallel.ForEach to make use of all CPU Cores available in order to speed up the computation of the graph. 

Due to the nature of how Parallel for each works (multiple threads) I had to use a thread safe collection, in this case I used a ConcurrentDictionary to facilitate and speed up the lookup for words and it's related words (graph adjacency list).


WordPuzzleEngine
---
This is the glue between all processes, I applied the strategy pattern and this way the engine is agnostic of which algorithm is used as also is on how it will get/save the words, it only cares about to receive a strategy and a provider to get/save the words.

The Challenge
===

After some investigation on how to solve the challenge I found the one of the best way is was to use an unweighted graph with the Breadth First Search to find the shortest path between the two words.


References
------

https://www.youtube.com/watch?v=09_LlHjoEiY

https://www.udemy.com/course/graph-theory-algorithms/

https://www.letscodethemup.com/graph-search-breadth-first-search/

https://medium.com/better-programming/5-ways-to-find-the-shortest-path-in-a-graph-88cfefd0030f

https://hellokoding.com/shortest-paths/

https://arxiv.org/pdf/cs/0502012


How to use the console UI
---
- Have installed .NET 5.
- Either open the project with visual studio and press F5 having selected the project named WordPuzzle or run the following command in a command window:

dotnet run --project {path to the WordPuzzle.cproj}

It will guide you with what you should provide.


