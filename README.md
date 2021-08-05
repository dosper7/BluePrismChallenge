BluePrismChallenge
===
For this challenge I created 3 main files
>- WordPuzzleEngine.cs - class that works with two interfaces (ISearchStrategy and IWordsDB).
>- IWordsDB.cs - Interface that defines the type of database the WordPuzzleEngine will use to get and save the words.
>- ISearchStrategy.cs - Interface that defines the type of search to be done by the WordPuzzleEngine.

FileWordsDB (IWordsDB implementation)
---
The method GetWordsAsync a BufferedStream that helps with performance and memory allocations in case of big files.

Also it has the added benefit that in the rere case that the amount of words to read are bigger than Int.MaxValue, if I used File.ReadAllLines that returns an array it avoids the memory overflow exception.

The method SaveWordsAsync uses a StreamWriter to write to file, I could instead use File.WriteAllText because the amount of data to write is small (at least during my tests) but with the StreamWrite gives more control on how to write to disk (by setting up the buffer size, for instance, can increase performance for big files).


BreadthFirstSearchStrategy (ISearchStrategy implementation)
---

References
------

https://www.youtube.com/watch?v=09_LlHjoEiY

https://www.udemy.com/course/graph-theory-algorithms/

https://www.letscodethemup.com/graph-search-breadth-first-search/

https://medium.com/better-programming/5-ways-to-find-the-shortest-path-in-a-graph-88cfefd0030f

https://hellokoding.com/shortest-paths/


Requirements
---
Have installed .NET 5.

How to use the console application:
===

