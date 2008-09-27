using System;
using System.Collections.Generic;
using System.Text;

namespace GoomezSearchHelper
{
    public class Tokenizer
    {
        public static string Tokenize(string path, bool manageExtension)
        {
            path = path.Replace(@"\", " ");
            path = path.Replace(@"_", " ");
            path = path.Replace(@"-", " ");
            
            if (manageExtension)
            {
                int dot = path.LastIndexOf(".");
                if (dot != -1)
                    path = path.Insert(dot, " ");
            }

            string output = "";

            bool lower = true;
            bool upper = true;

            foreach (char c in path.Trim())
            {
                if (char.IsNumber(c))
                {
                    output += c;

                    lower = false;
                    upper = false;
                    continue;
                }
                else if (char.IsLetter(c))
                {
                    if (char.IsLower(c))
                    {
                        if (!lower && !upper)
                            output += " ";

                        output += c;

                        lower = true;
                        upper = false;
                        continue;
                    }
                    else if (char.IsUpper(c))
                    {
                        if (!upper)
                            output += " ";

                        output += c;

                        lower = false;
                        upper = true;
                        continue;
                    }
                }
                else
                {
                    output += c;

                    lower = true;
                    upper = true;
                    continue;

                }
            }

            //Correct GeneXus and DeKlarit
            //output = output.Replace("Gene Xus", "GeneXus");
            //output = output.Replace("De Klarit", "DeKlarit");
            return output;
        }

        private static string Spaceize(string fullPath)
        {
            fullPath = fullPath.Replace(@"\", " ");
            fullPath = fullPath.Replace(@"_", " ");
            fullPath = fullPath.Replace(@"-", " ");
            fullPath = fullPath.Replace(@".", " ");

            return fullPath;
        }

        public static string TokenizeToIndex(string fullPath)
        {
            string strRet = Tokenize(fullPath, true);
            strRet = strRet + " " + Spaceize(strRet);

            return strRet;
        }
    }
}
