using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Input;

public class PlayerController : Controller
{
    #region Class Vars

    /// <summary>
    /// The current player that this is set to, which is set in the constructor
    /// </summary>
    public int PlayerNum { get; }

    public KeyMapping<Keys> KeyboardMapping
    {
        get => _keyMapping;
        set => _keyMapping = value;
    }

    public KeyMapping<Buttons> JoystickMapping
    {
        get => _buttonMapping;
        set => _buttonMapping = value;
    }

    /// <summary>
    /// The keyboard mapping set to this controller, it is set to the default if it isn't changed
    /// </summary>
    private KeyMapping<Keys> _keyMapping = _defaultKeyboardMap;

    /// <summary>
    /// The joystick mapping set to this controller, it is set to the default if it isn't changed
    /// </summary>
    private KeyMapping<Buttons> _buttonMapping = _defaultJoystickMap;

    private List<ControllerButtonAndAction> _buttonAndActions = new List<ControllerButtonAndAction>();

    #endregion

    #region Constructor

    /// <summary>
    /// A player controller, these should all be spawned at the beginning of the game and set to the proper values.
    /// </summary>
    /// <param name="playerNumber"></param>
    public PlayerController(int playerNumber = 0) : base()
    {
        PlayerNum = playerNumber;
    }

    #endregion

    /// <summary>
    /// Checks to see if the current button has just been pressed on the keyboard and joystick for this player
    /// </summary>
    /// <param name="button">The controller button to check against</param>
    /// <returns>Returns if the button has just been pressed</returns>
    public override bool IsButtonPressed(ControllerButtons button)
    {
        return button switch
        {
            ControllerButtons.Default => false,
            ControllerButtons.Up => _input.KeyPressed(_keyMapping.UpButton) ||
                                    _input.KeyPressed(PlayerNum, _buttonMapping.UpButton),
            ControllerButtons.Right => _input.KeyPressed(_keyMapping.RightButton) ||
                                       _input.KeyPressed(PlayerNum, _buttonMapping.RightButton),
            ControllerButtons.Down => _input.KeyPressed(_keyMapping.DownButton) ||
                                      _input.KeyPressed(PlayerNum, _buttonMapping.DownButton),
            ControllerButtons.Left => _input.KeyPressed(_keyMapping.LeftButton) ||
                                      _input.KeyPressed(PlayerNum, _buttonMapping.LeftButton),
            ControllerButtons.Y => _input.KeyPressed(_keyMapping.YButton) ||
                                   _input.KeyPressed(PlayerNum, _buttonMapping.YButton),
            ControllerButtons.B => _input.KeyPressed(_keyMapping.BButton) ||
                                   _input.KeyPressed(PlayerNum, _buttonMapping.BButton),
            ControllerButtons.A => _input.KeyPressed(_keyMapping.AButton) ||
                                   _input.KeyPressed(PlayerNum, _buttonMapping.AButton),
            ControllerButtons.X => _input.KeyPressed(_keyMapping.XButton) ||
                                   _input.KeyPressed(PlayerNum, _buttonMapping.XButton),
            ControllerButtons.Select => _input.KeyPressed(_keyMapping.SelectButton) ||
                                        _input.KeyPressed(PlayerNum, _buttonMapping.SelectButton),
            ControllerButtons.Start => _input.KeyPressed(_keyMapping.StartButton) ||
                                       _input.KeyPressed(PlayerNum, _buttonMapping.StartButton),
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    /// <summary>
    /// Checks to see if the curront button is being held down on the keyboard and joystick for this player
    /// </summary>
    /// <param name="button">The controller button to check against</param>
    /// <returns>If the button is being held</returns>
    public override bool IsButtonHeld(ControllerButtons button)
    {
        return button switch
        {
            ControllerButtons.Default => false,
            ControllerButtons.Up => _input.KeyDown(_keyMapping.UpButton) ||
                                    _input.KeyDown(PlayerNum, _buttonMapping.UpButton),
            ControllerButtons.Right => _input.KeyDown(_keyMapping.RightButton) ||
                                       _input.KeyDown(PlayerNum, _buttonMapping.RightButton),
            ControllerButtons.Down => _input.KeyDown(_keyMapping.DownButton) ||
                                      _input.KeyDown(PlayerNum, _buttonMapping.DownButton),
            ControllerButtons.Left => _input.KeyDown(_keyMapping.LeftButton) ||
                                      _input.KeyDown(PlayerNum, _buttonMapping.LeftButton),
            ControllerButtons.Y => _input.KeyDown(_keyMapping.YButton) ||
                                   _input.KeyDown(PlayerNum, _buttonMapping.YButton),
            ControllerButtons.B => _input.KeyDown(_keyMapping.BButton) ||
                                   _input.KeyDown(PlayerNum, _buttonMapping.BButton),
            ControllerButtons.A => _input.KeyDown(_keyMapping.AButton) ||
                                   _input.KeyDown(PlayerNum, _buttonMapping.AButton),
            ControllerButtons.X => _input.KeyDown(_keyMapping.XButton) ||
                                   _input.KeyDown(PlayerNum, _buttonMapping.XButton),
            ControllerButtons.Select => _input.KeyDown(_keyMapping.SelectButton) ||
                                        _input.KeyDown(PlayerNum, _buttonMapping.SelectButton),
            ControllerButtons.Start => false,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    /// <summary>
    /// Checks to see if the button was just released on the keyboard and the joystick for this player
    /// </summary>
    /// <param name="button">The controller button to look for</param>
    /// <returns>If the button was just released</returns>
    public override bool IsButtonReleased(ControllerButtons button)
    {
        return button switch
        {
            ControllerButtons.Default => false,
            ControllerButtons.Up => _input.KeyReleased(_keyMapping.UpButton) ||
                                    _input.KeyReleased(PlayerNum, _buttonMapping.UpButton),
            ControllerButtons.Right => _input.KeyReleased(_keyMapping.RightButton) ||
                                       _input.KeyReleased(PlayerNum, _buttonMapping.RightButton),
            ControllerButtons.Down => _input.KeyReleased(_keyMapping.DownButton) ||
                                      _input.KeyReleased(PlayerNum, _buttonMapping.DownButton),
            ControllerButtons.Left => _input.KeyReleased(_keyMapping.LeftButton) ||
                                      _input.KeyReleased(PlayerNum, _buttonMapping.LeftButton),
            ControllerButtons.Y => _input.KeyReleased(_keyMapping.YButton) ||
                                   _input.KeyReleased(PlayerNum, _buttonMapping.YButton),
            ControllerButtons.B => _input.KeyReleased(_keyMapping.BButton) ||
                                   _input.KeyReleased(PlayerNum, _buttonMapping.BButton),
            ControllerButtons.A => _input.KeyReleased(_keyMapping.AButton) ||
                                   _input.KeyReleased(PlayerNum, _buttonMapping.AButton),
            ControllerButtons.X => _input.KeyReleased(_keyMapping.XButton) ||
                                   _input.KeyReleased(PlayerNum, _buttonMapping.XButton),
            ControllerButtons.Select => _input.KeyReleased(_keyMapping.SelectButton) ||
                                        _input.KeyReleased(PlayerNum, _buttonMapping.SelectButton),
            ControllerButtons.Start => false,

            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    public void Update(GameTime gameTime)
    {
        CheckAllButtonActions();
        SendButtonEvents();
    }


    private void CheckAllButtonActions()
    {
        _buttonAndActions.Clear();
        // if(GameWorld.IsDebugConsoleOpen)return;
        foreach (var _controllerButton in (ControllerButtons[])Enum.GetValues(typeof(ControllerButtons)))
        {
            CheckIfButtonPressed(_controllerButton);
            CheckIfButtonHeld(_controllerButton);
            CheckIfButtonReleased(_controllerButton);
        }
    }

    private void CheckIfButtonPressed(ControllerButtons buttonToCheck)
    {
        if (IsButtonPressed(buttonToCheck))
            _buttonAndActions.Add(new ControllerButtonAndAction
                { ButtonAction = ButtonActions.Pressed, ButtonPressed = buttonToCheck });
    }

    private void CheckIfButtonHeld(ControllerButtons buttonToCheck)
    {
        if (IsButtonHeld(buttonToCheck))
            _buttonAndActions.Add(new ControllerButtonAndAction
                { ButtonAction = ButtonActions.Held, ButtonPressed = buttonToCheck });
    }

    private void CheckIfButtonReleased(ControllerButtons buttonToCheck)
    {
        if (IsButtonReleased(buttonToCheck))
            _buttonAndActions.Add(new ControllerButtonAndAction
                { ButtonAction = ButtonActions.Released, ButtonPressed = buttonToCheck });
    }

    public delegate void ButtonPressedEventHandler(object sender, List<ControllerButtonAndAction> actionsThisFrame);

    public event ButtonPressedEventHandler OnButtonsPressed;

    private void SendButtonEvents()
    {
        OnButtonsPressed?.Invoke(this, _buttonAndActions);
    }
}

public enum ButtonActions
{
    None = 0,
    Pressed = 1,
    Held = 2,
    Released = 3
}

public struct ControllerButtonAndAction
{
    public ControllerButtons ButtonPressed;
    public ButtonActions ButtonAction;
}