using System;
using CustomTreeDrawer.Base;

namespace CustomTreeDrawer.Drawers.Interfaces
{
	public interface ICustomTreeDrawer : IDisposable
	{
		void DrawNode(double x, double y, double size, bool isSelected, object nodeInfo);
		void DrawLine(double x1, double x2, double y1, double y2);
		void OnUpdate(double width, double height);
		void BeginDraw();
		void EndDraw();
		bool AllowsPartialRedraw { get; }
		void DrawWholeLineSelected(double y, double height);
		double GetSizeOf(CustomTreeNode node, double nodeSize, bool isSelected);
    }
}
