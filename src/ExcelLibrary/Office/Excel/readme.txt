This library is built based on the following documents, thanks to their authors.

Compound Document file format
http://sc.openoffice.org/compdocfileformat.pdf

Excel file format 
http://sc.openoffice.org/excelfileformat.pdf

Microsoft Office 97 Drawing File Format
http://chicago.sourceforge.net/devel/docs/escher/

Record structures in BIFF8/BIFF8X format are implemented.

It can read read worksheets in a workbook and read cells in a worksheet.
It can read cell content(text,number,datetime or error) and 
cell format(font,alignment,linestyle,background,etc).
It can read pictures in the file, get informations of image size, position,
data and format.

Liu Junfeng 2006-11-02

Update notes:
2007-5-17
display each Sheet in separate tabpage		
fixed some bugs	in FORMULA.cs and Form1.cs

2007-5-26
decode FORMULAR result

2008-1-23
change default Encoding from to ASCII to UTF8 in Record.cs ln97.(suggested by ragundo)
return EmptyCell instead of null for nonexist cells.(requested by amrlafi)
bug fix in Record.cs ln83 and ln133. (proposed by dhirshjr and ilogpasig)
try to bug fix in CompoundDocument.cs. (found by stevenbright)

2008-9-15
Create, Open and Modify CompoundDocument.

2008-9-22
Save modified CompoundDocument.

2008-10-26
Encode workbook and worksheet to BIFF8 records.
Fixes a bug that compressed Unicode string is wrongly decoded by UTF8.

2008-11-10
Fixed XF format so that the created xls file can be opened by MS Excel 2003.

2008-11-16
Assign number format for number and date time values.
Set column widths.

2009-01-20
Fix for getting 'NaN' when reading string value of a formula cell.

2009-01-23
Support decode cell format and encode predefined cell format.

2009-02-12
Initial implementation of encode images.

