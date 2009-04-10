using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class Property : ClassMember
    {
        public CodeBlock Get = new CodeBlock("get");
        public CodeBlock Set = new CodeBlock("set");

        public Property(string type, string name)
            : base(type, name)
        {
            Modifiers.Add("public");
        }

        public override ICodeBlock CodeBlock
        {
            get
            {
                CodeBlock block = new CodeBlock();
                block.Leading = string.Format("{0} {1} {2}", modifiers, Type, Name);
                if (Get.NestedBlocks.Count > 0)
                {
                    if (this.IsAbstract)
                    {
                        block.Add("get;");
                    }
                    else
                    {
                        block.Add(Get);
                    }
                }
                if (Set.NestedBlocks.Count > 0)
                {
                    if (this.IsAbstract)
                    {
                        block.Add("set;");
                    }
                    else
                    {
                        block.Add(Set);
                    }
                }
                return block;
            }
        }
    }
}
