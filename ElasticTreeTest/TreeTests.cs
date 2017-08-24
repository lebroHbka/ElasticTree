using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ElasticTree.src;
using ElasticTree.src.Composite;

namespace ElasticTreeTest
{
    [TestClass]
    public class TreeTests
    {
        SimpleData d0, d2, d3, d4, d5, d6, d7, d8; 
        Tree tree;

        Component root1, n2, n3, n4, n5, n6, n7, n8;

        [TestInitialize]
        public void TestInitialize()
        {
            
            d0 = new SimpleData(new List<int> { 1 });
            d2 = new SimpleData(new List<int> { 2 });
            d3 = new SimpleData(new List<int> { 3 });
            d4 = new SimpleData(new List<int> { 4 });
            d5 = new SimpleData(new List<int> { 5 });
            d6 = new SimpleData(new List<int> { 6 });
            d7 = new SimpleData(new List<int> { 7 });
            d8 = new SimpleData(new List<int> { 8 });

            root1 = new Node(d0);
            n2 = new Node(d2);
            n3 = new Node(d3);
            n4 = new Node(d4);
            n5 = new Node(d5);
            n6 = new Node(d6);
            n7 = new Node(d7);
            n8 = new Node(d8);
            tree = new Tree(root1);

        }

        #region Add node
        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void AddNode_addToNotExistedParent_test()
        {
            tree.AddNode(n2, n3);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void AddNode_addNullObject_test()
        {
            tree.AddNode(tree.RootNode, null);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void AddNode_addToNullparent_test()
        {
            tree.AddNode(null, n2);
        }


        [TestMethod]
        public void AddNode_addNodeNoChild_test()
        {
            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1 };
            var expectedRootChildren = new List<Component> { };
            Component expectedN2Parent = null;
            Component expectedN3Parent = null;
            var expectedN2Level = 0;
            var expectedN3Level = 0;

            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;
            var actualN3Parent = n3.Parent;
            var actualN2Level = n2.Level;
            var actualN3Level = n3.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN3Parent, actualN3Parent);
            Assert.AreEqual(expectedN2Level, actualN2Level);
            Assert.AreEqual(expectedN3Level, actualN3Level);


            // check after add first node to root
            tree.AddNode(tree.RootNode, n2);
            expectedTreeIncludedNodes = new List<Component> { root1, n2 };
            expectedRootChildren = new List<Component> { n2 };
            expectedN2Parent = tree.RootNode;
            expectedN3Parent = null;
            expectedN2Level = 1;
            expectedN3Level = 0;

            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN3Parent = n3.Parent;
            actualN2Level = n2.Level;
            actualN3Level = n3.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN3Parent, actualN3Parent);
            Assert.AreEqual(expectedN2Level, actualN2Level);
            Assert.AreEqual(expectedN3Level, actualN3Level);


            // check after add second node to root
            tree.AddNode(tree.RootNode, n3);
            expectedTreeIncludedNodes = new List<Component> { root1, n2, n3 };
            expectedRootChildren = new List<Component> { n2, n3 };
            expectedN2Parent = tree.RootNode;
            expectedN3Parent = tree.RootNode;
            expectedN2Level = 1;
            expectedN3Level = 1;

            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN3Parent = n3.Parent;
            actualN2Level = n2.Level;
            actualN3Level = n3.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN3Parent, actualN3Parent);
            Assert.AreEqual(expectedN2Level, actualN2Level);
            Assert.AreEqual(expectedN3Level, actualN3Level);
        }

        [TestMethod]
        public void AddNode_addNodeWithChild_test()
        {
            /*
             *        (1)     (4)
             *        / \      |
             *      (2) (3)   (5)
             *          
             *          
             *        (1)     
             *        / \      
             *      (2) (3)   
             *       |
             *      (4)
             *       |
             *      (5)
             */
            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3 };
            var expectedN2Children = new List<Component> { };
            Component expectedN4Parent = null;
            Component expectedN5Parent = n4;
            var expectedN4Level = 0;
            var expectedN5Level = 1;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            n4.AddChild(n5);
            n5.SetParent(n4);
            n5.Level = n5.Parent.Level + 1;
            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualN2Children = n2.Children;
            var actualN4Parent = n4.Parent;
            var actualN5Parent = n5.Parent;
            var actualN4Level = n4.Level;
            var actualN5Level = n5.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);


            // check after add n4_node to n2_node
            tree.AddNode(n2, n4);
            expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4, n5 };
            expectedN2Children = new List<Component> { n4 };
            expectedN4Parent = n2;
            expectedN5Parent = n4;
            expectedN4Level = 2;
            expectedN5Level = 3;

            actualTreeIncludedNodes = tree.IncludedNodes;
            actualN2Children = n2.Children;
            actualN4Parent = n4.Parent;
            actualN5Parent = n5.Parent;
            actualN4Level = n4.Level;
            actualN5Level = n5.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);
        }

        #endregion

        #region DeleteHard
        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void DeleteHard_deleteNullObject()
        {
            tree.DeleteHard(null);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteHard_deleteNotExistNode()
        {
            tree.DeleteHard(n2);
        }

        [TestMethod]
        public void DeleteHard_deleteNodeWithNoChild()
        {
            /*
             *        (1)     
             *        / \     
             *      (2) (3)   
             *       |
             *      (4)
             */

            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4 };
            var expectedN2Children = new List<Component> { n4 };
            Component expectedN4Parent = n2;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualN2Children = n2.Children;
            var actualN4Parent = n4.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);


            // check after deleting
            expectedTreeIncludedNodes = new List<Component> { root1, n2, n3 };
            expectedN2Children = new List<Component> { };
            expectedN4Parent = null;

            tree.DeleteHard(n4);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualN2Children = n2.Children;
            actualN4Parent = n4.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);

        }

        [TestMethod]
        public void DeleteHard_deleteNodeWithChild()
        {
            /*
             *        (1)     
             *        / \     
             *      (2) (3)   
             *       |
             *      (4)
             */

            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4 };
            var expectedRootChildren = new List<Component> { n2, n3 };
            Component expectedN2Parent = tree.RootNode;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);


            // check after deleting N2 node(with all child)
            expectedTreeIncludedNodes = new List<Component> { root1, n3 };
            expectedRootChildren = new List<Component> { n3 };
            expectedN2Parent = null;

            tree.DeleteHard(n2);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
        }

        [TestMethod]
        public void DeleteHard_deleteRoot()
        {
            Assert.IsNotNull(tree.RootNode);
            tree.AddNode(tree.RootNode, n2);
            tree.DeleteHard(tree.RootNode);
            Assert.IsNull(tree.RootNode);
        }

        #endregion

        #region DeleteSafe
        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void DeleteSafe_deleteNullObject()
        {
            tree.DeleteHard(null);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteSafe_deleteNotExistNode()
        {
            tree.DeleteHard(n2);
        }

        [TestMethod]
        public void DeleteSafe_deleteNodeWithNoChild()
        {
            /*
             *        (1)     
             *        / \     
             *      (2) (3)   
             *       |
             *      (4)
             */

            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4 };
            var expectedN2Children = new List<Component> { n4 };
            Component expectedN4Parent = n2;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualN2Children = n2.Children;
            var actualN4Parent = n4.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);


            // check after deleting
            expectedTreeIncludedNodes = new List<Component> { root1, n2, n3 };
            expectedN2Children = new List<Component> { };
            expectedN4Parent = null;

            tree.DeleteSafe(n4);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualN2Children = n2.Children;
            actualN4Parent = n4.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
        }

        [TestMethod]
        public void DeleteSafe_deleteNodeWithChild()
        {
            /*
                *        (1)     
                *        / \     
                *      (2) (3)   
                *      / \
                *    (4) (5)
                *     |
                *    (6)
                */

            // check start condition
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4, n5, n6 };
            var expectedRootChildren = new List<Component> { n2, n3 };
            var expectedN2Parent = tree.RootNode;
            var expectedN4Parent = n2;
            var expectedN5Parent = n2;
            var expectedN6Parent = n4;
            var expectedN4Level = 2;
            var expectedN5Level = 2;
            var expectedN6Level = 3;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);
            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;
            var actualN4Parent = n4.Parent;
            var actualN5Parent = n5.Parent;
            var actualN6Parent = n6.Parent;
            var actualN4Level = n4.Level;
            var actualN5Level = n5.Level;
            var actualN6Level = n6.Level;


            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            Assert.AreEqual(expectedN6Parent, actualN6Parent);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);
            Assert.AreEqual(expectedN6Level, actualN6Level);


            // check after deleting N2 node(with all child)
            expectedTreeIncludedNodes = new List<Component> { root1, n3, n4, n5, n6 };
            expectedRootChildren = new List<Component> { n3, n4, n5 };
            expectedN2Parent = null;
            expectedN4Parent = tree.RootNode;
            expectedN5Parent = tree.RootNode;
            expectedN6Parent = n4;
            expectedN4Level = 1;
            expectedN5Level = 1;
            expectedN6Level = 2;

            tree.DeleteSafe(n2);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN4Parent = n4.Parent;
            actualN5Parent = n5.Parent;
            actualN6Parent = n6.Parent;
            actualN4Level = n4.Level;
            actualN5Level = n5.Level;
            actualN6Level = n6.Level;


            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            Assert.AreEqual(expectedN6Parent, actualN6Parent);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);
            Assert.AreEqual(expectedN6Level, actualN6Level);


        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteSafe_deleteRoot()
        {
            tree.DeleteSafe(tree.RootNode);
        }

        #endregion

        #region Find 

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Find_findNullObject()
        {
            tree.Find(null);
        }

        [TestMethod]
        public void Find_findRoot()
        {
            var expectedAnswer = root1;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            var actualAnswer = tree.Find(d0);

            Assert.AreEqual(expectedAnswer, actualAnswer);
        }

        [TestMethod]
        public void Find_findNotExistNode()
        {
            Component expectedAnswer = null;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);

            var actualAnswer = tree.Find(d7);

            Assert.AreEqual(expectedAnswer, actualAnswer);
        }

        [TestMethod]
        public void Find_findNode()
        {
            /*
            *        (1)     
            *        / \     
            *      (2) (3)   
            *      / \
            *    (4) (5)
            *     |
            *    (6)
            */
            var expectedAnswer = n6;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);
            var actualAnswer = tree.Find(d6);

            Assert.AreEqual(expectedAnswer, actualAnswer);
        }

        #endregion

        #region ForEach
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ForEach_StartFromNullNode()
        {

            tree.ForEach((Component a) => { }, null);
        }

        [TestMethod]
        public void ForEach_AllTreeIterateCheck()
        {
            /*
                *        (1)     
                *       /   \     
                *     (2)   (3)   
                *     / \    |
                *   (4) (5) (6)
                *    |   
                *   (7)  
                */

            var expecterNodeCount = 7;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n3, n6);
            tree.AddNode(n4, n7);
            var actuarNodeCount = 0;
            tree.ForEach((Component a) => { actuarNodeCount++; }, tree.RootNode);

            Assert.AreEqual(expecterNodeCount, actuarNodeCount);
        }

        [TestMethod]
        public void ForEach_IterateFromChild()
        {
            /*
            *             (1)     
            *            /   \     
            * start(2)- (2)   (3)   
            *           / \    |
            *          (4) (5) (6)
            *           |   
            *          (7)  
            */

            var expecterNodeCount = 4;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n3, n6);
            tree.AddNode(n4, n7);
            var actuarNodeCount = 0;
            tree.ForEach((Component a) => { actuarNodeCount++; }, n2);

            Assert.AreEqual(expecterNodeCount, actuarNodeCount);

        }

        #endregion

        #region ReplaceChild
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceChild_nullParent()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(n2, n3);
            tree.ReplaceChild(null, n3, n4);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceChild_nullOldChild()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(n2, n3);
            tree.ReplaceChild(n2, null, n4);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceChild_nullNewChild()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(n2, n3);
            tree.ReplaceChild(n2, n3, null);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ReplaceChild_notExistOldChild()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(n2, n3);
            tree.ReplaceChild(n2, n4, n5);
        }

        [TestMethod]
        public void ReplaceChild()
        {
            /*
            *        (1)       (7)
            *        / \        |
            *      (2) (3)     (8)
            *      / \
            *    (4) (5)
            *     |
            *    (6)
            *    replace (2) for (7)
            */

            // check start initialization
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4, n5, n6 };
            var expectedRootChildren = new List<Component> { n2, n3 };
            Component expectedN2Parent = tree.RootNode;
            Component expectedN7Parent = null;
            var expectedN8Parent = n7;
            var expectedN7Level = 0;
            var expectedN8Level = 1;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);
            n7.AddChild(n8);
            n8.SetParent(n7);
            n8.Level = 1;
           

            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;
            var actualN7Parent = n7.Parent;
            var actualN8Parent = n8.Parent;
            var actualN7Level = n7.Level;
            var actualN8Level = n8.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN7Parent, actualN7Parent);
            Assert.AreEqual(expectedN8Parent, actualN8Parent);
            Assert.AreEqual(expectedN7Level, actualN7Level);
            Assert.AreEqual(expectedN8Level, actualN8Level);


            // replace (2) for (7)
            expectedTreeIncludedNodes = new List<Component> { root1, n3, n7, n8 };
            expectedRootChildren = new List<Component> { n7, n3 };
            expectedN2Parent = null;
            expectedN7Parent = tree.RootNode;
            expectedN8Parent = n7;
            expectedN7Level = 1;
            expectedN8Level = 2;

            tree.ReplaceChild(tree.RootNode, n2, n7);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN7Parent = n7.Parent;
            actualN8Parent = n8.Parent;
            actualN7Level = n7.Level;
            actualN8Level = n8.Level;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN7Parent, actualN7Parent);
            Assert.AreEqual(expectedN8Parent, actualN8Parent);
            Assert.AreEqual(expectedN7Level, actualN7Level);
            Assert.AreEqual(expectedN8Level, actualN8Level);

        }

        #endregion

        #region ReplaceNode

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceNode_null_oldNode()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.ReplaceNode(null, n3);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceNode_null_newNode()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.ReplaceNode(n2, null);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ReplaceNode_replaceRoot()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.ReplaceNode(tree.RootNode, n3);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ReplaceNode_replaceNotExistNode()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.ReplaceNode(n3, n4);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ReplaceNode_replaceNewNodeHasChild()
        {
            tree.AddNode(tree.RootNode, n2);
            n3.AddChild(n4);
            tree.ReplaceNode(n2, n3);
        }



        [TestMethod]
        public void ReplaceNode()
        {
            /*
            *        (1)       (7)
            *        / \       
            *      (2) (3)     
            *      / \
            *    (4) (5)
            *     |
            *    (6)
            *    replace (2) for (7)
            */

            // check start initialization
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4, n5, n6 };
            var expectedRootChildren = new List<Component> { n2, n3 };
            Component expectedN2Parent = tree.RootNode;
            var expectedN2Children = new List<Component> { n4, n5 };
            Component expectedN7Parent = null;
            var expectedN7Children = new List<Component> { };
            var expectedN7Level = 0;
            var expectedN4Parent = n2;
            var expectedN5Parent = n2;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);

            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;
            var actualN2Children = n2.Children;
            var actualN7Parent = n7.Parent;
            var actualN7Children = n7.Children;
            var actualN7Level = n7.Level;
            var actualN4Parent = n4.Parent;
            var actualN5Parent = n5.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN7Parent, actualN7Parent);
            CollectionAssert.AreEqual(expectedN7Children, actualN7Children);
            Assert.AreEqual(expectedN7Level, actualN7Level);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);


            // replace (2) node for (7)
            expectedTreeIncludedNodes = new List<Component> { root1, n7, n3, n4, n5, n6 };
            expectedRootChildren = new List<Component> { n7, n3 };
            expectedN2Parent = null;
            expectedN2Children = new List<Component> { };
            expectedN7Parent = tree.RootNode;
            expectedN7Children = new List<Component> { n4, n5 };
            expectedN7Level = 1;
            expectedN4Parent = n7;
            expectedN5Parent = n7;

            tree.ReplaceNode(n2, n7);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN2Children = n2.Children;
            actualN7Parent = n7.Parent;
            actualN7Children = n7.Children;
            actualN7Level = n7.Level;
            actualN4Parent = n4.Parent;
            actualN5Parent = n5.Parent;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN7Parent, actualN7Parent);
            CollectionAssert.AreEqual(expectedN7Children, actualN7Children);
            Assert.AreEqual(expectedN7Level, actualN7Level);
            Assert.AreEqual(expectedN4Parent, actualN4Parent);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
        }
        #endregion

        #region ReplaceRoot

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ReplaceRoot_null_newNode()
        {
            tree.AddNode(tree.RootNode, n2);
            tree.ReplaceRoot(null);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ReplaceRoot_replaceNewRootHasChild()
        {
            tree.AddNode(tree.RootNode, n2);
            n3.AddChild(n4);
            tree.ReplaceRoot(n3);
        }

        [TestMethod]
        public void ReplaceRoot()
        {
            /*
            *        (1)    
            *        / \       
            *      (2) (3)        (5) - level 1
            *       |
            *      (4)
            */

            // check start initialization
            var expectedTreeIncludedNodes = new List<Component> { root1, n2, n3, n4};
            var expectedRootChildren = new List<Component> { n2, n3 };
            var expectedN2Parent = root1;
            var expectedN3Parent = root1;
            var expectedN2Children = new List<Component> { n4 };
            Component expectedN5Parent = null;
            var expectedN5Children = new List<Component> { };
            var expectedN2Level = 1;
            var expectedN3Level = 1;
            var expectedN4Level = 2;
            var expectedN5Level = 1;
            var expectedRoot = root1;
            var expectedRoot1Children = new List<Component> { n2, n3 };


            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            n5.Level = 1;

            var actualTreeIncludedNodes = tree.IncludedNodes;
            var actualRootChildren = tree.RootNode.Children;
            var actualN2Parent = n2.Parent;
            var actualN3Parent = n2.Parent;
            var actualN2Children = n2.Children;
            var actualN5Parent = n5.Parent;
            var actualN5Children = n5.Children;
            var actualN2Level = n2.Level;
            var actualN3Level = n3.Level;
            var actualN4Level = n4.Level;
            var actualN5Level = n5.Level;
            var actualRoot = tree.RootNode;
            var actualRoot1Children = root1.Children;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN3Parent, actualN3Parent);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            CollectionAssert.AreEqual(expectedN5Children, actualN5Children);
            Assert.AreEqual(expectedN2Level, actualN2Level);
            Assert.AreEqual(expectedN3Level, actualN3Level);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);
            Assert.AreEqual(expectedRoot, actualRoot);
            CollectionAssert.AreEqual(expectedRoot1Children, actualRoot1Children);


            // replace root
            expectedTreeIncludedNodes = new List<Component> { n5, n2, n3, n4 };
            expectedRootChildren = new List<Component> { n2, n3 };
            expectedN2Parent = n5;
            expectedN3Parent = n5;
            expectedN2Children = new List<Component> { n4 };
            expectedN5Parent = null;
            expectedN5Children = new List<Component> { n2, n3 };
            expectedN2Level = 1;
            expectedN3Level = 1;
            expectedN4Level = 2;
            expectedN5Level = 0;
            expectedRoot = n5;
            expectedRoot1Children = new List<Component> {  };

            tree.ReplaceRoot(n5);
            actualTreeIncludedNodes = tree.IncludedNodes;
            actualRootChildren = tree.RootNode.Children;
            actualN2Parent = n2.Parent;
            actualN3Parent = n2.Parent;
            actualN2Children = n2.Children;
            actualN5Parent = n5.Parent;
            actualN5Children = n5.Children;
            actualN2Level = n2.Level;
            actualN3Level = n3.Level;
            actualN4Level = n4.Level;
            actualN5Level = n5.Level;
            actualRoot = tree.RootNode;
            actualRoot1Children = root1.Children;

            CollectionAssert.AreEqual(expectedTreeIncludedNodes, actualTreeIncludedNodes);
            CollectionAssert.AreEqual(expectedRootChildren, actualRootChildren);
            Assert.AreEqual(expectedN2Parent, actualN2Parent);
            Assert.AreEqual(expectedN3Parent, actualN3Parent);
            CollectionAssert.AreEqual(expectedN2Children, actualN2Children);
            Assert.AreEqual(expectedN5Parent, actualN5Parent);
            CollectionAssert.AreEqual(expectedN5Children, actualN5Children);
            Assert.AreEqual(expectedN2Level, actualN2Level);
            Assert.AreEqual(expectedN3Level, actualN3Level);
            Assert.AreEqual(expectedN4Level, actualN4Level);
            Assert.AreEqual(expectedN5Level, actualN5Level);
            Assert.AreEqual(expectedRoot, actualRoot);
            CollectionAssert.AreEqual(expectedRoot1Children, actualRoot1Children);
        }

        #endregion

        #region Siblings

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Sibling_NullNode()
        {
            foreach (var item in tree.Siblings(null))
            {
            }
        }

        [TestMethod]
        public void Sibling_RootSiblings()
        {
            var expectedSiblings = new List<Component> { };
            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);

            var actualSiblings = new List<Component> { };

            foreach (var sibling in tree.Siblings(tree.RootNode))
            {
                actualSiblings.Add(sibling);
            }

            CollectionAssert.AreEqual(expectedSiblings, actualSiblings);

        }

        [TestMethod]
        public void Sibling_NoSibling()
        {
           /*
            *        (  1  )    
            *        /  |  \   
            *      (2) (3) (4)       
            *       |       |
            *      (5)     (6)
            *      
            *      (6) - siblings
            */
            var expectedSiblings = new List<Component> { };

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(tree.RootNode, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);

            var actualSiblings = new List<Component> { };

            foreach (var sibling in tree.Siblings(n6))
            {
                actualSiblings.Add(sibling);
            }

            CollectionAssert.AreEqual(expectedSiblings, actualSiblings);

        }

        [TestMethod]
        public void Sibling()
        {
            /*
             *        (  1  )    
             *        /  |  \   
             *      (2) (3) (4)       
             *       |       |
             *      (5)     (6)
             *      
             *      (4) - siblings
             */
            var expectedSiblings = new List<Component> { n2, n3 };

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(tree.RootNode, n4);
            tree.AddNode(n2, n5);
            tree.AddNode(n4, n6);

            var actualSiblings = new List<Component> { };

            foreach (var sibling in tree.Siblings(n4))
            {
                actualSiblings.Add(sibling);
            }

            CollectionAssert.AreEqual(expectedSiblings, actualSiblings);

        }
        #endregion

        #region UpwardForEach

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UpwardForEach_NullStartNode()
        {
            tree.UpwardForEach((Component a) => { }, null);
        }

        [TestMethod]
        public void UpwardForEach()
        {
            /*
             *        (1)    
             *        / \  
             *      (2) (3)        
             *       |   |    
             *      (4) (5)   
             *           |
             *          (6)
             */
            var expectedDepthN6 = 4;
            var expectedDepthN3 = 2;

            tree.AddNode(tree.RootNode, n2);
            tree.AddNode(tree.RootNode, n3);
            tree.AddNode(n2, n4);
            tree.AddNode(n3, n5);
            tree.AddNode(n5, n6);
            var actualDepthN6 = 0;
            var actualDepthN3 = 0;

            tree.UpwardForEach((Component a) => { actualDepthN6++; }, n6);
            tree.UpwardForEach((Component a) => { actualDepthN3++; }, n3);

            Assert.AreEqual(expectedDepthN6, actualDepthN6);
            Assert.AreEqual(expectedDepthN3, actualDepthN3);
        }
        #endregion

    }
}
