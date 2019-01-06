using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using GXPEngine;

public partial class Level : GameObject
{
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

    public void SetUpLayers()
    {
        backgroundLayer = new Pivot();
        AddChild(backgroundLayer);
        behindBackFenceLayer = new Pivot();
        AddChild(behindBackFenceLayer);
        backFenceLayer = new Pivot();
        AddChild(backFenceLayer);
        behindHouseLayer = new Pivot();
        AddChild(behindHouseLayer);
        houseLayer = new Pivot();
        AddChild(houseLayer);
        behindFenceLayer = new Pivot();
        AddChild(behindFenceLayer);
        fenceLayer = new Pivot();
        AddChild(fenceLayer);
        hitboxLayer = new Pivot();
        AddChild(hitboxLayer);
        packageLayer = new Pivot();
        AddChild(packageLayer);
        playerLayer = new Pivot();
        AddChild(playerLayer);
        HUD_Layer = new Pivot();
        AddChild(HUD_Layer);
    }

    public void CreateTile(int pCol, int pRow, int pTile, TileSet pTileSet, Layer pLayer)
    {
        if (pTileSet.Image.Source.Contains("../"))
        {
            pTileSet.Image.Source = pTileSet.Image.Source.Replace("../", "");
        }

        Pivot layer = backgroundLayer;

        switch (pLayer.Name.ToLower())
        {
            case "fences":
                {
                    layer = fenceLayer;
                    break;
                }
            case "houses":
                {
                    layer = houseLayer;
                    break;
                }
            case "back fences":
                {
                    layer = backFenceLayer;
                    break;
                }
            case "decor":
                {
                    layer = behindFenceLayer;
                    break;
                }
        }

        AnimationSprite sprite = new AnimationSprite(pTileSet.Image.Source, pTileSet.Columns, pTileSet.Image.Height / tileSize);
        sprite.currentFrame = pTile - pTileSet.FirstGID;
        layer.AddChild(sprite);
        sprite.SetXY(pRow * tileSize, pCol * tileSize);
        sprite.alpha = pLayer.Opacity;
        sprite.visible = (pLayer.Visible == 0) ? false : true;
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

            if (layer.Name.ToLower() == "fences" && layer.Properties != null && layer.Properties.Property[0] != null && layer.Properties.Property[0].Name.ToLower() == "depth")
                frontFenceDepth = float.Parse(layer.Properties.Property[0].Value);
            else if (layer.Name.ToLower() == "back fence" && layer.Properties != null && layer.Properties.Property[0] != null && layer.Properties.Property[0].Name.ToLower() == "depth")
                houseDepth = float.Parse(layer.Properties.Property[0].Value);
            else if (layer.Name.ToLower() == "houses" && layer.Properties != null && layer.Properties.Property[0] != null && layer.Properties.Property[0].Name.ToLower() == "depth")
                backFenceDepth = float.Parse(layer.Properties.Property[0].Value);

            int[,] level = ParseLayer(pMap.Layer[i]);

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

    private void ParseObjects(Map pMap, int numberOfObjectGroups = 0)
    {
        if (pMap.ObjectGroup[0] != null && pMap.ObjectGroup.Length > 0)
        {
            foreach (ObjectGroup objectgroup in pMap.ObjectGroup)
            {
                if (pMap.ObjectGroup[0].Object[0] != null && pMap.ObjectGroup[0].Object.Length > 0)
                {
                    foreach (XmlObject _object in objectgroup.Object)
                    {
                        switch (_object.Type.ToLower())
                        {
                            case "truck":
                                {
                                    //Truck 
                                    truck = new Truck(_object, _cameraOffsetLeft, _cameraOffsetTop, this);
                                    playerLayer.AddChild(truck);
                                    truck.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    //this.truck = truck;
                                    break;
                                }
                            case "street":
                                {
                                    Street street = new Street(_object);
                                    hitboxLayer.AddChild(street);
                                    horzList.Add(street);
                                    street.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    break;
                                }
                            case "garden":
                                {
                                    Garden garden = new Garden(_object);
                                    hitboxLayer.AddChild(garden);
                                    horzList.Add(garden);
                                    garden.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    break;
                                }
                            case "door":
                                {
                                    DoorArea door = new DoorArea(_object);
                                    hitboxLayer.AddChild(door);
                                    horzList.Add(door);
                                    door.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    break;
                                }
                            case "vert":
                                {
                                    VerticalHitbox fence = new VerticalHitbox(_object);
                                    hitboxLayer.AddChild(fence);
                                    vertList.Add(fence);
                                    fence.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    break;
                                }
                            case "sky":
                                {
                                    skybox = new Skybox(_object);
                                    hitboxLayer.AddChild(skybox);
                                    skybox.SetXY(_object.xCoordinate, _object.yCoordinate);
                                    skybox.depth = depth;
                                    //skybox.alpha = 1;
                                    break;
                                }
                            default:
                                {
                                    TileSet tileset;
                                    for (int i = pMap.TileSet.Length - 1; i >= 0; i--)
                                    {
                                        tileset = pMap.TileSet[i];
                                        if (_object.gid != 0 && _object.gid >= tileset.FirstGID)
                                        {
                                            if (tileset.Image.Source.Contains("../"))
                                            {
                                                tileset.Image.Source = tileset.Image.Source.Replace("../", "");
                                            }

                                            switch (_object.Type.ToLower())
                                            {
                                                case "window":
                                                    {
                                                        Window window = new Window(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(window);
                                                        window.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            window.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "person":
                                                    {
                                                        Person person = new Person(_object, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(person);
                                                        //person.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        break;
                                                    }
                                                case "largedog":
                                                    {
                                                        BigDog dog = new BigDog(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(dog);
                                                        dog.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            dog.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "smalldog":
                                                    {
                                                        SmallDog dog = new SmallDog(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(dog);
                                                        dog.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            dog.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "cat":
                                                    {
                                                        Cat cat = new Cat(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(cat);
                                                        cat.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            cat.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "mailbox":
                                                    {
                                                        Mailbox mailbox = new Mailbox(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(mailbox);
                                                        mailbox.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            mailbox.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "flamingo":
                                                    {
                                                        Flamingo flamingo = new Flamingo(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(flamingo);
                                                        flamingo.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            flamingo.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "trash":
                                                    {
                                                        Trash trash = new Trash(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(trash);
                                                        trash.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            trash.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "sign":
                                                    {
                                                        Sign sign = new Sign(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(sign);
                                                        sign.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            sign.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                case "gnome":
                                                    {
                                                        Gnome gnome = new Gnome(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns, this, _object.gid - tileset.FirstGID);
                                                        behindFenceLayer.AddChild(gnome);
                                                        gnome.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        if (_object.Properties != null &&
                                                            _object.Properties.Property[0] != null &&
                                                            _object.Properties.Property[0].Name.ToLower() == "depth")
                                                            gnome.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        AnimationSprite sprite = new AnimationSprite(tileset.Image.Source, tileset.Columns, tileset.TileCount / tileset.Columns);
                                                        sprite.currentFrame = _object.gid - tileset.FirstGID;
                                                        behindFenceLayer.AddChild(sprite);
                                                        sprite.SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);
                                                        //if (_object.Properties != null &&
                                                        //    _object.Properties.Property[0] != null &&
                                                        //    _object.Properties.Property[0].Name.ToLower() == "depth")
                                                        //    sprite.z = float.Parse(_object.Properties.Property[0].Value);
                                                        break;
                                                    }
                                                    //break;
                                            }
                                            break;
                                        }
                                        //break;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}