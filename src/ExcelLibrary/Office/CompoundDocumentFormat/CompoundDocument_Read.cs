using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public partial class CompoundDocument
    {
        public static CompoundDocument Read(string file)
        {
            CompoundDocument doc = CompoundDocument.Open(file);
            doc.ReadAllStreamData();
            doc.Close();
            return doc;
        }

        private static FileHeader ReadHeader(BinaryReader reader)
        {
            FileHeader header = new FileHeader();
            header.FileTypeIdentifier = reader.ReadBytes(8);
            header.FileIdentifier = new Guid(reader.ReadBytes(16));
            header.RevisionNumber = reader.ReadUInt16();
            header.VersionNumber = reader.ReadUInt16();
            header.ByteOrderMark = reader.ReadBytes(2);
            header.SectorSizeInPot = reader.ReadUInt16();
            header.ShortSectorSizeInPot = reader.ReadUInt16();
            header.UnUsed10 = reader.ReadBytes(10);
            header.NumberOfSATSectors = reader.ReadInt32();
            header.FirstSectorIDofDirectoryStream = reader.ReadInt32();
            header.UnUsed4 = reader.ReadBytes(4);
            header.MinimumStreamSize = reader.ReadInt32();
            header.FirstSectorIDofShortSectorAllocationTable = reader.ReadInt32();
            header.NumberOfShortSectors = reader.ReadInt32();
            header.FirstSectorIDofMasterSectorAllocationTable = reader.ReadInt32();
            header.NumberOfMasterSectors = reader.ReadInt32();
            header.MasterSectorAllocationTable = ReadArrayOfInt32(reader, 109);
            return header;
        }

        private static DirectoryEntry ReadDirectoryEntry(BinaryReader reader)
        {
            DirectoryEntry entry = new DirectoryEntry();
            entry.NameBuffer = reader.ReadChars(32);
            entry.NameDataSize = reader.ReadUInt16();
            entry.EntryType = reader.ReadByte();
            entry.NodeColor = reader.ReadByte();
            entry.LeftChildDID = reader.ReadInt32();
            entry.RightChildDID = reader.ReadInt32();
            entry.MembersTreeNodeDID = reader.ReadInt32();
            entry.UniqueIdentifier = new Guid(reader.ReadBytes(16));
            entry.UserFlags = reader.ReadInt32();
            entry.CreationTime = DateTime.FromFileTime(reader.ReadInt64());
            entry.LastModificationTime = DateTime.FromFileTime(reader.ReadInt64());
            entry.FirstSectorID = reader.ReadInt32();
            entry.StreamLength = reader.ReadInt32();
            entry.UnUsed = reader.ReadInt32();
            return entry;
        }

        static bool ArrayEqual(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1.Length != bytes2.Length) return false;
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i]) return false;
            }
            return true;
        }

        private void ReadDirectoryEntries()
        {
            DirectoryStream = new MemoryStream(GetStreamDataAsBytes(Header.FirstSectorIDofDirectoryStream));
            BinaryReader reader = new BinaryReader(DirectoryStream, Encoding.Unicode);
            DirectoryEntries = new Dictionary<int, DirectoryEntry>();
            DirectoryEntry root = ReadDirectoryEntry(reader);
            root.Document = this;
            root.ID = 0;
            DirectoryEntries.Add(0, root);

            // The first sector has to be obtained from the root storage entry in the directory
            ShortStreamContainer = new MemoryStream(GetStreamDataAsBytes(root.FirstSectorID, root.StreamLength));

            ReadDirectoryEntry(reader, root.MembersTreeNodeDID, root);
        }

        private void ReadDirectoryEntry(BinaryReader reader, int DID, DirectoryEntry parent)
        {
            if (DID != -1 && !DirectoryEntries.ContainsKey(DID))
            {
                reader.BaseStream.Position = DID * 128;
                DirectoryEntry entry = ReadDirectoryEntry(reader);
                entry.Document = this;
                entry.ID = DID;
                DirectoryEntries[DID] = entry;
                parent.AddChild(entry);
                ReadDirectoryEntry(reader, entry.LeftChildDID, parent);
                ReadDirectoryEntry(reader, entry.RightChildDID, parent);
                ReadDirectoryEntry(reader, entry.MembersTreeNodeDID, entry);
            }
        }

        private void ReadAllStreamData()
        {
            foreach (DirectoryEntry entry in this.DirectoryEntries.Values)
            {
                entry.Data = GetStreamData(entry);
            }
        }

        private int GetSectorOffset(int SID)
        {
            return 512 + SectorSize * SID;
        }

        private int GetShortSectorOffset(int SSID)
        {
            return ShortSectorSize * SSID;
        }

        internal int[] ReadSectorDataAsIntegers(int SID)
        {
            int offset = GetSectorOffset(SID);
            Reader.BaseStream.Position = offset;
            return ReadArrayOfInt32(Reader, SectorSize / 4);
        }

        private byte[] ReadSectorDataAsBytes(int SID)
        {
            int offset = GetSectorOffset(SID);
            Reader.BaseStream.Position = offset;
            return Reader.ReadBytes(SectorSize);
        }

        private byte[] ReadShortSectorDataAsBytes(int SSID)
        {
            int offset = GetShortSectorOffset(SSID);
            ShortStreamContainer.Seek(offset, SeekOrigin.Begin);
            return StreamHelper.ReadBytes(ShortStreamContainer, ShortSectorSize);
        }

        private byte[] GetStreamDataAsBytes(int StartSID)
        {
            List<int> chain = SectorAllocation.GetSIDChain(StartSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsBytes(sid));
            }
            return data.ToArray();
        }

        private byte[] GetStreamDataAsBytes(int StartSID, int length)
        {
            List<int> chain = SectorAllocation.GetSIDChain(StartSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsBytes(sid));
            }
            if (data.Count > length)
            {
                data.RemoveRange(length, data.Count - length);
            }
            return data.ToArray();
        }

        internal List<int> GetStreamDataAsIntegers(int StartSID)
        {
            List<int> chain = SectorAllocation.GetSIDChain(StartSID);
            List<int> data = new List<int>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsIntegers(sid));
            }
            return data;
        }

        private byte[] GetShortStreamDataAsBytes(int StartSSID)
        {
            List<int> chain = ShortSectorAllocation.GetSIDChain(StartSSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadShortSectorDataAsBytes(sid));
            }
            return data.ToArray();
        }

        private byte[] GetShortStreamDataAsBytes(int StartSSID, int length)
        {
            List<int> chain = ShortSectorAllocation.GetSIDChain(StartSSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadShortSectorDataAsBytes(sid));
            }
            if (data.Count > length)
            {
                data.RemoveRange(length, data.Count - length);
            }
            return data.ToArray();
        }

        public byte[] GetStreamData(DirectoryEntry entry)
        {
            if (entry.EntryType == EntryType.Stream)
            {
                if (entry.StreamLength < Header.MinimumStreamSize)
                {
                    return GetShortStreamDataAsBytes(entry.FirstSectorID, entry.StreamLength);
                }
                else
                {
                    return GetStreamDataAsBytes(entry.FirstSectorID, entry.StreamLength);
                }
            }
            return null;
        }

        public DirectoryEntry FindDirectoryEntry(DirectoryEntry entry, string entryName)
        {
            if (entry.Members.ContainsKey(entryName)) return entry.Members[entryName];
            foreach (DirectoryEntry subentry in entry.Members.Values)
            {
                return FindDirectoryEntry(subentry, entryName);
            }
            return null;
        }

        public byte[] GetStreamData(string streamName)
        {
            DirectoryEntry userstream = FindDirectoryEntry(RootStorage, streamName);
            if (userstream != null)
            {
                return userstream.Data;
            }
            return null;
        }
    }
}
