using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bruteforcetest
{
    class Program
    {
        public static List<char> Chars = GetChars();
        public static string Password = string.Empty;
        public static int Count = 0;

        static void Main(string[] args)
        {
            ItteratePasswords(8);

            if (Password != "") Console.WriteLine(">> " + Password + " <<");
        }


        public static void ItteratePasswords(int depth, string prefix = "")
        {
            Parallel.ForEach(Chars, (c) =>
            {
                if (Password != "") return;

                var password = prefix + c;
                var response = SendRequest(password);

                Console.WriteLine(password + " - " + Count);

                if (response != "false" && response != null)
                {
                    Password = password;
                    return;
                }
                else
                {
                    if (depth - 1 > 0) ItteratePasswords(depth - 1, password);
                    if (Password != "") return;
                }
                Count++;
            });
        }
        
        public static List<char> GetChars()
        {
            var chars = new List<char>();
            for (int i = char.MinValue; i <= 126; i++)
            {
                char c = Convert.ToChar(i);
                if (Regex.Match(c.ToString(), "[cws52qbz]").Success)
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
