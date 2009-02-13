using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public partial class CompoundDocument
    {
        private void WriteHeader()
        {
            this.FileStorage.Position = 0;
            WriteHeader(this.Writer, this.Header);
        }

        private static void WriteHeader(BinaryWriter writer, FileHeader header)
        {
            // header size is 512 bytes
            writer.Write(header.FileTypeIdentifier);
            writer.Write(header.FileIdentifier.ToByteArray());
            writer.Write(header.RevisionNumber);
            writer.Write(header.VersionNumber);
            writer.Write(header.ByteOrderMark);
            writer.Write(header.SectorSizeInPot);
            writer.Write(header.ShortSectorSizeInPot);
            writer.Write(header.UnUsed10);
            writer.Write(header.NumberOfSATSectors);
            writer.Write(header.FirstSectorIDofDirectoryStream);
            writer.Write(header.UnUsed4);
            writer.Write(header.MinimumStreamSize);
            writer.Write(header.FirstSectorIDofShortSectorAllocationTable);
            writer.Write(header.NumberOfShortSectors);
            writer.Write(header.FirstSectorIDofMasterSectorAllocationTable);
            writer.Write(header.NumberOfMasterSectors);
            WriteArrayOfInt32(writer, header.MasterSectorAllocationTable);
        }

        private static void WriteDirectoryEntry(BinaryWriter writer, DirectoryEntry entry)
        {
            writer.Write(entry.NameBuffer);
            writer.Write(entry.NameDataSize);
            writer.Write(entry.EntryType);
            writer.Write(entry.NodeColor);
            writer.Write(entry.LeftChildDID);
            writer.Write(entry.RightChildDID);
            writer.Write(entry.MembersTreeNodeDID);
            writer.Write(entry.UniqueIdentifier.ToByteArray());
            writer.Write(entry.UserFlags);
            writer.Write(entry.CreationTime.ToFileTime());
            writer.Write(entry.LastModificationTime.ToFileTime());
            writer.Write(entry.FirstSectorID);
            writer.Write(entry.StreamLength);
            writer.Write(entry.UnUsed);
        }

        //void WriteMasterSectorAllocationTable(BinaryWriter writer)
        //{
        //    int extraIDcount = MasterSectorAllocationTable.Count - Header.MasterSectorAllocationTable.Length;
        //    if (extraIDcount <= 0)
        //    {
        //        Header.FirstSectorIDofMasterSectorAllocationTable = SID.EOC;
        //        for (int i = 0; i < MasterSectorAllocationTable.Count; i++)
        //        {
        //            Header.MasterSectorAllocationTable[i] = MasterSectorAllocationTable[i];
        //        }
        //    }
        //    else
        //    {
        //        Header.FirstSectorIDofMasterSectorAllocationTable = 0;
        //        for (int i = 0; i < Header.MasterSectorAllocationTable.Length; i++)
        //        {
        //            Header.MasterSectorAllocationTable[i] = MasterSectorAllocationTable[i];
        //        }
        //        int sectorIDcount = SectorSize / 4 - 1;
        //        int sectorIndex = Header.MasterSectorAllocationTable.Length;
        //        int nextSectID = 1;
        //        while (extraIDcount > sectorIDcount)
        //        {
        //            for (int i = 0; i < sectorIDcount; i++)
        //            {
        //                writer.Write(MasterSectorAllocationTable[sectorIndex]);
        //                sectorIndex++;
        //            }
        //            writer.Write(nextSectID);
        //            nextSectID++;
        //            extraIDcount -= sectorIDcount;
        //        }
        //        for (int i = 0; i < extraIDcount; i++)
        //        {
        //            writer.Write(MasterSectorAllocationTable[sectorIndex]);
        //            sectorIndex++;
        //        }
        //        for (int i = 0; i < sectorIDcount - extraIDcount; i++)
        //        {
        //            writer.Write(-1);
        //        }
        //        writer.Write(SID.EOC);
        //    }
        //}

        public void WriteStreamData(string[] streamPath, byte[] data)
        {
            DirectoryEntry entry = GetOrCreateDirectoryEntry(streamPath);
            entry.EntryType = EntryType.Stream;
            entry.StreamLength = data.Length;
            if (entry.StreamLength < Header.MinimumStreamSize)
            {
                if (entry.FirstSectorID == SID.EOC)
                {
                    entry.FirstSectorID = AllocateShortSector();
                }
                WriteShortStreamData(entry.FirstSectorID, data);
            }
            else
            {
                if (entry.FirstSectorID == SID.EOC)
                {
                    entry.FirstSectorID = AllocateDataSector();
                }
                WriteStreamData(entry.FirstSectorID, data);
            }
        }

        internal void WriteStreamData(int startSID, byte[] data)
        {
            int prev_sid = SID.EOC;
            int sid = startSID;
            int index = 0;
            while (index < data.Length)
            {
                if (sid == SID.EOC)
                {
                    if (prev_sid == SID.EOC)
                    {
                        sid = this.AllocateDataSector();
                    }
                    else
                    {
                        sid = this.AllocateDataSectorAfter(prev_sid);
                    }
                }
                int offset = GetSectorOffset(sid);
                Writer.BaseStream.Position = offset;
                if (index + SectorSize < data.Length)
                {
                    Writer.Write(data, index, SectorSize);
                }
                else
                {
                    Writer.Write(data, index, data.Length - index);
                }
                index += SectorSize;
                prev_sid = sid;
                sid = this.SectorAllocation.GetNextSectorID(prev_sid);
            }
            if (sid != SID.EOC && prev_sid != SID.EOC)
            {
                SectorAllocation.LinkSectorID(prev_sid, SID.EOC);
                while (sid != SID.EOC)
                {
                    int next_sid = SectorAllocation.GetNextSectorID(sid);
                    SectorAllocation.LinkSectorID(sid, SID.Free);
                    sid = next_sid;
                }
            }
        }

        private void AppendStreamData(int startSID, int streamLength, byte[] data)
        {
            int sid = startSID;
            int next_sid = SectorAllocation.GetNextSectorID(sid);
            int index = 0;
            while (next_sid != SID.EOC)
            {
                sid = next_sid;
                next_sid = SectorAllocation.GetNextSectorID(sid);
                index += SectorSize;
            }
            if (index < streamLength)
            {
                int position = streamLength - index;
                int length = SectorSize - position;
                if (data.Length <= length)
                {
                    WriteInSector(sid, position, data, 0, data.Length);
                }
                else
                {
                    WriteInSector(sid, position, data, 0, length);
                    next_sid = AllocateDataSectorAfter(sid);
                    byte[] remained_data = new byte[data.Length - length];
                    Array.Copy(data, length, remained_data, 0, remained_data.Length);
                    WriteStreamData(next_sid, remained_data);
                }
            }
            else
            {
                next_sid = AllocateDataSectorAfter(sid);
                WriteStreamData(next_sid, data);
            }
        }

        internal void WriteShortStreamData(int startSID, byte[] data)
        {
            int prev_sid = SID.EOC;
            int sid = startSID;
            int index = 0;
            while (index < data.Length)
            {
                if (sid == SID.EOC)
                {
                    if (prev_sid == SID.EOC)
                    {
                        sid = this.ShortSectorAllocation.AllocateSector();
                    }
                    else
                    {
                        sid = this.ShortSectorAllocation.AllocateSectorAfter(prev_sid);
                    }
                }
                int offset = GetShortSectorOffset(sid);
                ShortStreamContainer.Position = offset;
                if (index + ShortSectorSize < data.Length)
                {
                    ShortStreamContainer.Write(data, index, ShortSectorSize);
                }
                else
                {
                    ShortStreamContainer.Write(data, index, data.Length - index);
                }
                index += ShortSectorSize;
                prev_sid = sid;
                sid = this.ShortSectorAllocation.GetNextSectorID(prev_sid);
            }
            if (sid != SID.EOC && prev_sid != SID.EOC)
            {
                ShortSectorAllocation.LinkSectorID(prev_sid, SID.EOC);
                while (sid != SID.EOC)
                {
                    int next_sid = ShortSectorAllocation.GetNextSectorID(sid);
                    ShortSectorAllocation.LinkSectorID(sid, SID.Free);
                    sid = next_sid;
                }
            }
        }

        private DirectoryEntry GetOrCreateDirectoryEntry(string[] streamPath)
        {
            DirectoryEntry entry = this.RootStorage;
            foreach (string entryName in streamPath)
            {
                if (!entry.Members.ContainsKey(entryName))
                {
                    DirectoryEntry newEntry = new DirectoryEntry(this, entryName);
                    newEntry.ID = this.DirectoryEntries.Count;
                    this.DirectoryEntries.Add(newEntry.ID, newEntry);
                    entry.AddChild(newEntry);
                }
                entry = entry.Members[entryName];
            }
            return entry;
        }

        public void DeleteDirectoryEntry(string[] streamPath)
        {
            DirectoryEntry entry = GetOrCreateDirectoryEntry(streamPath);
            DeleteDirectoryEntry(entry);
        }

        public void DeleteDirectoryEntry(DirectoryEntry entry)
        {
            entry.EntryType = EntryType.Empty;
            entry.StreamLength = 0;
            entry.Parent.Members.Remove(entry.Name);
        }
    }
}
