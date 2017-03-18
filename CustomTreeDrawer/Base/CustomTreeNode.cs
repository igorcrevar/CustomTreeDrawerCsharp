using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomTreeDrawer.Base
{
	public class CustomTreeNode
	{
		/// <summary>
		/// Info object. Node is wrapper around this object
		/// </summary>
		private object info;
		/// <summary>
		/// Children nodes of this node (null if no children is available, not empty list!)
		/// </summary>
		private IList<CustomTreeNode> children;
		/// <summary>
		/// Width
		/// </summary>
		private int width = -1;
		/// <summary>
		/// Height
		/// </summary>
		private int height = -1;
		/// <summary>
		/// Left padding
		/// </summary>
		private int leftPadding = -1;
		/// <summary>
		/// Top padding
		/// </summary>
		private int topPadding = -1;
		/// <summary>
		/// order number of node
		/// </summary>
		private int id = -1;
		/// <summary>
		/// order number of node parent
		/// </summary>
		private int parentId = -1;
		/// <summary>
		/// max child id
		/// </summary>
		private int maxChildId = -1;

		public CustomTreeNode()
		{
		}

		public CustomTreeNode(object info)
		{
			this.info = info;
		}

		public CustomTreeNode(object info, IEnumerable<CustomTreeNode> children)
		{
			this.info = info;
			this.children = children.ToList();
		}

		public void AddChild(CustomTreeNode node)
		{
			if (children == null)
			{
				children = new List<CustomTreeNode>(4);
			}

			children.Add(node);
		}

		public int Id
		{
			get
			{
				if (id == -1)
				{
					throw new InvalidOperationException("Id is not defined. Call Init()");
				}

				return id;
			}
		}

		public int ParentId
		{
			get
			{
				if (parentId == -1)
				{
					throw new InvalidOperationException("Parentid is not defined. Call Init()");
				}

				return parentId;
			}
		}

		public int MaxChildId
		{
			get
			{
				if (maxChildId == -1)
				{
					throw new InvalidOperationException("MaxChildId is not defined. Call Init()");
				}

				return maxChildId;
			}
		}

		public object Info
		{
			get
			{
				return info;
			}

			set
			{
				info = value;
			}
		}

		public IEnumerable<CustomTreeNode> Children
		{
			get
			{
				if (children != null)
				{

					foreach (var item in children)
					{
						yield return item;
					}
				}
			}
		}

		public int ChildrenCount
		{
			get
			{
				return children == null ? 0 : children.Count;
			}
		}

		public int Width
		{
			get
			{
				if (width == -1)
				{
					throw new InvalidOperationException("Width is not defined. Call Init()");
				}

				return width;
			}
		}

		public int Height
		{
			get
			{
				if (height == -1)
				{
					throw new InvalidOperationException("Height is not defined. Call Init()");
				}

				return height;
			}
		}

		public int LeftPadding
		{
			get
			{
				if (leftPadding == -1)
				{
					throw new InvalidOperationException("LeftPadding is not defined. Call Init()");
				}

				return leftPadding;
			}
		}

		public int TopPadding
		{
			get
			{
				if (topPadding == -1)
				{
					throw new InvalidOperationException("TopPadding is not defined. Call Init()");
				}

				return topPadding;
			}
		}

		public int ChildrenWidth
		{
			get
			{
				if (children == null)
				{
					return 0;
				}

				int result = children[children.Count - 1].LeftPadding;
				if (result == -1)
				{
					throw new InvalidOperationException("LastChildLeftPadding is not defined. Call Init()");
				}

				return result - LeftPadding;
			}
		}

		/// <summary>
		/// Traverse whole tree where root is current node. Callback is called for every node included root (this)
		/// </summary>
		/// <returns>Ienumerable of all nodes in tree</returns>
		public IEnumerable<CustomTreeNode> TraverseBreadthFirst()
		{
			Queue<CustomTreeNode> q = new Queue<CustomTreeNode>();
			q.Enqueue(this);
			while (q.Any())
			{
				var n = q.Dequeue();
				yield return n;
				foreach (var c in n.Children)
				{
					q.Enqueue(c);
				}
			}
		}

		public void Init()
		{
			DetermineIds();
			DetermineMeasurements(0, 0);
		}

		private void DetermineIds()
		{
			int indexer = 0;
			Queue<CustomTreeNode> q = new Queue<CustomTreeNode>();
			parentId = 0;
			q.Enqueue(this);
			while (q.Any())
			{
				var n = q.Dequeue();
				n.id = ++indexer;
				foreach (var c in n.Children)
				{
					c.parentId = n.id;
					q.Enqueue(c);
				}
			}
		}

		private void DetermineMeasurements(int currRow, int currCol)
		{
			topPadding = currRow;
			leftPadding = currCol;
			if (children == null)
			{
				maxChildId = id;
				width = height = 1;
			}
			else
			{
				width = 0;
				height = 1;
				maxChildId = id;
				foreach (var child in children)
				{
					child.DetermineMeasurements(currRow + 1, currCol + width);
					width += child.Width;
					height = Math.Max(height, 1 + child.height);
					maxChildId = Math.Max(maxChildId, child.maxChildId);
				}
			}
		}
	}
}
