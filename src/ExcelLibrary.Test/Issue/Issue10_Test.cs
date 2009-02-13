using System;
using ExcelLibrary.Office.Excel;
using NUnit.Framework;

namespace ExcelLibrary.Test.Issue
{
    [TestFixture]
    public class Issue10_Test
    {
        [Test]
        public void Test()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Test");
            workbook.Worksheets.Add(worksheet);
            
            DateTime date = new DateTime(2009, 2, 13, 11, 30, 45);
            worksheet.Cells[0, 0] = new Cell(date);
            worksheet.Cells[0, 0].DateTimeValue = date;

            Assert.AreEqual(date, worksheet.Cells[0, 0].DateTimeValue);
        }

        [Test]
        public void RelatedTest()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Test");
            workbook.Worksheets.Add(worksheet);

            DateTime date = new DateTime(2009, 2, 13, 11, 30, 45);
            worksheet.Cells[0, 0] = new Cell(date);
            

            Assert.IsNotNull(worksheet.Cells[0, 0].Format);
        }
    }
}
