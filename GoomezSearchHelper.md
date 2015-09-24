What does GoomezSearchHelper do?

Basicaly, GoomezSearchHelper is a helper , with the following classes.

  * Constants - This is the place where we keep the constants like the index name, and the name of the "fields" stored in the index.
  * IndexFile - You create an object of this clas when you want to index a file. It has some basic properties but pretty much are the same that the File class has.
  * IndexHelper - This is the class that do the tricks. Take a look a the Search and DidYouMean functions.
  * TextSearched - One of this objects is created and a new entry is added to the History index. Also, when you search in your history this is the kind ob object you get.
  * Tokenizer - This class has a couple of static (public) method used to create the tokens that will be saved in the index.