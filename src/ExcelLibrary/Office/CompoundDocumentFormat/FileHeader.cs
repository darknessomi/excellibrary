using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class FileHeader
    {
        /// <summary>
        /// Compound document file identifier: D0H CFH 11H E0H A1H B1H 1AH E1H
        /// </summary>
        public byte[] FileTypeIdentifier;

        /// <summary>
        /// Unique identifier (UID) of this file (not of interest in the following, may be all 0)
        /// </summary>
        public Guid FileIdentifier;

        /// <summary>
        /// Revision number of the file format (most used is 003EH)
        /// </summary>
        public UInt16 RevisionNumber;

        /// <summary>
        /// Version number of the file format (most used is 0003H)
        /// </summary>
        public UInt16 VersionNumber;

        /// <summary>
        /// Byte order identifier: FEH FFH = Little-Endian FFH FEH = Big-Endian
        /// </summary>
        public byte[] ByteOrderMark;

        /// <summary>
        /// Size of a sector in the compound document file in power-of-two (ssz),
        /// (most used value is 9 which means 512 bytes, minimum value is 7 which means 128 bytes)
        /// </summary>
        public UInt16 SectorSizeInPot;

        /// <summary>
        /// Size of a short-sector in the short-stream container stream in power-of-two (ssz),
        /// (most used value is 6 which means 64 bytes, maximum value is sector size  in power-of-two)
        /// </summary>
        public UInt16 ShortSectorSizeInPot;

        /// <summary>
        /// Not used
        /// </summary>
        public byte[] UnUsed10;

        /// <summary>
        /// Total number of sectors used for the sector allocation table
        /// </summary>
        public Int32 NumberOfSATSectors;

        /// <summary>
        /// SID of first sector of the directory stream
        /// </summary>
        public Int32 FirstSectorIDofDirectoryStream;

        /// <summary>
        /// Not used
        /// </summary>
        public byte[] UnUsed4;

        /// <summary>
        /// Minimum size of a standard stream (in bytes, most used size is 4096 bytes),
        /// streams smaller than this value are stored as short-streams
        /// </summary>
        public Int32 MinimumStreamSize;

        /// <summary>
        /// SID of first sector of the short-sector allocation table,
        /// or ¨C2 (End Of Chain SID) if not extant
        /// </summary>
        public Int32 FirstSectorIDofShortSectorAllocationTable;

        /// <summary>
        /// Total number of sectors used for the short-sector allocation table
        /// </summary>
        public Int32 NumberOfShortSectors;

        /// <summary>
        /// SID of first sector of the master sector allocation table,
        /// or ¨C2 (End Of Chain SID) if no additional sectors used
        /// </summary>
        public Int32 FirstSectorIDofMasterSectorAllocationTable;

        /// <summary>
        /// Total number of sectors used for the master sector allocation table
        /// </summary>
        public Int32 NumberOfMasterSectors;

        /// <summary>
        /// First part of the master sector allocation table containing 109 SIDs
        /// </summary>
        public Int32[] MasterSectorAllocationTable;
    }
}
