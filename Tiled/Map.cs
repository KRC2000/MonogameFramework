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
}
