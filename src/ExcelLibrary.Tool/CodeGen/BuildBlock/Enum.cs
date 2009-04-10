using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.CodeGen
{
    public class Enum : ClassMember
    {
        public List<string> Values;

        public string Modifier = "public";

        public string UnderlyingType;

        public Enum(string name)
        {
            Name = CSharp.Identifier(name);
            Values = new List<string>();
        }
        public void SetValues(params string[] values)
        {
            Values.AddRange(values);
        }
        public override ICodeBlock CodeBlock
        {
            get
            {
                CodeBlock block = new CodeBlock();
                if (UnderlyingType == null)
                {
                    block.Leading = string.Format("{0} enum {1}", Modifier, Name);
                }
                else
                {
                    block.Leading = string.Format("{0} enum {1} : {2}", Modifier, Name, UnderlyingType);
                }
                for (int i = 0; i < Values.Count - 1; i++)
                {
                    block.Add(Values[i] + ",");
                }
                if (Values.Count > 0)
                {
                    block.Add(Values[Values.Count - 1]);
                }
                return block;
            }
        }
    }
}
