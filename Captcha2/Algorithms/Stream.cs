using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;

namespace Captcha1.Algorithms
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
                //result.RemoveAt(0);
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
        public static void writeToFile(string fileName, int[, ,] array)
        {
            int number = array.GetUpperBound(0)+1;
            int height = array.GetUpperBound(1)+1;
            int width= array.GetUpperBound(2)+1;
            using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
            {
                for (int k = 0; k < number; k++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            sw.Write(array[k, i, j]);
                            sw.Write(' ');
                        }
                        sw.WriteLine("");
                    }
                    sw.WriteLine("");
                }
            }
        }

        public static void saveImageToFile(string fileName, BitmapSource image)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.FlipHorizontal = true;
            //encoder.FlipVertical = false;
            //encoder.QualityLevel = 30;
            //encoder.Rotation = Rotation.Rotate90;
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
            stream.Close();
        }

    }
}
