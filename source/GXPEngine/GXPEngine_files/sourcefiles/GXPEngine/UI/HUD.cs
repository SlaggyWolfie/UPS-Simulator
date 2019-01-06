using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class HUD : GameObject
{
    private Sprite _windBackground;
    private Sprite _nextPackageBackground;
    private Sprite _pointsBackground;

    private Sprite _windArrow;
    private Sprite _nextPackage;
    
    private TextBox _pointbox;
    private int _points = 0;

    private Level _level;

    public HUD(Level level)
    {
        _level = level;

        _windBackground = new Sprite("assets/menu/buttons/HUD.png");
        _windBackground.SetOrigin(_windBackground.width / 2, _windBackground.height / 2);
        _windBackground.SetScaleXY(_level.tileSize / _windBackground.height * 1.5f);
        _windBackground.SetXY(game.width - _windBackground.width / 2 - 20, _windBackground.height / 2);
        _windBackground.alpha = 0;
        AddChild(_windBackground);

        _nextPackageBackground = new Sprite("assets/menu/buttons/HUD.png");
        _nextPackageBackground.SetOrigin(_nextPackageBackground.width / 2, _nextPackageBackground.height / 2);
        _nextPackageBackground.SetScaleXY(_level.tileSize / _nextPackageBackground.height * 1.5f);
        _nextPackageBackground.SetXY(game.width / 3, game.height - _nextPackageBackground.height / 2);
        AddChild(_nextPackageBackground);

        _pointsBackground = new Sprite("assets/menu/buttons/HUD.png");
        _pointsBackground.SetOrigin(_pointsBackground.width / 2, _pointsBackground.height / 2);
        _pointsBackground.SetScaleXY(_level.tileSize / _pointsBackground.height * 1.5f);
        _pointsBackground.SetXY(game.width * 2 / 3, game.height - _pointsBackground.height / 2);
        AddChild(_pointsBackground);

        _windArrow = new Sprite("assets/menu/buttons/arrow.png");
        _windArrow.SetOrigin(_windArrow.width / 2, _windArrow.height / 2);
        _windArrow.SetXY(_windBackground.x, _windBackground.y);
        AddChild(_windArrow);

        _pointbox = new TextBox(game.width, game.height, 0, 0, 20);
        AddChild(_pointbox);

        //_level.truck.player.score += 10;
    }

    private void Update()
    {
        if (PauseManager.GetPause()) return;
        UpdatePoints();
        UpdateWindArrow();
        UpdateNextPackage(_level.truck.player.nextPackage);
    }

    private void UpdatePoints()
    {
        if ((_points == 0 && _level.truck.player.score != 0) || _points != _level.truck.player.score)
        {
            _points = _level.truck.player.score;
            _pointbox.SetText(_points.ToString(), _pointsBackground.x, _pointsBackground.y, true, true, true); 
        }
    }

    private void UpdateNextPackage(Package package)
    {
        //if ((_nextPackage != null && package != null) && _nextPackage != package)
        //{
        //    //Used to contain everything else
        //}
        if (_nextPackage != null)
        {
            _nextPackage.Destroy();
        }

        if (package != null)
        {
            _nextPackage = new Sprite(package.filename);
            _nextPackage.SetOrigin(_nextPackage.width / 2, _nextPackage.height / 2);
            _nextPackage.SetXY(_nextPackageBackground.x, _nextPackageBackground.y);
            AddChild(_nextPackage);
        }
    }

    private void UpdateWindArrow()
    {
        if (_windArrow.rotation != _level.wind.GetAngleDegrees()) _windArrow.rotation = _level.wind.GetAngleDegrees();
    }
}