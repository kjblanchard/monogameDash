using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Input;

public class InputGameComponent : GameComponent
{
    private KeyboardState _currentKeyboardState, _previousKeyboardState;
        private MouseState _currentMouseState, _previouMouseState;
        private readonly GamePadState[] _previousGamePadStates = new GamePadState[4];
        private readonly GamePadState[] _currentGamePadStates = new GamePadState[4];
        // public PlayerController[] PlayerControllers = new PlayerController[4];

        public override void Initialize()
        {
            CreatePlayerControllers();

        }

        /// <summary>
        /// Updates the controller and joystick states
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            _previouMouseState = _currentMouseState;
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
            _currentGamePadStates.CopyTo(_previousGamePadStates, 0);
            // foreach (var _playerController in PlayerControllers)
            // {
            //     _playerController.Update(gameTime);
            // }
            // for (int i = 0; i < _currentGamePadStates.Length; i++)
            // {
            //     _currentGamePadStates[i] = GamePad.GetState((PlayerIndex)(i));
            // }
        }

        /// <summary>
        /// Checks and returns whether the player has started pressing a certain keyboard key in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool KeyPressed(Keys k)
        {
            return _currentKeyboardState.IsKeyDown(k) && _previousKeyboardState.IsKeyUp(k);
        }
        /// <summary>
        /// Checks and returns whether the player has started pressing a certain joystick key in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool KeyPressed(int playerNumber, Buttons button)
        {
            return _currentGamePadStates[playerNumber].IsButtonDown(button) &&
                   _previousGamePadStates[playerNumber].IsButtonUp(button);
        }

        /// <summary>
        /// Checks and returns whether the player has stopped pressing a certain keyboard key in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is no longer pressed but was still pressed in the previous frame; false otherwise.</returns>
        public bool KeyReleased(Keys k)
        {
            return _currentKeyboardState.IsKeyUp(k) && _previousKeyboardState.IsKeyDown(k);
        }
        /// <summary>
        /// Checks and returns whether the player has stopped pressing a certain joystick button in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is no longer pressed but was still pressed in the previous frame; false otherwise.</returns>
        public bool KeyReleased(int playerNumber, Buttons button)
        {
            return _currentGamePadStates[playerNumber].IsButtonUp(button) && _previousGamePadStates[playerNumber].IsButtonDown(button);
        }

        /// <summary>
        /// Checks and returns whether the player is currently holding a certain keyboard key down.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is currently being held down; false otherwise.</returns>
        public bool KeyDown(Keys k)
        {
            return _currentKeyboardState.IsKeyDown(k);
        }
        /// <summary>
        /// Checks and returns whether the player is holding a joystick button down
        /// </summary>
        /// <param name="playerNumber">The current joystick to check</param>
        /// <param name="button">The button to check</param>
        /// <returns>Returns if the button is held or not</returns>
        public bool KeyDown(int playerNumber, Buttons button)
        {
            return _currentGamePadStates[playerNumber].IsButtonDown(button);
        }
        /// <summary>
        /// This is ran during the initialization stage, so that the Controllers can properly get their input set.
        /// These controllers are used for all of the players
        /// </summary>
        private void CreatePlayerControllers()
        {
            // for (int i = 0; i < PlayerControllers.Length; i++)
            // {
            //     PlayerControllers[i] = new PlayerController(i);
            // }

        }
        public Vector2 MousePosition()
        {
            return _currentMouseState.Position.ToVector2();
        }

        public bool WasThereMouseMovement()
        {
            return _previouMouseState.Position != _currentMouseState.Position;
        }
        public bool LeftMouseButtonClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _previouMouseState.LeftButton == ButtonState.Released;
        }
        public bool RightMouseButtonClicked()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed &&_previouMouseState.RightButton == ButtonState.Released;
        }

        public InputGameComponent(Game game) : base(game)
        {
        }
}