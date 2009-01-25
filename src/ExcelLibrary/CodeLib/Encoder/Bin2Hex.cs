using System;
using System.Text;
using System.Collections.Generic;

namespace QiHe.CodeLib
{
    /// <summary>
    /// Binary data to Hexadecimal string
    /// </summary>
    public class Bin2Hex
    {
        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encode(byte[] data)
        {
            StringBuilder code = new StringBuilder(data.Length * 2);
            foreach (byte bt in data)
            {
                code.Append(bt.ToString("X2"));
            }
            return code.ToString();
        }

        /// <summary>
        /// Decodes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static byte[] Decode(string code)
        {
            byte[] bytes = new byte[code.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(code.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bytes;
        }

        /// <summary>
        /// Formats the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Format(byte[] data)
        {
            return Format(data, "{0:X2} ", 16);
        }

        /// <summary>
        /// Formats the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <param name="bytesPerRow">The bytes per row.</param>
        /// <returns></returns>
        public static string Format(byte[] data, string format, int bytesPerRow)
        {
            StringBuilder code = new StringBuilder();
            int count = 0;
            foreach (byte bt in data)
            {
                code.AppendFormat(format, bt);
                count++;
                if (count == bytesPerRow)
                {
                    code.AppendLine();
                    count = 0;
                }
            }
            return code.ToString();
        }

        /// <summary>
        /// Parses the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static byte[] Parse(string text)
        {
            List<byte> bytes = new List<byte>();
            char[] chars = new char[2];
            bool start = true;
            foreach (char ch in text)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    if (start)
                    {
                        chars[0] = ch;
                        start = false;
                    }
                    else
                    {
                        chars[1] = ch;
                        bytes.Add(Convert.ToByte(new string(chars), 16));
                        start = true;
                    }
                }
            }
            return bytes.ToArray();
        }
    }
}
