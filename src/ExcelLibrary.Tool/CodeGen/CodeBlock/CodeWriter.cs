using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.CodeGen
{
    public class CodeWriter
    {
        TextWriter Writer;

        public CodeWriter(TextWriter writer)
        {
            Writer = writer;
        }
        public CodeWriter(Stream stream)
        {
            Writer = new StreamWriter(stream, Encoding.UTF8);
        }
        public CodeWriter(string path)
        {
            Writer = new StreamWriter(path, false, Encoding.UTF8);
        }
        public int Indent = 0;

        public void WriteIndent()
        {
            Writer.Write(new string('\t', Indent));
        }

        public void WriteLine()
        {
            Writer.WriteLine();
        }

        public void WriteLine(string text)
        {
            this.WriteIndent();
            Writer.WriteLine(text);
        }

        public void WriteComment(string text)
        {
            this.WriteLine("//" + text);
        }

        public void Close()
        {
            Writer.Close();
        }

        public static string PutCodeString(string str)
        {
            return str == null ? "null" : "\"" + EscapeString(str) + "\"";
        }

        public static string EscapeChar(char ch)
        {
            return EscapeString(ch.ToString(), true);
        }

        public static string EscapeChar(string text)
        {
            return EscapeString(text, true);
        }

        public static string EscapeString(string text)
        {
            return EscapeString(text, false);
        }

        public static string EscapeString(string text, bool withinSingleQuote)
        {
            if (text == null) return null;
            StringBuilder escapedtext = new StringBuilder();
            foreach (char ch in text)
            {
                switch (ch)
                {
                    case '\\':
                        escapedtext.Append("\\\\");
                        break;
                    case '\'':
                        if (withinSingleQuote)
                        {
                            escapedtext.Append("\\\'");
                        }
                        else
                        {
                            escapedtext.Append(ch);
                        }
                        break;
                    case '\"':
                        if (withinSingleQuote)
                        {
                            escapedtext.Append(ch);
                        }
                        else
                        {
                            escapedtext.Append("\\\"");
                        }
                        break;
                    case '\r':
                        escapedtext.Append("\\r");
                        break;
                    case '\n':
                        escapedtext.Append("\\n");
                        break;
                    case '\t':
                        escapedtext.Append("\\t");
                        break;
                    case '\v':
                        escapedtext.Append("\\v");
                        break;
                    case '\a':
                        escapedtext.Append("\\a");
                        break;
                    case '\b':
                        escapedtext.Append("\\b");
                        break;
                    case '\f':
                        escapedtext.Append("\\f");
                        break;
                    case '\0':
                        escapedtext.Append("\\0");
                        break;
                    default:
                        escapedtext.Append(ch);
                        break;
                }
            }
            return escapedtext.ToString();
        }

        public static string GetKeywordTypeName(string typeName)
        {
            switch (typeName)
            {
                case "Int8":
                    return "byte";
                case "Int16":
                    return "short";
                case "Int32":
                    return "int";
                case "Int64":
                    return "long";
                case "UInt8":
                    return "ubyte";
                case "UInt16":
                    return "ushort";
                case "UInt32":
                    return "uint";
                case "UInt64":
                    return "ulong";
                default:
                    return typeName;
            }
        }
    }
}
