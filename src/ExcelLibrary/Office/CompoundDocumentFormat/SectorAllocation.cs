using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class SectorAllocation
    {
        CompoundDocument Document;

        int SecIDCapacity;

        public SectorAllocation(CompoundDocument document)
        {
            this.Document = document;
            this.SecIDCapacity = document.SectorSize / 4;
        }

        public int AllocateSector()
        {
            int newSectorID = Document.AllocateNewSector();
            LinkSectorID(newSectorID, SID.EOC);
            return newSectorID;
        }

        public void LinkSectorID(int sectorID, int newSectorID)
        {
            if (sectorID < 0)
            {
                throw new ArgumentOutOfRangeException("sectorID");
            }
            int SATSectorIndex = sectorID / SecIDCapacity;
            int SectorIndex = sectorID % SecIDCapacity;

            int SATSectorID = Document.MasterSectorAllocation.GetSATSectorID(SATSectorIndex);
            Document.WriteInSector(SATSectorID, SectorIndex * 4, newSectorID);
        }

        public int GetNextSectorID(int sectorID)
        {
            if (sectorID < 0)
            {
                throw new ArgumentOutOfRangeException("sectorID");
            }
            int SATSectorIndex = sectorID / SecIDCapacity;
            int SectorIndex = sectorID % SecIDCapacity;

            int SATSectorID = Document.MasterSectorAllocation.GetSATSectorID(SATSectorIndex);
            return Document.ReadInt32InSector(SATSectorID, SectorIndex * 4);
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
    }
}
