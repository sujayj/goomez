using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Xml;
using System.Reflection;
using System.Diagnostics;

namespace GoomezCrawler
{
    class Program
    {
        private static int m_indexed = 0;
        private static int m_scanned = 1;
        private const string K_SERVERS = "server";
        private const string K_EXTENSIONS = "extension";
        private const string K_EXCLUSIONS = "exclusion";
        private const string K_INCLUSIONS = "inclusion";
        private const string K_INDEXNAME = "GoomezIndex";
        private const string K_LOGERRORFILE = "GoomezCrawlerErrors.log";
        private const string K_CONFIGFILE = "GoomezCrawler.XML";
        private static string K_CURRENTPATH = "";
        private static List<string> m_exclusions = null;
        private static List<string> m_extensions = null;
        private static Dictionary<string,string> m_errors = new Dictionary<string,string>();
        private static bool errors = false;
        private static IndexWriter m_indexWriter = null;
        private static StreamWriter m_logFile = null;
        private static DateTime started;
        //private static string m_status = "|";

        static int Main(string[] args)
        {
            started = DateTime.Now;

            try
            {
                K_CURRENTPATH = Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.LastIndexOf(@"\"));
#if DEBUG
                m_logFile = new StreamWriter(Path.Combine(K_CURRENTPATH, "GoomezCrawler.log"), false);
#endif
      
                if (args.Length > 1)
                {
                    Console.WriteLine("Too many arguments");
                    PrintHelp();
                    return -1;
                }

                m_exclusions = GetConfigList(K_EXCLUSIONS);

                m_extensions = GetConfigList(K_EXTENSIONS);
                if (m_extensions.Count == 0)
                    throw new ApplicationException("No extensions found.");

                File.Delete(K_LOGERRORFILE);

                if (args.Length == 1)
                {
                    string sharedFolder = args[0];
                    if (Directory.Exists(sharedFolder))
                        IndexSharedFolder(sharedFolder);
                    else
                        throw new ApplicationException(sharedFolder + " not found.");

                    return 0;
                }

                IndexFiles();
                return errors ? -1 : 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Internal error");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                
            }
            finally
            {
                if (m_logFile != null)
                    m_logFile.Close();
            }

            return -1;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("GoomezCrawler [<sharedfolder>]");
            Console.WriteLine("<sharedfolder>    Path to the shared folder to index");
            Console.WriteLine("There must exists 2 files: " + K_SERVERS + " and " + K_EXTENSIONS + ", " + K_EXCLUSIONS + " is optional");
        }

        private static IndexWriter GoomezIndex
        {
            get {
                if (m_indexWriter == null)
                    m_indexWriter = new IndexWriter(Path.Combine(K_CURRENTPATH, K_INDEXNAME), new StandardAnalyzer(), true);

                return m_indexWriter;
            }
        }

        private static void IndexSharedFolder(string sharedFolder)
        {
            DateTime started = DateTime.Now;
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                IndexFolder(sharedFolder);
            }
            catch (Exception ex)
            {
                ShowAndLogException(FileType.Folder, sharedFolder, ex);
            }
            finally
            {
                ShowSumamry();
            }
        }

        private static void IndexFiles()
        {
            try
            {
                List<string> servers = GetConfigList(K_SERVERS);

                foreach (string server in servers)
                {
                    foreach (string folder in GetShares(server))
                    {
                        if (folder.EndsWith("$"))
                            continue;

                        string folderFullPath = @"\\" + server + @"\" + folder;

                        try
                        {
                            IndexFolder(folderFullPath);
                        }
                        catch (Exception ex)
                        {
                            ShowAndLogException(FileType.Folder, folderFullPath, ex);
                        }
                    }
                }

                List<string> inclusions = GetConfigList(K_INCLUSIONS);
                foreach (string folder in inclusions)
                {
                    try
                    {
                        IndexFolder(folder);
                    }
                    catch (Exception ex)
                    {
                        ShowAndLogException(FileType.Folder, folder, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("== ERROR ==");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (GoomezIndex != null)
                {
                    GoomezIndex.Optimize();
                    GoomezIndex.Close();
                }

                ShowSumamry();
            }
        }
        
        private static void IndexFolder(string folder)
        {
            try
            {
                if (m_exclusions.Contains(folder))
                    return;

                foreach (string childFolder in Directory.GetDirectories(folder))
                {
                    IndexFolder(childFolder);
                }

    #if DEBUG
                //Console.ForegroundColor = ConsoleColor.DarkGreen;
                //Console.WriteLine("Folder->" + folder);
    #endif
                foreach (string file in Directory.GetFiles(folder))
                {
                    FileInfo fi = new FileInfo(file);
                    if (m_extensions.Contains(fi.Extension))
                        IndexFile(fi);

                    m_scanned++;
                }
            }
            catch (Exception ex)
            {
                ShowAndLogException(FileType.Folder, folder, ex);
            }
        }

        private static void IndexFile(FileInfo file)
        {
            try
            {
                Document doc = new Document();
                doc.Add(Field.Keyword("full", file.FullName));
                doc.Add(Field.UnIndexed("file", file.Name));
                doc.Add(Field.UnIndexed("folder",file.Directory.FullName));
                doc.Add(Field.UnIndexed("extension",file.Extension.Replace(".","")));
                doc.Add(Field.UnIndexed("size",file.Length.ToString()));
                doc.Add(new Field("content", GoomezSearchHelper.Tokenizer.TokenizeToIndex(file.FullName), Field.Store.YES, Field.Index.TOKENIZED));
                GoomezIndex.AddDocument(doc);
                m_indexed++;

#if DEBUG
                Console.ForegroundColor = ConsoleColor.Green;
#endif
                Console.WriteLine("File->" + file.FullName);
                
                Log(GoomezSearchHelper.Tokenizer.TokenizeToIndex(file.FullName));

            }
            catch (Exception ex)
            {
                ShowAndLogException(FileType.File, file.FullName, ex);
            }
        }

        private static List<string> GetConfigList(string items)
        {
            List<string> list = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(K_CURRENTPATH, K_CONFIGFILE));

            XmlNodeList nodes = doc.SelectNodes("//" + items);
            foreach (XmlNode node in nodes)
            {
                list.Add(node.InnerText);
            }

            return list;
        }

        private static void ShowAndLog(string message)
        {
            Console.WriteLine(message);
            Log(message);
        }

        private static void ShowAndLogException(FileType type, string file, Exception ex)
        {
            errors = true;
            string errorMsg = string.Format("Error on {0}:{1}",file ,ex.Message);
            m_errors.Add(file,errorMsg);

            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine(errorMsg);
            Console.ForegroundColor = cc;

            StreamWriter output = new StreamWriter(K_LOGERRORFILE, true);
            output.WriteLine(errorMsg);
            output.WriteLine(ex.Message);
            output.Close();
        }

        private static void ShowSumamry()
        {
            ShowAndLog("Indexing started at " + started.ToShortTimeString() + " and ended at " + DateTime.Now.ToShortTimeString());
            ShowAndLog(string.Format("{0} files indexed", m_indexed));
            if (errors)
            {
                Console.WriteLine("=== ERRORS ===");
                foreach (string errorLine in m_errors.Values)
                {
                    Console.WriteLine(errorLine);
                }
                Console.WriteLine("=== ERRORS ===");
                ShowAndLog(string.Format("There were {0} erros, please check {1}",m_errors.Values.Count, K_LOGERRORFILE));
                AddExclusions();
            }
            else
                Console.WriteLine("No errors found.");
        }

        private static void AddExclusions()
        {
            StreamWriter output = new StreamWriter(K_LOGERRORFILE, true);
            try
            {
                output.WriteLine("== EXCLUSIONS ==");
                foreach (string dir in m_errors.Keys)
                {
                    output.WriteLine(string.Format("<exclusion>{0}</exclusion>", dir));
                }

            }
            finally
            {
                output.Close();
            }
        }

        private enum FileType
        {
            File,
            Folder
        }

        private static void Log(string tokens)
        {
#if DEBUG
            m_logFile.WriteLine(tokens);
#endif
        }

        #region Win32 API


        [StructLayout(LayoutKind.Sequential)]
        public struct SHARE_INFO_0
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public String shi0_netname;
        }


        [DllImport("Netapi32.dll")]
        public static extern int NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] 
            string servername,
            Int32 level,
            out IntPtr bufptr,
            Int32 prefmaxlen,
            [MarshalAs(UnmanagedType.LPArray)] Int32[] entriesread,
            [MarshalAs(UnmanagedType.LPArray)] Int32[] totalentries,
            [MarshalAs(UnmanagedType.LPArray)] Int32[] resume_handle
                                );

        [DllImport("Netapi32.dll")]
        public static extern int NetApiBufferFree(long lpBuffer);

        public static string[] GetShares(string server)
        {
            IntPtr buf = new IntPtr(0);
            Int32[] dwEntriesRead = new Int32[1];
            dwEntriesRead[0] = 0;
            Int32[] dwTotalEntries = new Int32[1];
            dwTotalEntries[0] = 0;
            Int32[] dwResumeHandle = new Int32[1];
            dwResumeHandle[0] = 0;
            Int32 success = 0;
            string[] shares = new string[0];


            success = NetShareEnum(server, 0, out buf, -1, dwEntriesRead, dwTotalEntries, dwResumeHandle);
            if (dwEntriesRead[0] > 0)
            {
                SHARE_INFO_0[] s0 = new SHARE_INFO_0[dwEntriesRead[0]];
                shares = new string[dwEntriesRead[0]];
                for (int i = 0; i < dwEntriesRead[0]; i++)
                {
                    s0[i] = (SHARE_INFO_0)Marshal.PtrToStructure(buf, typeof(SHARE_INFO_0));
                    shares[i] = s0[i].shi0_netname;
                    buf = (IntPtr)((long)buf + Marshal.SizeOf(s0[0]));
                }
                //NetApiBufferFree((long)buf);
            }
            return shares;
        }

        #endregion


    }
}
