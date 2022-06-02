using System;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.Sound;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Input;
using SupergoonDashCrossPlatform.SupergoonEngine.Physics;
using SupergoonEngine.Tiled;

//ImGUI
using Num = System.Numerics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameWorld : Game
{
    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected SoundGameComponent _soundGameComponent;
    protected TiledGameComponent _tiledGameComponent;
    protected GraphicsGameComponent _graphicsGameComponent;
    public PhysicsGameComponent PhysicsGameComponent;
    public static InputGameComponent InputGameComponent;

    //ImGUI

    private RenderTarget2D _imguiRenderTarget;
    private Texture2D _xnaTexture;
    private IntPtr _imGuiTexture;
    private ImGuiRenderer _imGuiRenderer;

    public static bool moveUp;
    public static bool moveDown;
    public static bool moveRight;
    public static bool moveLeft;

    public LevelStateMachine LevelStateMachine => _levelStateMachine;
    protected LevelStateMachine _levelStateMachine;


    public GameWorld()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        AttachAllGameComponents();
        //Add in the statics for the cameras to be used in the game.
        Camera.Camera.InitializeCamera(GraphicsDevice, _graphicsGameComponent);
        _graphicsGameComponent.Initialize();
        _soundGameComponent.PlayBgm();
        // _tiledGameComponent.LoadedTmxContent.CreateTileGameObjectsFromContent();
        _levelStateMachine = new LevelStateMachine(_tiledGameComponent);

        //ImGui
        _imguiRenderTarget = new RenderTarget2D(_graphics.GraphicsDevice, 1920, 1080);
        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
        _xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
        {
            var red = (pixel % 300) / 2;
            return new Color(red, 1, 1);
        });

        // Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
        _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        moveDown = moveLeft = moveRight = moveUp = false;
        //Handle input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
            moveRight = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
            moveDown = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
            moveUp = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
            moveLeft = true;

        _levelStateMachine.Update(gameTime);

        Camera.Camera.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //ImGui
        GraphicsDevice.SetRenderTarget(_imguiRenderTarget);
        GraphicsDevice.Clear(Color.Transparent);
        // Call BeforeLayout first to set things up
        _imGuiRenderer.BeforeLayout(gameTime);
        // Draw our UI
        ImGuiLayout();
        // Call AfterLayout now to finish up and draw all the things
        _imGuiRenderer.AfterLayout();
        GraphicsDevice.SetRenderTarget(null);
        //EndImgui
        
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
            _graphicsGameComponent.SpriteScale * Camera.Camera.GetCameraTransformMatrix());

        _levelStateMachine.Draw(_spriteBatch);
        _spriteBatch.End();
        
        
        //Draw imgui render target
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            DepthStencilState.Default, RasterizerState.CullNone, null, null);
        _spriteBatch.Draw(_imguiRenderTarget, Vector2.Zero, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        _spriteBatch.End();
        //EndDrawImgui
        

        base.Draw(gameTime);
    }


    private void AttachAllGameComponents()
    {
        _soundGameComponent = new SoundGameComponent(this);
        _tiledGameComponent = new TiledGameComponent(this);
        _graphicsGameComponent = new GraphicsGameComponent(this, _graphics, GraphicsDevice);
        PhysicsGameComponent = new PhysicsGameComponent(this, _tiledGameComponent);
        InputGameComponent = new InputGameComponent(this);

        Components.Add(_tiledGameComponent);
        Components.Add(_soundGameComponent);
        Components.Add(_graphicsGameComponent);
        Components.Add(PhysicsGameComponent);
        Components.Add(InputGameComponent);

        GameObject._gameWorld = this;
    }

    protected virtual void AddLevels(params Level[] levels)
    {
        foreach (var level in levels)
        {
            _levelStateMachine.AddLevel(level);
        }

        _levelStateMachine.Initialize();
    }

    protected void InitializeLevels()
    {
        _levelStateMachine.InitializeStates();
    }

    protected void ChangeLevel(int levelTag)
    {
        _levelStateMachine.ChangeState(levelTag);
    }


    //ImGui
    // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/sdl_opengl2_example/main.cpp
    private float f = 0.0f;

    private bool show_test_window = false;
    private bool show_another_window = false;
    private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
    private byte[] _textBuffer = new byte[100];

    protected virtual void ImGuiLayout()
    {
        // 1. Show a simple window
        // Tip: if we don't call ImGui.Begin()/ImGui.End() the widgets appears in a window automatically called "Debug"
        {
            ImGui.Text("Hello, world!");
            ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty);
            ImGui.ColorEdit3("clear color", ref clear_color);
            if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
            if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
            ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)",
                1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

            ImGui.InputText("Text input", _textBuffer, 100);

            ImGui.Text("Texture sample");
            ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One,
                Num.Vector4.One); // Here, the previously loaded texture is used
        }

        // 2. Show another simple window, this time using an explicit Begin/End pair
        if (show_another_window)
        {
            ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
            ImGui.Begin("Another Window", ref show_another_window);
            ImGui.Text("Hello");
            ImGui.End();
        }

        // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
        if (show_test_window)
        {
            ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
            ImGui.ShowDemoWindow(ref show_test_window);
        }
    }

    public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        Color[] data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }
}