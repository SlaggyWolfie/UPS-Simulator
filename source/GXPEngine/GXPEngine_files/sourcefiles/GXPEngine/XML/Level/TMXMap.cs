using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


/// <summary>
/// Represents a Tiled map.
/// </summary>
[XmlRoot("map")]
public partial class Map
{
    [XmlAttribute("orientation")]
    public string Orientation;

    [XmlAttribute("renderorder")]
    public string RenderOrder;

    [XmlAttribute("width")]
    public int Width = 0;

    [XmlAttribute("height")]
    public int Height = 0;

    [XmlAttribute("tilewidth")]
    public int TileWidth = 0;

    [XmlAttribute("tileheight")]
    public int TileHeight = 0;

    [XmlAttribute("backgroundcolor")]
    public uint BackgroundColor;

    [XmlElement("tileset")]
    public TileSet[] TileSet;

    [XmlElement("layer")]
    public Layer[] Layer;
    
    [XmlElement("objectgroup")]
    public ObjectGroup[] ObjectGroup;

    [XmlElement("imagelayer")]
    public ImageLayer[] ImageLayer;

    [XmlElement("properties")]
    public Properties Properties;

    public Map ()
    {
        
    }
}

[XmlRoot("tileset")]
public partial class TileSet
{
    [XmlAttribute("firstgid")]
    public int FirstGID = 0;

    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("tilewidth")]
    public int TileWidth = 0;

    [XmlAttribute("tileheight")]
    public int TileHeight = 0;

    /// <summary>
    /// Do not use - might result in texture bleeding;
    /// </summary>
    [XmlAttribute("spacing")]
    public int Spacing = 0;

    /// <summary>
    /// Do not use - might result in texture bleeding;
    /// </summary>
    [XmlAttribute("margin")]
    public int Margin = 0;

    [XmlAttribute("tilecount")]
    public int TileCount = 0;

    [XmlAttribute("columns")]
    public int Columns = 0;

    [XmlElement("tileoffset")]
    public TileOffSet TileOffSet;

    [XmlElement("properties")]
    public Properties Properties;

    [XmlElement("image")]
    public XmlImage Image;

    [XmlElement("terraintypes")]
    public TerrainType TerrainType;

    [XmlElement("tile")]
    public Tile Tile;
}

[XmlRoot("tileoffset")]
public partial class TileOffSet
{
    [XmlAttribute("x")]
    public int xOffset = 0;

    [XmlAttribute("y")]
    public int yOffset = 0;
}

[XmlRoot("image")]
public partial class XmlImage
{
    //Not supported in Tiled yet.
    [XmlAttribute("format")]
    public string Format;

    [XmlAttribute("source")]
    public string Source;

    [XmlAttribute("trans")]
    public uint ColorToBeTransparent;

    [XmlAttribute("width")]
    public int Width = 0;

    [XmlAttribute("height")]
    public int Height = 0;
}

[XmlRoot("terraintypes")]
public partial class TerrainType
{
    [XmlElement("terrain")]
    public Terrain[] Terrain;
}

[XmlRoot("terrain")]
public partial class Terrain
{
    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("tile")]
    public int LocalTileID = 0;

    [XmlElement("properties")]
    public Properties Properties;
}

[XmlRoot("tile")]
public partial class Tile
{
    [XmlAttribute("id")]
    public int LocalTileID = 0;

    [XmlAttribute("terrain")]
    public string Terrain;

    [XmlAttribute("probability")]
    public float Probability = 1;

    [XmlElement("properties")]
    public Properties Properties;

    [XmlElement("image")]
    public XmlImage Image;

    [XmlElement("objectgroup")]
    public ObjectGroup ObjectGroup;

    [XmlElement("animation")]
    public Animation Animation;
}

[XmlRoot("animation")]
public partial class Animation
{
    [XmlElement("frame")]
    public Frame[] FrameArray;
    //public List<Frame> FrameList = new List<Frame> { };
}

[XmlRoot("frame")]
public partial class Frame
{
    [XmlAttribute("tileid")]
    public int LocalTileID = 0;

    [XmlAttribute("duration")]
    public int Duration = 0; //Milliseconds
}

/// <summary>
/// Represents a tile layer in a Tiled map
/// </summary>
[XmlRoot("layer")]
public partial class Layer
{
    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("opacity")]
    public float Opacity = 1.0f;

    [XmlAttribute("width")]
    public int Width = 0;

    [XmlAttribute("height")]
    public int Height = 0;

    //Can probably "converted" to a bool.
    [XmlAttribute("visible")]
    public int Visible = 1;

    [XmlElement("properties")]
    public Properties Properties;

    [XmlElement("data")]
    public Data Data;
}

/// <summary>
/// Represents the data tag in a map layer
/// </summary>
[XmlRoot("data")]
public partial class Data
{
    [XmlAttribute("encoding")]
    public string Encoding;

    [XmlText]
    public string innerXML;

    public Data ()
    {
    }
}

/// <summary>
/// Represents the object layer (a collection of objects and their depth).
/// </summary>
[XmlRoot("objectgroup")]
public partial class ObjectGroup
{
    [XmlAttribute("color")]
    public uint Color;

    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("opacity")]
    public float Opacity = 1.0f;

    //Can probably be "converted" to a bool.
    [XmlAttribute("visible")]
    public int Visible = 1;

    [XmlAttribute("draworder")]
    public string DrawOrder;

    [XmlElement("properties")]
    public Properties Properties;

    [XmlElement("object")]
    public XmlObject[] Object;
}

/// <summary>
/// Represents an object from an ObjectGroup.
/// </summary>
[XmlRoot("object")]
public partial class XmlObject
{
    [XmlAttribute("id")]
    public int ID = 0;

    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("type")]
    public string Type = "";

    [XmlAttribute("x")]
    public float xCoordinate = 0;

    [XmlAttribute("y")]
    public float yCoordinate = 0;
    
    [XmlAttribute("width")]
    public float Width = 0;

    [XmlAttribute("height")]
    public float Height = 0;

    [XmlAttribute("rotation")]
    public int Rotation = 0;

    [XmlAttribute("gid")]
    public int gid = 0;

    //Can probably "converted" to a bool.
    [XmlAttribute("visible")]
    public int Visible = 1;

    [XmlElement("properties")]
    public Properties Properties;

    [XmlElement("ellipse")]
    public XmlEllipse Ellipse;

    [XmlElement("polygon")]
    public XmlPolygon Polygon;

    [XmlElement("polyline")]
    public XmlPolyline Polyline;
}

/// <summary>
/// Represents the collections of properties an object contains.
/// </summary>
[XmlRoot("properties")]
public partial class Properties
{
    [XmlElement("property")]
    public Property[] Property;
}

/// <summary>
/// Represents a single property of an object.
/// </summary>
[XmlRoot("property")]
public partial class Property
{
    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("type")]
    public string Type;
    //This type is unrealistic since there might be more types but Tiled has a limited number
    //so a string is sufficient for now.

    [XmlAttribute("value")]
    public string Value;
}


[XmlRoot("rectangle")]
public partial class XmlRectangle
{
    //Not needed.
}

/// <summary>
/// Ellipsical shape of an object.
/// </summary>
[XmlRoot("ellipse")]
public partial class XmlEllipse
{
    //Form is calculated in another way in Tiled. Otherwise no other info provided.
}

/// <summary>
/// Polygonical shape of an object.
/// </summary>
[XmlRoot("polygon")]
public partial class XmlPolygon
{
    [XmlAttribute("points")]
    public string Points;
}

/// <summary>
/// A line or multiple connected lines.
/// </summary>
[XmlRoot("polyline")]
public partial class XmlPolyline
{
    [XmlAttribute("points")]
    public string Points;
}

/// <summary>
/// Image layer containing one image.
/// </summary>
[XmlRoot("imagelayer")]
public partial class ImageLayer
{
    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("offsetx")]
    public float OffsetX = 0.0f;

    [XmlAttribute("offsety")]
    public float OffsetY = 0.0f;

    [XmlAttribute("opacity")]
    public float Opacity = 1.0f;

    //Can probably "converted" to a bool.
    [XmlAttribute("visible")]
    public int Visible = 1;

    [XmlElement("image")]
    public XmlImage Image;
}