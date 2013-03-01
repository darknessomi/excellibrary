using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelLibrary.CompoundDocumentFormat;
using NUnit.Framework;

namespace ExcelLibrary.Test.Issue
{
    [TestFixture]
    class Issue141_Test
    {
        [Test]
        public void Test()
        {
            string filename = Path.GetTempFileName();

            try
            {
                // create invalid file for read
                using (var writer = new StreamWriter(filename))
                    writer.WriteLine("test");

                try
                {
                    // triggers exception
                    CompoundDocument.Open(filename);
                }
                catch
                {
                    Assert.IsTrue(true, "Expection trigger.");
                }
            }
            finally
            {
                // should not trigger any IO Exception
                File.Delete(filename);
            }
        }
    }
}
