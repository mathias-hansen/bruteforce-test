using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bruteforcetest
{
    class Program
    {

        static void Main(string[] args)
        {
            var t = ItteratePasswords("", 0);
        }

        public static string ItteratePasswords(string prefix, int depth)
        {
            int MaxLength = 2;
            string password;
            string result;
            int i = 0;

            foreach (var c in Chars)
            {
                password = GetNextPassword(i, prefix);
                if (password != null)
                {
                    if (SendRequest(password) != "false") return password;
                    if ((depth + 1) < MaxLength)
                    {
                        result = ItteratePasswords(password, (depth + 1));
                        if (result != "false" && result != null) return result;
                    }
                }
                i++;
            }
            return null;
        }

        public static string GetNextPassword(int i, string prefix)
        {
            try
            {
                return prefix + Chars[(i + 1)];
            }
            catch (Exception)
            {
                return null;
            }
            
            
        }

        public static List<char> Chars = GetChars();
        public static List<char> GetChars()
        {
            var chars = new List<char>();
            for (int i = char.MinValue; i <= 126; i++)
            {
                char c = Convert.ToChar(i);
                if (!char.IsControl(c))
                {
                    chars.Add(c);
                }
            }
            return chars;
        }

        public static string SendRequest(string password)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:50758/home/login");

            var postData = "username=mathias";
            postData += "&password=" + password;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
