using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class CompoundFileHeader : FileHeader
    {
        public new static readonly byte[] FileTypeIdentifier = new byte[8] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        /// <summary>
        /// Create a CompoundFileHeader with default values.
        /// </summary>
        public CompoundFileHeader()
        {
            base.FileTypeIdentifier = FileTypeIdentifier;
            FileIdentifier = Guid.NewGuid();
            RevisionNumber = 0x3E;
            VersionNumber = 0x03;
            ByteOrderMark = ByteOrderMarks.LittleEndian;
            SectorSizeInPot = 9;
            ShortSectorSizeInPot = 6;
            UnUsed10 = new byte[10];
            UnUsed4 = new byte[4];
            MinimumStreamSize = 4096;
            FirstSectorIDofShortSectorAllocationTable = SID.EOC;
            FirstSectorIDofMasterSectorAllocationTable = SID.EOC;
            FirstSectorIDofDirectoryStream = SID.EOC;
            MasterSectorAllocationTable = new Int32[109];
            for (int i = 0; i < MasterSectorAllocationTable.Length; i++)
            {
                MasterSectorAllocationTable[i] = SID.Free;
            }
        }
    }
}
