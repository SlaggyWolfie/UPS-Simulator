using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

public class Level : GameObject
{
    //level relative variables
    public int tileSize;
    public int width;
    public int height;
    public int[,] level;

    //Camera/Scrolling relevant variables
    private Player _scrollTarget;

    //Collision stuff
    public Player player { get; private set; }
    public List<Wall> wallList;
    public List<Enemy> enemyList;
    public List<InvisibleWall> invisWallList;

    //Drawing stuff
    Canvas myCanvas;

    public Level(string filename)
    {
        TMXParser tmxParser = new TMXParser();
        Map map = tmxParser.Parse(filename);

        wallList = new List<Wall> { };
        enemyList = new List<Enemy> { };
        invisWallList = new List<InvisibleWall> { };

        //tileSize = 64;
        SetTileSize(map);

        width = map.Width * tileSize;
        height = map.Height * tileSize;
        Console.WriteLine("Width: {0} \nHeight: {1}", width, height);

        myCanvas = new Canvas(width, height);
        AddChild(myCanvas);

        ParseMap(map);
        ParseObjectsFromTMX(map);

    }

    private void SetTileSize(Map pMap)
    {
        if (pMap.TileHeight == pMap.TileWidth)
        {
            tileSize = pMap.TileHeight;
        }
        else if (pMap.TileHeight > pMap.TileWidth)
        {
            tileSize = pMap.TileWidth;
        }
        else
        {
            tileSize = 64;
        }
    }

    private void Update()
    {
        ScrollToTarget();
    }

    public void CreateTile(int pCol, int pRow, int pTile, TileSet pTileSet, Layer pLayer)
    {
        if (pLayer.Name == "Walkable")
        {
            Wall newWall = new Wall(pTileSet.Image.Source, pTileSet.Columns, pTileSet.Image.Height / tileSize);
            newWall.currentFrame = pTile - pTileSet.FirstGID;
            AddChild(newWall);
            newWall.SetXY(pRow * tileSize, pCol * tileSize);
            wallList.Add(newWall);
            return;
        }

        AnimationSprite sprite = new AnimationSprite(pTileSet.Image.Source, pTileSet.Columns, pTileSet.Image.Height / tileSize);
        sprite.currentFrame = pTile - pTileSet.FirstGID;
        AddChild(sprite);
        sprite.SetXY(pRow * tileSize, pCol * tileSize);
    }

    private int[,] ParseLayer(Layer pLayer)
    {
        string data = pLayer.Data.innerXML;

        int[,] level = TMXParser.SplitArray(data);

        return level;
    }

    private void ParseMap(Map pMap)
    {
        for (int i = 0; i < pMap.Layer.Length; i++)
        {
            TileSet tileSet;
            Layer layer = pMap.Layer[i];
            int [,] level = ParseLayer(pMap.Layer[i]);
            
            for (int row = 0; row < level.GetLength(0); row++)
            {
                for (int column = 0; column < level.GetLength(1); column++)
                {
                    int tile = level[row, column];
                    if (tile != 0) // 0 Is empty in tiled
                    {
                        for (int j = 0; j < pMap.TileSet.Length; j++)
                        {
                            if (tile > pMap.TileSet[j].FirstGID - 1 && j == pMap.TileSet.Length - 1)
                            {
                                tileSet = pMap.TileSet[pMap.TileSet.Length - 1];
                                CreateTile(row, column, tile, tileSet, layer);
                            }

                            else if (tile > pMap.TileSet[j].FirstGID - 1 && tile < pMap.TileSet[j + 1].FirstGID - 1)
                            {
                                tileSet = pMap.TileSet[j];
                                CreateTile(row, column, tile, tileSet, layer);
                            }
                        }
                    }
                }
            }
        }
    }

    private void ParseObjectsFromTMX(Map pMap, int numberOfObjectGroups = 0)
    {
        //numberOfObjectGroups = _numberOfObjectGroups;
        Console.WriteLine(numberOfObjectGroups);
        if (pMap.ObjectGroup[0] != null && pMap.ObjectGroup.Length > 0)
        {
            foreach (ObjectGroup objectgroup in pMap.ObjectGroup)
            {
                if (pMap.ObjectGroup[0].Object[0] != null && pMap.ObjectGroup[0].Object.Length > 0)
                {
                    foreach (XmlObject _object in objectgroup.Object)
                    {
                        switch (_object.Type)
                        {
                            case "Player":
                                {
                                    Console.WriteLine(2);
                                    player = new Player(this);
                                    AddChild(player);
                                    player.SetXY(_object.xCoordinate + tileSize / 2, _object.yCoordinate + tileSize / 2);
                                    _scrollTarget = player;
                                    break;
                                }
                            case "GroundEnemy":
                                {
                                    GroundEnemy newGroundEnemy = new GroundEnemy(this);
                                    AddChild(newGroundEnemy);
                                    newGroundEnemy.SetXY(_object.xCoordinate + tileSize / 2, _object.yCoordinate + tileSize / 2);
                                    enemyList.Add(newGroundEnemy);
                                    break;
                                }
                            case "FlyingEnemy":
                                {
                                    FlyingEnemy newFlyingEnemy = new FlyingEnemy(this);
                                    AddChild(newFlyingEnemy);
                                    newFlyingEnemy.SetXY(_object.xCoordinate + tileSize / 2, _object.yCoordinate + tileSize / 2);
                                    enemyList.Add(newFlyingEnemy);
                                    break;
                                }
                            case "InvisibleWall":
                                {
                                    InvisibleWall newInvisWall = new InvisibleWall();
                                    AddChild(newInvisWall);
                                    newInvisWall.SetXY(_object.xCoordinate + tileSize / 2, _object.yCoordinate + tileSize / 2);
                                    invisWallList.Add(newInvisWall);
                                    break;
                                }
                            case "TestPolygon":
                                {
                                    _object.Polygon.SplitXY();
                                    List<PointF> newPolygonPoints = new List<PointF> { };
                                    if (pMap.ObjectGroup[0] != null && pMap.ObjectGroup.Length > 0)
                                    {
                                        for (int i = 0; i < _object.Polygon.PolyXY.Count; i++)
                                        {
                                            PointF pointF = new PointF(_object.xCoordinate + _object.Polygon.PolyXY[i].x, _object.yCoordinate + _object.Polygon.PolyXY[i].y);
                                            newPolygonPoints.Add(pointF);
                                        }
                                    }
                                    myCanvas.graphics.DrawPolygon(Pens.Green, newPolygonPoints.ToArray<PointF>());
                                    myCanvas.graphics.FillPolygon(Brushes.Blue, newPolygonPoints.ToArray<PointF>());
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }

    private void ScrollToTarget()
    {
        //Camera
        if (_scrollTarget.x + game.width / 2 < width) x = game.width / 2 - _scrollTarget.x;
        if (_scrollTarget.x - game.width / 2 < 0) x = 0;
        if (_scrollTarget.y + game.height / 2 < height) y = 500 - _scrollTarget.y;
        if (_scrollTarget.y - game.height / 2 < 0) y = 0;
    }
}