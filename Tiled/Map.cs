using System.Xml.Serialization;

using Microsoft.Xna.Framework.Graphics;

namespace Framework.Tiled
{
	[XmlRoot("map")]
	public class Map
	{
		[XmlElement("tileset")]
		public Tileset_ref[] Tileset_refs { get; set; }

		[XmlElement("layer")]
		public Layer[] Layers { get; set; }

		[XmlElement("objectgroup")]
		public ObjectGroup[] ObjectGroups { get; set; }
	}

	public class Layer
	{
		[XmlAttribute("id")]
		public int Id { get; set; }
		[XmlAttribute("name")]
		public string Name { get; set; }
		[XmlAttribute("width")]
		public int Width { get; set; }
		[XmlAttribute("height")]
		public int Height { get; set; }
		[XmlElement("data")]
		public string Data_str { get; set; }
		[XmlIgnore]
		public uint[,] Data { get; set; }
	}
	public class Tileset_ref
	{
		[XmlAttribute("firstgid")]
		public int FirstElementId { get; set; }
		[XmlAttribute("source")]
		public string Source_tsx { get; set; }
	}

	[XmlRoot("tileset")]
	public class Tileset
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("tilewidth")]
		public int TileWidth { get; set; }

		[XmlAttribute("tileheight")]
		public int TileHeight { get; set; }

		[XmlAttribute("tilecount")]
		public int TileCount { get; set; }

		[XmlAttribute("columns")]
		public int Columns { get; set; }

		[XmlElement("image")]
		public Image Image { get; set; }

		// Reference to this tileset from map file
		[XmlIgnore]
		public Tileset_ref this_ref { get; set; }
	}

	public class Image
	{
		[XmlAttribute("source")]
		public string Source_png { get; set; }

		[XmlAttribute("width")]
		public int Width { get; set; }

		[XmlAttribute("height")]
		public int height { get; set; }

		[XmlIgnore]
		public Texture2D Texture { get; set; }
	}
	
	public class ObjectGroup
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("object")]
		public Object[] Objects { get; set; }
	}

	public class Object
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("x")]
		public int X { get; set; }

		[XmlAttribute("y")]
		public int Y { get; set; }

		[XmlAttribute("width")]
		public int Width { get; set; }

		[XmlAttribute("height")]
		public int Height { get; set; }

		[XmlElement("point")]
		public PointFlag Point { get; set; }

		[XmlElement("polygon")]
		public Polygon Polygon { get; set; }
	}

	public class Polygon
	{
		[XmlAttribute("points")]
		public string Points { get; set; }
	}

	public class PointFlag
	{
	}
}
