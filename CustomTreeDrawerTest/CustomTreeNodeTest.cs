using CustomTreeDrawer.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomTreeDrawerTest
{
    /// <summary>
    /// CustomTreeNodeTest tests
    /// </summary>
    [TestFixture]
	public class CustomTreeNodeTest
	{
		private CustomTreeNode complexRoot;
		private CustomTreeNode node43;
		private CustomTreeNode node32;

		/// <summary>
		/// CustomTreeNodeTest FixtureSetUp
		/// </summary>
		[OneTimeSetUp]
		public void FixtureSetUp()
		{
			CustomTreeNode node71 = new CustomTreeNode(new SomeCustomClass("71"));
			CustomTreeNode node61 = new CustomTreeNode(new SomeCustomClass("61"), new List<CustomTreeNode>() { node71 });

			CustomTreeNode node51 = new CustomTreeNode(new SomeCustomClass("51"));
			CustomTreeNode node52 = new CustomTreeNode(new SomeCustomClass("52"));
			CustomTreeNode node53 = new CustomTreeNode(new SomeCustomClass("53"), new List<CustomTreeNode>() { node61 });


			CustomTreeNode node14 = new CustomTreeNode(new SomeCustomClass("14"), new List<CustomTreeNode>() { node51, node52 });
			CustomTreeNode node24 = new CustomTreeNode(new SomeCustomClass("24"), new List<CustomTreeNode>() { node53 });

			CustomTreeNode node13 = new CustomTreeNode(new SomeCustomClass("13"));
			CustomTreeNode node23 = new CustomTreeNode(new SomeCustomClass("23"));
			CustomTreeNode node33 = new CustomTreeNode(new SomeCustomClass("33"));
			node43 = new CustomTreeNode(new SomeCustomClass("Vratice se NODE"), new List<CustomTreeNode>() { node14, node24 });

			CustomTreeNode node12 = new CustomTreeNode(new SomeCustomClass("12"), new List<CustomTreeNode>() { node13, node23 });
			CustomTreeNode node22 = new CustomTreeNode(new SomeCustomClass("22"), new List<CustomTreeNode>() { node33, node43 });
			node32 = new CustomTreeNode(new SomeCustomClass("Coban je nas boban"));

			complexRoot = new CustomTreeNode(new SomeCustomClass("11"), new List<CustomTreeNode>() { node12, node22, node32 });
			complexRoot.Init(); // must initialize everything @see PropertiesShouldFailBeforeInitTest
		}

		/// <summary>
		/// CustomTreeNodeTest FixtureTearDown
		/// </summary>
		[OneTimeTearDown]
		public void FixtureTearDown()
		{
			complexRoot = null;
		}

		/// <summary>
		/// Check invalid parameters test
		/// </summary>
		[Test]
		public void PropertiesShouldFailBeforeInitTest()
		{
			CustomTreeNode node = new CustomTreeNode();
			Assert.Throws<InvalidOperationException>(delegate { var id = node.Id; });
			Assert.Throws<InvalidOperationException>(delegate { var parentId = node.ParentId; });
			Assert.Throws<InvalidOperationException>(delegate { var height = node.Height; });
			Assert.Throws<InvalidOperationException>(delegate { var width = node.Width; });
			Assert.Throws<InvalidOperationException>(delegate { var topPadding = node.TopPadding; });
			Assert.Throws<InvalidOperationException>(delegate { var leftPadding = node.LeftPadding; });
			Assert.Throws<InvalidOperationException>(delegate { var maxChildId = node.MaxChildId; });
		}

		/// <summary>
		/// CustomTreeNodeTest InitTest
		/// </summary>
		[Test]
		public void InitTest()
		{
			Assert.AreEqual(7, complexRoot.Height);
			Assert.AreEqual(7, complexRoot.Width);
			Assert.AreEqual(8, node43.Id);
			Assert.AreEqual(3, node43.ParentId);
			Assert.AreEqual(3, node43.Width);
			Assert.AreEqual(5, node43.Height);
			Assert.AreEqual(1, node32.Width);
			Assert.AreEqual(1, node32.Height);
			Assert.AreEqual(2, node43.ChildrenCount);
			Assert.AreEqual(2, node43.Children.Count());
			Assert.AreEqual(0, node32.ChildrenCount);
			Assert.AreEqual(0, node32.Children.Count());
			Assert.AreEqual(3, node43.LeftPadding);
			Assert.AreEqual(2, node43.TopPadding);
			Assert.AreEqual(6, node32.LeftPadding);
			Assert.AreEqual(1, node32.TopPadding);
			Assert.AreEqual(15, node43.MaxChildId);
		}

		/// <summary>
		/// CustomTreeNodeTest AddChildTest
		/// </summary>
		[Test]
		public void AddChildTest()
		{
			CustomTreeNode node = new CustomTreeNode();
			Assert.AreEqual(0, node.Children.Count());
			node.AddChild(new CustomTreeNode());
			Assert.AreEqual(1, node.Children.Count());
		}

		/// <summary>
		/// CustomTreeNodeTest TraverseTest
		/// </summary>
		[Test]
		public void TraverseTest()
		{
			int cnt = 0;
			foreach (var node in complexRoot.TraverseBreadthFirst())
			{
				cnt += ((SomeCustomClass)node.Info).Title.Length <= 2 ? 1 : 0;
			}

			Assert.AreEqual(13, cnt);
		}
	}
}
