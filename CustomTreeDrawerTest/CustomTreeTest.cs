using CustomTreeDrawer.Base;
using CustomTreeDrawer.Drawers;
using CustomTreeDrawer.Drawers.Interfaces;
using CustomTreeDrawer.TreeTypes;
using NSubstitute;
using NUnit.Framework;
using System;

namespace CustomTreeDrawerTest
{
	/// <summary>
	/// Tests for custom tree
	/// </summary>
	[TestFixture]
	public class CustomTreeTest
	{
		private CustomTree customTree;
		
		/// <summary>
		/// CustomTreeTest FixtureSetUp
		/// </summary>
		[OneTimeSetUp]
		public void FixtureSetUp()
		{
			var actualDrawerMock = Substitute.For<ICustomTreeDrawer>();
			var settings = new CustomTreeDrawerSettings()
			{
				SegmentHeight = 4,
				SegmentWidth = 2,
				CanvasPaddingX = 1,
				CanvasPaddingY = 1,
				NodeSize = 2,
			};
			customTree = new CustomTree(new NewLineCustomTreeType(settings, actualDrawerMock));
		}

		/// <summary>
		/// CustomTreeTest SelectedNode, currently too simple
		/// </summary>
		[Test]
		public void SelectedNodeTest()
		{
			customTree.SelectedNode = null;
			Assert.AreEqual(null, customTree.SelectedNode);
			var node = new CustomTreeNode();
			customTree.SelectedNode = node;
			Assert.AreEqual(node, customTree.SelectedNode);
		}

		/// <summary>
		/// CustomTreeTest update test
		/// </summary>
		[Test]
		public void UpdateTest()
		{
			var node = new CustomTreeNode();
			customTree.SelectedNode = node;
			customTree.Update(node);
			Assert.DoesNotThrow(delegate { var id = node.Id; }); // because update must call init for root node!
			Assert.AreEqual(null, customTree.SelectedNode); // update should clear selection
		}

		/// <summary>
		/// CustomTreeTest Refresh test
		/// </summary>
		[Test]
		public void RefreshTest()
		{
			Assert.DoesNotThrow(() => customTree.Update(null)); // update will null root shuld not throw
			var node = new CustomTreeNode();
			customTree.SelectedNode = node;
			customTree.Refresh();
			Assert.Throws<InvalidOperationException>(delegate { var id = node.Id; }); // because refresh must not call init for root node!
			Assert.AreEqual(node, customTree.SelectedNode); // refresh should not clear selection
		}
	}
}
