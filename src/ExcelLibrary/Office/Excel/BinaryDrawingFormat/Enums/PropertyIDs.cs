using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public enum PropertyIDs : ushort
    {
		LockAgainstGrouping = 127,
        FitTextToShape = 191,
        BlipId = 260,
		BlipName = 261,
		IsActive = 319,
        FillColor = 385,
		NoFillHitTest = 447,
        LineColor = 448,
		NoLineDrawDash = 511,
		Background = 831,
		ShapeName = 896,
		Description = 897
    }
}
