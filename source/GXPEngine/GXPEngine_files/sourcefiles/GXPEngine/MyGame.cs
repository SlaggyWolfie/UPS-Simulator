using System;
using System.Drawing;
using GXPEngine;

public class MyGame : Game //MyGame is a Game
{
    public static bool gameHasStarted;
    public static Level currentLevel;

    //initialize game here
    public MyGame() : base(1600, 900, false)
    {
        MainMenu menu = new MainMenu();
        AddChild(menu);
    }

    //update game here
    void Update()
    {
        //Console.WriteLine(currentFps);
        //if (currentLevel != null && currentLevel.truck != null && currentLevel.truck.player != null) Console.WriteLine(currentLevel.truck.player + " exists");
        if (currentLevel != null && currentLevel.truck != null && currentLevel.truck.player != null) Console.WriteLine(currentLevel.truck.player.x + " " + currentLevel.truck.player.y);
    }

    //system starts here
    static void Main()
    {
        new MyGame().Start();
    }

    private void Rescale(Sprite pSprite)
    {
        float xScale = (float)game.width / pSprite.width;
        float yScale = (float)game.height / pSprite.height;
        pSprite.SetScaleXY(xScale, yScale);
    }

    protected override void OnDestroy()
    {
        SFX.StopAllSounds();
        base.OnDestroy();
    }
}
