using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class CodeLine : ICodeBlock
    {
        public string Text;

        public CodeLine(string text)
        {
            Text = text;
        }

        public override void Output(CodeWriter writer)
        {
            base.Output(writer);
            writer.WriteLine(Text);
        }
    }
}
