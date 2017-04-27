using System.IO;
using System.Text;

namespace DocXcoDe.Util
{
    public class FileHelper
    {
        public static byte[] Read(string filename)
        {
            var fi = new FileInfo(filename);

            byte[] result;
            using (var fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                result = new byte[fi.Length];
                fs.Read(result, 0, (int)fi.Length);
                fs.Close();
            }

            return result;
        }

        public static string ReadSmart(string filename)
        {
            var bytes = Read(filename);
            if (bytes[0] == 255 && bytes[1] == 254)
                return Encoding.UTF8.GetString(bytes);

            return Encoding.GetEncoding(1251).GetString(bytes); ;
        }

    }
}