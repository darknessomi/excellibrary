using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class CSharp
    {
        public static List<string> Keywords = new List<string>(
            new string[]{
                "break",
                "class",
                "decimal",
                "delegate",
                "event",
                "float",
                "lock",
                "long",
                "namespace",
                "override",
                "ref",
                "short"
            });

        public static bool IsKeyword(string word)
        {
            return Keywords.Contains(word);
        }

        public static string Identifier(string word)
        {
            if (IsKeyword(word))
            {
                return "@" + word;
            }
            else
            {
                return word;
            }
        }
    }
}
