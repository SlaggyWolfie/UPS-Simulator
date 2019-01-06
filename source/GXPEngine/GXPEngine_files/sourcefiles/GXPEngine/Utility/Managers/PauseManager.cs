using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public static class PauseManager
{
    public static bool pause = false;
    //false is for unpaused;

    public static void Pause()
    {
        foreach (KeyValuePair<Sound, SoundChannel> kvp in SFX.soundChannelLibrary)
        {
            kvp.Value.IsPaused = true;
        }
        pause = true;
    }
    public static void UnPause()
    {
        foreach (KeyValuePair<Sound, SoundChannel> kvp in SFX.soundChannelLibrary)
        {
            kvp.Value.IsPaused = false;
        }
        pause = false;
    }
    public static bool GetPause()
    {
        return pause;
    }
}
