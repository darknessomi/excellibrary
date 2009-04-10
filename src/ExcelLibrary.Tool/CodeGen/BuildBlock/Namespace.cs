using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class Namespace : BuildingBlock
    {
        public string Name;

        public List<string> Usings;

        public List<BuildingBlock> Classes;

        public Namespace()
        {
            Usings = new List<string>();
            Classes = new List<BuildingBlock>();

            Usings.AddRange(
                new string[]{
                    "System",
                    "System.Collections.Generic",
                    "System.Text"
            });
        }

        public Namespace(string name)
            : this()
        {
            Name = name;
        }

        public void AddClass(Class @class)
        {
            this.Usings.AddRange(@class.Usings);
            this.Classes.Add(@class);
        }

        public override ICodeBlock CodeBlock
        {
            get
            {
                CodeBlock block = new CodeBlock();
                if (Name != null)
                {
                    block.Leading = "namespace " + Name;
                }
                foreach (BuildingBlock @class in Classes)
                {
                    block.Add(@class.Header);
                    block.Add(@class.CodeBlock);
                }
                return block;
            }
        }

        void WriteUsings(CodeWriter writer)
        {
            foreach (string ns in Usings)
            {
                writer.WriteLine(string.Format("using {0};", ns));
            }
            writer.WriteLine();
        }

        public override void Output(CodeWriter writer)
        {
            WriteUsings(writer);
            base.Output(writer);
        }

        public void Output(string path)
        {
            CodeWriter writer = new CodeWriter(path);
            this.Output(writer);
            writer.Close();
        }
    }
}
