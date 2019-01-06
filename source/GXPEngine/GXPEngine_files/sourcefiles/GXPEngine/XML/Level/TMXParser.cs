using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

public class TMXParser
{
    public TMXParser()
    {
        //Parse(filename);
    }

    ///<summary>
    ///Parse a TMX file
    /// </summary>
    public Map Parse(string filename)
    {
        //serialise a map class as an XML file
        XmlSerializer serializer = new XmlSerializer(typeof(Map));

        //opening the file, and reading the Map class from it
        TextReader tmxReader = new StreamReader(filename);
        Map map = serializer.Deserialize(tmxReader) as Map;
        tmxReader.Close();

        //test to verify the innermost data is there
        //Console.WriteLine(map.Layer.Data.innerXML + "<<");
        //Console.WriteLine(map.Layer.Data.innerXML + "<<");

        return map;

    }

    public static int[,] SplitArray(string innerData)
    {
        int[,] level;
        string[] lines = innerData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int numRows = lines.Length;
        int numColumns = lines[0].Split(',').Length;

        level = new int[numRows, numColumns];

        for (int row = 0; row < numRows; row++)
        {
            Console.WriteLine();
            string[] lineRow = lines[row].Split(',');
            for (int column = 0; column < numColumns; column++)
            {
                try
                {
                    level[row, column] = int.Parse(lineRow[column]);
                    //Console.Write(" " +level[row, column]);
                }
                catch
                {
                }
            }
        }

        return level;
    }

}
