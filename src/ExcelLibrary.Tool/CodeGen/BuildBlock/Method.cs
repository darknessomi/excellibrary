using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.CodeGen
{
    internal struct Parameter
    {
        public string Name;
        public string Type;

        public Parameter(string type, string name)
        {
            Type = CSharp.Identifier(type);
            Name = CSharp.Identifier(name);
        }

        public override string ToString()
        {
            return Type + " " + Name;
        }
    }
    public class Method : ClassMember
    {
        internal List<Parameter> Params;

        public CodeBlock MethodBody;

        public Method(string type, string name)
            : base(type, name)
        {
            Params = new List<Parameter>();
            MethodBody = new CodeBlock();
        }

        public void AddParameter(string type, string name)
        {
            Params.Add(new Parameter(type, name));
        }

        protected string parameters
        {
            get
            {
                return StringHelper.ContactWithDelim(Params, ", ", "(", ")");
            }
        }

        public override ICodeBlock CodeBlock
        {
            get
            {
                string[] parts = new string[] { modifiers, Type, Name + parameters };
                if (IsAbstract)
                {
                    return new CodeLine(StringHelper.ContactWithDelimSkipEmpty(parts, " ") + ";");
                }
                else
                {
                    MethodBody.Leading = StringHelper.ContactWithDelimSkipEmpty(parts, " ");
                    return MethodBody;
                }
            }
        }
    }
}
