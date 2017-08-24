using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ElasticTree.src;
using ElasticTree.src.Composite;

namespace ElasticTreeTest
{
    [TestClass]
    public class ComponentTests
    {
        SimpleData d1, d2, d3, d4;
        Component root, node, leaf, newNode;

        [TestInitialize]
        public void TestInitialize()
        {
            d1 = new SimpleData(new List<int> { 1 });
            d2 = new SimpleData(new List<int> { 2 });
            d3 = new SimpleData(new List<int> { 3 });
            d4 = new SimpleData(new List<int> { 9 });

            root = new Node(d1);
            node = new Node(d2);
            leaf = new Leaf(d3);
            newNode = new Node(d4);

        }
        // set parent
        [TestMethod]
        public void SetParent_test()
        {

            // check start condition
            Component expectedNodeParent = null;
            Component expectedLeafParent = null;

            var actualNodeParent = node.Parent;
            var actualLeafParent = leaf.Parent;

            Assert.AreEqual(expectedNodeParent, actualNodeParent);
            Assert.AreEqual(expectedLeafParent, actualLeafParent);


            // check <node> parent after replace from "oldParent" to <newNode>
            expectedNodeParent = newNode;
            expectedLeafParent = null;

            node.SetParent(newNode);
            actualNodeParent = node.Parent;
            actualLeafParent = leaf.Parent;

            Assert.AreEqual(expectedNodeParent, actualNodeParent);
            Assert.AreEqual(expectedLeafParent, actualLeafParent);


            // check <node> parent after replace from "oldParent" to <newNode>
            expectedNodeParent = newNode;
            expectedLeafParent = newNode;

            leaf.SetParent(newNode);
            actualNodeParent = node.Parent;
            actualLeafParent = leaf.Parent;

            Assert.AreEqual(expectedNodeParent, actualNodeParent);
            Assert.AreEqual(expectedLeafParent, actualLeafParent);

        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void SetParent_nullObject_test()
        {
            node.SetParent(null);
        }


        // remove parent
        [TestMethod]
        public void RemoveParent_test()
        {
            // check start condition
            var expectedNodeParent = root;

            node.SetParent(root);
            var actualNodeParent = node.Parent;

            Assert.AreEqual(expectedNodeParent, actualNodeParent);


            // node remove child
            expectedNodeParent = null;  

            node.RemoveParent();
            actualNodeParent = node.Parent;

            Assert.AreEqual(expectedNodeParent, actualNodeParent);
        }

    }
}
