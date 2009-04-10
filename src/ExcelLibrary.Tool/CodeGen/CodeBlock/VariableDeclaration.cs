using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class VariableDeclaration : ICodeBlock
    {
        public string TypeName;
        public string VariableName;
        public string Value;
        public bool AsignNewValue;
        public bool AlreadyDeclared;

        public VariableDeclaration(string type, string varName, string defaultValue)
        {
            TypeName = type;
            VariableName = varName;
            Value = defaultValue;
            AsignNewValue = true;
        }

        public VariableDeclaration(string type, string varName, string defaultValue, bool asingNewValue)
        {
            TypeName = type;
            VariableName = varName;
            Value = defaultValue;
            AsignNewValue = asingNewValue;
        }

        public override void Output(CodeWriter writer)
        {
            if (AlreadyDeclared)
            {
                if (AsignNewValue)
                {
                    base.Output(writer);
                    writer.WriteLine(String.Format("{0} = {1};", VariableName, Value));
                }
            }
            else
            {
                base.Output(writer);
                if (Value != null)
                {
                    writer.WriteLine(String.Format("{0} {1} = {2};", TypeName, VariableName, Value));
                }
                else
                {
                    writer.WriteLine(String.Format("{0} {1};", TypeName, VariableName));
                }
            }
        }
    }
}
