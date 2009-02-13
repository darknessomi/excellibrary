using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class SID
    {
        /// <summary>
        /// Free SID,  Free sector
        /// (may exist in the file, but is not part of any stream)
        /// </summary>
        public const int Free = -1;

        /// <summary>
        /// End Of Chain SID
        /// </summary>
        public const int EOC = -2;

        /// <summary>
        /// SAT SID, Sector is used by the sector allocation table
        /// </summary>
        public const int SAT = -3;

        /// <summary>
        /// MSAT SID, Sector is used by the master sector allocation table
        /// </summary>
        public const int MSAT = -4;
    }
}
