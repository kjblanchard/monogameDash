using System;
using FMOD.Studio;
using Microsoft.Xna.Framework;

namespace SupergoonDashCrossPlatform.Sound;

public class SoundGameComponent : GameComponent
{
    #region Configuration

    private FMOD.Studio.System _fmodStudioSystem;

    private EventInstance _currentBgm = new EventInstance();

    public SoundGameComponent(Game game) : base(game)
    {
        InitializeFmodStudio();
    }

    #endregion


    #region Methods

    /// <summary>
    /// Creates the fmod studio system, and initializes the system
    /// </summary>
    private void InitializeFmodStudio()
    {
        FMOD.Studio.System.create(out _fmodStudioSystem);
        _fmodStudioSystem.initialize(512, INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);

        //load the bank files.
        //Currently using a null out as I am not accessing the banks after loading them into the sound system.
        _fmodStudioSystem.loadBankFile("Content/Sounds/Desktop/Master.bank", LOAD_BANK_FLAGS.NORMAL, out _);
        _fmodStudioSystem.loadBankFile("Content/Sounds/Desktop/Master.strings.bank", LOAD_BANK_FLAGS.NORMAL,
            out _);
        
    }

    public void PlayBgm(string bgmName, float volumeLevel = 1)
    {
        //If there is a current bgm, stop it from playing
        if (_currentBgm.isValid())
            _currentBgm.stop(STOP_MODE.IMMEDIATE);
        //get an event from the bank into a event description
        _fmodStudioSystem.getEvent($"event:/{bgmName}", out var description);
        //load the song into memory
        description.loadSampleData();
        //create a playable instance
        description.createInstance(out var tempInstance);
        //play the song
        tempInstance.start();
        _currentBgm = tempInstance;
    }

    public void PlaySfx(string sfxName, float volumeLevel = 1)
    {
        _fmodStudioSystem.getEvent($"event:/{sfxName}", out var description);
        description.loadSampleData();
        description.createInstance(out var tempInstance);
        tempInstance.setVolume(volumeLevel);
        tempInstance.start();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _fmodStudioSystem.update();
    }

    #endregion
}