using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ElasticTree.src;
using ElasticTree.src.Composite;

namespace ElasticTreeTest
{
    [TestClass]
    public class NodeTests
    {
        SimpleData d1, d2, d3, d4;
        Component root, node, leaf, newNode;


        [TestInitialize]
        public void TestInitialize()
        {
            d1 = new SimpleData(new List<int> { 1, 2, 3});
            d2 = new SimpleData(new List<int> { 1, 2 });
            d3 = new SimpleData(new List<int> {3});
            d4 = new SimpleData(new List<int> { 9, 1 });

            root = new Node(d1);
            node = new Node(d2);
            leaf = new Leaf(d3);
            newNode = new Node(d4);

        }
        // add child
        [TestMethod]
        public void AddChildren_test()
        {
            /*
             *          (1,2,3)
             *           /   \
             *       (1,2)   (3)
             *       
             */

             // start
            var expectedRootChildren = new List<Component>();
            
            var actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // add first child
            expectedRootChildren = new List<Component> { node };

            root.AddChild(node);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);

            // add second child

            expectedRootChildren = new List<Component> { node, leaf };

            root.AddChild(leaf);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);

        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddChildren_nullObject_test()
        {
            root.AddChild(null);
        }



        //has child
        [TestMethod]
        public void HasChild_test()
        {
            Assert.IsFalse(root.HasChild(node));
            Assert.IsFalse(root.HasChild(leaf));

            root.AddChild(node);
            Assert.IsTrue(root.HasChild(node));
            Assert.IsFalse(root.HasChild(leaf));

            root.AddChild(leaf);
            Assert.IsTrue(root.HasChild(node));
            Assert.IsTrue(root.HasChild(leaf));
        }



        // replace child
        [TestMethod]
        public void ReplaceChild_test()
        {
            /*                      outside node     
            *           (1,2,3)        (9)
            *            /   \
            *        (1,2)   (3)
            *        
            */
            // check start condition
            var expectedRootChildren = new List<Component> { node, leaf };

            root.AddChild(node);
            root.AddChild(leaf);
            var actualRootChildren = root.Children;
            
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // check after replace node with newNode
            expectedRootChildren = new List<Component> { newNode, leaf };

            root.ReplaceChild(node, newNode);

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
        }

        [TestMethod]
        public void ReplaceChild_thatNotExist_test()
        {
            // check start condition
            var expectedRootChildren = new List<Component> { node};

            root.AddChild(node);
            var actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // check after replace not exist child
            expectedRootChildren = new List<Component> { node};


            root.ReplaceChild(leaf, newNode);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceChild_nullObject_test()
        {
            root.AddChild(node);

            root.ReplaceChild(node, null);
            root.ReplaceChild(null, node);
        }



        // remove child
        [TestMethod]
        public void RemoveChild_test()
        {
            /*
             *      root
             *      /  \
             *    node  leaf
             *    
             */
            // check start condition
            var expectedRootChildren = new List<Component> { node, leaf };

            root.AddChild(node);
            root.AddChild(leaf);
            var actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // delete leaf
            expectedRootChildren = new List<Component> { node };

            root.RemoveChild(leaf);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // delete node
            expectedRootChildren = new List<Component> { };

            root.RemoveChild(node);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
        }

        [TestMethod]
        public void RemoveChild_thatNotExist_test()
        {
            // check start condition
            var expectedRootChildren = new List<Component> { node };

            root.AddChild(node);
            var actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);


            // check after remove not exist child
            expectedRootChildren = new List<Component> { node };

            root.RemoveChild(leaf);
            actualRootChildren = root.Children;

            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void RemoveChild_nullObject_test()
        {
            root.RemoveChild(null);
        }
    }
}
