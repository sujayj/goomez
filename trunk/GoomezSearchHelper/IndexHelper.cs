using System;
using System.Data;
using System.Configuration;

using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace GoomezSearchHelper
{

    /// <summary>
    /// Summary description for IndexSearcher
    /// </summary>
    public class IndexHelper
    {
        private string m_SearchPath;
        private string m_HistoryPath;

        public IndexHelper(string indexPath, string historyPath)
        {

            if (string.IsNullOrEmpty(indexPath) || (string.IsNullOrEmpty(historyPath)))
                throw new ArgumentException("indexPath and historyPath parameters are mandatory");

            m_SearchPath = indexPath;
            m_HistoryPath = historyPath;
        }

        public string SearchPath
        {
            get { return m_SearchPath; }
            set { m_SearchPath = value; }
        }
        
        public string HistoryPath
        {
            get { return m_HistoryPath; }
            set { m_HistoryPath = value; }
        }

        public List<IndexedFile> Search(string pattern)
        {
            IndexSearcher searcher = new IndexSearcher(m_SearchPath);
            List<IndexedFile> list = null;
            try
            {
                list = new List<IndexedFile>();

                QueryParser parser = new QueryParser(Constants.Content, new StandardAnalyzer());
                parser.SetDefaultOperator(QueryParser.AND_OPERATOR);
                string tokenizedPattern = Tokenizer.Tokenize(pattern, false);
                if (pattern != tokenizedPattern)
                    pattern = pattern + " OR \"" + tokenizedPattern + "\"";

                Query query = parser.Parse(pattern);

                Hits hits = searcher.Search(query);
                for (int i = 0; i < hits.Length(); i++)
                {
                    IndexedFile fi = new IndexedFile();

                    Document doc = hits.Doc(i);
                    fi.FileFullName = doc.Get(Constants.Full);
                    fi.FileName = doc.Get(Constants.File);
                    fi.FileFolderName = doc.Get(Constants.Folder);
                    fi.FileExtension = doc.Get(Constants.Extension);
                    fi.FileSizeinBytes = long.Parse(doc.Get(Constants.Size));

                    list.Add(fi);
                }
                return list;
            }
            finally
            {
                searcher.Close();
            }
        }

        public List<TextSearched> GetHistoryByUserDate(DateTime datePicked)
        {
            IndexSearcher searcher = new IndexSearcher(m_HistoryPath);
            List<TextSearched> list = null;
            try
            {
                list = new List<TextSearched>();

                string from = datePicked.ToString("yyyyMMddHHmmss");
                string to = datePicked.AddDays(1).ToString("yyyyMMddHHmmss");

                QueryParser parserUser = new QueryParser(Constants.VisitUser, new WhitespaceAnalyzer());
                QueryParser parserDate = new QueryParser(Constants.DateTicks, new WhitespaceAnalyzer());

                Query queryUser = parserUser.Parse(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                Query queryDate = parserDate.Parse("[" + from + " TO " + to + "]");

                BooleanQuery query = new BooleanQuery();

                query.Add(queryUser, BooleanClause.Occur.MUST);
                query.Add(queryDate, BooleanClause.Occur.MUST);

                Hits hits = searcher.Search(query);
                for (int i = 0; i < hits.Length(); i++)
                {
                    TextSearched st = new TextSearched();

                    Document doc = hits.Doc(i);
                    st.DateTicks = doc.Get(Constants.DateTicks);
                    st.VisitUser = doc.Get(Constants.VisitUser);
                    st.SearchedText = doc.Get(Constants.SearchedText);

                    list.Add(st);
                }

                list.Sort();
                return list;
            }
            finally
            {
                searcher.Close();
            }
        }

        public List<TextSearched> SearchHistoryByUserPattern(string pattern)
        {
            IndexSearcher searcher = new IndexSearcher(m_HistoryPath);
            List<TextSearched> list = null;
            try
            {
                list = new List<TextSearched>();

                QueryParser parserUser = new QueryParser(Constants.VisitUser, new WhitespaceAnalyzer());
                parserUser.SetDefaultOperator(QueryParser.AND_OPERATOR);
                QueryParser parserPattern = new QueryParser(Constants.SearchedText, new WhitespaceAnalyzer());
                parserPattern.SetDefaultOperator(QueryParser.AND_OPERATOR);

                Query queryUser = parserUser.Parse(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                Query queryPattern = parserPattern.Parse(pattern);

                BooleanQuery query = new BooleanQuery();
                query.Add(queryUser, BooleanClause.Occur.MUST);
                query.Add(queryPattern, BooleanClause.Occur.MUST);

                Hits hits = searcher.Search(query);
                for (int i = 0; i < hits.Length(); i++)
                {
                    TextSearched st = new TextSearched();

                    Document doc = hits.Doc(i);
                    st.DateTicks = doc.Get(Constants.DateTicks);
                    st.VisitUser = doc.Get(Constants.VisitUser);
                    st.SearchedText = doc.Get(Constants.SearchedText);

                    list.Add(st);
                }
                list.Sort();
                return list;
            }
            finally
            {
                searcher.Close();
            }
        }

        public List<TextSearched> GetHistoryBetweenDates(DateTime fromDate, DateTime toDate)
        {
            IndexSearcher searcher = new IndexSearcher(m_HistoryPath);
            List<TextSearched> list = null;
            try
            {
                list = new List<TextSearched>();

                string from = fromDate.ToString("yyyyMMddHHmmss");
                string to = toDate.ToString("yyyyMMddHHmmss");

                //QueryParser parserUser = new QueryParser(Constants.VisitUser, new WhitespaceAnalyzer());
                QueryParser parserDate = new QueryParser(Constants.DateTicks, new WhitespaceAnalyzer());

                //Query queryUser = parserUser.Parse(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                Query queryDate = parserDate.Parse("[" + from + " TO " + to + "]");

                BooleanQuery query = new BooleanQuery();

                //query.Add(queryUser, BooleanClause.Occur.MUST);
                query.Add(queryDate, BooleanClause.Occur.MUST);

                Hits hits = searcher.Search(query);
                for (int i = 0; i < hits.Length(); i++)
                {
                    TextSearched st = new TextSearched();

                    Document doc = hits.Doc(i);
                    st.DateTicks = doc.Get(Constants.DateTicks);
                    st.VisitUser = doc.Get(Constants.VisitUser);
                    st.SearchedText = doc.Get(Constants.SearchedText);

                    list.Add(st);
                }
                return list;
            }
            finally
            {
                searcher.Close();
            }
        }

        public void SaveSearchHistory(string pattern)
        {
            IndexWriter index = new IndexWriter(m_HistoryPath, new StandardAnalyzer(), !Directory.Exists(m_HistoryPath));

            Document doc = new Document();
            doc.Add(new Field(Constants.DateTicks, DateTime.Now.ToString("yyyyMMddHHmmss"), Field.Store.YES, Field.Index.UN_TOKENIZED));
            doc.Add(new Field(Constants.VisitUser, System.Threading.Thread.CurrentPrincipal.Identity.Name.Replace(@"\", ""), Field.Store.YES, Field.Index.UN_TOKENIZED));
            doc.Add(new Field(Constants.SearchedText, pattern, Field.Store.YES, Field.Index.TOKENIZED));

            index.AddDocument(doc);
            index.Close();
        }

        public string DidYouMean(string pattern)
        {
            try
            {
                IndexSearcher searcher = new IndexSearcher(m_HistoryPath);

                Term t = new Term(Constants.SearchedText, pattern);
                FuzzyQuery query = new FuzzyQuery(t);

                Hits hits = searcher.Search(query);

                if (hits.Length() != 0)
                    return hits.Doc(0).Get(Constants.SearchedText);
                else
                    return "";

            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}