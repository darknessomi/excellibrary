using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryFileFormat
{
    /*
     * Added, 2-13-2009
     * Sunil Shenoi
     * 
     * This class stores rich text formatting for a single string in the Shared String Table.
     * See 2.5.1 and 2.5.3 of the MS Excel File Format for more information.
     */
    public class RichTextFormat
    {
        public List<UInt16> CharIndexes;
        public List<UInt16> FontIndexes;

        public RichTextFormat()
        {
            CharIndexes = new List<UInt16>();
            FontIndexes = new List<UInt16>();
        }

        public RichTextFormat(int numberOfFormattingRuns)
        {
            CharIndexes = new List<UInt16>(numberOfFormattingRuns);
            FontIndexes = new List<UInt16>(numberOfFormattingRuns);
        }
    }
}
