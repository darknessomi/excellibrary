using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class ShortSectorAllocation
    {
        CompoundDocument Document;

        List<Int32> ShortSectorAllocationTable;

        public ShortSectorAllocation(CompoundDocument document)
        {
            this.Document = document;
            ShortSectorAllocationTable = document.GetStreamDataAsIntegers(document.Header.FirstSectorIDofShortSectorAllocationTable);
            //ShortSectorAllocationTable.RemoveRange(document.Header.NumberOfShortSectors, ShortSectorAllocationTable.Count - document.Header.NumberOfShortSectors);
            while (ShortSectorAllocationTable.Count > 0 && ShortSectorAllocationTable[ShortSectorAllocationTable.Count - 1] == SID.Free)
            {
                ShortSectorAllocationTable.RemoveAt(ShortSectorAllocationTable.Count - 1);
            }
        }

        public int AllocateSector()
        {
            int newSectorID = ShortSectorAllocationTable.Count;
            LinkSectorID(newSectorID, SID.EOC);
            Document.AllocateNewShortSector();
            Document.Header.NumberOfShortSectors++;
            return newSectorID;
        }

        public int AllocateSectorAfter(int sectorID)
        {
            int newSectorID = this.AllocateSector();
            this.LinkSectorID(sectorID, newSectorID);
            return newSectorID;
        }

        public void LinkSectorID(int sectorID, int newSectorID)
        {
            if (sectorID < ShortSectorAllocationTable.Count)
            {
                ShortSectorAllocationTable[sectorID] = newSectorID;
            }
            else if (sectorID == ShortSectorAllocationTable.Count)
            {
                ShortSectorAllocationTable.Add(newSectorID);
            }
            else
            {
                throw new ArgumentOutOfRangeException("sectorID");
            }
        }

        public int GetNextSectorID(int sectorID)
        {
            if (sectorID < ShortSectorAllocationTable.Count)
            {
                return ShortSectorAllocationTable[sectorID];
            }
            else
            {
                return SID.EOC;
            }
        }

        public List<int> GetSIDChain(int StartSID)
        {
            List<int> chain = new List<int>();
            int sid = StartSID;
            while (sid != SID.EOC)
            {
                chain.Add(sid);
                sid = GetNextSectorID(sid);
            }
            return chain;
        }

        public void Save()
        {
            if (ShortSectorAllocationTable.Count > 0)
            {
                if (Document.Header.FirstSectorIDofShortSectorAllocationTable == SID.EOC)
                {
                    int SecIDCapacity = Document.SectorSize / 4;
                    int[] sids = new Int32[SecIDCapacity];
                    for (int i = 0; i < sids.Length; i++)
                    {
                        sids[i] = SID.Free;
                    }
                    Document.Header.FirstSectorIDofShortSectorAllocationTable = Document.AllocateDataSector();
                    Document.WriteInSector(Document.Header.FirstSectorIDofShortSectorAllocationTable, 0, sids);
                }
                MemoryStream satStream = new MemoryStream(ShortSectorAllocationTable.Count * 4);
                CompoundDocument.WriteArrayOfInt32(new BinaryWriter(satStream), ShortSectorAllocationTable.ToArray());
                Document.WriteStreamData(Document.Header.FirstSectorIDofShortSectorAllocationTable, satStream.ToArray());
            }
        }
    }
}
