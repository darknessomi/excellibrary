using System;
using System.IO;
using System.Text;
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

        [Test]
        public void SimpleMultipleSheetTest()
        {
            int worksheetToCreate = 10;

            string tempFilePath = Path.GetTempFileName();
            {
                Workbook workbook = new Workbook();
                for (int i = 0; i < worksheetToCreate; i++)
                {
                    Worksheet worksheetWrite2 = new Worksheet(String.Format("Sheet {0}", i));
                    workbook.Worksheets.Add(worksheetWrite2);
                }

                workbook.Save(tempFilePath);
            }

            {
                Workbook workbook = Workbook.Open(tempFilePath);

                Assert.AreEqual(worksheetToCreate, workbook.Worksheets.Count);
                for (int i = 0; i < worksheetToCreate; i++)
                {
                    Assert.AreEqual(String.Format("Sheet {0}", i), workbook.Worksheets[i].Name);
                }
            }
        }

        [Test]
        public void WriteLongTextTest()
        {
            int longTextLength = 50000;

            StringBuilder builder = new StringBuilder(longTextLength);
            for (int i = 0; i < longTextLength; i++)
                builder.Append('A');

            string longText = builder.ToString();

            string tempFilePath = Path.GetTempFileName();
            {
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Test");
                worksheet.Cells[0, 0] = new Cell(longText);

                workbook.Worksheets.Add(worksheet);
                workbook.Save(tempFilePath);
            }

            {
                Workbook workbook = Workbook.Open(tempFilePath);
                Assert.AreEqual(longText, workbook.Worksheets[0].Cells[0, 0].Value);
            }
        }

        [Test]
        public void WriteMultipleCellTest()
        {
            int worksheetToWrite = 30;
            int rowToWrite = 200;
            int columnToWrite = 15;

            string tempFilePath = Path.GetTempFileName();
            {
                int start = Environment.TickCount;

                Workbook workbook = new Workbook();
                for (int sheet = 0; sheet < worksheetToWrite; sheet++)
                {
                    Worksheet worksheet = new Worksheet(String.Format("Sheet {0}", sheet));

                    for (int row = 0; row < rowToWrite; row++)
                        for (int column = 0; column < columnToWrite; column++)
                            worksheet.Cells[row, column] = new Cell(String.Format("{0}{1}", row, column));

                    workbook.Worksheets.Add(worksheet);
                }
                workbook.Save(tempFilePath);

                int end = Environment.TickCount;
                Console.WriteLine(String.Format("Write tick count: {0}", end - start));
            }

            {
                int start = Environment.TickCount;
                
                Workbook workbook = Workbook.Open(tempFilePath);
                for (int sheet = 0; sheet < worksheetToWrite; sheet++)
                    for (int row = 0; row < rowToWrite; row++)
                        for (int column = 0; column < columnToWrite; column++)
                        {
                            Assert.AreEqual(String.Format("{0}{1}", row, column), workbook.Worksheets[sheet].Cells[row, column].Value);
                        }

                int end = Environment.TickCount;

                Console.WriteLine(String.Format("Read tick count: {0}", end - start));
            }
        }

        [Test]
        public void WriteFormulaTest()
        {
            string tempFilePath = "C:\\Test.xls";
            {
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Test");
                worksheet.Cells[0, 0] = new Cell(10);
                worksheet.Cells[0, 1] = new Cell(20);
                worksheet.Cells[0, 2] = new Cell("=A1+B1");

                workbook.Worksheets.Add(worksheet);
                workbook.Save(tempFilePath);
            }

            {
                Workbook workbook = Workbook.Open(tempFilePath);
                Assert.AreEqual(10, workbook.Worksheets[0].Cells[0, 0].Value);
                Assert.AreEqual(20, workbook.Worksheets[0].Cells[0, 1].Value);
                Assert.AreEqual("=A1+B1", workbook.Worksheets[0].Cells[0, 2].Value);
            }
        }

    }
}
