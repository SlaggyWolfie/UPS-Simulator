using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Person : AnimationSprite
{
    public enum State
    {
        Happy, None, Hit
    };
    public State state;

    private List<DoorArea> _listOfDoorAreas = new List<DoorArea> { };
    private DoorArea _closestDoorArea;
    private Level _level;
    private int _startFrame;
    public float z;

    public Person(XmlObject _object, Level level, int startingFrame) : base("assets/tilesheets/People.png", 6, 2)
    {
        z = 128;
        _level = level;
        _startFrame = startingFrame;
        currentFrame = _startFrame;
        state = State.None;

        if (_object.Properties != null && _object.Properties.Property[0] != null && _object.Properties.Property[0].Name.ToLower() == "depth") z = float.Parse(_object.Properties.Property[0].Value);

        SetXY(_object.xCoordinate, _object.yCoordinate - _object.Height);

        foreach (DoorArea door in _level.horzList)
        {
            _listOfDoorAreas.Add(door);
        }

        _listOfDoorAreas.OrderBy(d => d.GetDistanceWith(x, y));

        _closestDoorArea = _listOfDoorAreas[0];
    }

    private void Update()
    {
        //Console.WriteLine("x: " + x + "y: " + y);
        if (PauseManager.GetPause()) return;
        if (state != State.Hit)
        {
            foreach (Package package in _level.listPackage)
            {
                if (package.flying)
                {
                    if (z >= package.z && z <= package.z + package.depth + _level.tileSize / 8)
                    {
                        if (package.weight >= 3)
                        {
                            if (HitTest(package))
                            {
                                if (state == State.Happy) currentFrame = _startFrame;
                                currentFrame--;
                                state = State.Hit;
                            }
                        }
                    }
                }
            }
        }
        if (state == State.None)
        {
            if (_closestDoorArea.gotHit)
            {
                state = State.Happy;
                currentFrame++;
            }
        }
    }
}

public abstract class Reaction : AnimationSprite
{
    protected readonly float weightThreshold = 3;
    protected bool has2Frames;
    protected bool hit;
    protected Level level;
    protected Sound relatedSound;
    protected int startFrame;
    public float z;

    public Reaction(string filename, int columns, int rows, Level level, int startingFrame, bool has2Frames = true) : base(filename, columns, rows)
    {
        z = 256;
        hit = false;
        startFrame = startingFrame;
        currentFrame = startFrame;
        this.has2Frames = has2Frames;
        this.level = level;
    }

    protected virtual void Update()
    {
        if (PauseManager.GetPause()) return;
        if (!hit)
        {
            foreach (Package package in level.listPackage)
            {
                if (package.flying)
                {
                    if (package.weight >= weightThreshold)
                    {
                        if (z >= package.z - (package.depth + level.tileSize / 1) && z <= package.z + package.depth)
                        {
                            if (HitTest(package))
                            {
                                if (GetType() == typeof(Window))
                                {
                                    package.Destroy();
                                    level.truck.player.score -= 20;
                                }
                                level.truck.player.score -= 20;
                                SFX.PlaySound(relatedSound);
                                if (has2Frames) currentFrame++;
                                hit = true;
                            }
                        }
                    }
                }
            }
        }
    }
}

public class Window : Reaction
{
    public Window(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = (MyUtils.RandomBool()) ? SFX.windowBreak : SFX.windowBreak2;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Flamingo : Reaction
{
    public Flamingo(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = SFX.metalPlateDrop;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Gnome : Reaction
{
    public Gnome(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = SFX.gnomeBreak;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Trash : Reaction
{
    public Trash(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = SFX.trashDrop;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Sign : Reaction
{
    public Sign(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = SFX.signHit;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Mailbox : Reaction
{
    public Mailbox(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame)
    {
        relatedSound = SFX.mailboxHit;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class BigDog : Reaction
{
    public BigDog(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame, false)
    {
        relatedSound = SFX.largeDogBark;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class SmallDog : Reaction
{
    public SmallDog(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame, false)
    {
        relatedSound = SFX.smallDogBark;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}

public class Cat : Reaction
{
    public Cat(string filename, int columns, int rows, Level level, int startingFrame) : base(filename, columns, rows, level, startingFrame, false)
    {
        relatedSound = SFX.angryCat;
    }

    protected override void Update()
    {
        if (PauseManager.GetPause()) return;
        base.Update();
    }
}