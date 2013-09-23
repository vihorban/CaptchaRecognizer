using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AnalyzerData.Methods
{
    public class Stream
    {
        public static List<string> loadFromFile(string fileName, out string info)
        {
            List<string> result = new List<string>();
            try
            {
                string[] allText = File.ReadAllLines(fileName);
                info = allText[0];
                result = allText.ToList();
                result.RemoveAt(0);
            }
            catch (FileNotFoundException e)
            {
                info=e.ToString();
            }
            return result;
        }
        public static List<string> loadFromFile(string fileName)
        {
            List<string> result = new List<string>();
            try
            {
                string[] allText = File.ReadAllLines(fileName);
                result = allText.ToList();
                result.RemoveAt(0);
            }
            catch (FileNotFoundException e)
            {
            }
            return result;
        }
        public static void writeToFile(string fileName, string info, List<string> data)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine(info);
                foreach (string s in data)
                {
                    sw.WriteLine(s);
                }
            }
        }
    }
}
