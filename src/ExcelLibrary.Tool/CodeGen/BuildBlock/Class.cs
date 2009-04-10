using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.CodeGen
{
    public class Class : ClassMember
    {
        public List<string> Usings;

        public string BaseClass;

        public string GenericConstraints;

        public List<string> Constructors;

        public List<ClassMember> Members;

        public Class(string name)
        {
            Name = CSharp.Identifier(name);
            Usings = new List<string>();
            Members = new List<ClassMember>();
            Constructors = new List<string>();

            Modifiers = new List<string>();
        }


        public override ICodeBlock CodeBlock
        {
            get
            {
                string head = Name;
                if (!string.IsNullOrEmpty(BaseClass))
                {
                    head += " : " + BaseClass;
                }
                if (!string.IsNullOrEmpty(GenericConstraints))
                {
                    head += GenericConstraints;
                }
                string modifier = StringHelper.ContactWithDelim(Modifiers, " ", null);

                CodeBlock block = new CodeBlock();
                block.Leading = modifier + " class " + head;

                foreach (string constructor in Constructors)
                {
                    block.Add(constructor);
                    block.AddBlankLine();
                }

                foreach (BuildingBlock member in Members)
                {
                    block.Add(member.Header);
                    block.Add(member.CodeBlock);
                    block.AddBlankLine();
                }
                return block;
            }
        }
    }
}
