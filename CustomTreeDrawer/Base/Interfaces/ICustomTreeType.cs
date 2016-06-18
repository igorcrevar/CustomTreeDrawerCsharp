using System;

namespace CustomTreeDrawer.Base.Interfaces
{
	public interface ICustomTreeType : IDisposable
	{
		void Update(CustomTreeNode rootNode);
		void BeginDraw();
		void EndDraw();
		void Draw(CustomTreeNode node, bool isSelected);
		void GetXY(CustomTreeNode node, out double x, out double y);
		bool IsHit(CustomTreeNode node, double x, double y);
		double Width { get; }
		double Height { get; }
		bool AllowsPartialRedraw { get; }
	}
}
