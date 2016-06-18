using CustomTreeDrawer.Base;
using CustomTreeDrawer.Base.Interfaces;
using CustomTreeDrawer.Drawers;
using CustomTreeDrawer.Drawers.Interfaces;
using CustomTreeDrawer.Helpers;
using CustomTreeDrawer.TreeTypes;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace CustomTreeDrawerTest
{
	/// <summary>
	/// CustomTreeNode Helper tests
	/// </summary>
	[TestFixture]
	public class CustomTreeNodeHelperTest
	{
		/// <summary>
		/// Test for GetSizeOf
		/// </summary>
		[Test]
		public void GetSizeOfTest()
		{
			string str1 = "Jaako Dugacko";
			string str2 = "Dugacko";
			var n1 = new CustomTreeNode(new SomeCustomClass(str1));
			var n2 = new CustomTreeNode(new SomeCustomClass(str2));
			var root = new CustomTreeNode(new SomeCustomClass("11"), new List<CustomTreeNode>() { n1, n2 });
			root.Init();
			
			var sett = new CustomTreeDrawerSettings();
			sett.CanvasPaddingX = sett.CanvasPaddingY = 0;
			sett.SegmentHeight = sett.SegmentWidth = 5;
			sett.NodeSize = 2;
			
			ICustomTreeDrawer customTypeDrawer = Substitute.For<ICustomTreeDrawer>();
			customTypeDrawer.GetSizeOf(Arg.Any<CustomTreeNode>(), Arg.Any<double>(), Arg.Any<bool>()).Returns(a => {
				var node = (CustomTreeNode)a[0];
				var size = (double)a[1];
				return size + 5 * ((SomeCustomClass)node.Info).Title.Length;
			});

			ICustomTreeType type = new NewLineCustomTreeType(sett, customTypeDrawer);
			//x = settings.CanvasPaddingX + settings.SegmentWidth * node.LeftPadding + settings.NodeSize / 2.0f;

			((SomeCustomClass)n1.Info).Title = str1;
			var mxSize = CustomTreeHelper.DetermineSize(root, type, customTypeDrawer, sett.NodeSize);
			Assert.AreEqual(str1.Length * 5 + 2 + 5 * 0 + 1 - 1, mxSize);

			((SomeCustomClass)n1.Info).Title = str2;
			root.Init();
			mxSize = CustomTreeHelper.DetermineSize(root, type, customTypeDrawer, sett.NodeSize);
			Assert.AreEqual(str2.Length * 5 + 2 + 5 * 1 + 1 - 1, mxSize);
		}
	}
}
