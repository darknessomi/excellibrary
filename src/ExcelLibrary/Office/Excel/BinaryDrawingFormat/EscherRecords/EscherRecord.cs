using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class EscherRecord
	{
		public static EscherRecord Read(Stream stream)
		{
			EscherRecord record = EscherRecord.ReadBase(stream);
			switch (record.Type)
			{
				case EscherRecordType.MsofbtDgg:
					return new MsofbtDgg(record);
				case EscherRecordType.MsofbtBSE:
					return new MsofbtBSE(record);
				case EscherRecordType.MsofbtDg:
					return new MsofbtDg(record);
				case EscherRecordType.MsofbtSpgr:
					return new MsofbtSpgr(record);
				case EscherRecordType.MsofbtSp:
					return new MsofbtSp(record);
				case EscherRecordType.MsofbtOPT:
					return new MsofbtOPT(record);
				case EscherRecordType.MsofbtTextbox:
					return new MsofbtTextbox(record);
				case EscherRecordType.MsofbtClientTextbox:
					return new MsofbtClientTextbox(record);
				case EscherRecordType.MsofbtAnchor:
					return new MsofbtAnchor(record);
				case EscherRecordType.MsofbtChildAnchor:
					return new MsofbtChildAnchor(record);
				case EscherRecordType.MsofbtClientAnchor:
					return new MsofbtClientAnchor(record);
				case EscherRecordType.MsofbtClientData:
					return new MsofbtClientData(record);
				case EscherRecordType.MsofbtConnectorRule:
					return new MsofbtConnectorRule(record);
				case EscherRecordType.MsofbtAlignRule:
					return new MsofbtAlignRule(record);
				case EscherRecordType.MsofbtArcRule:
					return new MsofbtArcRule(record);
				case EscherRecordType.MsofbtClientRule:
					return new MsofbtClientRule(record);
				case EscherRecordType.MsofbtCLSID:
					return new MsofbtCLSID(record);
				case EscherRecordType.MsofbtCalloutRule:
					return new MsofbtCalloutRule(record);
				case EscherRecordType.MsofbtRegroupItems:
					return new MsofbtRegroupItems(record);
				case EscherRecordType.MsofbtSelection:
					return new MsofbtSelection(record);
				case EscherRecordType.MsofbtColorMRU:
					return new MsofbtColorMRU(record);
				case EscherRecordType.MsofbtDeletedPspl:
					return new MsofbtDeletedPspl(record);
				case EscherRecordType.MsofbtSplitMenuColors:
					return new MsofbtSplitMenuColors(record);
				case EscherRecordType.MsofbtOleObject:
					return new MsofbtOleObject(record);
				case EscherRecordType.MsofbtColorScheme:
					return new MsofbtColorScheme(record);
				case EscherRecordType.MsofbtDggContainer:
					return new MsofbtDggContainer(record);
				case EscherRecordType.MsofbtDgContainer:
					return new MsofbtDgContainer(record);
				case EscherRecordType.MsofbtBstoreContainer:
					return new MsofbtBstoreContainer(record);
				case EscherRecordType.MsofbtSpgrContainer:
					return new MsofbtSpgrContainer(record);
				case EscherRecordType.MsofbtSpContainer:
					return new MsofbtSpContainer(record);
				case EscherRecordType.MsofbtSolverContainer:
					return new MsofbtSolverContainer(record);
				case EscherRecordType.MsofbtBlipStart:
					return new MsofbtBlipStart(record);
				case EscherRecordType.MsofbtBlipMetafileEMF:
					return new MsofbtBlipMetafileEMF(record);
				case EscherRecordType.MsofbtBlipMetafileWMF:
					return new MsofbtBlipMetafileWMF(record);
				case EscherRecordType.MsofbtBlipMetafilePICT:
					return new MsofbtBlipMetafilePICT(record);
				case EscherRecordType.MsofbtBlipBitmapJPEG:
					return new MsofbtBlipBitmapJPEG(record);
				case EscherRecordType.MsofbtBlipBitmapPNG:
					return new MsofbtBlipBitmapPNG(record);
				case EscherRecordType.MsofbtBlipBitmapDIB:
					return new MsofbtBlipBitmapDIB(record);
				case EscherRecordType.MsofbtBlipEnd:
					return new MsofbtBlipEnd(record);
				default:
					return record;
			}
		}

	}
}
