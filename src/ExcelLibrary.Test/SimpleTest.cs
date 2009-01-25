using System.IO;
using NUnit.Framework;
using QiHe.Office.Excel;

namespace ExcelLibrary.Test
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void SimpleReadWriteTest()
        {
            string tempFilePath = Path.GetTempFileName();
            {
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Test1");
                worksheet.Cells[0, 1] = new Cell(100);
                worksheet.Cells[2, 0] = new Cell("Test String");
                workbook.Worksheets.Add(worksheet);
                workbook.Save(tempFilePath);
            }

            {
                Workbook workbook = Workbook.Open(tempFilePath);
                Assert.AreEqual(1, workbook.Worksheets.Count);

                Worksheet worksheet = workbook.Worksheets[0];
                Assert.AreEqual("Test1", worksheet.Name);
                Assert.AreEqual(100, worksheet.Cells[0, 1].Value);
                Assert.AreEqual("Test String", worksheet.Cells[2, 0].Value);
            }
        }
    }
}
