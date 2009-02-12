using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.Office.Excel
{
    public partial class Record
    {
        public UInt16 Type;
        public UInt16 Size;
        public byte[] Data;

        public List<Record> ContinuedRecords;

        public const UInt16 MaxContentLength = 8224;

        public Record()
        {
            ContinuedRecords = new List<Record>();
        }

        public Record(Record record)
        {
            Type = record.Type;
            Size = record.Size;
            Data = record.Data;
            ContinuedRecords = record.ContinuedRecords;
        }

        public virtual void Decode()
        {
        }

        public virtual void Encode()
        {
            this.ContinuedRecords.Clear();
            if (Size > 0 && Data.Length > MaxContentLength)
            {
                int index = MaxContentLength;
                while (index < Data.Length)
                {
                    CONTINUE continuedRecord = new CONTINUE();
                    int size = Math.Min(MaxContentLength, Data.Length - index);
                    continuedRecord.Data = Algorithm.ArraySection(Data, index, size);
                    continuedRecord.Size = (ushort)size;
                    this.ContinuedRecords.Add(continuedRecord);
                    index += size;
                }
                this.Size = MaxContentLength;
                this.Data = Algorithm.ArraySection(Data, 0, MaxContentLength);
            }
        }

        public static Record ReadBase(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            Record record = new Record();
            record.Type = reader.ReadUInt16();
            record.Size = reader.ReadUInt16();
            record.Data = reader.ReadBytes(record.Size);
            return record;
        }

        /// <summary>
        /// Data size plus header size
        /// </summary>
        public int FullSize
        {
            get
            {
                int full_size = 4 + Size;
                foreach (Record record in ContinuedRecords)
                {
                    full_size += 4 + record.Size;
                }
                return full_size;
            }
        }

        public int TotalSize
        {
            get
            {
                int total_size = Size;
                foreach (Record record in ContinuedRecords)
                {
                    total_size += record.Size;
                }
                return total_size;
            }
        }

        public byte[] AllData
        {
            get
            {
                if (ContinuedRecords.Count == 0) return Data;
                else
                {
                    List<byte> data = new List<byte>(TotalSize);
                    data.AddRange(Data);
                    foreach (Record record in ContinuedRecords)
                    {
                        data.AddRange(record.AllData);
                    }
                    return data.ToArray();
                }
            }
        }

        protected int ContinuedIndex = -1;
        public string ReadString(BinaryReader reader, int lengthbits)
        {
            BinaryReader continuedReader;
            return ReadString(reader, lengthbits, out continuedReader);
        }

        public string ReadString(BinaryReader reader, int lengthbits, out BinaryReader continuedReader)
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                if (ContinuedIndex < ContinuedRecords.Count - 1)
                {
                    reader = SwitchToContinuedRecord();
                }
                else
                {
                    continuedReader = reader;
                    return null;
                }
            }
            int stringlength = lengthbits == 8 ? reader.ReadByte() : reader.ReadUInt16();
            byte option = reader.ReadByte();
            bool compressed = (option & 0x01) == 0;
            bool phonetic = (option & 0x04) == 0x04;
            bool richtext = (option & 0x08) == 0x08;
            int runs = 0; //Number of Rich-Text formatting runs
            int size = 0; //Size of Asian phonetic settings block in bytes
            if (richtext)
            {
                runs = reader.ReadUInt16();
            }
            if (phonetic)
            {
                size = reader.ReadInt32();
            }

            string firstpart = ReadString(reader, stringlength, compressed);
            StringBuilder text = new StringBuilder();
            text.Append(firstpart);
            continuedReader = reader;
            if (firstpart.Length < stringlength)
            {
                continuedReader = SwitchToContinuedRecord();
                text.Append(ReadContinuedString(continuedReader, stringlength - firstpart.Length, out continuedReader));
            }
            ReadBytes(continuedReader, 4 * runs + size);
            return text.ToString();
        }

        private string ReadContinuedString(BinaryReader reader, int stringlength, out BinaryReader continuedReader)
        {
            continuedReader = reader;
            if (reader.BaseStream.Position == reader.BaseStream.Length) return null;
            byte option = reader.ReadByte();
            bool compressed = (option & 0x01) == 0;
            string firstpart = ReadString(reader, stringlength, compressed);
            if (firstpart.Length < stringlength)
            {
                continuedReader = SwitchToContinuedRecord();
                StringBuilder text = new StringBuilder();
                text.Append(firstpart);
                text.Append(ReadContinuedString(continuedReader, stringlength - firstpart.Length, out continuedReader));
                return text.ToString();
            }
            else
            {
                return firstpart;
            }
        }

        private static string ReadString(BinaryReader reader, int stringlength, bool compressed)
        {
            byte[] textData;
            if (compressed)
            {
                byte[] bytes = reader.ReadBytes(stringlength);
                textData = new byte[bytes.Length * 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    textData[i * 2] = bytes[i];
                    textData[i * 2 + 1] = 0;
                }
            }
            else
            {
                textData = reader.ReadBytes(stringlength * 2);
            }
            return Encoding.Unicode.GetString(textData);
        }

        protected byte[] ReadBytes(BinaryReader reader, int count)
        {
            byte[] bytes = reader.ReadBytes(count);
            int bytesRead = bytes.Length;
            if (bytesRead < count)
            {
                byte[] allbytes = new byte[count];
                byte[] remainedbytes = ReadBytes(SwitchToContinuedRecord(), count - bytesRead);
                bytes.CopyTo(allbytes, 0);
                remainedbytes.CopyTo(allbytes, bytesRead);
                return allbytes;
            }
            return bytes;
        }

        protected BinaryReader SwitchToContinuedRecord()
        {
            ContinuedIndex++;
            MemoryStream stream = new MemoryStream(ContinuedRecords[ContinuedIndex].Data);
            return new BinaryReader(stream);
        }

        public static object DecodeRK(uint value)
        {
            bool muled = (value & 0x01) == 1;
            bool isFloat = (value & 0x02) == 0;
            if (isFloat)
            {
                UInt64 data = ((UInt64)(value & 0xFFFFFFFC)) << 32;
                double num = TreatUInt64AsDouble(data);
                if (muled) num /= 100;
                return num;
            }
            else
            {
                Int32 num = (int)(value & 0xFFFFFFFC) >> 2;
                if (muled)
                {
                    return (decimal)num / 100;
                }
                return num;
            }
        }

        public static double TreatUInt64AsDouble(UInt64 data)
        {
            //byte[] bytes = new byte[8];
            //MemoryStream stream = new MemoryStream(bytes);
            //BinaryWriter writer = new BinaryWriter(stream);
            //BinaryReader reader = new BinaryReader(stream);
            //writer.Write(data);
            //stream.Position = 0;
            //return reader.ReadDouble();

            byte[] bytes = BitConverter.GetBytes(data);
            return BitConverter.ToDouble(bytes, 0);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Type);
            writer.Write(this.Size);
            if (this.Size > 0)
            {
                writer.Write(this.Data);
                if (this.ContinuedRecords.Count > 0)
                {
                    foreach (Record record in ContinuedRecords)
                    {
                        writer.Write(record.Type);
                        writer.Write(record.Size);
                        writer.Write(record.Data);
                    }
                }
            }
        }

        public static void WriteString(BinaryWriter writer, string text, int lengthbits)
        {
            if (lengthbits == 8)
            {
                writer.Write((byte)text.Length);
            }
            else if (lengthbits == 16)
            {
                writer.Write((ushort)text.Length);
            }
            else
            {
                throw new ArgumentException("Invalid lengthbits, must be 8 or 16.");
            }

            if (TextEncoding.FitsInASCIIEncoding(text))
            {
                writer.Write((byte)0);
                writer.Write(Encoding.ASCII.GetBytes(text));
            }
            else
            {
                writer.Write((byte)1);
                writer.Write(Encoding.Unicode.GetBytes(text));
            }
        }

        public static int GetStringDataLength(string text)
        {
            if (TextEncoding.FitsInASCIIEncoding(text))
            {
                return Encoding.ASCII.GetByteCount(text) + 3;
            }
            else
            {
                return text.Length * 2 + 3;
            }
        }

        public static void EncodeRecords(List<Record> records)
        {
            foreach (Record record in records)
            {
                record.Encode();
            }
        }

        public static int CountDataLength(List<Record> records)
        {
            int dataLength = 0;
            foreach (Record record in records)
            {
                dataLength += record.FullSize;
            }
            return dataLength;
        }
    }
}
