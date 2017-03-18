using CustomTreeDrawer.Base;
using CustomTreeDrawer.Drawers.Interfaces;
using CustomTreeDrawer.TreeTypes;
using CustomTreeDrawerWPF.Drawers;
using System.Collections.Generic;
using System.Windows;

namespace CustomTreeDrawerWPFExample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private CustomTree tree;
		private ICustomTreeDrawer drawer;
		private WPFTreeNodeDrawerSettings wpfDrawerSettings;
		private CustomTreeNode rootNode;

		public MainWindow()
		{
			InitializeComponent();
			wpfDrawerSettings = WPFTreeNodeDrawerSettings.CreateDefault();
			rootNode = GetNodes();
			drawer = new WPFTreeNodeDrawer(TreeGrid, wpfDrawerSettings);
			Button_Click_1(null, new RoutedEventArgs());
		}

		private CustomTreeNode GetNodes()
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
			var node43 = new CustomTreeNode(new SomeCustomClass("Long title"), new List<CustomTreeNode>() { node14, node24 });

			CustomTreeNode node12 = new CustomTreeNode(new SomeCustomClass("12"), new List<CustomTreeNode>() { node13, node23 });
			CustomTreeNode node22 = new CustomTreeNode(new SomeCustomClass("22"), new List<CustomTreeNode>() { node33, node43 });
			var node32 = new CustomTreeNode(new SomeCustomClass("Even longer title"));

			var complexRoot = new CustomTreeNode(new SomeCustomClass("11"), new List<CustomTreeNode>() { node12, node22, node32 });
			return complexRoot;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var treeType = new NewLineCustomTreeType(drawer);
			tree = new CustomTree(treeType);
			tree.Update(rootNode);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var treeType = new DefaultCustomTreeType(drawer);
			tree = new CustomTree(treeType);
			tree.Update(rootNode);
		}

		private void TreeGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var pos = System.Windows.Input.Mouse.GetPosition(TreeGrid);
			var node = tree.GetFor(pos.X, pos.Y);
			tree.SelectedNode = node;
		}
	}
}
