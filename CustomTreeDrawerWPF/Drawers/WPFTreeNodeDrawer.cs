using CustomTreeDrawer.Base;
using CustomTreeDrawer.Drawers.Interfaces;
using CustomTreeDrawerWPF.Drawers.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomTreeDrawerWPF.Drawers
{
	public class WPFTreeNodeDrawer : ICustomTreeDrawer
	{
		private class VisualHost : FrameworkElement
		{
			private VisualCollection children;
			private DrawingVisual drawingVisual;

			public VisualHost()
			{
				this.ClipToBounds = false; // really really important for DrawWholeLineSelected parent.ActualWidth
				drawingVisual = new DrawingVisual();
				children = new VisualCollection(this);
				children.Add(drawingVisual);
			}

			public DrawingVisual Drawing { get { return drawingVisual; } }

			protected override int VisualChildrenCount
			{
				get { return children.Count; }
			}

			protected override Visual GetVisualChild(int index)
			{
				if (index < 0 || index >= children.Count)
					throw new ArgumentOutOfRangeException();

				return children[index];
			}
		}

		private const int VERY_LARGE_WIDTH_BECAUSE_OF_ACTUALWIDTH_PROBLEM = 20192;
		private readonly Panel parent;
		private readonly VisualHost drawingHost = new VisualHost();
		private DrawingContext drawingContext;
		private WPFTreeNodeDrawerSettings settings;

		public WPFTreeNodeDrawer(Panel parent, WPFTreeNodeDrawerSettings settings)
		{
			this.parent = parent;
			this.settings = settings;
			this.parent.Children.Add(drawingHost);
			this.drawingHost.VerticalAlignment = VerticalAlignment.Top;
			this.drawingHost.HorizontalAlignment = HorizontalAlignment.Left;
		}

		public void DrawLine(double x1, double x2, double y1, double y2)
		{
			drawingContext.DrawLine(new Pen(settings.LineColor, settings.LineThickness), new Point(x1, y1), new Point(x2, y2));
		}

		public virtual void DrawNode(double x, double y, double size, bool isSelected, object nodeInfo)
		{
			var formattedText = GetFormattedText(nodeInfo, isSelected);
			drawingContext.DrawText(formattedText, new Point(x + size + settings.TextPositionCorrection.X, y + settings.TextPositionCorrection.Y));
			drawingContext.DrawRectangle(!isSelected ? settings.NodeColor : settings.NodeSelectedColor,
											 new Pen(settings.LineColor, settings.LineThickness), new Rect(x, y, size, size));
		}

		public void OnUpdate(double width, double height)
		{
			drawingHost.Width = width;
			drawingHost.Height = height;
		}

		public void EndDraw()
		{
			if (drawingContext != null)
			{
				drawingContext.Close();
			}
		}

		public void BeginDraw()
		{
			drawingContext = drawingHost.Drawing.RenderOpen();
			// fill parent with background color. Allow us clicking anywhere on parent
			drawingContext.DrawRectangle(settings.BackgroundColor, new Pen(settings.BackgroundColor, 0.001), new Rect(0, 0, drawingHost.Width, drawingHost.Height));
		}

		public bool AllowsPartialRedraw
		{
			get { return false; }
		}

		public void DrawWholeLineSelected(double y, double height)
		{
			// draw some huge rect in width - because parent.ActualWidth is piece of s.it and you can not rely on that 
			var rect = new Rect(0, y, VERY_LARGE_WIDTH_BECAUSE_OF_ACTUALWIDTH_PROBLEM, height);
			drawingContext.DrawRectangle(settings.NodeSelectedColor, new Pen(settings.LineColor, 0.01), rect);
		}

		public virtual double GetSizeOf(CustomTreeNode node, double nodeSize, bool isSelected)
		{
			if (node.Info is ICustomTreeSimpleNodeInfo)
			{
				var formattedText = GetFormattedText(node.Info, isSelected);
				return formattedText.Width + nodeSize + settings.TextPositionCorrection.X;
			}

			return 0.0;
		}

		public void Dispose()
		{
		}

		protected FormattedText GetFormattedText(object nodeInfo, bool isSelected)
		{
			ICustomTreeSimpleNodeInfo versionNodeInfo = nodeInfo as ICustomTreeSimpleNodeInfo;
			if (versionNodeInfo == null)
			{
				throw new ArgumentException("Node.Info must be ICustomTreeSimpleNodeInfo implementation for WPF drawer");
			}
			var formattedText = new FormattedText(versionNodeInfo.Title,
												  settings.FontCultureInfo,
												  FlowDirection.LeftToRight,
												  new Typeface(settings.FontFamily, FontStyles.Normal, isSelected ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal),
												  settings.FontSize, settings.FontColor);
			return formattedText;
		}
	}
}
