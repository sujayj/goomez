using System;
using System.Data;
using System.Configuration;


namespace GoomezSearchHelper
{
    /// <summary>
    /// Summary description for IndexedFile
    /// </summary>
    public class IndexedFile
    {
        public IndexedFile() { }

        string m_full;
        string m_folder;
        string m_file;
        string m_extension;
        long m_size;

        public long FileSizeinBytes
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public string FileExtension
        {
            get { return m_extension; }
            set { m_extension = value; }
        }

        public string FileName
        {
            get { return m_file; }
            set { m_file = value; }
        }

        public string FileFolderName
        {
            get { return m_folder; }
            set { m_folder = value; }
        }

        public string FileFullName
        {
            get { return m_full; }
            set { m_full = value; }
        }


    }
}