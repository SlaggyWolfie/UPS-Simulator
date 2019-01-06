using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class SFX
{
    private static string sfxFolder = "assets/sfx/";
    private static float _sfxVolume = 1f;

    public static Sound angryCat = new Sound(sfxFolder + "Angry_Cat.mp3");
    public static Sound birdChirpingBackground = new Sound(sfxFolder + "Bird_Chirping_Background.mp3", false, true); //For the menu
    //public static Sound carOpenCloseEngine = new Sound(sfxFolder + "car_door_open_and_close_with_engine_running.mp3");
    public static Sound engineRunning = new Sound(sfxFolder + "Engine_running.mp3", true, true); //press start
    public static Sound gnomeBreak = new Sound(sfxFolder + "Gnome_breaking.mp3");
    public static Sound largeDogBark = new Sound(sfxFolder + "Large Dog Barking.mp3");
    public static Sound smallDogBark = new Sound(sfxFolder + "Small Dog Barking.mp3");
    public static Sound mailboxHit = new Sound(sfxFolder + "Mailbox_hit.mp3");
    public static Sound metalPlateDrop = new Sound(sfxFolder + "metal_weight_plate_dropped.mp3");
    public static Sound packageDrop = new Sound(sfxFolder + "Package_drop.mp3");
    public static Sound packageDrop2 = new Sound(sfxFolder + "Package_drop2.mp3");
    public static Sound signHit = new Sound(sfxFolder + "Sign_hit.mp3");
    public static Sound trashDrop = new Sound(sfxFolder + "Trash_dropping.mp3");
    public static Sound windowBreak = new Sound(sfxFolder + "Window_breaking.mp3");
    public static Sound windowBreak2 = new Sound(sfxFolder + "Window_breaking2.mp3");

    //music
    public static Sound backgroundMusic = new Sound("assets/music/Background_Music.mp3", true, true); //questionable use of looping = true

    private static List<SoundChannel> sfxList = new List<SoundChannel>
    {

    };

    public static Dictionary<Sound, SoundChannel> soundChannelLibrary = new Dictionary<Sound, SoundChannel> { };

    public static SoundChannel sfxSoundChannel; //Do not use Stop().

    /// <summary>
    /// Sets the sound. Input volume as 0...1f.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetSFXVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public static float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public static void PlaySound(Sound sound, bool setVolume = false, float volume = 1f)
    {
        sfxSoundChannel = sound.Play();
        if (setVolume) sfxSoundChannel.Volume = volume;
        else sfxSoundChannel.Volume = _sfxVolume;

        soundChannelLibrary[sound] = sfxSoundChannel;
        //soundChannelLibrary.Add(sound, sfxSoundChannel);
    }

    public static SoundChannel GetSoundChannel(Sound sound)
    {
        return soundChannelLibrary[sound];
    }

    public static void StopAllSounds()
    {
        foreach (KeyValuePair<Sound, SoundChannel> kvp in SFX.soundChannelLibrary)
        {
            kvp.Value.Stop();
        }
        soundChannelLibrary.Clear();
    }
}