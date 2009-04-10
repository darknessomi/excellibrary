using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeGen
{
    public class CodeBlock : ICodeBlock
    {
        public string Leading;
        public List<ICodeBlock> NestedBlocks;

        public CodeBlock OuterBlock;
        // <name, type> pairs
        public Dictionary<string, string> Variables = new Dictionary<string, string>();

        public bool IsEmpty
        {
            get { return NestedBlocks.Count == 0; }
        }

        public CodeBlock()
        {
            NestedBlocks = new List<ICodeBlock>();
        }

        public CodeBlock(string leading)
        {
            Leading = leading;
            NestedBlocks = new List<ICodeBlock>();
        }

        public void Add(string statement)
        {
            NestedBlocks.Add(new CodeLine(statement));
        }

        public void AddRange(List<ICodeBlock> nestedBlocks)
        {
            foreach (ICodeBlock block in nestedBlocks)
            {
                this.Add(block);
            }
        }

        public void Add(ICodeBlock block)
        {
            if (block is CodeBlock)
            {
                CodeBlock codeBlock = block as CodeBlock;
                if (codeBlock.Leading == null)
                {
                    this.AddRange(codeBlock.NestedBlocks);
                    return;
                }
            }
            NestedBlocks.Add(block);
        }

        public void AddBlankLine()
        {
            NestedBlocks.Add(new BlankLine());
        }

        public bool VariableDeclared(string name)
        {
            CodeBlock block = this;
            while (block != null)
            {
                if (block.Variables.ContainsKey(name))
                {
                    return true;
                }
                block = block.OuterBlock;
            }
            return false;
        }

        private void CheckVariableDeclarations()
        {
            foreach (ICodeBlock block in NestedBlocks)
            {
                if (block is VariableDeclaration)
                {
                    VariableDeclaration varDeclare = block as VariableDeclaration;
                    if (this.VariableDeclared(varDeclare.VariableName))
                    {
                        varDeclare.AlreadyDeclared = true;
                    }
                    else
                    {
                        this.Variables.Add(varDeclare.VariableName, varDeclare.TypeName);
                    }
                }
                else if (block is CodeBlock)
                {
                    CodeBlock codeBlock = block as CodeBlock;
                    codeBlock.OuterBlock = this;
                }
            }
        }

        public enum ForamttingStyle { Block, Line }
        public ForamttingStyle Style = ForamttingStyle.Block;

        public override void Output(CodeWriter writer)
        {
            this.CheckVariableDeclarations();

            base.Output(writer);

            if (Style == ForamttingStyle.Line && IsAllCodeLines())
            {
                StringBuilder text = new StringBuilder();
                text.Append(Leading);
                text.Append(" {");
                foreach (CodeLine codeLine in NestedBlocks)
                {
                    text.Append(" ");
                    text.Append(codeLine.Text);
                }
                text.Append(" }");
                writer.WriteLine(text.ToString());
            }
            else
            {
                if (Leading != null)
                {
                    writer.WriteLine(Leading);
                    writer.WriteLine("{");
                    writer.Indent++;
                }
                foreach (ICodeBlock block in NestedBlocks)
                {
                    block.Output(writer);
                }
                if (Leading != null)
                {
                    writer.Indent--;
                    writer.WriteLine("}");
                }
            }
        }

        private bool IsAllCodeLines()
        {
            foreach (ICodeBlock block in NestedBlocks)
            {
                if (!(block is CodeLine))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
