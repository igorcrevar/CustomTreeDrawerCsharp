using CustomTreeDrawer.Base;
using CustomTreeDrawer.Base.Interfaces;
using CustomTreeDrawer.Drawers;
using CustomTreeDrawer.Drawers.Interfaces;
using CustomTreeDrawer.Helpers;
using System;

namespace CustomTreeDrawer.TreeTypes
{
	public class NewLineCustomTreeType : ICustomTreeType
	{
		private readonly ICustomTreeDrawer actualDrawer;
		private CustomTreeDrawerSettings settings;
		private double width = double.NaN;
		private double height = double.NaN;

		public NewLineCustomTreeType(CustomTreeDrawerSettings settings, ICustomTreeDrawer actualDrawer)
		{
			this.settings = settings;
			this.actualDrawer = actualDrawer;
		}

        public NewLineCustomTreeType(ICustomTreeDrawer actualDrawer)
        {
            this.settings = CreateDefaultSettings();
            this.actualDrawer = actualDrawer;
        }

        public void Draw(CustomTreeNode node, bool isSelected)
		{
			double nodeSizeHalf = settings.NodeSize / 2.0f;
			double x, y;
			GetXY(node, out x, out y);

			// has at least one child
			if (node.ChildrenCount > 0)
			{
				// has at least one children draw half of vertical line bellow
				actualDrawer.DrawLine(x,
										  x,
										  y + settings.NodeSize,
										  y + settings.NodeSize + settings.SegmentHeight / 2.0f);

				// more children
				if (node.ChildrenCount > 1)
				{
					// horizonatal line
					actualDrawer.DrawLine(x,
											  x + settings.SegmentWidth * node.ChildrenWidth,
											  y + settings.NodeSize + settings.SegmentHeight / 2.0f,
											  y + settings.NodeSize + settings.SegmentHeight / 2.0f);
				}
			}

			// not root draw half of vertical line above
			if (node.TopPadding > 0)
			{
				actualDrawer.DrawLine(x,
										  x,
										  y - settings.SegmentHeight / 2.0f - (settings.SegmentHeight + settings.NodeSize) * (node.Id - node.ParentId - 1),
										  y);
			}

			if (isSelected)
			{
				actualDrawer.DrawWholeLineSelected(y - settings.SegmentHeight / 2.0f, settings.SegmentHeight + settings.NodeSize);
			}

			actualDrawer.DrawNode(x - settings.NodeSize / 2.0f, 
									  y,
									  settings.NodeSize,
									  false, node.Info);
		}

		public void GetXY(CustomTreeNode node, out double x, out double y)
		{
			if (node == null)
			{
				throw new ArgumentNullException();
			}

			x = settings.CanvasPaddingX + settings.SegmentWidth * node.LeftPadding + settings.NodeSize / 2.0f;
			y = settings.CanvasPaddingY + (settings.NodeSize + settings.SegmentHeight) * (node.Id - 1);
		}

		/// <summary>
		/// Check if given node position matches coords (specified by (X, Y)
		/// This is highly unoptimized. This can be done much better / faster
		/// </summary>
		/// <param name="node">CustomTreeNode object</param>
		/// <param name="x">x position </param>
		/// <param name="y">y position</param>
		/// <returns>returns true if matched</returns>
		public bool IsHit(CustomTreeNode node, double x, double y)
		{
			double nx, ny;
			GetXY(node, out nx, out ny);
			double startY = ny - settings.SegmentHeight / 2.0f;
			double endY = startY + settings.SegmentHeight + settings.NodeSize;
			return startY <= y && y <= endY;
		}

		public void Update(CustomTreeNode rootNode)
		{
			width = settings.CanvasPaddingX * 2 + CustomTreeHelper.DetermineSize(rootNode, this, this.actualDrawer, settings.NodeSize);
			height = settings.CanvasPaddingY * 2 + (rootNode.MaxChildId - 1) * settings.SegmentHeight + settings.NodeSize * rootNode.MaxChildId;
			actualDrawer.OnUpdate(width, height);
		}
		
		public double Width
		{
			get { return width; }
		}

		public double Height
		{
			get { return height; }
		}

		public void BeginDraw()
		{
            actualDrawer.BeginDraw();
		}

		public void EndDraw()
		{
			actualDrawer.EndDraw();
		}

		public bool AllowsPartialRedraw
		{
			get { return actualDrawer.AllowsPartialRedraw; }
		}

		public void Dispose()
		{
			actualDrawer.Dispose();
		}

		public static CustomTreeDrawerSettings CreateDefaultSettings()
		{
			var settings = new CustomTreeDrawerSettings()
			{
				SegmentHeight = 10,
				SegmentWidth = 100,
				CanvasPaddingX = 5,
				CanvasPaddingY = 5,
				NodeSize = 16,
			};
			return settings;
		}
	}
}
