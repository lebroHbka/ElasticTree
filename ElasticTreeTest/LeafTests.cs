using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ElasticTree.src;
using ElasticTree.src.Composite;

namespace ElasticTreeTest
{
    [TestClass]
    public class LeafTests
    {
        SimpleData d1, d2, d3;
        Component leaf1, leaf2, leaf3;


        [TestInitialize]
        public void TestInitialize()
        {
            d1 = new SimpleData(new List<int> { 1 });
            d2 = new SimpleData(new List<int> { 2 });
            d3 = new SimpleData(new List<int> { 3 });

            leaf1 = new Leaf(d1);
            leaf2 = new Leaf(d2);
            leaf3 = new Leaf(d3);

        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void AddChildren_test()
        {
            leaf1.AddChild(leaf2);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ChangeChild_test()
        {
            leaf1.ReplaceChild(leaf2, leaf3);
        }

        [TestMethod]
        public void HasChild_test()
        {
            Assert.IsFalse(leaf1.HasChild(leaf2));
            Assert.IsFalse(leaf1.HasChild(leaf3));
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void RemoveChild_test()
        {
            leaf1.RemoveChild(leaf2);
        }



    }
}
