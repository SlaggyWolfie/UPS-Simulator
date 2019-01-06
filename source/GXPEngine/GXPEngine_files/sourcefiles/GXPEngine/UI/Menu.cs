using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using GXPEngine;


public abstract class Menu : GameObject
{
    protected float buttonOffset;
    protected float hoverDown = 1.2f;
    protected float hoverUp = 1f;

    protected Sprite background;

    public Menu()
    {
        buttonOffset = 50;
    }

    protected virtual void Update()
    {

    }

    protected virtual void DrawButton(ref Sprite button, string filename, float yPos, float scale = 1)// float xPos = gamewidth / 2)
    {
        button = new Sprite(filename);
        AddChild(button);
        button.SetOrigin(button.width / 2, button.height / 2);
        button.SetXY(game.width / 2, yPos);
        button.scale = scale;
    }

    protected virtual void DrawButton(ref Sprite button, string filename, float x, float y, float scale = 1)// float xPos = gamewidth / 2)
    {
        button = new Sprite(filename);
        AddChild(button);
        button.SetOrigin(button.width / 2, button.height / 2);
        button.SetXY(x, y);
        button.scale = scale;
    }

    public void HoverOverButton(ref Sprite button)
    {
        if (Input.mouseX > button.x - button.width / 2 && Input.mouseX < button.x + button.width / 2 &&
            Input.mouseY > button.y - button.height / 2 && Input.mouseY < button.y + button.height / 2)
        {
            button.SetScaleXY(hoverDown);
            if (Input.GetMouseButtonUp(0))
            {
                HandleClicks(ref button);
            }
        }
        else
        {
            button.SetScaleXY(hoverUp);
        }
    }

    protected virtual void HandleClicks(ref Sprite button)
    {
        //Does nothing
    }

    protected void LoadLevel2(string filename)
    {
        SFX.StopAllSounds();
        Destroy();
        MyGame.currentLevel = new Level(filename);
        game.AddChild(MyGame.currentLevel);
    }

    public static void ReturnToMenu()
    {
        SFX.StopAllSounds();
        MainMenu menu = new MainMenu();
        Game.main.AddChild(menu);
    }
}

public class MainMenu : Menu
{
    private readonly int _cooldownChirp = 6000; //milliseconds
    private int _lastUpdateChirp;
    private int _chirpLength = 27000; //ms

    private Sprite _start;
    private Sprite _levels;
    private Sprite _exit;

    public MainMenu()
    {
        background = new Sprite("assets/menu/buttons/Main_screen.png");
        float bw = background.width;
        float bh = background.height;
        background.SetScaleXY(game.width / bw, game.height / bh);
        AddChild(background);
        DrawTheButtons();

        _lastUpdateChirp = Time.time - _chirpLength - _cooldownChirp;
    }

    protected override void Update()
    {
        if (Time.time > _lastUpdateChirp + _chirpLength + _cooldownChirp)
        {
            SFX.PlaySound(SFX.birdChirpingBackground);
            _lastUpdateChirp = Time.time;
            //Console.WriteLine("Bird Chirp start. " + Time.time);
        }
        HandleHovering();
    }

    private void DrawTheButtons()
    {
        DrawButton(ref _start, "assets/menu/buttons/Start_Button.png", game.height / 3 + buttonOffset);
        DrawButton(ref _levels, "assets/menu/buttons/Level_Button.png", game.height / 2 + buttonOffset);
        DrawButton(ref _exit, "assets/menu/buttons/Exit_Button.png", game.height * 2 / 3 + buttonOffset);
    }

    protected override void HandleClicks(ref Sprite button)
    {
        //SFX.PlaySound(SFX.click);
        if (button == _start)
        {
            LoadLevel2(LevelManager.levelFilenameList[0]);
        }
        else if (button == _levels)
        {
            Destroy();
            LevelSelect levelPage = new LevelSelect();
            game.AddChild(levelPage);
        }
        else if (button == _exit)
        {
            game.Destroy();
        }
    }

    public void HandleHovering()
    {
        HoverOverButton(ref _start);
        HoverOverButton(ref _levels);
        HoverOverButton(ref _exit);
    }
}

public class LevelSelect : Menu
{
    //private List<Sprite> buttonList = new List<Sprite> { };

    private string _unlockedFilename = "assets/menu/buttons/Unlocked_Level.png";
    private string _lockedFilename = "assets/menu/buttons/Locked_Level.png";

    private Sprite _previous;
    private Sprite _next;
    private Sprite _back;
    private Sprite _lock;
    private TextBox _levelName;

    private int levelNumber;
    private bool _levelIsSelected;

    protected new float buttonOffset = 100;

    public LevelSelect()
    {
        background = new Sprite("assets/menu/buttons/Level_selection-Update.png");
        float bw = background.width;
        float bh = background.height;
        background.SetScaleXY(game.width / bw, game.height / bh);
        AddChild(background);
        
        DrawTheButtons();
        
        _levelName = new TextBox(game.width, game.height, 0, 0, 30);
        AddChild(_levelName);

        levelNumber = 0;
        _levelIsSelected = false;
    }


    protected override void Update()
    {
        HideButtons();
        HandleHovering();
        CurrentLevel(game.width / 2, game.height / 2 + buttonOffset / 2 + 10, levelNumber, true);
    }

    private void DrawTheButtons()
    {
        DrawButton(ref _back, "assets/menu/buttons/Level-Selection_backtomenu.png", game.width / 2, game.height * 9 / 10);
        DrawButton(ref _previous, "assets/menu/buttons/Level-Selection_Arrow_Left.png", game.width / 2 - buttonOffset * 3, game.height / 2 + buttonOffset * 0.66f);
        DrawButton(ref _next, "assets/menu/buttons/Level-Selection_Arrow_Right.png", game.width / 2 + buttonOffset * 3, game.height / 2 + buttonOffset * 0.66f);
    }

    private Sprite DrawAndReturnButton(string filename, float pX, float pY)
    {
        Sprite button = new Sprite(filename);
        AddChild(button);
        button.SetOrigin(button.width / 2, button.height / 2);
        button.SetXY(pX, pY);
        return button;
    }

    protected override void DrawButton(ref Sprite button, string filename, float pX, float pY)
    {
        button = new Sprite(filename);
        AddChild(button);
        button.SetOrigin(button.width / 2, button.height / 2);
        button.SetXY(pX, pY);
    }

    private void HideButtons()
    {
        if (levelNumber <= 0) _previous.alpha = 0;
        else _previous.alpha = 1;

        if (levelNumber >= LevelManager.levelFilenameList.Count - 1) _next.alpha = 0;
        else _next.alpha = 1;
    }

    protected override void HandleClicks(ref Sprite button)
    {
        //SFX.PlaySound(SFX.click);
        if (button == _back)
        {
            Destroy();
            ReturnToMenu();
        }
        if (button == _previous && levelNumber > 0)
        {
            levelNumber--;
            levelNumber = Math.Max(0, levelNumber);
            _levelIsSelected = false;
        }
        if (button == _next && levelNumber < LevelManager.levelFilenameList.Count)
        {
            levelNumber++;
            levelNumber = Math.Min(levelNumber, LevelManager.levelFilenameList.Count - 1);
            _levelIsSelected = false;
        }
        if (button == _lock)
        {
            LoadLevel2(LevelManager.levelFilenameList[levelNumber]);
        }
    }

    public void HandleHovering()
    {
        HoverOverButton(ref _back);
        HoverOverButton(ref _next);
        HoverOverButton(ref _previous);
        if (_lock != null) HoverOverButton(ref _lock);
    }

    private void CurrentLevel(float x, float y, int id = 0, bool unlocked = false)
    {
        if (!_levelIsSelected)
        {
            if (_lock != null) _lock.Destroy();

            float offset = 50;
            string filenameLock = (unlocked) ? _unlockedFilename : _lockedFilename;
            string text = LevelManager.levelFilenameList[id].Replace("tmx\\", "").Replace(".tmx", "").Replace("_", " ");
            if (text.Contains("Level 0")) text = text.Replace("Level 0 ", "");
            
            _lock = new Sprite(filenameLock);
            _lock.SetOrigin(_lock.width / 2, _lock.height / 2);
            _lock.SetXY(x - offset * 4.25f, y);
            AddChild(_lock);
            
            if (_levelName != null) _levelName.SetText("");
            //_levelName.SetText(text, x - offset * 3, y, true, false, true);
            _levelName.SetText(text, x + _lock.width / 2 - 30, y, true, true, true);

            _levelIsSelected = true;
        }
    }
}