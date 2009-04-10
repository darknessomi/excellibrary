using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public abstract class BuildingBlock
    {
        public abstract ICodeBlock CodeBlock { get;}

        public string Summary;

        public List<string> Attributes = new List<string>();

        internal CodeLines Header
        {
            get
            {
                CodeLines header = new CodeLines();
                if (summary != null)
                {
                    header.Comment = summary;
                }
                foreach (string attr in Attributes)
                {
                    header.Lines.Add(String.Format("[{0}]", attr));
                }
                return header;
            }
        }

        private List<string> summary
        {
            get
            {
                if (!String.IsNullOrEmpty(Summary))
                {
                    List<string> comment = new List<string>();
                    comment.Add("/ <summary>");
                    string[] lines = Summary.Split('\n');
                    foreach (string line in lines)
                    {
                        comment.Add("/ " + line.Trim());
                    }
                    comment.Add("/ </summary>");
                    return comment;
                }
                return null;
            }
        }

        public virtual void Output(CodeWriter writer)
        {
            CodeBlock.Output(writer);
        }
    }
}
