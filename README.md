# Custom Tree Drawer

### Customizable tree drawer
Easily customized for different drawers. Currently only WPF; new ones are easy to create - just make your implementation of ICustomTreeDrawer.

Easily customizable for different tree types. Currently there are:
- DefaultCustomTreeType draws all nodes on same level in same line
- NewLineCustomTreeTypeReverse draws nodes similar to tree rendering in Tortoisehg
- NewLineCustomTreeType draws nodes similar to tree rendering in Tortoisehg but root node is on top

### How to use?
Nodes are created with CustomTreeNode class. 
CustomTreeNode class receives object which is actual node info and optional list of child nodes.
For WPF drawer which comes with library, info object must be object which class implements ICustomTreeSimpleNodeInfo.
In example, SomeCustomClass is class which implements that interface.
``` c#
var child = new CustomTreeNode(new SomeCustomClass("child node");
var root = new CustomTreeNode(new SomeCustomClass("root node", new List<CustomTreeNode>() { child }));
```

Actual drawer is created with something like:
``` c#
var wpfDrawerSettings = WPFTreeNodeDrawerSettings.CreateDefault();
var drawer = new WPFTreeNodeDrawer(TreeGrid, wpfDrawerSettings); // TreeGrid is some panel object in your xaml
var treeType = new NewLineCustomTreeTypeReverse(drawer);
tree = new CustomTree(treeType);            
tree.Update(rootNode);
```

You can retrieve node element at position (x, y) with:
``` c#
Point pos = ; // retrieve/create point. Look at example for more information
var node = tree.GetFor(pos.X, pos.Y);
```

You can select some node in tree with
``` c#
tree.SelectedNode = node; // node is some CustomTreeNode, probably retrieved with tree.GetFor - look at example for more info
```

### What else?
Unit tests are in project CustomTreeDrawerTest, example for WPF is in CustomTreeDrawerWPFExample project
