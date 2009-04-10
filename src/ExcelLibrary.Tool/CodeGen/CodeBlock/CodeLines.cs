using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class CodeLines : ICodeBlock
    {
        public List<string> Lines;

        public CodeLines()
        {
            Lines = new List<string>();
        }

        public override void Output(CodeWriter writer)
        {
            base.Output(writer);
            foreach (string text in Lines)
            {
                writer.WriteLine(text);
            }
        }
    }
}
