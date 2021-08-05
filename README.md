WordPuzzle
===
For this challenge I created 3 main files
- WordPuzzleEngine.cs - class that works with two interfaces (ISearchStrategy and IWordsDB).
- IWordsDB.cs - Interface that defines the type of database the WordPuzzleEngine will use to get and save the words.
- ISearchStrategy.cs - Interface that defines the type of search to be done by the WordPuzzleEngine.

FileWordsDB (IWordsDB implementation)
---
The method GetWords a BufferedStream that helps with performance and memory allocations in case of big files.

At first i've set the method as async but after running some benchmarks (project inside the solution) I noticed that the sync method was faster and allocates less memory.

```
|                            Method |       Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------------------------- |-----------:|----------:|----------:|---------:|---------:|---------:|----------:|
|            ReadLinesBufferedAsync | 7,111.3 μs | 141.98 μs | 241.09 μs | 562.5000 | 257.8125 | 125.0000 |  3,575 KB |
|             ReadLinesBufferedSync | 1,936.4 μs |  31.77 μs |  32.62 μs | 246.0938 | 121.0938 | 121.0938 |  1,603 KB |
| ReadLinesBufferedSyncBiggerBuffer | 1,971.3 μs |  28.02 μs |  29.99 μs | 246.0938 | 121.0938 | 121.0938 |  1,603 KB |
|                         ReadLines | 2,068.0 μs |  40.83 μs |  62.35 μs | 246.0938 | 121.0938 | 121.0938 |  1,603 KB |
|                       ReadAllText |   479.2 μs |   9.51 μs |  12.70 μs | 142.5781 | 142.5781 | 142.5781 |    997 KB |
|                      ReadAllBytes |   193.9 μs |   3.87 μs |   6.77 μs |  76.9043 |  76.9043 |  76.9043 |    244 KB |
|                      ReadAllLines | 1,984.5 μs |  16.50 μs |  13.78 μs | 277.3438 | 183.5938 | 183.5938 |  1,813 KB |
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


How to use the console UI
---
- Have installed .NET 5.
- Either open the project with visual studio and press F5 having selected the project named WordPuzzle or run the following command in a command window:

dotnet run --project {path to the WordPuzzle.cproj}

It will guide you with what you should provide.


