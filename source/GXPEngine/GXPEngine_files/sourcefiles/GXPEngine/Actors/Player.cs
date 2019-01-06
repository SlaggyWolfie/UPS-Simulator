using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
using GXPEngine;

public class Player : Slingshot
{
    //Actual player data
    public int score;

    public enum EndState
    {
        WIN, LOSS, ONGOING
    }

    public EndState end;

    private PauseMenu _pauseScreen;
    
    public Player(Truck truck, float cameraX, float cameraY,  float offsetX, float offsetY, Level pLevel) : base (truck, cameraX, cameraY, offsetX, offsetY, pLevel)
    {
        end = EndState.ONGOING;
    }

    protected override void Update()
    {
        score = Math.Max(score, 0);
        OtherControls();
        if (end != EndState.ONGOING)
        {
            //PauseManager.Pause();
            EndLevel();
        }
        base.Update();
    }

    private void OtherControls()
    {
        if (MyGame.gameHasStarted)
        {
            if (Input.GetKeyDown(Key.P) || Input.GetKeyDown(Key.ESCAPE))
            {
                if (!PauseManager.GetPause())
                {
                    _pauseScreen = new PauseMenu();
                    level.HUD_Layer.AddChild(_pauseScreen);
                    PauseManager.Pause();
                }
                else if (PauseManager.GetPause())
                {
                    if (_pauseScreen != null)
                    {
                        _pauseScreen.Destroy();
                        level.HUD_Layer.RemoveChild(_pauseScreen);
                        _pauseScreen = null;
                    }

                    PauseManager.UnPause();
                }
            }
            if (Input.GetKey(Key.LEFT_SHIFT) && Input.GetKeyDown(Key.SPACE))
            {
                score += 100;
            }
        }
    }

    private void EndLevel()
    {
        if (end == EndState.WIN)
        {
            end = EndState.ONGOING;
            SFX.StopAllSounds();
            WinLoseMenu menu = new WinLoseMenu(true);
            level.HUD_Layer.AddChild(menu);
        }
        else if (end == EndState.LOSS)
        {
            end = EndState.ONGOING;
            SFX.StopAllSounds();
            WinLoseMenu menu = new WinLoseMenu(false);
            level.HUD_Layer.AddChild(menu);
        }
    }
}