using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine.Core;

public partial class Map
{
    public override string ToString()
    {
        string stringToReturn = "Map with a " + Orientation + " orientation and a " + RenderOrder + " render order.";
        if (Width != 0)
        {
            stringToReturn += "\nIt has a width of " + Width + " tiles.";
        }

        if (Height != 0)
        {
            stringToReturn += "\nIt has a height of " + Height + " tiles.";
        }

        if (TileWidth != 0)
        {
            stringToReturn += "\nTiles have a width of " + TileWidth + " pixels.";
        }

        if (TileHeight != 0)
        {
            stringToReturn += "\nTiles have a height of " + TileHeight + " pixels.";
        }

        if (BackgroundColor != 0)
        {
            stringToReturn += "\nIts background color has a value of " + BackgroundColor + ".";
        }
        
        if (TileSet.Length > 0 && TileSet[0] != null)
        {
            string _is = " is";
            string _are = "s are";
            string isAre = (TileSet.Length == 1) ? _is : _are;
            stringToReturn += "\n " + TileSet.Length + " tileset" + isAre + " used by this map.";
            for (int i = 0; i < TileSet.Length; i++)
            {
                stringToReturn += "\n" + TileSet[i].ToString();
            }
        }

        if (Layer.Length > 0 && Layer[0] != null)
        {
            string _is = " is";
            string _are = "s are";
            string isAre = (Layer.Length == 1) ? _is : _are;
            stringToReturn += "\n " + Layer.Length + " layer" + isAre + " used by this map.";
            for (int i = 0; i < Layer.Length; i++)
            {
                stringToReturn += "\n" + Layer[i].ToString();
            }
        }
        
        if (ObjectGroup.Length > 0 && ObjectGroup[0] != null)
        {
            string _is = " is";
            string _are = "s are";
            string isAre = (ObjectGroup.Length == 1) ? _is : _are;
            stringToReturn += "\n " + ObjectGroup.Length + " object group" + isAre + " used by this map.";
            for (int i = 0; i < ObjectGroup.Length; i++)
            {
                stringToReturn += "\n" + ObjectGroup[i].ToString();
            }
        }

        if (ImageLayer.Length > 0 && ImageLayer[0] != null)
        {
            string _is = " is";
            string _are = "s are";
            string isAre = (ImageLayer.Length == 1) ? _is : _are;
            stringToReturn += "\n " + ImageLayer.Length + " image layer" + isAre + " used by this map.";
            for (int i = 0; i < ImageLayer.Length; i++)
            {
                stringToReturn += "\n  " + ImageLayer[i].ToString();
            }
        }
        return stringToReturn;
    }
}

public partial class TileSet
{
    public override string ToString()
    {
        string stringToReturn = "   Tileset " + Name + " has a first GID of " + FirstGID + ". " + Image;
        stringToReturn += "\n   The number of tiles is " + TileCount + " and there are " + Columns + " columns.";

        if (TileWidth != 0)
        {
            stringToReturn += "\n   Tiles have a width of " + TileWidth + " pixels.";
        }

        if (TileHeight != 0)
        {
            stringToReturn += "\n   Tiles have a height of " + TileHeight + " pixels.";
        }

        if (Spacing != 0)
        {
            stringToReturn += "\n   Warning! Spacing is not 0. Spacing is " + Spacing + ". Please do not use in GXPEngine.";
        }

        if (Margin != 0)
        {
            stringToReturn += "\n   Warning! Margin is not 0. Margin is " + Margin + ". Please do not use in GXPEngine.";
        }

        if (TileOffSet != null)
        {
            stringToReturn += "\n   Tile offset is: " + TileOffSet;
        }

        if (TerrainType != null)
        {
            stringToReturn += "\n   The terrrain types are " + TerrainType;
        }

        if (Properties != null)
        {
            stringToReturn += Properties;
        }

        if (Tile != null)
        {
            stringToReturn += Tile;
        }
        return stringToReturn;
    }
}

public partial class TileOffSet
{
    public override string ToString()
    {
        return "X: " + xOffset + ", Y: " + yOffset;
    }
}

public partial class XmlImage
{
    public override string ToString()
    {
        string stringToReturn = "\n    Using the image " + Source + ".";
        if (Width != 0)
        {
            stringToReturn += "\n     It has a width of " + Width + " pixels.";
        }

        if (Height != 0)
        {
            stringToReturn += "\n     It has a height of " + Height + " pixels.";
        }

        if (ColorToBeTransparent != 0)
        {
            stringToReturn += "\n    The color to be transparent has a value of " + ColorToBeTransparent + ".";
        }

        return stringToReturn;
    }
}

public partial class TerrainType
{
    public override string ToString()
    {
        string stringToReturn = "";
        if (Terrain[0] != null && Terrain.Length > 0)
        {
            for (int i = 0; i < Terrain.Length; i++)
            {
                stringToReturn += Terrain.ToString();
            }
        }
        return stringToReturn;
    }
}

public partial class Terrain
{
    public override string ToString()
    {
        return "\n     Terrain " + Name + " with a local tile ID of " + LocalTileID + "." + Properties.ToString();
    }
}

public partial class Tile
{
    public override string ToString()
    {
        string probabilityString = "";
        if (Probability >= 0.99f)
        {
            probabilityString += " with a probability of taking over the spacing of " + Probability + " ";
        }
        string stringToReturn = "\n     Tile Number " + LocalTileID + " with " + Terrain + " terrain " + probabilityString;

        if (Image != null)
        {
            stringToReturn += Image.ToString();
        }

        if (ObjectGroup != null)
        {
            stringToReturn += ObjectGroup;
        }

        if (Animation != null)
        {
            stringToReturn += Animation;
        }

        return stringToReturn;
    }
}

public partial class Animation
{
    public override string ToString()
    {
        string stringToReturn = "";
        if (FrameArray[0] != null && FrameArray.Length > 0)
        {
            for (int i = 0; i < FrameArray.Length; i++)
            {
                stringToReturn += FrameArray[i].ToString();
            }
        }
        return stringToReturn;
    }
}

public partial class Frame
{
    public override string ToString()
    {
        return "\n       Frame " + LocalTileID + " with a duration of " + Duration + " ms";
    }
}

public partial class Layer
{
    public override string ToString()
    {
        string stringToReturn = "  Layer " + Name + " with opacity " + Opacity;
        stringToReturn += "\n" + Data.ToString();
        if (Visible != 1)
        {
            stringToReturn += "\n  It is not visible.";
        }
        return stringToReturn;
    }
}

public partial class Data
{
    public override string ToString()
    {
        int[,] array = TMXParser.SplitArray(innerXML);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                //Console.Write(array[i, j] + ",");
            }
            //Console.WriteLine();
        }
        return null;
    }
}

public partial class ObjectGroup
{
    public override string ToString()
    {
        string stringToReturn = "  Object Group " + Name + " has an opacity of " + Opacity + ".";

        if (Object == null) return stringToReturn;

        if (Object.Length > 0 && Object[0] != null)
        {
            string _is = " is";
            string _are = "s are";
            string isAre = (Object.Length == 1) ? _is : _are;
            if (Visible != 1)
            {
                stringToReturn += "\n  It is not visible.";
            }
            stringToReturn += "\n  " + Object.Length + " Object" + isAre + " used by this map.";
            for (int i = 0; i < Object.Length; i++)
            {
                stringToReturn += "\n   " + Object[i].ToString();
            }
        }

        return stringToReturn;
    }
}

public partial class XmlObject
{
    public override string ToString()
    {
        string stringToReturn = "Object " + Name + " has an ID of " + ID + "."; 
        
        if (Type != null)
        {
            stringToReturn += "\n    It is of type " + Type + ".";
        }

        if (xCoordinate != 0)
        {
            stringToReturn += "\n    X: " + xCoordinate;
        }

        if (yCoordinate != 0)
        {
            stringToReturn += "\n    Y: " + yCoordinate;
        }

        if (Width != 0)
        {
            stringToReturn += "\n    Width: " + Width + ".";
        }

        if (Height != 0)
        {
            stringToReturn += "\n    Height: " + Height + ".";
        }

        if (Rotation != 0)
        {
            stringToReturn += "\n    Rotation: " + Rotation + ".";
        }

        if (gid != 0)
        {
            stringToReturn += "\n    GID: " + gid + ".";
        }

        if (Visible != 1)
        {
            stringToReturn += "\n    It is not visible.";
        }

        if (Ellipse != null)
        {
            stringToReturn += "\n    " + Ellipse.ToString();
        }
        else if (Polygon != null)
        {
            stringToReturn += "\n    " + Polygon.ToString();
        }
        else if (Polyline != null)
        {
            stringToReturn += "\n    " + Polyline.ToString();
        }

        if (Properties != null)
        {
            stringToReturn += Properties.ToString();
        }

        return stringToReturn;
    }
} 

public partial class Properties
{
    public override string ToString()
    {
        string stringToReturn = null;
        if (Property.Length > 0 && Property[0] != null)
        {
            string _is = "y is";
            string _are = "ies are";
            string isAre = (Property.Length == 1) ? _is : _are;
            stringToReturn += "\n    " + Property.Length + " Propert" + isAre + " used by this map.";
            for (int i = 0; i < Property.Length; i++)
            {
                stringToReturn += "\n     " + Property[i].ToString();
            }
        }

        return stringToReturn;
    }
}


public partial class Property
{
    public override string ToString()
    {
        string stringToReturn = "Property " + Name + " is type " + Type + " with a value of " + Value + ".";

        

        return stringToReturn;
    }
}

public partial class XmlEllipse
{
    public override string ToString()
    {
        return "It's an ellipse.";
    }
}

public partial class XmlPolygon
{
    public List<Vector2> PolyXY = new List<Vector2> ();

    public override string ToString()
    {
        string[] points = Points.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string stringToReturn = "It's a polygon.";
        stringToReturn += "\n    Points: " + Points + ".";
        SplitXY();
        if (PolyXY.Count > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                stringToReturn += "\n    " + PolyXY[i].ToString();
            }
        }
        return stringToReturn;
    }

    public void SplitXY()
    {
        string[] points = Points.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (points[0] != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                string[] coords = points[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Vector2 newVector = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
                PolyXY.Add(newVector);
            }
        }
    }
}
/*
 * public partial class XmlPolygon
{
    public override string ToString()
    {
        string[] lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string stringToReturn = "It's a polygon.";
        stringToReturn += "\n    Points: " + Points + ".";
        return stringToReturn;
    }
}*/

public partial class XmlPolyline
{
    List<Vector2> PolyXY = new List<Vector2> { };

    public override string ToString()
    {
        string[] points = Points.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string stringToReturn = "It's a polyline.";
        stringToReturn += "\n    Points: " + Points + ".";
        SplitXY();
        if (PolyXY.Count > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                stringToReturn += "\n    " + PolyXY[i].ToString();
            }
        }
        return stringToReturn;
    }

    public void SplitXY()
    {
        string[] points = Points.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (points[0] != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                string[] coords = points[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Vector2 newVector = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
                PolyXY.Add(newVector);
            }
        }
    }
}
public partial class ImageLayer
{
    public override string ToString()
    {
        string stringToReturn = "Image Layer " + Name + " has an opacity of " + Opacity + ".";
        stringToReturn += Image.ToString();

        return stringToReturn;
    }
}
