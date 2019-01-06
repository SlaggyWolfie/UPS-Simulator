using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Truck : Sprite
{
    private readonly float _speed;
    public Player player;

    private Dictionary<string, string> _properties = new Dictionary<string, string> { };
    private bool _hasProperties;

    public Truck (XmlObject _object, float cameraX, float cameraY, Level pLevel) : base("assets/player/truckwithwheels.png")
    {
        _hasProperties = false;
        if (_object.Properties != null && _object.Properties.Property.Length > 0)
        {
            for (int i = 0; i < _object.Properties.Property.Length; i++)
            {
                _properties[_object.Properties.Property[i].Name.ToLower()] = _object.Properties.Property[i].Value;
            }
            _hasProperties = true;
        }

        if (_hasProperties)
        {
            if (_properties.ContainsKey("speed")) _speed = float.Parse(_properties["speed"]) / 10;
        }
        else _speed = (float)1 / 10;

        //SetOrigin(width / 2, height / 2);
        //SetOrigin(width, height);
        //SetXY(pX, pY);

        alpha = 0;

        TruckAnimation truckAnimation = new TruckAnimation();
        AddChild(truckAnimation);

        float offsetX = width * 3 / 4;
        float offsetY = height * 3 / 4;
        player = new Player(this, cameraX, cameraY, offsetX, offsetY, pLevel);
        //player.SetXY(x, y);
        player.SetXY(x - offsetX, y - offsetY);
        AddChild(player);

        SFX.PlaySound(SFX.engineRunning, true, 0.1f);
    }

    private void Update()
    {
        if (PauseManager.GetPause()) return;
        x += _speed * Time.deltaTime;

        //Determine win or loss, end the level
        if (x > player.level.width)
        {
            PauseManager.Pause();
            if (player.score >= player.level.scoreToWin)
            {
                player.end = Player.EndState.WIN;
            }
            else player.end = Player.EndState.LOSS;

            MyGame.gameHasStarted = false;
        }

        //MyUtils.ControlObject(this);
    }
}

public class TruckAnimation : AnimationSprite
{
    private const int ANIMATION_DELAY = 50;
    private int _lastUpdatedAnimationTime;
    private int wouldBeFrame;

    public TruckAnimation() : base("assets/player/PlayerSpritesheet3.png", 2, 9)
    {
        wouldBeFrame = 0;
        currentFrame = wouldBeFrame;
        SetOrigin(width, height);

        _lastUpdatedAnimationTime = 0;
    }

    private void Update()
    {
        if (PauseManager.GetPause()) return;
        Animation();
    }

    private void Animation()
    {
        if (Time.time > _lastUpdatedAnimationTime + ANIMATION_DELAY)
        {
            wouldBeFrame++;
            if (wouldBeFrame >= 17) wouldBeFrame= 0;
            currentFrame = wouldBeFrame;
            _lastUpdatedAnimationTime = Time.time;
        }
    }
}