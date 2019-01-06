using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GXPEngine;

public class LevelManager
{
    public static List<string> levelFilenameList = Directory.GetFiles(@"tmx", "*.tmx").ToList();

    public static List<LevelProperty> levelList = new List<LevelProperty> { };
}

public class LevelProperty
{
    //Scrap this. Implement external XML.
    public static bool unlocked;
    public static string filename;
    public static string name;
    public int id;
}