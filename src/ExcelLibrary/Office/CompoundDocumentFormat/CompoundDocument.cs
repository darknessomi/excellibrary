using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public partial class CompoundDocument
    {
        internal FileHeader Header;
        internal int SectorSize;
        internal int ShortSectorSize;
        private int TotalSectors;
        internal MasterSectorAllocation MasterSectorAllocation;
        internal SectorAllocation SectorAllocation;
        internal ShortSectorAllocation ShortSectorAllocation;

        private MemoryStream ShortStreamContainer;
        private MemoryStream DirectoryStream;
        private Dictionary<int, DirectoryEntry> DirectoryEntries;

        public DirectoryEntry RootStorage
        {
            get { return DirectoryEntries[0]; }
        }

        internal Stream FileStorage;
        private BinaryReader Reader;
        private BinaryWriter Writer;

        internal CompoundDocument(Stream stream, FileHeader header)
        {
            this.FileStorage = stream;
            this.Reader = new BinaryReader(this.FileStorage);
            this.Writer = new BinaryWriter(this.FileStorage, Encoding.Unicode);

            this.Header = header;
            this.SectorSize = (int)Math.Pow(2, Header.SectorSizeInPot);
            this.ShortSectorSize = (int)Math.Pow(2, Header.ShortSectorSizeInPot);
            this.TotalSectors = stream.Length == 0 ? 0
                : (int)(stream.Length - 512) / this.SectorSize;

            this.MasterSectorAllocation = new MasterSectorAllocation(this);
            this.SectorAllocation = new SectorAllocation(this);
            this.ShortSectorAllocation = new ShortSectorAllocation(this);
        }

        public static CompoundDocument Create(string file)
        {
            FileStream stream = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            CompoundDocument document = new CompoundDocument(stream, new CompoundFileHeader());
            document.WriteHeader();
            document.MasterSectorAllocation.AllocateSATSector();
            document.InitializeDirectoryEntries();
            return document;
        }

        public static CompoundDocument Open(string file)
        {
            FileStream stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            BinaryReader reader = new BinaryReader(stream);
            FileHeader header = ReadHeader(reader);

            CompoundDocument document = new CompoundDocument(stream, header);
            if (!document.CheckHeader()) return null;

            document.ReadDirectoryEntries();

            return document;
        }

        public void Save()
        {
            SaveDirectoryEntries();
            SaveShortStreams();
            WriteHeader();
            Writer.Flush();
        }

        public void Close()
        {
            FileStorage.Close();
        }

        bool CheckHeader()
        {
            if (!ArrayEqual(Header.FileTypeIdentifier, CompoundFileHeader.FileTypeIdentifier)) throw new Exception("File header not recognized.");
            if (!ArrayEqual(Header.ByteOrderMark, ByteOrderMarks.LittleEndian)) throw new Exception("Endian not implemented.");
            return true;
        }

        void InitializeDirectoryEntries()
        {
            Header.FirstSectorIDofDirectoryStream = AllocateDataSector();
            DirectoryEntries = new Dictionary<int, DirectoryEntry>();
            DirectoryEntry root = new DirectoryEntry(this, "Root Entry");
            root.EntryType = EntryType.Root;
            root.NodeColor = NodeColor.Black;
            root.FirstSectorID = AllocateDataSector();
            root.StreamLength = 0;
            DirectoryEntries.Add(0, root);
            DirectoryStream = new MemoryStream();
            ShortStreamContainer = new MemoryStream();
        }

        void SaveDirectoryEntries()
        {
            DirectoryTree.Build(RootStorage);
            DirectoryStream.Position = 0;
            BinaryWriter writer = new BinaryWriter(DirectoryStream, Encoding.Unicode);
            for (int id = 0; id < DirectoryEntries.Count; id++)
            {
                WriteDirectoryEntry(writer, DirectoryEntries[id]);
            }
            WriteStreamData(Header.FirstSectorIDofDirectoryStream, DirectoryStream.ToArray());
        }

        void SaveShortStreams()
        {
            ShortSectorAllocation.Save();
            WriteStreamData(RootStorage.FirstSectorID, ShortStreamContainer.ToArray());
        }

        internal int ReadInt32(long position)
        {
            FileStorage.Position = position;
            return Reader.ReadInt32();
        }

        internal int ReadInt32InSector(int secID, int position)
        {
            int offset = GetSectorOffset(secID);
            FileStorage.Position = offset + position;
            return Reader.ReadInt32();
        }

        internal void Write(long position, int integer)
        {
            FileStorage.Position = position;
            Writer.Write(integer);
        }

        internal void WriteInSector(int secID, int position, int integer)
        {
            int offset = GetSectorOffset(secID);
            FileStorage.Position = offset + position;
            Writer.Write(integer);
        }

        internal void WriteInSector(int secID, int position, int[] integers)
        {
            int offset = GetSectorOffset(secID);
            FileStorage.Position = offset + position;
            WriteArrayOfInt32(Writer, integers);
        }

        internal void WriteInSector(int secID, int position, byte[] data, int index, int count)
        {
            int offset = GetSectorOffset(secID);
            FileStorage.Position = offset + position;
            Writer.Write(data, index, count);
        }

        internal int AllocateNewSector()
        {
            int secID = TotalSectors;
            FileStorage.Position = GetSectorOffset(secID);
            Writer.Write(new byte[SectorSize]);
            TotalSectors++;
            return secID;
        }

        internal int AllocateDataSector()
        {
            return this.SectorAllocation.AllocateSector();
        }

        internal int AllocateDataSectorAfter(int sectorID)
        {
            int newSectorID = this.SectorAllocation.AllocateSector();
            this.SectorAllocation.LinkSectorID(sectorID, newSectorID);
            return newSectorID;
        }

        internal int AllocateNewSector(int[] sectorData)
        {
            int secID = TotalSectors;
            FileStorage.Position = GetSectorOffset(secID);
            WriteArrayOfInt32(Writer, sectorData);
            TotalSectors++;
            return secID;
        }

        internal int AllocateShortSector()
        {
            return ShortSectorAllocation.AllocateSector();
        }

        internal void AllocateNewShortSector()
        {
            ShortStreamContainer.Position = ShortStreamContainer.Length;
            byte[] sidData = new byte[ShortSectorSize];
            for (int i = 0; i < sidData.Length; i++)
            {
                sidData[i] = 0xFF;
            }
            ShortStreamContainer.Write(sidData, 0, ShortSectorSize);
            RootStorage.StreamLength = (int)ShortStreamContainer.Length;
        }

        internal static Int32[] ReadArrayOfInt32(BinaryReader reader, int count)
        {
            Int32[] data = new Int32[count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = reader.ReadInt32();
            }
            return data;
        }

        internal static void WriteArrayOfInt32(BinaryWriter writer, Int32[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                writer.Write(data[i]);
            }
        }
    }
}
