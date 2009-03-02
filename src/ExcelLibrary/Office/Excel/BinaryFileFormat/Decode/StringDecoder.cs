using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public class StringDecoder
    {
        Record record;
        BinaryReader reader;
        int ContinuedIndex = -1;

        public StringDecoder(Record record, BinaryReader reader)
        {
            this.record = record;
            this.reader = reader;
        }

        public string ReadString(int lengthbits)
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                if (ContinuedIndex < record.ContinuedRecords.Count - 1)
                {
                    SwitchToContinuedRecord();
                }
                else
                {
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

            string firstpart = ReadString(stringlength, compressed);
            StringBuilder text = new StringBuilder();
            text.Append(firstpart);
            if (firstpart.Length < stringlength)
            {
                SwitchToContinuedRecord();
                text.Append(ReadContinuedString(stringlength - firstpart.Length));
            }
            ReadBytes(4 * runs + size);
            return text.ToString();
        }

        private string ReadString(int stringlength, bool compressed)
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

        private string ReadContinuedString(int stringlength)
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length) return null;
            byte option = reader.ReadByte();
            bool compressed = (option & 0x01) == 0;
            string firstpart = ReadString(stringlength, compressed);
            if (firstpart.Length < stringlength)
            {
                SwitchToContinuedRecord();
                StringBuilder text = new StringBuilder();
                text.Append(firstpart);
                text.Append(ReadContinuedString(stringlength - firstpart.Length));
                return text.ToString();
            }
            else
            {
                return firstpart;
            }
        }

        private byte[] ReadBytes(int count)
        {
            byte[] bytes = reader.ReadBytes(count);
            int bytesRead = bytes.Length;
            if (bytesRead < count)
            {
                SwitchToContinuedRecord();
                byte[] allbytes = new byte[count];
                byte[] remainedbytes = ReadBytes(count - bytesRead);
                bytes.CopyTo(allbytes, 0);
                remainedbytes.CopyTo(allbytes, bytesRead);
                return allbytes;
            }
            return bytes;
        }

        private void SwitchToContinuedRecord()
        {
            ContinuedIndex++;
            MemoryStream stream = new MemoryStream(record.ContinuedRecords[ContinuedIndex].Data);
            reader = new BinaryReader(stream);
        }

        public string ReadString(int lengthbits, out RichTextFormat rtf)
        {
            /* BEGIN - SAME AS ReadString() */
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                if (ContinuedIndex < record.ContinuedRecords.Count - 1)
                {
                    SwitchToContinuedRecord();
                }
                else
                {
                    rtf = null;
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

            string firstpart = ReadString(stringlength, compressed);
            StringBuilder text = new StringBuilder();
            text.Append(firstpart);
            if (firstpart.Length < stringlength)
            {
                SwitchToContinuedRecord();
                text.Append(ReadContinuedString(stringlength - firstpart.Length));
            }
            /* END - SAME AS ReadString() */

            /* 
             * Added, 2-13-2009
             * Sunil Shenoi
             * 
             * Process the rich text formatting information as in Section 2.5.3.
             */
            byte[] richTextBytes = ReadBytes(4 * runs + size);
            rtf = DecodeRichTextFormatting(richTextBytes, runs);

            return text.ToString();
        }

        /*
         * Added, 2-13-2009
         * Sunil Shenoi
         * 
         * Decode the rich text formatting information associated with a given string.
         */
        private RichTextFormat DecodeRichTextFormatting(byte[] richTextBytes, int runs)
        {
            RichTextFormat rtf = new RichTextFormat(runs);

            // process the byte array into pairs of UInt16's
            for (int i = 0; i < runs; i++)
            {
                rtf.CharIndexes.Add(BitConverter.ToUInt16(richTextBytes, (i * 4)));
                rtf.FontIndexes.Add(BitConverter.ToUInt16(richTextBytes, (i * 4) + 2));
            }

            return rtf;
        }
    }
}
