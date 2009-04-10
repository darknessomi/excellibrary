using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;
using QiHe.CodeGen;

namespace ExcelLibrary.Tool
{
    public class ExcelRecord
    {
        public static Class BuildClass(Namespace ns, Record record, Dictionary<string, Record> AllRecords)
        {
            ns.Usings.Add("System.IO");
            if (record.Fields.Exists(delegate(RecordField member)
            {
                return member.Type.StartsWith("FastSearchList<");
            }))
            {
                ns.Usings.Add("QiHe.CodeLib");
            }
            if (record.IsAbstract)
            {
                return BuildBaseClass(record, AllRecords);
            }
            else
            {
                return BuildClass(record, AllRecords);
            }
        }

        private static Class BuildBaseClass(Record record, Dictionary<string, Record> AllRecords)
        {
            string className = record.Name;
            Class baseClass = new Class(className);
            baseClass.Modifiers.Add("public");
            baseClass.Modifiers.Add("partial");
            if (record.Parent != null)
            {
                baseClass.BaseClass = record.Parent;
                baseClass.Constructors.Add(String.Format("public {0}() {{ }}", className));
                baseClass.Constructors.Add(String.Format(
                    "public {0}({1} record) : base(record) {{ }}",
                    className, baseClass.BaseClass
                    ));
                if (record.Fields.Count > 0)
                {
                    foreach (RecordField member in record.Fields)
                    {
                        Field field = new Field(member.Type, member.Name);
                        field.Modifiers.Add("public");
                        if (member.Description != null)
                        {
                            field.Summary = member.Description;
                        }
                        baseClass.Members.Add(field);
                    }
                }
            }
            else
            {
                Method method = new Method(className, "Read");
                method.Modifiers.Add("public");
                method.Modifiers.Add("static");
                method.Params.Add(new Parameter("Stream", "stream"));

                method.MethodBody.Add(String.Format("{0} record = {0}.ReadBase(stream);", className));
                CodeBlock switchblock = new CodeBlock("switch (record.Type)");
                AddChildCase(record, className, switchblock);
                CodeLines defaultblock = new CodeLines();
                defaultblock.Lines.Add("default:");
                defaultblock.Lines.Add("\treturn record;");
                switchblock.Add(defaultblock);
                method.MethodBody.Add(switchblock);

                baseClass.Members.Add(method);
            }
            return baseClass;
        }

        private static void AddChildCase(Record record, string className, CodeBlock switchblock)
        {
            foreach (Record childRecord in record.ChildRecords)
            {
                if (!childRecord.IsAbstract)
                {
                    string name = childRecord.Name;
                    CodeLines caseblock = new CodeLines();
                    caseblock.Lines.Add(String.Format("case {0}Type.{1}:", className, name));
                    caseblock.Lines.Add(String.Format("\treturn new {0}(record);", name));
                    switchblock.Add(caseblock);
                }
                else
                {
                    AddChildCase(childRecord, className, switchblock);
                }
            }
        }

        public static Class BuildClass(Record record, Dictionary<string, Record> allRecords)
        {
            string className = record.Name;
            string baseName = record.Parent;
            while (allRecords[baseName].Parent != null)
            {
                baseName = allRecords[baseName].Parent;
            }
            Class elementClass = new Class(className);
            elementClass.Summary = record.Description;
            elementClass.Modifiers.Add("public");
            elementClass.Modifiers.Add("partial");
            elementClass.BaseClass = record.Parent;
            elementClass.Constructors.Add(
                String.Format("public {0}({1} record) : base(record) {{ }}", className, baseName));

            Constructor constructor = new Constructor(elementClass);
            constructor.MethodBody.Add(String.Format("this.Type = {0}Type.{1};", baseName, className));
            foreach (RecordField member in record.Fields)
            {
                if (member.Type.StartsWith("List<") || member.Type.StartsWith("FastSearchList<"))
                {
                    constructor.MethodBody.Add(String.Format("this.{0} = new {1}();", member.Name, member.Type));
                }
            }
            elementClass.Members.Add(constructor);

            foreach (RecordField member in record.Fields)
            {
                Field field = new Field(member.Type, member.Name);
                field.Modifiers.Add("public");
                if (member.Description != null)
                {
                    field.Summary = member.Description;
                }
                elementClass.Members.Add(field);
            }
            List<RecordField> members = GetAllMembers(record, allRecords);
            if (members.Count > 0)
            {
                elementClass.Members.Add(DecodeMethod(record, baseName, members));
                elementClass.Members.Add(EncodeMethod(record, baseName, members));
            }
            return elementClass;
        }

        private static List<RecordField> GetAllMembers(Record record, Dictionary<string, Record> allRecords)
        {
            List<RecordField> members = new List<RecordField>();
            string baseName = record.Parent;
            while (baseName != null)
            {
                Record parent = allRecords[baseName];
                members.AddRange(parent.Fields);
                baseName = parent.Parent;
            }
            members.AddRange(record.Fields);
            return members;
        }

        private static Method DecodeMethod(Record record, string baseName, List<RecordField> members)
        {
            Method method = new Method("void", "Decode");
            method.Modifiers.Add("public");
            if (record.IsCutomized)
            {
                method.Name = "decode";
            }
            else
            {
                method.Modifiers.Add("override");
            }

            method.MethodBody.Add("MemoryStream stream = new MemoryStream(Data);");
            method.MethodBody.Add("BinaryReader reader = new BinaryReader(stream);");

            foreach (RecordField member in members)
            {
                string typeName = member.Type;
                if (typeName.StartsWith("List<") || typeName.StartsWith("FastSearchList<"))
                {
                    method.MethodBody.Add(String.Format("int count = {0};", member.ExtraInfo));
                    method.MethodBody.Add(String.Format("this.{0} = new {1}(count);", member.Name, typeName));
                    typeName = StringHelper.GetSubStringBetween(typeName, '<', '>');
                    string format = "{0}.Add(" + GetReadingCode(typeName, "16") + ");";
                    CodeBlock for_loop = new CodeBlock("for (int i = 0; i < count; i++)");
                    for_loop.Add(String.Format(format, member.Name));
                    method.MethodBody.Add(for_loop);
                }
                else
                {
                    string format = "this.{0} = " + GetReadingCode(member.Type, member.ExtraInfo) + ";";
                    method.MethodBody.Add(String.Format(format, member.Name));
                }
            }
            return method;
        }

        private static string GetReadingCode(string typeName, string extraInfo)
        {
            switch (typeName)
            {
                case "String":
                    return "this.ReadString(reader, " + extraInfo + ")";
                case "Byte[]":
                    return "reader.ReadBytes((int)(stream.Length - stream.Position))";
                case "Guid":
                    return "new Guid(reader.ReadBytes(16))";
                case "StringOffset":
                    return "ReadStringOffset(reader)";
                default:
                    string code = "reader.Read{0}()";
                    return String.Format(code, typeName);
            }
        }

        private static Method EncodeMethod(Record record, string baseName, List<RecordField> members)
        {
            Method method = new Method("void", "Encode");
            method.Modifiers.Add("public");
            if (record.IsCutomized)
            {
                method.Name = "encode";
            }
            else
            {
                method.Modifiers.Add("override");
            }

            method.MethodBody.Add("MemoryStream stream = new MemoryStream();");
            method.MethodBody.Add("BinaryWriter writer = new BinaryWriter(stream);");

            foreach (RecordField member in members)
            {
                string typeName = member.Type;
                string format = "writer.Write({0});";
                switch (typeName)
                {
                    case "String":
                        format = "Record.WriteString(writer, {0}," + member.ExtraInfo + ");";
                        break;
                    case "Guid":
                        format = "writer.Write({0}.ToByteArray());";
                        break;
                    case "List<StringOffset>":
                        format = "WriteStringOffset(writer, {0});";
                        break;
                }
                if (typeName.StartsWith("List<") || typeName.StartsWith("FastSearchList<"))
                {
                    typeName = StringHelper.GetSubStringBetween(typeName, '<', '>');
                    CodeBlock foreach_block = new CodeBlock();
                    string loopVar = typeName.ToLower() + "Var";
                    foreach_block.Leading = String.Format("foreach({0} {1} in {2})", typeName, loopVar, member.Name);
                    foreach_block.Add(String.Format(format, loopVar));
                    method.MethodBody.Add(foreach_block);
                }
                else
                {
                    method.MethodBody.Add(String.Format(format, member.Name));
                }
            }

            string sizeType = baseName == "EscherRecord" ? "UInt32" : "UInt16";

            method.MethodBody.Add("this.Data = stream.ToArray();");
            method.MethodBody.Add("this.Size = (" + sizeType + ")Data.Length;");
            method.MethodBody.Add("base.Encode();");

            return method;
        }
    }
}
