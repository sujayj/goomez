How does GoomezCrawler work?

Easy!, GoomezCrawler has an xml file called GoomezCrawler.xml which has all the data the crawler needs to create the index.

  * servers - Here you add the servers you want to add for the crawler to search. The crawler will search in every shared (with permissions) folder. Add just the name of he server in the "server" tag.
  * exclusions - There might be some folders you don't want the crawl into. Add those shared folders in the "exclusion" tag.
  * inclusions - If you have a server with 20 shared folders but only want to crawl in one of them. Well, instead of adding the server and excluding 19 folders, you can just include the single folder you want in the "inclusion" tag.
  * extensions - What kind of file do you want the index? Make sure you add the extension (with the dot) in the "extension" tag