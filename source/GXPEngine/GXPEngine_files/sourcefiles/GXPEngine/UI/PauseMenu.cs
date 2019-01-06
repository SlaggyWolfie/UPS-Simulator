using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class PauseMenu : Menu
{
    private Sprite _continue;
    private Sprite _exit;
    private Sprite _backToMenu;

    public PauseMenu()
    {
        background = new Sprite("assets/menu/buttons/menu-button-2.png");
        background.SetOrigin(background.width / 2, background.height / 2);
        background.SetXY(game.width / 2, game.height / 2);
        AddChild(background);

        buttonOffset = 120;
        DrawTheButtons();
    }

    protected override void Update()
    {
        HandleHovering();
    }

    private void DrawTheButtons()
    {
        float buttonSet = game.height * 4 / 9;
        DrawButton(ref _continue, "assets/menu/buttons/Continue-Button.png", buttonSet);
        buttonSet += buttonOffset;
        DrawButton(ref _backToMenu, "assets/menu/buttons/Back-to-menu.png", buttonSet);
        buttonSet += buttonOffset;
        DrawButton(ref _exit, "assets/menu/buttons/Exit_Button.png", buttonSet);
    }

    protected override void HandleClicks(ref Sprite button)
    {
        //SFX.PlaySound(SFX.click);
        if (button == _continue)
        {
            Destroy();
            PauseManager.UnPause();
        }
        else if (button == _backToMenu)
        {
            PauseManager.UnPause();
            Destroy();
            MyGame.currentLevel.Destroy();
            ReturnToMenu();
        }
        else if (button == _exit)
        {
            PauseManager.UnPause();
            game.Destroy();
        }
    }

    public void HandleHovering()
    {
        HoverOverButton(ref _continue);
        HoverOverButton(ref _backToMenu);
        HoverOverButton(ref _exit);
    }
}

public class WinLoseMenu : Menu
{
    private bool _win;

    private TextBox _pointbox;
    private Sprite _continue;
    private Sprite _retry;
    private Sprite _backToMenu;


    public WinLoseMenu(bool win)
    {
        hoverUp = 1;
        hoverDown = 0.9f;
        //Console.WriteLine("Creating an end screen.");
        _win = win;
        string filename = (_win) ? "assets/menu/buttons/Win_Screen1.png" : "assets/menu/buttons/Loose_Screen.png";

        background = new Sprite(filename);
        background.SetOrigin(background.width / 2, background.height / 2);
        background.SetXY(game.width / 2, game.height / 2);
        AddChild(background);
        background.alpha = 0.8f;

        DrawButtons();

        _pointbox = new TextBox(game.width, game.height, 0, 0, 30);
        AddChild(_pointbox);

        string text = "Score: " + MyGame.currentLevel.truck.player.score.ToString();
        if (!_win) text += "/" + MyGame.currentLevel.scoreToWin.ToString();
        _pointbox.SetText(text, game.width / 2 + 30, game.height * 4 / 9 - buttonOffset, true, true, true);
    }

    protected override void Update()
    {
        HandleHovering();
    }

    private void DrawButtons()
    {
        if (!_win)
        {
            _retry = new Sprite("assets/menu/buttons/Re-try_Button.png");
            _retry.SetOrigin(_retry.width / 2, _retry.height / 2);
            //_retry.SetXY(game.width / 2 - _retry.width * 1.75f, game.height / 2 + _retry.height * 3 / 4);
            _retry.SetXY(game.width / 2, game.height / 2 + _retry.height * 1.5f);
            //_retry.SetScaleXY(0.8f);
            AddChild(_retry);
        }

        if (_win)
        {
            _continue = new Sprite("assets/menu/buttons/Win_Screen_Continue_Button.png");
            _continue.SetOrigin(_continue.width / 2, _continue.height / 2);
            _continue.SetXY(game.width / 2 + _continue.width - 75, background.height / 2 + background.y - _continue.height);
            AddChild(_continue);
        }

        _backToMenu = new Sprite("assets/menu/buttons/Back_to_Menu_Button.png");
        _backToMenu.SetOrigin(_backToMenu.width / 2, _backToMenu.height / 2);
        _backToMenu.SetXY(game.width - _backToMenu.width / 2, game.height - _backToMenu.height / 2);
        //_backToMenu.alpha = 0.6f;
        AddChild(_backToMenu);
    }

    private void HandleHovering()
    {
        if (_continue != null) HoverOverButton(ref _continue);
        HoverOverButton(ref _backToMenu);
        if (_retry != null) HoverOverButton(ref _retry);
    }

    protected override void HandleClicks(ref Sprite button)
    {
        //SFX.PlaySound(SFX.click);
        if (button == _continue)
        {
            MyGame.currentLevel.Destroy();
            Destroy();
            PauseManager.UnPause();
            LoadLevel2(AutomaticLevelSystem(true));
        }
        else if (button == _backToMenu)
        {
            MyGame.currentLevel.Destroy();
            Destroy();
            PauseManager.UnPause();
            ReturnToMenu();
        }
        else if (button == _retry)
        {
            MyGame.currentLevel.Destroy();
            Destroy();
            PauseManager.UnPause();
            LoadLevel2(AutomaticLevelSystem(false));
        }
    }

    private string AutomaticLevelSystem(bool nextLevel)
    {
        for (int i = 0; i < LevelManager.levelFilenameList.Count; i++)
        {
            if (nextLevel)
            {
                if (i == LevelManager.levelFilenameList.Count - 1)
                {
                    if (PauseManager.GetPause()) PauseManager.UnPause();
                    Destroy();
                    MyGame.currentLevel.Destroy();
                    ReturnToMenu();
                    return null;
                }
                if (MyGame.currentLevel.filename == LevelManager.levelFilenameList[i])
                {
                    return LevelManager.levelFilenameList[i + 1];
                }
            }
            else if (!nextLevel && MyGame.currentLevel.filename == LevelManager.levelFilenameList[i])
            {
                return LevelManager.levelFilenameList[i];
            }
        }
        return LevelManager.levelFilenameList[0];
    }
}