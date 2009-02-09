using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace ExcelLibrary.Test
{
    [TestFixture]
    public class DataSetHelperTest
    {
        // TODO:
        // 1. use real Excel file for testing
        // 2. use more complicated cell data types

        [Test]
        public void SimpleCreateTest()
        {
            string tempFilePath = Path.GetTempFileName();

            {
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable("Table 1");
                dt1.Columns.Add("Column A", typeof (String));
                dt1.Columns.Add("Column B", typeof (String));
                dt1.Rows.Add("Test 1", "Test2");
                dt1.Rows.Add("Test 3", "Test4");
                ds.Tables.Add(dt1);

                DataTable dt2 = new DataTable("Table 2");
                ds.Tables.Add(dt2);

                DataTable dt3 = new DataTable("Table 3");
                dt3.Columns.Add("Column C", typeof (String));
                ds.Tables.Add(dt3);

                DataSetHelper.CreateWorkbook(tempFilePath, ds);
            }

            {
                DataSet ds = DataSetHelper.CreateDataSet(tempFilePath);
                Assert.AreEqual(3, ds.Tables.Count);
                Assert.AreEqual("Table 1", ds.Tables[0].TableName);
                Assert.AreEqual("Table 2", ds.Tables[1].TableName);
                Assert.AreEqual("Table 3", ds.Tables[2].TableName);

                Assert.AreEqual(2, ds.Tables[0].Columns.Count);
                Assert.AreEqual(1, ds.Tables[1].Columns.Count);
                Assert.AreEqual(1, ds.Tables[2].Columns.Count);
                Assert.AreEqual("Column A", ds.Tables[0].Columns[0].ColumnName);
                Assert.AreEqual("Column B", ds.Tables[0].Columns[1].ColumnName);
                Assert.AreEqual("Column C", ds.Tables[2].Columns[0].ColumnName);

                Assert.AreEqual(2, ds.Tables[0].Rows.Count);
                Assert.AreEqual(0, ds.Tables[1].Rows.Count);
                Assert.AreEqual(0, ds.Tables[2].Rows.Count);
                Assert.AreEqual("Test 1", ds.Tables[0].Rows[0][0]);
                Assert.AreEqual("Test 2", ds.Tables[0].Rows[0][1]);
                Assert.AreEqual("Test 3", ds.Tables[0].Rows[1][0]);
                Assert.AreEqual("Test 4", ds.Tables[0].Rows[1][1]);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyDataSetCreateTest()
        {
            string tempFilePath = Path.GetTempFileName();
            DataSet ds = new DataSet();

            DataSetHelper.CreateWorkbook(tempFilePath, ds);
        }

    }
}
