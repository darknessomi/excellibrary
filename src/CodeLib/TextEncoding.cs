using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib
{
    /// <summary>
    /// TextEncoding
    /// </summary>
    public class TextEncoding
    {
        /// <summary>
        /// check if all charaters in text are ASCII charater
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool FitsInASCIIEncoding(string text)
        {
            if(string.IsNullOrEmpty(text))return true;
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] > 127)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Encodings the is right.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static bool EncodingIsRight(Encoding encoding, byte[] data)
        {
            string text = encoding.GetString(data);
            byte[] bytes = encoding.GetBytes(text);
            if (Algorithm.ArrayEqual(bytes, data))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Safes the decode string.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static object SafeDecodeString(Encoding encoding, byte[] data)
        {
            string text = encoding.GetString(data);
            byte[] bytes = encoding.GetBytes(text);
            if (Algorithm.ArrayEqual(bytes, data))
            {
                return text;
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Group text by encoding.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<Pair<string, string>> GroupTextByEncoding(string text)
        {
            List<Pair<string, string>> frags = new List<Pair<string, string>>();
            StringBuilder buffer = new StringBuilder();
            string encoding = "ascii";
            foreach (char ch in text)
            {
                if (encoding == "ascii" && ch > 0x7f)
                {
                    if (buffer.Length > 0)
                    {
                        frags.Add(new Pair<string, string>("ascii", buffer.ToString()));
                        buffer.Length = 0;
                    }
                    encoding = "unicode";
                }
                else if (encoding == "unicode" && ch <= 0x7f)
                {
                    if (buffer.Length > 0)
                    {
                        frags.Add(new Pair<string, string>("unicode", buffer.ToString()));
                        buffer.Length = 0;
                    }
                    encoding = "ascii";
                }
                buffer.Append(ch);
            }
            if (buffer.Length > 0)
            {
                frags.Add(new Pair<string, string>(encoding, buffer.ToString()));
            }
            return frags;
        }

        public static string RemoveByteOrderMark(string text)
        {
            if (text[0] == 0xFEFF || text[0] == 0xFFFE)
            {
                return text.Substring(1);
            }
            else
            {
                return text;
            }
        }
    }
}
