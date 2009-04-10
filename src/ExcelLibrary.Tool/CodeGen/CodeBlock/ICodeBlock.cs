using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.CodeGen
{
    public abstract class ICodeBlock
    {
        public List<string> Comment;

        public virtual void Output(CodeWriter writer)
        {
            if (Comment != null)
            {
                foreach (string line in Comment)
                {
                    writer.WriteComment(line);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            CodeWriter writer = new CodeWriter(new StringWriter(text));
            this.Output(writer);
            return text.ToString();
        }
    }
}
