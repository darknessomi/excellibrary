using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class Constructor : Method
    {
        public string HeadCall;

        public Constructor(string classname)
            : base(null, classname)
        {
            Modifiers.Add("public");
        }

        public Constructor(Class whichclass)
            : base(null, whichclass.Name)
        {
            Modifiers.Add("public");
        }

        public override ICodeBlock CodeBlock
        {
            get
            {
                string headCall = String.IsNullOrEmpty(HeadCall) ? null : " : " + HeadCall;
                MethodBody.Leading = string.Format("{0} {1}{2}{3}", modifiers, Name, parameters, headCall);
                return MethodBody;
            }
        }
    }
}
