Xom
===

Library to convert .Net Xml serialization objects into a code based schema.  This allows the easy creation of tools that can adapt to changes in Xml Serialization objects and allow easier creation of editors or xsd generation tools.


Xom vs Reflection
-----------------

Xom was created because reflection alone does not fully describe how the .Net framework serializes objects to XML.  For example, take the following class:

```
public class GameSaveState
{
	[XmlIgnore]
	public int? LevelNumber { get; set; }
	
	[XmlIgnore]
	public bool LevelValueSpecified { get; set; }

	[XmlAttribute("Level")]
	public int LevelValue 
	{
		get { return LevelNumber ?? default(int); }
		set { LevelNumber = value; }
	}
	
	[XmlArray]
	[XmlArrayItem("Player", typeof(Player))]
	[XmlArrayItem("Enemy", typeof(Enemy))]
	public List<Entity> LevelEntities { get; set; }
}
```

There are several considerations that plain reflection will not catch:
* LevelValue is an optional attribute and is not required
* The name of the attribute for `LevelValue` is `Level`
* `LevelEntities` is not just child nodes, but instead it will have a `LevelEntities` node that can contain either `Player` or `Enemy` child nodes.

These are the intracacies that Xom was created to accurately model.

Usage
-----

Using xom is as easy as 

```
var xom = new XomReader();
var nodes = xom.GenerateNodes(typeof(MySerializationType));
```

`GenerateNodes` looks at the type passed in and returns a collection of [XomNodes](https://github.com/KallDrexx/Xom/blob/master/Xom.Core/Models/XomNode.cs), which provides a runtime representation of the Xml types that is much easier to work with.

You can then use the `XomNode` collection returned to get details on the XML structure.  For example, say you want to display the schema of the XML elements in a WPF TreeView.  You could accomplish this by creating a view model for your nodes:

```
    public class SchemaNode : ViewModelBase
    {
        private string _name;
        private readonly ObservableCollection<SchemaNode> _children;
        private readonly XomNode _xomNode;

        public SchemaNode(XomNode node, string name, int depth)
        {
            _children = new ObservableCollection<SchemaNode>();
            _xomNode = node;
            Name = name;

            if (depth < 4)
            {
                var childNodes = _xomNode.Children
                                         .SelectMany(x => x.AvailableNodes)
                                         .OrderBy(x => x.Key);

                foreach (var keyValuePair in childNodes)
                    _children.Add(new UiNode(keyValuePair.Value, keyValuePair.Key, depth + 1));
            }
        }

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public IEnumerable<SchemaNode> Children { get { return _children; } }
    }
```

Next you have to create a view model for the window containing the TreeView.  In this view model you will generate the `XomNodes` for your desired type and create the `SchemaNode`s.

```
    public class WindowViewModel : ViewModelBase
    {
        private List<SchemaNode> _rootNode;

        public WindowViewModel()
        {
            var reader = new XomReader();
            var nodes = reader.GenerateNodes(typeof (AssetCollection));
            var rootNode = nodes.First(x => x.IsRoot);

            RootNode = new List<SchemaNode>
            {
                new SchemaNode(rootNode, "root", 0)
            };
        }

        public List<SchemaNode> RootNode
        {
            get { return _rootNode; }
            set { Set(() => RootNode, ref _rootNode, value); }
        }
    }
```

In this example, `AssetCollection` is the C# type that is used for serializing data to XML.  Finally you have to setup the tree view in the XAML.

```
    <Grid>
        <TreeView ItemsSource="{Binding RootNode}">
            <TreeView.ItemTemplate>
                <HierarchicalDataTemplate DataType="{x:Type vm:SchemaNode}" ItemsSource="{Binding Children}">
                    <TextBlock Text="{Binding Name}" />
                </HierarchicalDataTemplate>
            </TreeView.ItemTemplate>
        </TreeView>  
    </Grid>
```

This will produce: 

![Example](https://dl.dropboxusercontent.com/u/6753359/SchemaTreeView.PNG)
