using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.CodeLib
{
    /// <summary>
    /// This class represents a Stream Helper.
    /// </summary>
    public class StreamHelper
    {
        /// <summary>
        /// Write Byte Order Mark(BOM) of streamWriter's Encoding
        /// to streamWriter's underlying stream.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        public static void WriteByteOrderMark(StreamWriter streamWriter)
        {
            byte[] BOM = streamWriter.Encoding.GetPreamble();
            streamWriter.BaseStream.Write(BOM, 0, BOM.Length);
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bytes">The bytes.</param>
        public static void WriteBytes(Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(Stream stream, int count)
        {
            int length = Math.Min((int)stream.Length, count);
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="buffer">The buffer.</param>
        public static void ReadBytes(Stream stream, byte[] buffer)
        {
            stream.Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Reads to end.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadToEnd(Stream stream)
        {
            byte[] bytes = new byte[stream.Length - stream.Position];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Search the first occurance of values from current position in stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="values"></param>
        /// <returns>the stream position after values or -1 if not fount</returns>
        public static long SearchStream(Stream stream, byte[] values)
        {
            int index = 0;
            int code = -1;
            do
            {
                code = stream.ReadByte();
                if (code == values[index])
                {
                    index++;
                }
                else if (index > 0)
                {
                    stream.Position -= index; index = 0;
                }
            }
            while (code != -1 && index < values.Length);

            if (index == values.Length) { return stream.Position; }
            else { return -1; }
        }

        /// <summary>
        /// Search the first occurance of values from current position in stream within start range
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="values"></param>
        /// <param name="maxlength">maximum bytes to seach</param>
        /// <returns></returns>
        public static long SearchStream(Stream stream, byte[] values, int maxlength)
        {
            int index = 0;
            int code = -1;
            long maxpos = stream.Position + maxlength;
            do
            {
                code = stream.ReadByte();
                if (code == values[index]) { index++; }
                else { stream.Position -= index; index = 0; }
            }
            while (code != -1 && index < values.Length && stream.Position < maxpos);

            if (index == values.Length) { return stream.Position; }
            else { return -1; }
        }
    }
}
