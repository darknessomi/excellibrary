using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class BlankLine : ICodeBlock
    {
        public override void Output(CodeWriter writer)
        {
            base.Output(writer);
            writer.WriteLine();
        }
    }
}
