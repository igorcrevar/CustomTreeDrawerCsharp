using CustomTreeDrawer.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace CustomTreeDrawer.Base
{
	/// <summary>
	/// Class which provides all functionalities required to render tree
	/// </summary>
	public class CustomTree
	{
		private readonly CustomTreeNode fakeEmptyNode = new CustomTreeNode(); // empty tree (root == null) will be modeled with this fake root node

		private ICustomTreeType nodeDrawer;
		private CustomTreeNode rootNode;
		private CustomTreeNode selectedNode;

		public CustomTree(ICustomTreeType nodeDrawer)
		{
			this.nodeDrawer = nodeDrawer;
		}

		/// <summary>
		/// Update tree, refresh all settings and redraw
		/// </summary>
		/// <param name="rootNode">new root node. Can not be null</param>
		public void Update(CustomTreeNode rootNode)
		{
			// if null is set for root node, swap it with our fake node
			if (rootNode == null)
			{
				rootNode = fakeEmptyNode;
			}

			this.selectedNode = null; // selected node should be cleared
			this.rootNode = rootNode;
			this.rootNode.Init();
			Refresh();
		}

		/// <summary>
		/// Also Draw tree. 
		/// Note that Update method must be at least once called
		/// </summary>
		public void Refresh()
		{
			this.nodeDrawer.Update(rootNode);
			Draw();
		}

		/// <summary>
		/// Draw tree. 
		/// Note that Update method must be at least once called before this method
		/// </summary>
		public void Draw()
		{
			// if root is epmty (eq fake node) just refresh screen and quit
			if (rootNode == fakeEmptyNode)
			{
				this.nodeDrawer.BeginDraw();
				this.nodeDrawer.EndDraw();
				return;
			}

			this.nodeDrawer.BeginDraw();
            foreach (var node in rootNode.TraverseBreadthFirst())
			{
				nodeDrawer.Draw(node, node == selectedNode);
			}

			this.nodeDrawer.EndDraw();
		}

		/// <summary>
		/// Get node which is located on position (x, y)
		/// </summary>
		/// <param name="x">x position</param>
		/// <param name="y">y position</param>
		/// <returns>CustomTreeNode on position (x, y) or null if there is no node on this position</returns>
		public CustomTreeNode GetFor(double x, double y)
		{
			// do not process for empty tree
			if (rootNode == fakeEmptyNode)
			{
				return null;
			}

			foreach (var node in rootNode.TraverseBreadthFirst())
			{
				if (nodeDrawer.IsHit(node, x, y))
				{
					return node;
				}
			}

			return null;
		}

		/// <summary>
		/// Traverse all nodes in tree and yields only those which satisfies cond condition
		/// </summary>
		/// <param name="cond">condition</param>
		/// <returns>nodes in tree satisfies condition</returns>
		public IEnumerable<CustomTreeNode> FindNodes(Func<CustomTreeNode, bool> cond)
		{
			if (rootNode != fakeEmptyNode)
			{
				foreach (var node in rootNode.TraverseBreadthFirst())
				{
					if (cond(node))
					{
						yield return node;
					}
				}
			}
		}

		/// <summary>
		/// Change node which is selected
		/// </summary>
		public CustomTreeNode SelectedNode
		{
			get
			{
				return selectedNode;
			}

			set
			{
				// if nothing need to be changed or root node is empty -> quit
				if (value != selectedNode || rootNode == fakeEmptyNode)
				{
					var old = selectedNode;
					selectedNode = value;

					// redraw selection
					if (nodeDrawer.AllowsPartialRedraw)
					{
						RedrawSelected(old, selectedNode);
					}
					else
					{
						Draw();
					}
				}
			}
		}

		private void RedrawSelected(CustomTreeNode old, CustomTreeNode current)
		{
			this.nodeDrawer.BeginDraw();
			if (old != null)
			{
				nodeDrawer.Draw(old, false);
			}

			if (current != null)
			{
				nodeDrawer.Draw(current, true);
			}

			this.nodeDrawer.EndDraw();
		}
	}
}
