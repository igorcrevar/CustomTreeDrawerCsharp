using CustomTreeDrawer.Base;
using CustomTreeDrawer.Base.Interfaces;
using CustomTreeDrawer.Drawers.Interfaces;

namespace CustomTreeDrawer.Helpers
{
	/// <summary>
	/// Provides various helpers for custom tree
	/// </summary>
	public static class CustomTreeHelper
	{
		/// <summary>
		/// Determine size (max width). This matches the max (width + x position) of all nodes in tree
		/// </summary>
		/// <param name="rootNode">root node of the tree</param>
		/// <param name="treeType">ICustomTreeType instance</param>
		/// <param name="drawer">ICustomTreeDrawer instance</param>
		/// <param name="nodeSize">node size from settings</param>
        /// <returns>max size/width</returns>
		public static double DetermineSize(CustomTreeNode rootNode, ICustomTreeType treeType, ICustomTreeDrawer drawer, double nodeSize)
		{
			double maxSize = 0;
			foreach (var node in rootNode.TraverseBreadthFirst())
			{
				double x, y;
				treeType.GetXY(node, out x, out y);
				double size = x + drawer.GetSizeOf(node, nodeSize, false);
				if (size > maxSize)
				{
					maxSize = size;
				}
			}

			return maxSize - nodeSize / 2.0f;
		}
	}
}
