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

`GenerateNodes` looks at the type passed in and returns a collection of [Nodes](https://github.com/KallDrexx/Xom/blob/master/Xom.Core/Models/XomNode.cs), which provides a runtime representation of the Xml types that is much easier to work with.
