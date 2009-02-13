using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    /// <summary>
    /// The master sector allocation table (MSAT) is an array of SecIDs of all sectors
    /// used by the sector allocation table (SAT).
    /// </summary>
    public class MasterSectorAllocation
    {
        CompoundDocument Document;

        int NumberOfSecIDs;

        int CurrentMSATSector;

        int SecIDCapacity;

        List<Int32> MasterSectorAllocationTable;

        public MasterSectorAllocation(CompoundDocument document)
        {
            this.Document = document;
            this.NumberOfSecIDs = document.Header.NumberOfSATSectors;
            this.CurrentMSATSector = document.Header.FirstSectorIDofMasterSectorAllocationTable;
            this.SecIDCapacity = document.SectorSize / 4 - 1;
            InitializeMasterSectorAllocationTable();
        }

        private void InitializeMasterSectorAllocationTable()
        {
            this.MasterSectorAllocationTable = new List<int>(NumberOfSecIDs);
            SelectSIDs(Document.Header.MasterSectorAllocationTable);
            int msid = Document.Header.FirstSectorIDofMasterSectorAllocationTable;
            while (msid != SID.EOC)
            {
                CurrentMSATSector = msid;
                int[] SIDs = Document.ReadSectorDataAsIntegers(msid);
                SelectSIDs(SIDs);
                msid = SIDs[SIDs.Length - 1];
            }
        }

        private void SelectSIDs(int[] SIDs)
        {
            for (int i = 0; i < SIDs.Length; i++)
            {
                int sid = SIDs[i];
                if (MasterSectorAllocationTable.Count < NumberOfSecIDs)
                {
                    MasterSectorAllocationTable.Add(sid);
                }
                else
                {
                    break;
                }
            }
        }

        public int GetSATSectorID(int SATSectorIndex)
        {
            if (SATSectorIndex < NumberOfSecIDs)
            {
                return MasterSectorAllocationTable[SATSectorIndex];
            }
            else if (SATSectorIndex == NumberOfSecIDs)
            {
                return AllocateSATSector();
            }
            else
            {
                throw new ArgumentOutOfRangeException("SATSectorIndex");
            }
        }

        public int AllocateSATSector()
        {
            int[] sids = new Int32[SecIDCapacity];
            for (int i = 0; i < sids.Length; i++)
            {
                sids[i] = SID.Free;
            }
            int secID = Document.AllocateNewSector(sids);
            if (NumberOfSecIDs < 109)
            {
                Document.Header.MasterSectorAllocationTable[NumberOfSecIDs] = secID;
                Document.Write(76 + NumberOfSecIDs * 4, secID);
            }
            else
            {
                if (CurrentMSATSector == SID.EOC)
                {
                    CurrentMSATSector = AllocateMSATSector();
                    Document.Header.FirstSectorIDofMasterSectorAllocationTable = CurrentMSATSector;
                }
                int index = (NumberOfSecIDs - 109) % SecIDCapacity;
                Document.WriteInSector(CurrentMSATSector, index * 4, secID);
                if (index == SecIDCapacity - 1)
                {
                    int newMSATSector = AllocateMSATSector();
                    Document.WriteInSector(CurrentMSATSector, SecIDCapacity * 4, newMSATSector);
                    CurrentMSATSector = newMSATSector;
                }
            }
            MasterSectorAllocationTable.Add(secID);
            NumberOfSecIDs++;
            Document.SectorAllocation.LinkSectorID(secID, SID.SAT);
            Document.Header.NumberOfSATSectors++;
            return secID;
        }

        public int AllocateMSATSector()
        {
            int[] secIDs = new int[SecIDCapacity + 1];
            for (int i = 0; i < SecIDCapacity; i++)
            {
                secIDs[i] = SID.Free;
            }
            secIDs[SecIDCapacity] = SID.EOC;

            int newMSATSector = Document.AllocateNewSector();
            Document.WriteInSector(newMSATSector, 0, secIDs);

            Document.SectorAllocation.LinkSectorID(newMSATSector, SID.MSAT);
            Document.Header.NumberOfMasterSectors++;

            return newMSATSector;
        }
    }
}
