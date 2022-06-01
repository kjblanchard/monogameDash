using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Input;

        public abstract class Controller 
    {
        // public static Spritesheet MouseSpriteSheet;
        public static bool MouseDebugMode = false;
        protected Controller()
        {
            _input ??= GameWorld.InputGameComponent;
        }


        /// <summary>
        /// Default keyboard mapping for player 1
        /// </summary>
        protected static KeyMapping<Keys> _defaultKeyboardMap = new KeyMapping<Keys>()
        {
            UpButton = Keys.W,
            RightButton = Keys.D,
            DownButton = Keys.S,
            LeftButton = Keys.A,
            YButton = Keys.Y,
            BButton = Keys.X,
            AButton = Keys.Space,
            XButton = Keys.P,
            StartButton = Keys.Enter,
            SelectButton = Keys.Apps
        };

        /// <summary>
        /// The default button mapping in the game
        /// </summary>
        protected static KeyMapping<Buttons> _defaultJoystickMap = new KeyMapping<Buttons>
        {
            UpButton = Buttons.DPadUp,
            RightButton = Buttons.DPadRight,
            DownButton = Buttons.DPadDown,
            LeftButton = Buttons.DPadLeft,
            YButton = Buttons.Y,
            BButton = Buttons.B,
            AButton = Buttons.A,
            XButton = Buttons.X,
            StartButton = Buttons.Start,
            SelectButton = Buttons.Back
        };

        /// <summary>
        /// The input to check if buttons are pressed
        /// </summary>
        protected static InputGameComponent _input;

        public abstract bool IsButtonPressed(ControllerButtons button);
        public abstract bool IsButtonHeld(ControllerButtons button);
        public abstract bool IsButtonReleased(ControllerButtons button);

        public static Vector2 MousePosition()
        {
            return _input.MousePosition();
        }
        public static Vector2 MouseScreenToWorldResolution()
        {
            return Camera.Camera.ScreenToWorldResolution(_input.MousePosition());
        }

        public static RectangleF MouseBounds()
        {

           // return new RectangleF(Controller.MouseScreenToWorldResolution(),
           //      new Size2(10,10));
           return new RectangleF();
        }
        public static Vector2 MouseScreenCameraPosition()
        {

            return Camera.Camera.ScreenToWorldAndCamOffset(_input.MousePosition());
         }

        public static bool WasThereMouseMovement()
        {
            return _input.WasThereMouseMovement();
        }

        public static bool LeftMouseButtonClicked()
        {
            return _input.LeftMouseButtonClicked();
        }

        public bool RightMouseButtonClicked()
        {
            return _input.RightMouseButtonClicked();
        }

    }

    /// <summary>
    /// All of the controller buttons in the game
    /// </summary>
    public enum ControllerButtons
    {
        Default = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4,
        Y = 5,
        B = 6,
        A = 7,
        X = 8,
        Start = 9,
        Select = 10,
    }

    /// <summary>
    /// The key mappings used in the game, this is used to set the appropriate keys for keymapping
    /// </summary>
    /// <typeparam name="T">The type of keymapping, should be a button or a Key</typeparam>
    public class KeyMapping<T>
    {
        public T UpButton;
        public T RightButton;
        public T DownButton;
        public T LeftButton;
        public T YButton;
        public T BButton;
        public T AButton;
        public T XButton;
        public T StartButton;
        public T SelectButton;

    }
