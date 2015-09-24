Goomez is search suite that uses [Lucene.Net](http://incubator.apache.org/lucene.net/) as the search engine.

It has three main projects, GoomezCrawler, GoomezSearchHelper and a web client app used to query the search index.

  * GoomezCrawler is a Console App used to create the index
  * GoomezSearchHelper is a class library with helper functions used both by the indexer (crawler) and the web client.
  * GoomezSearch is a web app used to query the index and show th results.
The idea behind Goomez is having a tool to search software and instalers in a corporate environment.

The name (Goomez) is just a joke. I thought it'd be funny to have my last name writen in google like form. So, if any google lawyers gets around this project, don't sue me! I know I can't have a product with that logo. This is not even a product :)

Goomez is now live on the Windows Azure platform. I also stored the index on the SQL Azure database so it's not using Lucene.net anymore (at least not in the cloud) Give it a try (http://goomez.cloudapp.net)