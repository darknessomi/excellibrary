using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.CodeGen
{
    public class Field : ClassMember
    {
        public string Value;

        public Field(string type, string name)
            : base(type, name)
        {
        }

        public Field(string access, string type, string name)
            : base(type, name)
        {
            this.Modifiers.Add(access);
        }

        public override ICodeBlock CodeBlock
        {
            get
            {
                CodeLines code = new CodeLines();
                string[] parts = new string[] { modifiers, Type, Name };
                string line = StringHelper.ContactWithDelimSkipEmpty(parts, " ");
                if (this.Value == null)
                {
                    code.Lines.Add(line + ";");
                }
                else
                {
                    code.Lines.Add(String.Format("{0} = {1};", line, Value));
                }
                return code;
            }
        }
    }
}
