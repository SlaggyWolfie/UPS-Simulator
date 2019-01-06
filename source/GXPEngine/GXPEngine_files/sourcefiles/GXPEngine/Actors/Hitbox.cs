using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GXPEngine;

public abstract class HitBox : Sprite
{
    //public List<Line> listOfLines = new List<Line> { };

    protected Dictionary<string, string> properties = new Dictionary<string, string> { };
    protected XmlObject object_;

    protected float width_;
    protected float height_;
    public float depth;

    protected bool hasProperties;
    protected int linesToCreate = 2;
    public int numberOfBallsC = 4;
    public int numberOfBallsR = 1;
    public float startDepth = 0;
    public float endDepth = 1024;

    protected int pointPerPackage;

    public HitBox(XmlObject _object) : base("assets/placeholders/128x128.png")
    {
        object_ = _object;
        //properties = list.ToDictionary<string, string>( );
        if (object_.Properties != null && object_.Properties.Property.Length > 0)
        {
            for (int i = 0; i < object_.Properties.Property.Length; i++)
            {
                properties[object_.Properties.Property[i].Name.ToLower()] = object_.Properties.Property[i].Value;
            }
            hasProperties = true;
        }

        if (hasProperties)
        {
            if (properties.ContainsKey("lines")) linesToCreate = int.Parse(properties["lines"]);
            if (properties.ContainsKey("startingdepth")) startDepth = float.Parse(properties["startingdepth"]);
            if (properties.ContainsKey("enddepth")) endDepth = float.Parse(properties["enddepth"]);
            if (properties.ContainsKey("depth")) depth = float.Parse(properties["depth"]);
            else depth = endDepth - startDepth;
        }

        width_ = object_.Width;
        height_ = object_.Height;

        x = object_.xCoordinate;
        y = object_.yCoordinate - height_;
        
        scaleX = width_ / width;
        scaleY = height_ / height;

        //Console.WriteLine(width_ == width);
        //Console.WriteLine(height_ == height);

        alpha = 0;
    }

    protected virtual void Update()
    {
        if (PauseManager.GetPause()) return;
        //Remove the hitboxes + lines to optimise FPS
        if (x < -MyGame.currentLevel.x - width_ * 3)
        {
            Destroy();
        }
    }

    protected override void OnDestroy()
    {
        //foreach (Line line in listOfLines) line.Destroy();
        //listOfLines.Clear();
    }

    public override string ToString()
    {
        string text = GetType().ToString();
        text += "\nx: " + x + " y: " + y;
        text += "\nwidth: " + width_ + " height: " + height_;
        text += "\nlines: " + linesToCreate;
        return text;
    }
}

public class Skybox : HitBox
{
    public Skybox(XmlObject _object) : base (_object)
    {
        //scaleX = width_ / width;
        //scaleY = height_ / height;
    }
}


public abstract class HorizontalHitbox : HitBox
{
    public int ID_TYPE;

    public HorizontalHitbox(XmlObject _object) : base(_object)
    {
        //scaleX = width_ / width;
        //scaleY = height_ / height;

        numberOfBallsC = (int)(width_ / (MyGame.currentLevel.tileSize * 7 / 8)) + 2;
        numberOfBallsR = (int)(height_ / (MyGame.currentLevel.tileSize * 7 / 8)) + 2;

        for (int i = 0; i < linesToCreate; i++)
        {
            ////From Left to Right
            //Line line = new Line(new Vec2(0, height_ / linesToCreate * i), new Vec2(width_, height_ / linesToCreate * i), 0xffff0000);
            ////From Right to Left
            ////Line line = new Line(new Vec2(width_, height_ / linesToCreate * i), new Vec2(0, height_ / linesToCreate * i), 0xffff0000);

            //line.startDepth = startDepth + depth * i / linesToCreate;
            //line.endDepth = line.startDepth + depth / linesToCreate;

            //listOfLines.Add(line);
            //AddChild(line);
        }
    }
}

public class VerticalHitbox : HitBox
{
    public VerticalHitbox(XmlObject _object) : base(_object)
    {
        //linesToCreate = (int)(width_ / MyGame.currentLevel.tileSize * 2) + 1;
        //Console.WriteLine(this);
        //Console.WriteLine(linesToCreate);
        //alpha = 1;
        //for (int i = 0; i < linesToCreate; i++)
        //{
        //    Line line = new Line(new Vec2(width_ / linesToCreate * i, 0), new Vec2(width_ / linesToCreate * i, height_), 0xffffff00);

        //    AddChild(line);
        //    listOfLines.Add(line);

        //    //Console.WriteLine(line);
        //    //line.color = 0x00000000;
        //    line.visible = false;
        //}
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Street : HorizontalHitbox
{
    public Street(XmlObject _object) : base(_object)
    {
        ID_TYPE = 0;
        pointPerPackage = 0;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Garden : HorizontalHitbox
{
    public Garden(XmlObject _object) : base(_object)
    {
        ID_TYPE = 1;
        pointPerPackage = 10;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class DoorArea : HorizontalHitbox
{
    public bool gotHit;

    public DoorArea(XmlObject _object) : base(_object)
    {
        ID_TYPE = 2;
        pointPerPackage = 10;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }

    public float GetDistanceWith(float x, float y)
    {
        float cx = this.x - x;
        float cy = this.y - y;
        return Mathf.Sqrt(cx * cx + cy * cy);
    }

    public override string ToString()
    {
        return "\tx: " + x + "\n\ty: " + y + "\n\twidth: " + width + "\n\theight: " + height;
    }
}