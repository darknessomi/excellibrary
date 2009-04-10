using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.CodeGen
{
    public abstract class ClassMember : BuildingBlock
    {
        public string Name;
        public string Type;

        public List<string> Modifiers;

        public bool IsAbstract;

        internal ClassMember() { }

        public ClassMember(string type, string name)
        {
            Type = type;
            Name = CSharp.Identifier(name);
            Modifiers = new List<string>();
        }

        public void AddModifier(string modifier)
        {
            if (modifier == "abstract")
            {
                IsAbstract = true;
            }
            Modifiers.Add(modifier);
        }

        public void SetModifiers(List<string> modifiers)
        {
            Modifiers.Clear();
            foreach (string modifier in modifiers)
            {
                this.AddModifier(modifier);
            }
        }

        protected string modifiers
        {
            get
            {
                return StringHelper.ContactWithDelim(Modifiers, " ");
            }
        }
    }
}
