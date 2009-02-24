using System;
using System.Collections.Generic;
using NUnit.Framework;
using QiHe.CodeLib;

namespace ExcelLibrary.Test.CodeLib
{
    [TestFixture]
    public class FastSearchListTest
    {
        [Test]
        public void SimpleTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Add("Item 2");
            list.Add("Item 3");

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Item 1", list[0]);
            Assert.AreEqual("Item 2", list[1]);
            Assert.AreEqual("Item 3", list[2]);

            Assert.AreEqual(0, list.IndexOf("Item 1"));
            Assert.AreEqual(1, list.IndexOf("Item 2"));
            Assert.AreEqual(2, list.IndexOf("Item 3"));
        }

        [Test]
        public void AddDuplicateTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Add("Item 1");

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0, list.IndexOf("Item 1"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertDuplicateTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Insert(1, "Item 1");
        }


        [Test]
        public void RemoveAtTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Add("Item 2");
            list.Add("Item 3");

            list.RemoveAt(0);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Item 2", list[0]);
            Assert.AreEqual("Item 3", list[1]);
            Assert.AreEqual(0, list.IndexOf("Item 2"));
            Assert.AreEqual(1, list.IndexOf("Item 3"));

            list.Add("Item 1");

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Item 2", list[0]);
            Assert.AreEqual("Item 3", list[1]);
            Assert.AreEqual("Item 1", list[2]);
            Assert.AreEqual(0, list.IndexOf("Item 2"));
            Assert.AreEqual(1, list.IndexOf("Item 3"));
            Assert.AreEqual(2, list.IndexOf("Item 1"));
        }

        [Test]
        public void RemoveTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Add("Item 2");
            list.Add("Item 3");

            list.Remove("Item 2");

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Item 1", list[0]);
            Assert.AreEqual("Item 3", list[1]);
            Assert.AreEqual(0, list.IndexOf("Item 1"));
            Assert.AreEqual(1, list.IndexOf("Item 3"));

            list.Add("Item 4");

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Item 1", list[0]);
            Assert.AreEqual("Item 3", list[1]);
            Assert.AreEqual("Item 4", list[2]);
            Assert.AreEqual(0, list.IndexOf("Item 1"));
            Assert.AreEqual(1, list.IndexOf("Item 3"));
            Assert.AreEqual(2, list.IndexOf("Item 4"));
        }

        [Test]
        public void InsertTest()
        {
            IList<String> list = new FastSearchList<String>();

            list.Add("Item 1");
            list.Add("Item 2");
            list.Add("Item 3");

            list.Insert(1, "Item 1.5");

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("Item 1", list[0]);
            Assert.AreEqual("Item 1.5", list[1]);
            Assert.AreEqual("Item 2", list[2]);
            Assert.AreEqual("Item 3", list[3]);
            Assert.AreEqual(0, list.IndexOf("Item 1"));
            Assert.AreEqual(1, list.IndexOf("Item 1.5"));
            Assert.AreEqual(2, list.IndexOf("Item 2"));
            Assert.AreEqual(3, list.IndexOf("Item 3"));
        }
    }
}

